using GrooveMusicClone.Models;
using MediaManager;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GrooveMusicClone
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerPage : ContentPage
    {
        private double posval;
        int Current = 0;
        int TotalCount;
        public ImageSource image { get; set; }
        public string PlayingTitle { get; set; }
        public string TotTime { get; set; }
        Song PreviousSong { get; set; }
        Song NextSong { get; set; }
        Song CurrentSong { get; set; }
        //ISimpleAudioPlayer player { get; set; }
        public TimeSpan Pausetime { get; private set; }

        public bool PauseSkip = false;
        public bool Pausedornot { get; private set; }
        private double HalfWidth;
        private int x;
        private int Songindex;
        public List<Song> Songs = new List<Song> { };
        //public ISimpleAudioPlayer player;
        public double Duration;
        Stopwatch stopwatch { get; set; }


        public PlayerPage(int songindex, ObservableCollection<Song> songList)
        {
            InitializeComponent();
            PlayTapped(null, null);
            CrossMediaManager.Current.PositionChanged += Current_PositionChanged;
            CrossMediaManager.Current.StateChanged += Current_StateChanged;
            //PauseSkip = false;
            TotalCount = songList.Count();
            Songindex = songindex;
            TranslateOriginal();
            HalfWidth = Application.Current.MainPage.Width / 2 + Application.Current.MainPage.Width / 8;

            for (int i = songindex; i < songList.Count; i++)
            {
                songList[i].Index = Songs.Count;
                Songs.Add(songList[i]);
                if (i == songList.Count - 1)
                {
                    for (int c = 0; c < songindex; c++)
                    {
                        songList[c].Index = Songs.Count;
                        Songs.Add(songList[c]);
                    }
                }
            }
            
            Song song = Songs[0];
            CurrentSong = song;
            NextSong = Songs[1];

            image = song.imageSource();
            BackImage.Source = image;
            MainImage.Source = image;
            TitleLabel.Text = song.Title;
            PlayingTitle = song.Title;
            AlbumLabel.Text = song.Album;
            ArtistLabel.Text = song.Artist;
            TotalLabel.Text = song.Duration.ToString();
            TotTime = song.Duration.ToString();
            //QueueData.Player = this;
            //QueueData.CurrentList = Songs;
            //player = QueueData.player;
            //player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            //player.Load(GetStreamFromFile(song.Path));

            
            LoadView();
            Pausedornot = false;
            Pausetime = new TimeSpan(0, 0, 0);

            MainCollectionView.ItemsSource = Songs;
            
            stopwatch = new Stopwatch();

        }

        private void Current_StateChanged(object sender, MediaManager.Playback.StateChangedEventArgs e)
        {
            if(CrossMediaManager.Current.State == MediaManager.Player.MediaPlayerState.Playing)
                MainSlider.Maximum = CrossMediaManager.Current.Duration.TotalMilliseconds;
        }

        private void Current_PositionChanged(object sender, MediaManager.Playback.PositionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        Stream GetStreamFromFile(string filename)
        {

            Stream stream = File.OpenRead(filename);
            return stream;
        }




        public bool UpdatePosition()
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                ElapsedLabel.Text = CrossMediaManager.Current.Position.ToString().Substring(4, 4);
            });


            MainSlider.ValueChanged -= SliderPostionValueChanged;
            MainSlider.Value = CrossMediaManager.Current.Position.TotalMilliseconds;
            MainSlider.ValueChanged += SliderPostionValueChanged;

            return CrossMediaManager.Current.IsPlaying();
        }

        public void SliderPostionValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (MainSlider.Value != CrossMediaManager.Current.Duration.TotalMilliseconds)
                CrossMediaManager.Current.SeekTo(new TimeSpan(0, 0, Convert.ToInt32(MainSlider.Value / 1000)));
        }


        public async void PreviousTapped(object sender, EventArgs e)
        {
            try
            {

                Current--;
                await CrossMediaManager.Current.Play(Songs[Current].Path);
                PlayTapped(null, null);
                LoadView();
                    

            }
            catch (Exception u)
            {
                Current++;
                LoadView();
            }

        }
        public async void PlayPreviousitem()
        {
            try
            {
                Current--;
                await CrossMediaManager.Current.Play(Songs[Current].Path);
                PlayTapped(null, null);
                LoadView();
            }
            catch (Exception t)
            {
                Current++;
                LoadView();
                //await CrossMediaManager.Current.Play(Songs[Current]);
            }
        }
        public async void NextTapped(object sender, EventArgs e)
        {
            try
            {
                //await CrossMediaManager.Current.Play();
                await Task.Delay(500);
                Current++;
                await CrossMediaManager.Current.Play(Songs[Current].Path);
                PlayTapped(null, null);
                
                LoadView();


            }
            catch (Exception u)
            {
                Current--;
                //await CrossMediaManager.Current.Play(Songs[Current]);
                LoadView();
            }
        }
        public async void PlayTapped(object sender, EventArgs e)
        {
            if (CrossMediaManager.Current.IsPlaying())
            {
                PlayPauseSvg.Source = "Play.svg";

                await CrossMediaManager.Current.Pause();
            }

            else
            {
                PlayPauseSvg.Source = "Pause.svg";
                await CrossMediaManager.Current.Play();

                
                Duration = CrossMediaManager.Current.Duration.TotalMilliseconds;
                Populate(Current);
                //Device.StartTimer(TimeSpan.FromSeconds(0.5), UpdatePosition);


            }
        }



        public void Populate(int path)
        {
            Song song = Songs[path];
            image = song.imageSource();
            BackImage.Source = image;
            MainImage.Source = image;
            TitleLabel.Text = song.Title;
            PlayingTitle = song.Title;
            AlbumLabel.Text = song.Album;
            ArtistLabel.Text = song.Artist;
            TotalLabel.Text = song.Duration.ToString();
            TotTime = song.Duration.ToString();
            try
            {
                PreviousSong = Songs[path - 1];
            }
            catch (Exception e)
            {
                PreviousSong = null;
            }
            try
            {
                CurrentSong = Songs[path];
            }
            catch (Exception e)
            {

            }
            try
            {
                QueueData.musicPage.RefreshView();
                NextSong = Songs[path + 1];
            }
            catch (Exception e)
            {
                NextSong = null;
            }



        }

        public void ShuffleTapped(object sender, EventArgs e)
        {
            ShufleBox.IsVisible = true;
        }

        public void RepeatTapped(object sender, EventArgs e)
        {
            RepeatBox.IsVisible = true;
        }

        public async void TranslateOriginal()
        {
            MainGrid.IsVisible = false;
            MainCollectionView.FadeTo(0);
            MainCollectionView.IsVisible = false;
            MainCollectionView.FadeTo(1);

            MainGrid.TranslationX = 0;
            MainGrid.TranslateTo(0, 0);
            ControlsLayout.TranslateTo(0, 0);
            await Task.Delay(400);
            MainGrid2.TranslationX = Application.Current.MainPage.Width;
            MainGrid1.TranslationX = -(Application.Current.MainPage.Width);
            MainGrid.IsVisible = true;

            //CollectionStack.TranslationY = Application.Current.MainPage.Height;
        }
        public async void TranslateOriginalAnimate()
        {
            MainGrid2.TranslateTo(Application.Current.MainPage.Width, 0);
            MainGrid1.TranslateTo(-(Application.Current.MainPage.Width), 0);
            //CollectionStack.TranslationY = Application.Current.MainPage.Height;
            await MainGrid.TranslateTo(0, 0);
        }
        //public async Task<bool> AwaiterF()
        //{
        //    await Task.Delay(400);
        //    return true;
        //}
        public async void LoadView()
        {
            await Task.Delay(700);
            if (PreviousSong != null)
            {
                ImageSource image = PreviousSong.imageSource();
                BackImage1.Source = image;
                MainImage1.Source = image;
            }
            if (NextSong != null)
            {
                ImageSource image = NextSong.imageSource();
                BackImage2.Source = image;
                MainImage2.Source = image;
            }
        }

        public async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            stopwatch.Start();
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                    if (Math.Abs(e.TotalX) > Math.Abs(e.TotalY) && MainGrid.TranslationY == 0)
                    {
                        MainGrid.TranslationX =
                        Math.Min(MainGrid.TranslationX + e.TotalX, Application.Current.MainPage.Width);
                        MainGrid1.TranslationX =
                        Math.Min(MainGrid1.TranslationX + e.TotalX, Application.Current.MainPage.Width);
                        MainGrid2.TranslationX =
                        Math.Max(MainGrid2.TranslationX + e.TotalX, 0);
                        //Content.TranslationY =
                        //  Math.Max(Math.Min(0, y + e.TotalY), -Math.Abs(Content.Height - App.ScreenHeight));
                        break;
                    }
                    else if (MainGrid.TranslationX == 0 && e.TotalY < 0)
                    {
                        MainGrid.TranslationY =
                        Math.Max(MainGrid.TranslationY + e.TotalY, -Application.Current.MainPage.Height);
                        break;
                    }
                    break;



                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    stopwatch.Stop();
                    double time = Convert.ToDouble(stopwatch.ElapsedMilliseconds);
                    double speedx = MainGrid.TranslationX / time;
                    double speedy = MainGrid.TranslationY / time;

                    stopwatch.Reset();
                    if (-MainGrid.TranslationX > (HalfWidth) && (MainGrid.TranslationX < 0) || speedx < -1)
                    {
                        NextTapped(null, null);
                        MainGrid.TranslateTo(-(Application.Current.MainPage.Width), 0);

                        await MainGrid2.TranslateTo(0, 0);
                        TranslateOriginal();

                    }
                    else if (MainGrid.TranslationX > (HalfWidth) || Math.Abs(speedx) > 1)
                    {
                        PlayPreviousitem();
                        MainGrid.TranslateTo(Application.Current.MainPage.Width, 0);

                        await MainGrid1.TranslateTo(0, 0);
                        TranslateOriginal();

                    }
                    else if (-MainGrid.TranslationY > App.Current.MainPage.Height / 4 || speedy < -1)
                    {
                        MainGrid.TranslateTo(0, ControlsLayout.Height - App.Current.MainPage.Height);
                        await ControlsLayout.TranslateTo(0, ControlsLayout.Height - App.Current.MainPage.Height);
                        UpSvg.RotateTo(180);
                        MainCollectionView.Margin = new Thickness(0, ControlsLayout.Height, 0, 0);
                        MainCollectionView.IsVisible = true;
                        //CollectionStack.TranslateTo(0,-((ControlsLayout.Height) - App.Current.MainPage.Height));
                    }
                    else
                    {
                        TranslateOriginalAnimate();
                    }
                    break;
            }
        }
        public async void MainCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //int index = Math.Abs((((Song)e.CurrentSelection[0]).Index - Songindex) % TotalCount);
            //await CrossMediaManager.Current.PlayQueueItem(index);
            //Current = index;
            //Populate(Current);
            //LoadView();
            //if (CrossMediaManager.Current.State == MediaPlayerState.Stopped)
            //{
            //    PauseSkip = true;
            //}
            //int index = Math.Abs(TotalCount % (((Song)e.CurrentSelection[0]).Index + Songindex + 1) - 1);
            Current = ((Song)e.CurrentSelection[0]).Index;
            await CrossMediaManager.Current.Play(Songs[Current].Path);
            PlayTapped(null, null);
            Populate(Current);
            LoadView();
        }

        public async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (MainCollectionView.IsVisible == true)
            {
                TranslateOriginal();
            }
            else
            {
                MainGrid.TranslateTo(0, ControlsLayout.Height - App.Current.MainPage.Height);
                await ControlsLayout.TranslateTo(0, ControlsLayout.Height - App.Current.MainPage.Height);
                UpSvg.RotateTo(180);
                MainCollectionView.Margin = new Thickness(0, ControlsLayout.Height, 0, 0);
                MainCollectionView.IsVisible = true;
            }
        }
    }

}