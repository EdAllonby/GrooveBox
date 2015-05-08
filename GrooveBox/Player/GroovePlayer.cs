using System;
using System.IO;
using GrooveBox.Domain;
using NAudio.Wave;
using Utilities;

namespace GrooveBox.Player
{
    public sealed class GroovePlayer
    {
        private readonly QueuedWaveStream queuedWaveStream;
        private int totalBars;
        private IWavePlayer wavePlayer;

        public GroovePlayer(int totalBars)
        {
            Repeat = false;

            TotalBars = totalBars;

            queuedWaveStream = new QueuedWaveStream();

            queuedWaveStream.FinishedCurrentStream += QueuedWaveStreamFinishedCurrentStream;
        }

        public int TotalBars
        {
            private get { return totalBars; }
            set
            {
                if (value > 0)
                {
                    totalBars = value;
                }
            }
        }

        public bool Repeat { get; set; }
        public event EventHandler<CurrentBarEventArgs> NewBarStarted;

        public void Play()
        {
            Stop();

            StartPlayback();
        }

        public void ExportCurrentGrooves(string fileName)
        {
            queuedWaveStream.ExportToWav(fileName);
        }

        private void StartPlayback()
        {
            EventUtility.SafeFireEvent(NewBarStarted, this, new CurrentBarEventArgs(queuedWaveStream.CurrentPlayingIndex));

            wavePlayer = new WaveOut();

            queuedWaveStream.ContinuePlayback = true;

            wavePlayer.Init(queuedWaveStream);
            wavePlayer.Play();
        }

        public void Stop()
        {
            queuedWaveStream.Stop();

            if (wavePlayer != null)
            {
                wavePlayer.Stop();
            }
        }

        private void QueuedWaveStreamFinishedCurrentStream(object sender, EventArgs e)
        {
            if (!Repeat && queuedWaveStream.CurrentPlayingIndex == 1)
            {
                Stop();
            }
            else
            {
                EventUtility.SafeFireEvent(NewBarStarted, this,
                    new CurrentBarEventArgs(queuedWaveStream.CurrentPlayingIndex));
            }
        }

        public void ChangeAudio(Audio newGrooveAudio, int grooveBarPosition)
        {
            WaveStream wavestream = AudioToWaveStream(newGrooveAudio);

            queuedWaveStream.UpdateStreamAtPosition(wavestream, grooveBarPosition);
        }

        private static RawSourceWaveStream AudioToWaveStream(Audio audio)
        {
            return new RawSourceWaveStream(new MemoryStream(audio.AudioData), new WaveFormat());
        }

        public void ResetGrooves()
        {
            queuedWaveStream.Reset();
        }
    }
}