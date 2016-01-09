using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyWindowsMediaPlayer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaModel mediaModel;
        private Tool tool;

        public MainWindow()
        {
            InitializeComponent();
            mediaModel = new MediaModel();
            playlistBox.AllowDrop = true;
            tool = new Tool();
        }

        private void playlistBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
        }

        private void playlistBox_DragLeave(object sender, DragEventArgs e)
        {

        }

        private void playlistBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string file in files)
                playlistBox.Items.Add(file);
        }

        private void playButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (playlistBox.Items.IsEmpty)
                return;
            if (mediaModel.isStopped == true)
            {
                mediaModel.isPaused = false;
                mediaModel.isStopped = false;
                playButton.Source = new BitmapImage(new Uri(tool.imgPath + "pause.png"));
                media.Source = new Uri((string)playlistBox.Items.GetItemAt(0));
                media.Play();
            }
            else if (mediaModel.isPaused == true)
            {
                mediaModel.isPaused = false;
                playButton.Source = new BitmapImage(new Uri(tool.imgPath + "pause.png"));
                this.media.Play();
            }
            else
            {
                mediaModel.isPaused = true;
                playButton.Source = new BitmapImage(new Uri(tool.imgPath + "play.png"));
                this.media.Pause();
            }
        }

        private void speakerButton_click(object sender, MouseButtonEventArgs e)
        {
            if (mediaModel.isMuted == false)
            {
                mediaModel.volume = this.media.Volume;
                this.media.Volume = 0;
                mediaModel.isMuted = true;
                speakerButton.Source = new BitmapImage(new Uri(tool.imgPath + "mute.png"));
            }
            else
            {
                this.media.Volume = mediaModel.volume;
                mediaModel.isMuted = false;
                speakerButton.Source = new BitmapImage(new Uri(tool.imgPath + "speaker.png"));
            }
        }
    }
}
