using Microsoft.Win32;
using MyWindowsMediaPlayer.Models;
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
        private Playlist playlist;
        private Tool tool;

        public MainWindow()
        {
            InitializeComponent();
            playlist = new Playlist(playlistBox);
            mediaModel = new MediaModel(mediaGrid.Margin);
            tool = new Tool();
            mediaModel.timerVideo.Tick += new EventHandler(timer_tick);
            volumeSlider.Maximum = 1;
            volumeSlider.Value = 1;
        }

        private void playlistBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
        }


        private void playlistBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string file in files)
                playlist.add(new PlaylistItem(file));                
        }

        private void playMedia()
        {
            if (playlist.isEmpty())
                return;
            timeSlider.Value = 0;
            mediaModel.isPaused = false;
            mediaModel.isStopped = false;
            playButton.Source = new BitmapImage(new Uri(tool.imgPath + "pause.png"));
            PlaylistItem item = playlist.getSelectedItem();
            media.Source = new Uri(item.path);
            coverImage.Source = item.coverImage;
            artistLabel.Content = item.artist;
            albumLabel.Content = item.album;
            titleLabel.Content = item.title;
            media.Play();
            mediaModel.timerVideo.Start();
        }

        private void playButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (playlistBox.Items.IsEmpty)
                return;
            if (mediaModel.isStopped == true)
                playMedia();
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
                this.volumeSlider.Value = 0;
                mediaModel.isMuted = true;
                speakerButton.Source = new BitmapImage(new Uri(tool.imgPath + "mute.png"));
            }
            else
            {
                this.media.Volume = mediaModel.volume;
                mediaModel.isMuted = false;
                speakerButton.Source = new BitmapImage(new Uri(tool.imgPath + "speaker.png"));
                this.volumeSlider.Value = mediaModel.volume;
            }
        }

        private void timer_tick(Object sender, EventArgs e)
        {
            timeSlider.Value = media.Position.TotalSeconds;
            timeLabel.Content = ((int)media.Position.TotalMinutes).ToString() + ":" + (((int)media.Position.TotalSeconds) % 60).ToString();
        }

        private void timeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan ts = TimeSpan.FromSeconds(e.NewValue);
            this.media.Position = ts;
        }

        private void media_mediaOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.media.NaturalDuration.HasTimeSpan)
                {
                    TimeSpan ts = TimeSpan.FromSeconds(this.media.NaturalDuration.TimeSpan.TotalSeconds);
                    timeSlider.Maximum = ts.TotalSeconds;
                }
            }
            catch
            {
                MessageBox.Show("Le contenu que vous souhaiter ouvrir n'est pas compatible avec notre logiciel");
            }
          
        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.media.Volume = volumeSlider.Value;
            if (mediaModel.isMuted && volumeSlider.Value != 0)
            {
                mediaModel.isMuted = false;
                speakerButton.Source = new BitmapImage(new Uri(tool.imgPath + "speaker.png"));
            }
        }

        private void playlistBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            playlist.updateSelection();
            playMedia();
        }

        private void stopMedia()
        {
            this.media.Stop();
            mediaModel.isStopped = true;
            mediaModel.isPaused = false;
            playButton.Source = new BitmapImage(new Uri(tool.imgPath + "play.png"));
        }

        private void stopButton_click(object sender, MouseButtonEventArgs e)
        {
            stopMedia();
        }

        private void nextButton_Click(object sender, MouseButtonEventArgs e)
        {
            playlist.selectNextItem();
            playMedia();
        }

        private void deletePlaylistButton_Click(object sender, MouseButtonEventArgs e)
        {
            stopMedia();
            playlist.clear();
        }

        private void openButton_Click(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog ofd;
            
            ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media(*.*)|*.*";
            ofd.ShowDialog();

            try
            {
                playlist.add(new PlaylistItem(ofd.FileName));
                playlist.selectLast();
                playMedia();
            } catch
            {
                new NullReferenceException("Error");
            }
        }

        private void playlistBox_Select(object sender, MouseButtonEventArgs e)
        {
            playlist.updateSelection();
        }

        private void media_mediaEnded(object sender, RoutedEventArgs e)
        {
            if (playlist.isFinish() && playlist.getRepeatMode() > 0)
            {
                playlist.selectNextItem();
                playMedia();
            }
            else if (!playlist.isFinish())
            {
                playlist.selectNextItem();
                playMedia();
            }
        }

        private void prevButton_Click(object sender, MouseButtonEventArgs e)
        {
            playMedia();
        }

        private void prevButton_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
                playlist.selectPrevItem();
        }

        private void shuffleButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (playlist.isRandom)
            {
                playlist.isRandom = false;
                shuffleButton.Source = new BitmapImage(new Uri(tool.imgPath + "no-shuffle.png"));
            }
            else
            {
                playlist.isRandom = true;
                shuffleButton.Source = new BitmapImage(new Uri(tool.imgPath + "shuffle.png"));
            }
        }

        private void repeatButton_Click(object sender, MouseButtonEventArgs e)
        {
            repeatButton.Source = new BitmapImage(new Uri(tool.imgPath + playlist.getNextRepeat()));
        }

        private void saveButton_Click(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML-File | *.xml";
            sfd.Title = "Sauvegarder votre playlist";
            sfd.ShowDialog();
            if (sfd.FileName != "")
                Serializer.savePlaylist(playlist.list, sfd.FileName);
        }

        private void openPlaylistButton_Click(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog ofd;

            ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.Title = "Ouvrir une playlist";
            ofd.Filter = "XML-File | *.xml";
            ofd.ShowDialog();
            if (ofd.FileName != "")
            {
                List<PlaylistItem> playlistLoad = Serializer.getPlaylist(ofd.FileName);
                stopMedia();
                playlist.changePlaylist(playlistLoad);
                playMedia();
            }
        }

        private void fullscreenButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (!mediaModel.isFullscreen)
            {
                playlistMenuGrid.Visibility = Visibility.Hidden;
                Thickness newMediaMarginFullscreen = mediaGrid.Margin;
                newMediaMarginFullscreen.Right = windowsGrid.Margin.Right;
                mediaGrid.Margin = newMediaMarginFullscreen;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                mediaModel.isFullscreen = true;
                fullscreenButton.Source = new BitmapImage(new Uri(tool.imgPath + "minimize.png"));
            }
            else
            {
                playlistMenuGrid.Visibility = Visibility.Visible;
                mediaGrid.Margin = mediaModel.margin;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                mediaModel.isFullscreen = false;
                fullscreenButton.Source = new BitmapImage(new Uri(tool.imgPath + "maximize.png"));
            }
        }
    }
}
