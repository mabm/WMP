using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer.Models
{
    class RepeatManager
    {
        private List<String> repeatImg;
        public int mode;

        public RepeatManager()
        {
            repeatImg = new List<string>();
            repeatImg.Add("no-repeat.png");
            repeatImg.Add("repeat.png");
            repeatImg.Add("repeat-same.png");
            mode = 0;
        }

        public string getNextRepeat()
        {
            mode++;
            if (mode > 2)
                mode = 0;
            return (repeatImg.ElementAt(mode));
        }

    }
}
