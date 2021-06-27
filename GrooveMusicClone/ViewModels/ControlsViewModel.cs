using Android.Graphics;
using GrooveMusicClone.ViewModels;
using MediaManager;
using MediaManager.Playback;
using MediaManager.Queue;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace GrooveMusicClone.Views
{
    public class ControlsViewModel : BaseViewModel
    {
        bool playing;
        public bool Playing 
        { 
            get { return playing; }
            set { playing = value; NotifyPropertyChanged(); }
        }
        string title;
        public string Title
        {
            get { return title; }
            set { title = value; NotifyPropertyChanged(); }
        }
        string album;
        public string Album
        {
            get { return album; }
            set { album = value; NotifyPropertyChanged(); }
        }
        string artist;
        public string Artist
        {
            get { return artist; }
            set { artist = value; NotifyPropertyChanged(); }
        }
        string duration;
        public string Duration
        {
            get { return duration; }
            set { duration = value; NotifyPropertyChanged(); }
        }
        double durationinseconds;
        public double DurationInSeconds
        {
            get { return durationinseconds; }
            set { durationinseconds = value; NotifyPropertyChanged(); }
        }
        double currentPosition;
        public double CurrentPosition
        {
            get { return currentPosition; }
            set { currentPosition = value; NotifyPropertyChanged(); }
        }
        string currentPositionString;
        public string CurrentPositionString
        {
            get { return currentPositionString; }
            set { currentPositionString = value; NotifyPropertyChanged(); }
        }
        ImageSource albumArt;
        public ImageSource AlbumArt
        {
            get { return albumArt; }
            set { albumArt = value; NotifyPropertyChanged(); }
        }
        ShuffleMode shuffle;
        public ShuffleMode Shuffle
        {
            get { return shuffle; }
            set { shuffle = value; NotifyPropertyChanged(); }
        }
        RepeatMode repeat;
        public RepeatMode Repeat
        {
            get { return repeat; }
            set { repeat = value; NotifyPropertyChanged(); }
        }
        public AsyncCommand PlayOrPauseCommand { get; set; }
        public AsyncCommand PlayNextCommand { get; set; }
        public AsyncCommand PlayPreviousCommand { get; set; }
        public Command ShuffleCommand { get; set; }
        public Command RepeatCommand { get; set; }
        public ControlsViewModel()
        {
            Play();
            CurrentPosition = 0;
            DurationInSeconds = 1;
            Duration = "--:--";

            PlayOrPauseCommand = new AsyncCommand(PlayorPause, allowsMultipleExecutions: false);
            PlayPreviousCommand = new AsyncCommand(PlayPrevious, allowsMultipleExecutions: false);
            PlayNextCommand = new AsyncCommand(PlayNext, allowsMultipleExecutions: false);
            ShuffleCommand = new Command(ToggleShuffle);
            RepeatCommand = new Command(ToggleRepeat);

            Playing = CrossMediaManager.Current.IsPlaying();
            CrossMediaManager.Current.StateChanged += Current_StateChanged;
            CrossMediaManager.Current.MediaItemChanged += Current_MediaItemChanged;
            CrossMediaManager.Current.PropertyChanged += Current_PropertyChanged;
            CrossMediaManager.Current.PositionChanged += Current_PositionChanged;
            Repeat = CrossMediaManager.Current.RepeatMode;
            Shuffle = CrossMediaManager.Current.ShuffleMode;
        }

        private void Current_PositionChanged(object sender, MediaManager.Playback.PositionChangedEventArgs e)
        {
            CurrentPosition = CrossMediaManager.Current.Position.TotalSeconds;
            CurrentPositionString = CrossMediaManager.Current.Position.ToString().Substring(3, 5);


        }

        private void Current_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                var x = e.PropertyName;
                switch (e.PropertyName)
                {
                    case "Duration":
                        Duration = CrossMediaManager.Current.Queue.Current.Duration.ToString().Substring(3, 5);
                        DurationInSeconds = CrossMediaManager.Current.Queue.Current.Duration.TotalSeconds;
                        break;
                }
            }
            catch(Exception j)
            {

            }
            
        }
        
        public async void Seek(int seconds)
        {
            await CrossMediaManager.Current.SeekTo(new System.TimeSpan(0, 0, seconds));
        }
        void ToggleShuffle()
        {
            CrossMediaManager.Current.ToggleShuffle();
            Shuffle = CrossMediaManager.Current.ShuffleMode;
        }
        async Task PlayNext()
        {
            bool success = await CrossMediaManager.Current.PlayNext();
        }
        async Task PlayPrevious()
        {
            await CrossMediaManager.Current.PlayPrevious();
        }
        void ToggleRepeat()
        {
            CrossMediaManager.Current.ToggleRepeat();
            Repeat = CrossMediaManager.Current.RepeatMode;
        }

        async void Play()
        {
            var media = await CrossMediaManager.Current.Play("https://oxydebug.sgp1.cdn.digitaloceanspaces.com/04%20-%20Dangal%20-%20Title%20Song%20-%20DownloadMing.SE.mp3");
            //media.MetadataUpdated += Media_MetadataUpdated;
        }


        private void Media_MetadataUpdated(object sender, MediaManager.Media.MetadataChangedEventArgs e)
        {
            Title = e.MediaItem.Title;
            Album = e.MediaItem.Album;
            Artist = e.MediaItem.Artist;
            var uuu = e.MediaItem.AlbumImageUri;
            var uuuu = e.MediaItem.AlbumImage;
            
        }

        private async void Current_MediaItemChanged(object sender, MediaManager.Media.MediaItemEventArgs e)
        {
            await Task.Delay(50);
            Title = e.MediaItem.DisplayTitle;
            Album = e.MediaItem.Album;
            Artist = e.MediaItem.Artist;
            var bitmap = ((Bitmap)e.MediaItem.DisplayImage);
            if(bitmap != null)
            {
                AlbumArt = ImageSource.FromStream(() =>
                {
                    MemoryStream ms = new MemoryStream();
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                    ms.Seek(0L, SeekOrigin.Begin);
                    return ms;
                });
            }
        }

        private void Current_StateChanged(object sender, MediaManager.Playback.StateChangedEventArgs e)
        {
            switch (e.State)
            {
                case MediaManager.Player.MediaPlayerState.Playing:
                    Playing = true;
                    break;
                default:
                    Playing = false;
                    break;
            }
        }

        async Task PlayorPause()
        {
            await CrossMediaManager.Current.PlayPause();
            Playing = CrossMediaManager.Current.IsPlaying();
        }
    }
}