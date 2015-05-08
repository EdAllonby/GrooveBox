using System;

namespace GrooveBox.Player
{
    public sealed class CurrentBarEventArgs : EventArgs
    {
        public CurrentBarEventArgs(int currentBar)
        {
            CurrentBar = currentBar;
        }

        public int CurrentBar { get; private set; }
    }
}