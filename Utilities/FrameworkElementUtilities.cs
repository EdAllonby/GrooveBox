using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Size = System.Windows.Size;

namespace Utilities
{
    public static class FrameworkElementUtilities
    {
        public static void SafelyRegisterControl<T>(this FrameworkElement context, T control)
            where T : IFrameworkInputElement
        {
            if ((T) context.FindName(control.Name) != null)
            {
                context.UnregisterName(control.Name);
            }

            context.RegisterName(control.Name, control);
        }

        public static void SafelyUnregisterControl(this FrameworkElement context, IFrameworkInputElement grooveContainer)
        {
            if (context.FindName(grooveContainer.Name) != null)
            {
                context.UnregisterName(grooveContainer.Name);
            }
        }

        public static Image ElementToBitmap(this FrameworkElement window, UIElement canvas, int dpi)
        {
            Size size = new Size(window.Width, window.Height);
            canvas.Measure(size);

            var bitmapRenderer = new RenderTargetBitmap((int) window.Width, (int) window.Height, dpi, dpi,
                PixelFormats.Pbgra32);

            bitmapRenderer.Render(canvas);

            return ImageUtilities.BitmapSourceToImage(bitmapRenderer);
        }
    }
}