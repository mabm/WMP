using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer
{
    class MediaModel
    {
        public bool isMuted { get; set;}
        public bool isStopped { get; set; }
        public bool isPaused { get; set; }
        public double volume { get; set; }

        public MediaModel()
        {
            this.isMuted = false;
            this.isStopped = true;
            this.isPaused = false;
            this.volume = 0;
        }
    }
}
