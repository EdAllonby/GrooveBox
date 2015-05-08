using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using Utilities;

namespace GrooveBox.Player
{
    /// <summary>
    /// Stream for continous playback with a queued stream.
    /// </summary>
    public class QueuedWaveStream : WaveStream
    {
        private readonly IDictionary<int, WaveStream> streams = new SortedDictionary<int, WaveStream>();
        private int currentPlayingIndex = 1;

        /// <summary>
        /// Creates a new Loop stream
        /// </summary>
        public QueuedWaveStream()
        {
            ContinuePlayback = true;
        }

        public int CurrentPlayingIndex
        {
            get { return currentPlayingIndex; }
            private set
            {
                if (value > 0)
                {
                    currentPlayingIndex = value;
                }
            }
        }

        private WaveStream CurrentWavestream
        {
            get { return streams[CurrentPlayingIndex]; }
        }

        /// <summary>
        /// Use this to turn playback of next loop on or off
        /// </summary>
        public bool ContinuePlayback { private get; set; }

        /// <summary>
        /// Return source stream's wave format
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get { return streams[currentPlayingIndex].WaveFormat; }
        }

        /// <summary>
        /// QueuedWaveStream simply returns
        /// </summary>
        public override long Length
        {
            get { return CurrentWavestream.Length; }
        }

        /// <summary>
        /// QueuedWaveStream simply passes on positioning to source stream
        /// </summary>
        public override long Position
        {
            get { return CurrentWavestream.Position; }
            set { CurrentWavestream.Position = value; }
        }

        public void Stop()
        {
            foreach (var waveStream in streams.Values)
            {
                waveStream.Position = 0;
            }

            ContinuePlayback = false;

            CurrentPlayingIndex = 1;
        }

        public void UpdateStreamAtPosition(WaveStream stream, int position)
        {
            DisposeOldStreamAtKey(position);

            streams[position] = stream;
        }

        public void ExportToWav(string fileLocation)
        {
            if (!streams.Any())
            {
                return;
            }

            using (WaveFileWriter waveFileWriter = new WaveFileWriter(fileLocation, WaveFormat))
            {
                foreach (WaveStream stream in streams.Values)
                {
                    int read;

                    var buffer = new byte[1024];

                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        waveFileWriter.Write(buffer, 0, read);
                    }

                    stream.Position = 0;
                }
            }
        }

        public void Reset()
        {
            foreach (WaveStream waveStream in streams.Values)
            {
                waveStream.Dispose();
            }

            streams.Clear();
        }

        /// <summary>
        /// Fires when the currently playing stream has finished playback.
        /// </summary>
        public event EventHandler FinishedCurrentStream;

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = CurrentWavestream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);

                if (IsEndOfStream(bytesRead))
                {
                    if (ShouldStopPlayback())
                    {
                        Position = 0;
                        break;
                    }

                    LoadNextStream();
                }

                totalBytesRead += bytesRead;
            }

            return totalBytesRead;
        }

        private static bool IsEndOfStream(int bytesRead)
        {
            return bytesRead == 0;
        }

        private bool ShouldStopPlayback()
        {
            return CurrentWavestream.Position == 0 || !ContinuePlayback;
        }

        private void LoadNextStream()
        {
            CurrentWavestream.Position = 0;

            if (CurrentPlayingIndex < streams.Count)
            {
                currentPlayingIndex++;
            }
            else
            {
                CurrentPlayingIndex = 1;
            }

            EventUtility.SafeFireEvent(FinishedCurrentStream, this);
        }

        private void DisposeOldStreamAtKey(int position)
        {
            if (streams.ContainsKey(position))
            {
                WaveStream waveStream = streams[position];

                waveStream.Dispose();
            }
        }
    }
}