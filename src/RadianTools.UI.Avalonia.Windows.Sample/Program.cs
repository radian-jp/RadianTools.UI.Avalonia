using Avalonia;
using RadianTools.UI.Avalonia.Common;
using RadianTools.UI.Avalonia.Windows;
using System;

namespace RadianTools.UI.Avalonia.Sample;

internal sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        FolderItemFactoryProvider.Register(new WindowsFolderItemFactory(WindowsFolderRootMode.DesktopVirtualFolders));

        BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
