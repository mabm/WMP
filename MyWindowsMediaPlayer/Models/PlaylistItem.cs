using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer
{
    class PlaylistItem
    {
        public String name;
        public String path;
        
        public PlaylistItem(String itemPath)
        {
            FileInfo fileInfo = new FileInfo(itemPath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("The file was not found.", path);
            }
            name = fileInfo.Name;
            this.path = itemPath;
        }
    }
}
