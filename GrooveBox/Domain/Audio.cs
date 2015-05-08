namespace GrooveBox.Domain
{
    public class Audio
    {
        private readonly byte[] audio;
        private readonly int bpm;

        public Audio(byte[] audio, int bpm)
        {
            this.audio = audio;
            this.bpm = bpm;
        }

        public byte[] AudioData
        {
            get { return audio; }
        }

        public int Bpm
        {
            get { return bpm; }
        }
    }
}