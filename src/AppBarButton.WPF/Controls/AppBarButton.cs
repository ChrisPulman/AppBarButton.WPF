// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace CP.WPF.Controls;

/// <summary>Adds icon content to a standard button.</summary>
public class AppBarButton
    : Button
{
    /// <summary>The background glyph font size property.</summary>
    public static readonly DependencyProperty BackgroundGlyphFontSizeProperty = DependencyProperty.Register(
        nameof(BackgroundGlyphFontSize),
        typeof(double),
        typeof(AppBarButton),
        new PropertyMetadata(33D));

    /// <summary>The background glyph property.</summary>
    public static readonly DependencyProperty BackgroundGlyphProperty = DependencyProperty.Register(
        nameof(BackgroundGlyph),
        typeof(string),
        typeof(AppBarButton),
        new PropertyMetadata(string.Empty));

    /// <summary>Identifies the Ellipse Diameter property.</summary>
    public static readonly DependencyProperty ElipseDiameterProperty = DependencyProperty.Register(
        nameof(ElipseDiameter),
        typeof(double),
        typeof(AppBarButton),
        new PropertyMetadata(40D));

    /// <summary>The foreground glyph color property.</summary>
    public static readonly DependencyProperty ForegroundGlyphColorProperty = DependencyProperty.Register(
        nameof(ForegroundGlyphColor),
        typeof(Brush),
        typeof(AppBarButton),
        new PropertyMetadata(Brushes.LightGray));

    /// <summary>The foreground glyph font size property.</summary>
    public static readonly DependencyProperty ForegroundGlyphFontSizeProperty = DependencyProperty.Register(
        nameof(ForegroundGlyphFontSize),
        typeof(double),
        typeof(AppBarButton),
        new PropertyMetadata(33D));

    /// <summary>The foreground glyph property.</summary>
    public static readonly DependencyProperty ForegroundGlyphProperty = DependencyProperty.Register(
        nameof(ForegroundGlyph),
        typeof(string),
        typeof(AppBarButton),
        new PropertyMetadata(string.Empty));

    /// <summary>Identifies the IconData property.</summary>
    public static readonly DependencyProperty IconDataProperty = DependencyProperty.Register(
        nameof(IconData),
        typeof(Geometry),
        typeof(AppBarButton));

    /// <summary>Identifies the IconHeight property.</summary>
    public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register(
        nameof(IconHeight),
        typeof(double),
        typeof(AppBarButton),
        new PropertyMetadata(25D));

    /// <summary>Identifies the StandardIcon property.</summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(AppBarIcons),
        typeof(AppBarButton),
        new PropertyMetadata(AppBarIcons.None, SetIcon));

    /// <summary>Identifies the IconWidth property.</summary>
    public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(
        nameof(IconWidth),
        typeof(double),
        typeof(AppBarButton),
        new PropertyMetadata(25D));

    /// <summary>Initializes a new instance of the <see cref="AppBarButton"/> class.</summary>
    public AppBarButton() => DefaultStyleKey = typeof(AppBarButton);

    /// <summary>Gets or sets the background glyph.</summary>
    /// <value>The background glyph.</value>
    [Description("Gets/Sets the Background Glyph value")]
    [Category("Common")]
    public string BackgroundGlyph
    {
        get => (string)GetValue(BackgroundGlyphProperty);

        set => SetValue(BackgroundGlyphProperty, value);
    }

    /// <summary>Gets or sets the size of the background glyph font.</summary>
    /// <value>The size of the background glyph font.</value>
    [Description("Gets/Sets the Background Glyph FontSize value")]
    [Category("Common")]
    public double BackgroundGlyphFontSize
    {
        get => (double)GetValue(BackgroundGlyphFontSizeProperty);

        set => SetValue(BackgroundGlyphFontSizeProperty, value);
    }

    /// <summary>Gets or sets the Ellipse Diameter.</summary>
    /// <value>The Ellipse Diameter.</value>
    [Description("Gets/Sets the Ellipse Diameter value")]
    [Category("Layout")]
    public double ElipseDiameter
    {
        get => (double)GetValue(ElipseDiameterProperty);

        set => SetValue(ElipseDiameterProperty, value);
    }

    /// <summary>Gets or sets the foreground glyph.</summary>
    /// <value>The foreground glyph.</value>
    [Description("Gets/Sets the Foreground Glyph value")]
    [Category("Common")]
    public string ForegroundGlyph
    {
        get => (string)GetValue(ForegroundGlyphProperty);

        set => SetValue(ForegroundGlyphProperty, value);
    }

    /// <summary>Gets or sets the color of the foreground glyph.</summary>
    /// <value>The color of the foreground glyph.</value>
    [Description("Gets/Sets the Foreground Glyph Color value")]
    [Category("Brush")]
    public Brush ForegroundGlyphColor
    {
        get => (Brush)GetValue(ForegroundGlyphColorProperty);

        set => SetValue(ForegroundGlyphColorProperty, value);
    }

    /// <summary>Gets or sets the size of the foreground glyph font.</summary>
    /// <value>The size of the foreground glyph font.</value>
    [Description("Gets/Sets the Foreground Glyph FontSize value")]
    [Category("Common")]
    public double ForegroundGlyphFontSize
    {
        get => (double)GetValue(ForegroundGlyphFontSizeProperty);

        set => SetValue(ForegroundGlyphFontSizeProperty, value);
    }

    /// <summary>Gets or sets the icon path data from a list of default Icons.</summary>
    /// <value>The icon path data.</value>
    [Description("Gets/Sets the Icon value")]
    [Category("Common")]
    public AppBarIcons Icon
    {
        get
        {
            var icon = GetValue(IconProperty);
            return icon is null ? default : (AppBarIcons)icon;
        }

        set => SetValue(IconProperty, value);
    }

    /// <summary>Gets or sets the icon path data.</summary>
    /// <value>The icon path data.</value>
    [Description("Gets/Sets the IconData value")]
    [Category("Common")]
    public Geometry? IconData
    {
        get
        {
            var iconData = GetValue(IconDataProperty);
            return iconData is null ? default : (Geometry)iconData;
        }

        set => SetValue(IconDataProperty, value);
    }

    /// <summary>Gets or sets the icon height.</summary>
    /// <value>The icon height.</value>
    [Description("Gets/Sets the Icon Height value")]
    [Category("Layout")]
    public double IconHeight
    {
        get => (double)GetValue(IconHeightProperty);

        set => SetValue(IconHeightProperty, value);
    }

    /// <summary>Gets or sets the icon width.</summary>
    /// <value>The icon width.</value>
    [Description("Gets/Sets the Icon Width value")]
    [Category("Layout")]
    public double IconWidth
    {
        get => (double)GetValue(IconWidthProperty);

        set => SetValue(IconWidthProperty, value);
    }

    /// <summary>Sets the icon.</summary>
    /// <param name="d">The d.</param>
    /// <param name="e">
    /// The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.
    /// </param>
    private static void SetIcon(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is AppBarButton appBarButton)
        {
            var icon = (AppBarIcons)e.NewValue;
            appBarButton.IconData = icon == AppBarIcons.None
                ? default
                : appBarButton.GetIcon(icon);
        }
    }

    /// <summary>Looks up icon in embedded resources.</summary>
    /// <param name="icon">The icon.</param>
    /// <returns>Icon Data as Geometry.</returns>
    private Geometry? GetIcon(AppBarIcons icon)
    {
        const string appBarPrefix = "Ab_";
        const string materialDesignPrefix = "Md_";
        Geometry? geometry = default;

        var iconName = icon.ToString();
        if (iconName.StartsWith(appBarPrefix, StringComparison.Ordinal))
        {
            // find all embedded XAML icon files
            var assembly = GetType().Assembly;
            var shortIconName = iconName.Remove(0, appBarPrefix.Length).Replace('_', '.');
            var resourcePath = $"CP.Assets.AppBar.appbar.{shortIconName}.xaml";
            var resourceName = assembly.GetManifestResourceNames()
                .First(name => name.Equals(resourcePath, StringComparison.Ordinal));

            // load the resource stream
            using var stream = assembly.GetManifestResourceStream(resourceName);

            // parse the icon data using xml
            var path = XDocument.Load(stream!).Root?.Element(
                "{http://schemas.microsoft.com/winfx/2006/xaml/presentation}Path");
            if (path is not null)
            {
                geometry = Geometry.Parse((string?)path.Attribute("Data"));
            }
        }

        if (iconName.StartsWith(materialDesignPrefix, StringComparison.Ordinal))
        {
            // find all embedded SVG icon files
            var assembly = GetType().Assembly;
            var shortIconName = iconName.Remove(0, materialDesignPrefix.Length).Replace('_', '-');
            var resourcePath = $"CP.Assets.svg.{shortIconName}.svg";
            var resourceName = assembly.GetManifestResourceNames()
                .First(name => name.Equals(resourcePath, StringComparison.Ordinal));

            // load the resource stream
            using var stream = assembly.GetManifestResourceStream(resourceName);

            // parse the icon data using xml
            var path = XDocument.Load(stream!).Root?.Element("{http://www.w3.org/2000/svg}path");
            if (path is not null)
            {
                geometry = Geometry.Parse((string?)path.Attribute("d"));
            }
        }

        return geometry;
    }
}
