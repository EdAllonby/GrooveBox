using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Drawing.Point;

namespace Utilities
{
    public static class ImageUtilities
    {
        public static ImageSource ImageToImageSource(Image image)
        {
            MemoryStream memoryStream = new MemoryStream();

            image.Save(memoryStream, ImageFormat.Png);

            memoryStream.Position = 0;

            BitmapImage imageSource = new BitmapImage();

            imageSource.BeginInit();

            imageSource.StreamSource = memoryStream;

            imageSource.EndInit();

            return imageSource;
        }

        public static Image BitmapSourceToImage(BitmapSource bitmapSource)
        {
            Bitmap bmp = new Bitmap(
                bitmapSource.PixelWidth,
                bitmapSource.PixelHeight,
                PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(
                new Rectangle(Point.Empty, bmp.Size),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppPArgb);
            bitmapSource.CopyPixels(
                Int32Rect.Empty,
                data.Scan0,
                data.Height*data.Stride,
                data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        public static SolidColorBrush HexToBrush(string hexValue)
        {
            return (SolidColorBrush) new BrushConverter().ConvertFromString("#FF24313C");
        }
    }
}