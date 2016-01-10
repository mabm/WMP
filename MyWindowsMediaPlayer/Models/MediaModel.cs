using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MyWindowsMediaPlayer
{
    class MediaModel
    {
        public bool isMuted { get; set;}
        public bool isStopped { get; set; }
        public bool isPaused { get; set; }
        public bool isFullscreen { get; set; }
        public double volume { get; set; }
        public DispatcherTimer timerVideo;
        public Thickness margin;

        public MediaModel(Thickness mediaMargin)
        {
            this.isMuted = false;
            this.isStopped = true;
            this.isPaused = false;
            this.isFullscreen = false;
            this.volume = 0;
            this.timerVideo = new DispatcherTimer();
            timerVideo.Interval = TimeSpan.FromSeconds(1);
            margin = mediaMargin;
        }
    }
}
