using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyWindowsMediaPlayer.Models
{
    class Playlist
    {
        private Random rnd;
        public List<PlaylistItem> list;
        public int nbSelected;
        public ListBox listBox;
        public bool isRandom;
        public RepeatManager repeatManager;

        public Playlist(ListBox box)
        {
            rnd = new Random();
            isRandom = false;
            nbSelected = 0;
            list = new List<PlaylistItem>();
            listBox = box;
            listBox.AllowDrop = true;
            repeatManager = new RepeatManager();
        }

        public bool isEmpty()
        {
            if (listBox.Items.IsEmpty)
                return (true);
            return (false);
        }

        public void add(PlaylistItem item)
        {
            list.Add(item);
            listBox.Items.Add(item.name);
        }

        public void clear()
        {
            list.Clear();
            this.nbSelected = 0;
            listBox.Items.Clear();
            listBox.SelectedIndex = 0;
        }

        public PlaylistItem getSelectedItem()
        {  
            return (list.ElementAt(nbSelected));
        }

        public void selectNextItem()
        {
            if (repeatManager.mode == 2)
                return;
            if (isRandom)
            {
                int lastSelected = this.nbSelected;
                while (lastSelected == this.nbSelected && this.list.Count > 0)
                    this.nbSelected = rnd.Next(0, this.list.Count);
            }
            else
            {
                if (this.nbSelected + 1 >= this.list.Count)
                {
                    if (repeatManager.mode == 1)
                        this.nbSelected = -1;
                    else
                        return;
                }
                this.nbSelected++;
            }
            listBox.SelectedIndex = this.nbSelected;
        }

        public void updateSelection()
        {
            int index = this.listBox.SelectedIndex;
            if (index != -1)
                this.nbSelected = index;
        }

        public void selectLast()
        {
            if (this.list.Count > 0)
                this.nbSelected = this.list.Count() - 1;
            listBox.SelectedIndex = this.nbSelected;
        }

        public bool isFinish()
        {
            if (this.nbSelected == this.list.Count - 1)
                return (true);
            return (false);
        }

        public void selectPrevItem()
        {
            if (this.nbSelected == 0)
                return;
            this.nbSelected--;
            listBox.SelectedIndex = this.nbSelected;
        }

        public string getNextRepeat()
        {
            return (repeatManager.getNextRepeat());
        }

        public int getRepeatMode()
        {
            return (repeatManager.mode);
        }

        public void changePlaylist(List<PlaylistItem> playlistLoad)
        {
            this.clear();
            this.list = playlistLoad;
            foreach (var music in list)
            {
                this.listBox.Items.Add(music.name);
            }
        }
    }
}
