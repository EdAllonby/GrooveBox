using System.Drawing;
using NAudio.Wave;

namespace GrooveBox
{
    internal interface IAudioVisualiser
    {
        Image AudioFileToImage(AudioFileReader reader);
    }
}