using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using NAudio.Wave;
using Utilities;
using Image = System.Drawing.Image;

namespace GrooveBox
{
    internal sealed class AudioFileVisualiser : IAudioVisualiser
    {
        const int Mid = 100;
        const int YScale = 100;

        public Image AudioFileToImage(AudioFileReader reader)
        {
            var window = new Window();
            var canvas = new Canvas();

            var samples = reader.Length/(reader.WaveFormat.Channels*reader.WaveFormat.BitsPerSample/8);
            var max = 0.0f;
            var batch = (int) Math.Max(40, samples/300);
            float[] buffer = new float[batch];
            int read;
            var xPos = 0;

            while ((read = reader.Read(buffer, 0, batch)) == batch)
            {
                for (int n = 0; n < read; n++)
                {
                    max = Math.Max(Math.Abs(buffer[n]), max);
                }

                UIElement line = CreateWaveformPart(xPos, max);

                canvas.Children.Add(line);
                max = 0;
                xPos++;
            }

            canvas.Width = xPos;
            canvas.Height = Mid*2;

            RenderCanvas(window, canvas);

            return window.ElementToBitmap(canvas, 96);
        }

        private static UIElement CreateWaveformPart(int xPos, float max)
        {
            var line = new Line
            {
                X1 = xPos,
                X2 = xPos,
                Y1 = Mid + (max*YScale),
                Y2 = Mid - (max*YScale),
                StrokeThickness = 1,
                Stroke = ApplicationColourPalette.ItemColour
            };

            return line;
        }

        private static void RenderCanvas(Window window, Canvas canvas)
        {
            window.Height = Mid*2;
            window.Width = 600;
            window.Content = canvas;
            window.WindowState = WindowState.Minimized;

            // TODO: This is definitely a hack... Need to open the window to get the canvas to 'render' for the image output.
            window.Show();
            window.Close();
        }
    }
}