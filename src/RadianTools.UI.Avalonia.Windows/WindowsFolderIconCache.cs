using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace RadianTools.UI.Avalonia.Windows;

internal class WindowsFolderIconCache : ShellImageList
{
    private static object AvaloniaBitmapConverter(int width, int height, int stride, nint src)
        => new Bitmap(
            PixelFormat.Bgra8888,
            AlphaFormat.Unpremul,
            src,
            new PixelSize(width, height),
            new Vector(96, 96),
            stride
        );

    public WindowsFolderIconCache(SHIL shil) : base(shil, AvaloniaBitmapConverter)
    {
    }
}