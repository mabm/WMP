using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MyWindowsMediaPlayer.Models
{
    class Serializer
    {
        public static void savePlaylist(List<PlaylistItem> playlist, string playlistPath)
        {
            try
            {
                var xEle = new XElement("playlist",
                            from music in playlist
                            select new XElement("music",
                                new XAttribute("path", music.path)
                            ));

                xEle.Save(playlistPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static List<PlaylistItem> getPlaylist(string path)
        {
            List<PlaylistItem> playlist = new List<PlaylistItem>();

            XElement xEle = XElement.Load(path);
            IEnumerable<string> musics = xEle.Descendants("music").Select(element => element.Attribute("path").Value);
            foreach (var music in musics)
            {
                playlist.Add(new PlaylistItem(music));
            }
            return (playlist);
        }
    }
}
