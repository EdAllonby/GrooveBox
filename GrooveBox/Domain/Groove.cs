using System.Collections.Generic;
using System.Drawing;

namespace GrooveBox.Domain
{
    public sealed class Groove
    {
        private readonly Audio audio;
        private readonly string genre;
        private readonly int id;
        private readonly string name;
        private readonly List<string> tags;
        private readonly Image wave;

        public Groove(int id, string genre, string name, Image wave, Audio audio, List<string> tags)
        {
            this.id = id;
            this.genre = genre;
            this.name = name;
            this.wave = wave;
            this.audio = audio;
            this.tags = tags;
        }

        public int Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Genre
        {
            get { return genre; }
        }

        public Image WaveForm
        {
            get { return wave; }
        }

        public Audio Audio
        {
            get { return audio; }
        }

        public List<string> Tags
        {
            get { return tags; }
        }
    }
}