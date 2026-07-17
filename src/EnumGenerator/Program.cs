// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO.Compression;
using System.Text;

namespace EnumGenerator;

/// <summary>Refreshes the Material Design SVG assets and generates the icon enumeration.</summary>
internal static class Program
{
    private const string AppBarPrefix = "appbar.";

    private const string ArchiveUrl = "https://github.com/Templarian/MaterialDesign/archive/refs/heads/master.zip";

    private const int DownloadTimeoutMinutes = 10;

    private const string SvgSegment = "/svg/";

    private const string UserAgent = "AppBarButton.WPF-EnumGenerator";

    /// <summary>Runs the asset refresh and enum generation.</summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal static async Task Main()
    {
        var repositoryRoot = FindRepositoryRoot();
        var controlProjectPath = Path.Combine(repositoryRoot, "src", "AppBarButton.WPF");
        var svgPath = Path.Combine(controlProjectPath, "Assets", "svg");
        var appBarPath = Path.Combine(controlProjectPath, "Assets", "AppBar");
        var enumPath = Path.Combine(controlProjectPath, "Controls", "AppBarIcons.cs");

        await RefreshMaterialDesignIconsAsync(svgPath);
        GenerateEnum(svgPath, appBarPath, enumPath);
    }

    private static string FindRepositoryRoot()
    {
        var searchPaths = new[]
        {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory,
            };

        foreach (var searchPath in searchPaths)
        {
            var directory = new DirectoryInfo(searchPath);
            while (directory is not null)
            {
                var generatorProject = Path.Combine(
                    directory.FullName,
                    "src",
                    "EnumGenerator",
                    "EnumGenerator.csproj");
                var controlProject = Path.Combine(
                    directory.FullName,
                    "src",
                    "AppBarButton.WPF",
                    "AppBarButton.WPF.csproj");
                if (File.Exists(generatorProject) && File.Exists(controlProject))
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
            }
        }

        throw new DirectoryNotFoundException("Could not locate the AppBarButton.WPF repository root.");
    }

    private static async Task RefreshMaterialDesignIconsAsync(string svgPath)
    {
        Directory.CreateDirectory(svgPath);

        var stagingPath = Path.Combine(Path.GetTempPath(), $"AppBarButton.WPF-{Guid.NewGuid():N}");
        Directory.CreateDirectory(stagingPath);

        try
        {
            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(DownloadTimeoutMinutes),
            };
            client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);

            using var response = await client.GetAsync(ArchiveUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            using var archive = new ZipArchive(responseStream, ZipArchiveMode.Read);

            var svgEntries = archive.Entries
                .Where(IsSvgEntry)
                .OrderBy(entry => entry.Name, StringComparer.Ordinal)
                .ToArray();

            if (svgEntries.Length == 0)
            {
                throw new InvalidDataException("The Material Design archive did not contain any SVG files.");
            }

            var fileNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var entry in svgEntries)
            {
                if (!fileNames.Add(entry.Name))
                {
                    throw new InvalidDataException(
                        $"The Material Design archive contains duplicate SVG file name '{entry.Name}'.");
                }

                var stagedFile = Path.Combine(stagingPath, entry.Name);
                await using var source = entry.Open();
                await using var destination = new FileStream(
                    stagedFile,
                    FileMode.CreateNew,
                    FileAccess.Write,
                    FileShare.None);
                await source.CopyToAsync(destination);
            }

            foreach (var fileName in fileNames.OrderBy(name => name, StringComparer.Ordinal))
            {
                var destinationFile = Path.Combine(svgPath, fileName);
                File.Copy(Path.Combine(stagingPath, fileName), destinationFile, true);
            }
        }
        finally
        {
            Directory.Delete(stagingPath, true);
        }
    }

    private static bool IsSvgEntry(ZipArchiveEntry entry)
    {
        var normalizedPath = entry.FullName.Replace('\\', '/');
        var svgSegmentIndex = normalizedPath.IndexOf(SvgSegment, StringComparison.Ordinal);
        return svgSegmentIndex >= 0
            && normalizedPath.LastIndexOf('/') == svgSegmentIndex + SvgSegment.Length - 1
            && entry.Name.EndsWith(".svg", StringComparison.OrdinalIgnoreCase);
    }

    private static void GenerateEnum(string svgPath, string appBarPath, string enumPath)
    {
        var svgFiles = Directory.EnumerateFiles(svgPath, "*.svg", SearchOption.TopDirectoryOnly)
            .OrderBy(Path.GetFileName, StringComparer.Ordinal)
            .ToArray();
        var appBarFiles = Directory.EnumerateFiles(appBarPath, "*.xaml", SearchOption.TopDirectoryOnly)
            .OrderBy(Path.GetFileName, StringComparer.Ordinal)
            .ToArray();
        var members = new HashSet<string>(StringComparer.Ordinal)
            {
                "None",
            };
        var builder = CreateEnumHeader();

        AppendEnumMember(builder, "None", "No icon selected.");
        foreach (var file in svgFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            var memberName = $"Md_{fileName.Replace('-', '_')}";
            AddEnumMember(members, builder, memberName, "Material Design SVG asset.");
        }

        foreach (var file in appBarFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            if (!fileName.StartsWith(AppBarPrefix, StringComparison.Ordinal))
            {
                throw new InvalidDataException($"AppBar asset '{fileName}' does not start with 'appbar.'.");
            }

            var memberName = $"Ab_{fileName[AppBarPrefix.Length..].Replace('.', '_')}";
            AddEnumMember(members, builder, memberName, "Legacy AppBar XAML asset.");
        }

        builder.Remove(builder.Length - Environment.NewLine.Length, Environment.NewLine.Length)
            .AppendLine("}");

        File.WriteAllText(enumPath, builder.ToString(), new UTF8Encoding(false));
    }

    private static StringBuilder CreateEnumHeader()
    {
        return new StringBuilder()
            .AppendLine("// <auto-generated/>")
            .AppendLine("// Copyright (c) Chris Pulman. All rights reserved.")
            .AppendLine(
                "// Licensed under the MIT license. See LICENSE file in the project root " +
                "for full license information.")
            .AppendLine()
                .AppendLine("namespace CP.WPF.Controls;")
                .AppendLine()
                .AppendLine("/// <summary>Identifies an icon available to <see cref=\"AppBarButton\"/>.</summary>")
                .AppendLine("public enum AppBarIcons")
                .AppendLine("{");
    }

    private static void AddEnumMember(
        HashSet<string> members,
        StringBuilder builder,
        string memberName,
        string summary)
    {
        if (!members.Add(memberName))
        {
            throw new InvalidDataException($"Multiple icon assets map to enum member '{memberName}'.");
        }

        AppendEnumMember(builder, memberName, summary);
    }

    private static void AppendEnumMember(StringBuilder builder, string memberName, string summary)
    {
        builder.Append("    /// <summary>").Append(summary).AppendLine("</summary>")
            .Append("    ").Append(memberName).AppendLine(",")
            .AppendLine();
    }
}
