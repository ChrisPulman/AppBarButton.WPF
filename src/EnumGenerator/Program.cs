// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace EnumGenerator
{
    internal static class Program
    {
        private const string _rootPath = @"D:\Projects\GitHub\ChrisPulman\AppBarButton.WPF\src\AppBarButton.WPF";

        private static void Main(string[] args)
        {
            // Read all files from Assets\svg\ and generate an enum entry for each file in a file called AppBarIcons.cs
            var sgvfiles = Directory.GetFiles(@$"{_rootPath}\Assets\svg\", "*.svg");
            var xamlfiles = Directory.GetFiles(@$"{_rootPath}\Assets\AppBar\", "*.xaml");
            var sb = new StringBuilder();
            sb.AppendLine("// Copyright (c) Chris Pulman. All rights reserved.")
                .AppendLine("// Licensed under the MIT license. See LICENSE file in the project root for full license information.")
                .AppendLine()
                .AppendLine("namespace CP.WPF.Controls")
                .AppendLine("{")
                .AppendLine("    /// <summary>")
                .AppendLine("    /// AppBarIcons.")
                .AppendLine("    /// </summary>")
                .AppendLine("    public enum AppBarIcons")
                .AppendLine("    {")
                .AppendLine("#pragma warning disable RCS1243 // Duplicate word in a comment.")
                .AppendLine("#pragma warning disable SA1300 // Element should begin with upper-case letter")
                .Append("        ").AppendLine("/// <summary>")
                .Append("        ").AppendLine("/// No Icon Selected.")
                .Append("        ").AppendLine("/// </summary>")
                .Append("        ").Append("none").AppendLine(",").AppendLine();

            foreach (var file in sgvfiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                sb.Append("        ").AppendLine("/// <summary>")
                .Append("        ").Append("/// ").Append(fileName.Replace('-', ' ')).AppendLine(".")
                .Append("        ").AppendLine("/// </summary>")
                .Append("        ").Append("md_").Append(fileName.Replace('-', '_')).AppendLine(",").AppendLine();
            }

            foreach (var file in xamlfiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                sb.Append("        ").AppendLine("/// <summary>")
                .Append("        ").Append("/// ").Append(fileName.Replace('.', ' ')).AppendLine(".")
                .Append("        ").AppendLine("/// </summary>")
                .Append("        ").Append("ab_").Append(fileName.Remove(0, 7).Replace('.', '_')).AppendLine(",").AppendLine();
            }

            sb.AppendLine("#pragma warning restore SA1300 // Element should begin with upper-case letter").AppendLine("    }")
                .AppendLine("#pragma warning restore RCS1243 // Duplicate word in a comment.")
                .AppendLine("}");
            File.WriteAllText(@$"{_rootPath}\Controls\AppBarIcons.cs", sb.ToString());
            Console.WriteLine("Files generated.");
        }
    }
}
