using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyWindowsMediaPlayer
{
    class PlaylistItem
    {
        public String name;
        public String path;
        public String album;
        public String title;
        public String artist;
        public BitmapImage coverImage;
        
        public PlaylistItem(String itemPath)
        {
            FileInfo fileInfo = new FileInfo(itemPath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("The file was not found.", path);
            }
            name = fileInfo.Name;
            this.path = itemPath;

            try
            {
                TagLib.File f = TagLib.File.Create(path);
                BitmapImage bitmap = new BitmapImage();
                TagLib.IPicture pic = f.Tag.Pictures[0];
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                this.coverImage = bitmap;
                this.album = f.Tag.Album;
                this.title = f.Tag.Title;
                this.artist = f.Tag.AlbumArtists[0];
            }
            catch
            {
                Tool tool = new Tool();
                this.coverImage = new BitmapImage(new Uri(tool.imgPath + "unknown_artist_cover.jpg"));
                this.album = "Album Inconnu";
                this.title = "Titre Inconnu";
                this.artist = "Artiste Inconnu";
            }
        }
    }
}
