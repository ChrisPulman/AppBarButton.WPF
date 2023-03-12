# AppBarButton.WPF
AppBarButton for WPF

This is a simple implementation of the AppBarButton for WPF.

## Usage

```xaml
<Window x:Class="AppBarButton.WPF.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:local="clr-namespace:AppBarButton.WPF"
        xmlns:appbarbtn="https://github.com/ChrisPulman/AppBarButton.WPF"
        Title="MainWindow" Height="350" Width="525">
    <Grid>
        <appbarbtn:AppBarButton Content="adobe_acrobat" Icon="adobe_acrobat" />
    </Grid>
</Window>
```

## Icon

The Icon is a enum that is used to find the icon in the `Assets\AppBar` folder. The icon is a 48x48 Canvas file.

You can add your own icons by setting the IconData property with a suitable path of your choice.

## Themes

The theme is a ResourceDictionary that is merged into the main ResourceDictionary. The default theme is `/AppBarButton.WPF;component/Themes/Generic.xaml`.

Create a new ResourceDictionary and merge it into the main ResourceDictionary based on the following:

```xaml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/AppBarButton.WPF;component/Themes/AppBarButton.xaml" />
    </ResourceDictionary.MergedDictionaries>

    <!--  default accent colors and brushes  -->
    <Color x:Key="AccentColor">#1ba1e2</Color>
    <SolidColorBrush x:Key="Accent" Color="{DynamicResource AccentColor}" />
    <SolidColorBrush x:Key="AppBarButtonBorder" Color="#919191" />
    <SolidColorBrush x:Key="AppBarButtonBorderHover" Color="#d1d1d1" />
    <SolidColorBrush x:Key="AppBarButtonBorderPressed" Color="#d1d1d1" />
    <SolidColorBrush x:Key="AppBarButtonBorderDisabled" Color="#515151" />
    <SolidColorBrush x:Key="AppBarButtonIconBackgroundPressed" Color="{DynamicResource AccentColor}" />
    <SolidColorBrush x:Key="AppBarButtonIconForegroundPressed" Color="#d1d1d1" />
    <SolidColorBrush x:Key="AppBarButtonText" Color="#d1d1d1" />
    <SolidColorBrush x:Key="AppBarButtonTextHover" Color="#d1d1d1" />
    <SolidColorBrush x:Key="AppBarButtonTextPressed" Color="#d1d1d1" />
    <SolidColorBrush x:Key="AppBarButtonTextDisabled" Color="#515151" />
</ResourceDictionary>
```
