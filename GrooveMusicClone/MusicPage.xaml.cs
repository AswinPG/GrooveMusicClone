using GrooveMusicClone.Interfaces;
using GrooveMusicClone.Models;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace GrooveMusicClone
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicPage : ContentPage
    {
        private double posval = 0;
        PermissionStatus status;
        PermissionStatus status2;
        IPlaylistManager playlistManager;
        ObservableCollection<Song> songs { get; set; }
        public MusicPage()
        {
            InitializeComponent();
            playlistManager = DependencyService.Get<IPlaylistManager>();
            QueueData.musicPage = this;
            songs = new ObservableCollection<Song>() { };
            MainCollectionView.ItemsSource = songs;
            string path = Path.Combine(FileSystem.AppDataDirectory, "SongsData");
            CheckPermisions(path);
            if (status==PermissionStatus.Granted && status2 == PermissionStatus.Granted)
            {
                //AccessList(Path);
            }
            else
            {
                CheckPermisions(path);

            }
            
            
        }

        public void AccessList(string path)
        {
            if (System.IO.File.Exists(path))
            {
                try
                {
                    string jsondata = System.IO.File.ReadAllText(Path.Combine(FileSystem.AppDataDirectory, "SongsData"));
                    songs = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Song>>(jsondata);
                    MainCollectionView.ItemsSource = songs;
                    GetData();
                }
                catch (Exception t)
                {

                }
            }
            else
            {
                GetData();
            }
        }

        public async void CheckPermisions( string path)
        {
            try
            {
                status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                status2 = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.StorageRead>();
                }
                if (status != PermissionStatus.Granted)
                {
                    status2 = await Permissions.RequestAsync<Permissions.StorageWrite>();
                }
                AccessList(path);

            }
            catch(Exception j)
            {

            }
            

        }

        private void GetData()
        {
            List<string> mylist = (List<string>)DependencyService.Get<IMyfile>().GetFileLocation();
            try
            {
                playlistManager.GetAllSongs(songs);
                //string artists = "";
                //songs.Clear();
                //for (int i = 0; i < mylist.Count; i++)
                //{
                //    var tfile = TagLib.File.Create(mylist[i]);
                //    artists = "";
                //    for (int c = 0; c < tfile.Tag.Artists.Count(); c++)
                //    {
                //        artists = artists + tfile.Tag.Artists[c];
                //    }
                //    Song song = new Song(tfile.Tag.Pictures[0].Data.Count)
                //    {
                //        Album = tfile.Tag.Album,
                //        Index = i,
                //        Artist = artists,
                //        Duration = tfile.Properties.Duration.ToString().Substring(4, 4),
                //        Img = tfile.Tag.Pictures[0].Data.ToArray(),
                //        Title = tfile.Tag.Title,
                //        Path = mylist[i]
                //    };
                //    songs.Add(song);
                //}

                //MainCollectionView.ItemsSource = songs;
                //string jsondata = Newtonsoft.Json.JsonConvert.SerializeObject(songs);
                //System.IO.File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, "SongsData"), jsondata);


            }
            catch (Exception t)
            {

            }


            //var listView = new ListView();
            //if (mylist != null)
            //{
            //    //listView.ItemsSource = mylist;
            //}

            //Content = listView;
        }

        private async void MainCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainCollectionView.SelectedItem != null)
            {
                //await Navigation.PushAsync());
                Song song = (Song)e.CurrentSelection[0];
                await Navigation.PushAsync(new PlayerPage(song.Index, songs));
            }

            MainCollectionView.SelectedItem = null;

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            GetData();
        }



        private void MainSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (MainSlider.Value != QueueData.Player.player.Duration)
                QueueData.Player.player.Seek(MainSlider.Value);
        }
        public bool UpdatePosition()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        ElapsedLabel.Text = new TimeSpan(0, 0, (int)QueueData.Player.player.CurrentPosition).ToString().Substring(4, 4);

                    }
                    catch(Exception n) { }
                });


                MainSlider.ValueChanged -= MainSlider_ValueChanged;
                MainSlider.Value = QueueData.Player.player.CurrentPosition;
                MainSlider.ValueChanged += MainSlider_ValueChanged;

                return QueueData.Player.player.IsPlaying;
            }
            catch (Exception y)
            {
                return false;
            }

            
        }
        private void Previous(object sender, EventArgs e)
        {
            QueueData.Player.PreviousTapped(sender, e);
            
        }
        public void RefreshView()
        {
            TitleLabel.Text = QueueData.Player.PlayingTitle;
            BackImage.Source = QueueData.Player.image;
            MainSlider.Maximum = QueueData.Player.Duration;
            TotalLabel.Text = QueueData.Player.TotTime;
        }

        public async void PlayTapped(object sender, EventArgs e)
        {
            if (QueueData.Player.player.IsPlaying)
            {
                PlayPauseSvg.Source = "Play.svg";

                QueueData.Player.player.Pause();
            }

            else
            {
                PlayPauseSvg.Source = "Pause.svg";
                QueueData.Player.player.Play();

                

                Device.StartTimer(TimeSpan.FromSeconds(0.5), UpdatePosition);


            }
        }

        private void Next(object sender, EventArgs e)
        {
            QueueData.Player.NextTapped(sender, e);
        }

        private void Shuffle(object sender, EventArgs e)
        {
            QueueData.Player.ShuffleTapped(sender, e);
            TitleLabel.Text = QueueData.Player.PlayingTitle;
            BackImage.Source = QueueData.Player.image;
        }

        private void Repeat(object sender, EventArgs e)
        {
            QueueData.Player.RepeatTapped(sender, e);
            TitleLabel.Text = QueueData.Player.PlayingTitle;
            BackImage.Source = QueueData.Player.image;
        }

        private void TapGestureRecognizer_Tapped_5(object sender, EventArgs e)
        {
            QueueData.Player.PreviousTapped(sender, e);
            TitleLabel.Text = QueueData.Player.PlayingTitle;
            BackImage.Source = QueueData.Player.image;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                //CrossMediaManager.Current.PositionChanged += Current_PositionChanged;
                //CrossMediaManager.Current.StateChanged += Current_StateChanged;
                Device.StartTimer(TimeSpan.FromSeconds(0.5), UpdatePosition);
                Device.BeginInvokeOnMainThread(() => {
                    try
                    {
                        TitleLabel.Text = QueueData.Player.PlayingTitle;
                        TotalLabel.Text = QueueData.Player.TotTime;
                        BackImage.Source = QueueData.Player.image;
                        ControlGrid.IsVisible = true;
                        
                    }
                    catch (Exception n)
                    {
                        ControlGrid.IsVisible = false;
                    }

                });

            }
            catch
            {

            }
        }

        //private void Current_StateChanged(object sender, MediaManager.Playback.StateChangedEventArgs e)
        //{
        //    QueueData.Player.Current_StateChanged(sender, e);
        //}

        //private void Current_PositionChanged(object sender, MediaManager.Playback.PositionChangedEventArgs e)
        //{
        //    try
        //    {
        //        posval = CrossMediaManager.Current.Position.TotalSeconds / CrossMediaManager.Current.Duration.TotalSeconds;

        //        if (posval > 0 || posval < 1)
        //        {

        //            Device.BeginInvokeOnMainThread(() =>
        //            {
        //                MainSlider.Value = posval;
        //                ElapsedLabel.Text = CrossMediaManager.Current.Position.ToString().Substring(4, 4);
        //            });

        //        }
        //    }
        //    catch(Exception w)
        //    {

        //    }


        //}

        private async void NavigatePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(QueueData.Player);
        }
    }
}