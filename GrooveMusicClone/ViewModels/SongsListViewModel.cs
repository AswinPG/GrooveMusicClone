using GrooveMusicClone.Helpers;
using GrooveMusicClone.Models;
using GrooveMusicClone.ViewModels;
using MediaManager;
using MediaManager.Library;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GrooveMusicClone.ViewModels
{
    public class SongsListViewModel : BaseViewModel
    {
        public Command PlaySong { get; set; }
        public ObservableCollection<Song> AllSongs { get; set; }
        public SongsListViewModel()
        {
            AllSongs = new ObservableCollection<Song>();
            PlaySong = new Command(async (param) => await PlayListofSongs((Song)param));
            Init();
        }

        async Task PlayListofSongs(Song song)
        {
            try
            {
                CrossMediaManager.Current.Queue.Clear();
                await CrossMediaManager.Current.Play(song.Path);
                int index = AllSongs.IndexOf(song) + 1;
                await Task.Run(() =>
                {
                    for (int i = index; i < AllSongs.Count; i++)
                    {
                        Task.Delay(100);
                        CrossMediaManager.Current.Queue.Add(new MediaItem(AllSongs[i].Path));
                    }
                    for (int i = index - 2; i >= 0; i--)
                    {
                        CrossMediaManager.Current.Queue.Add(new MediaItem(AllSongs[i].Path));
                    }
                });
            }
            catch(Exception h)
            {

            }
            
        }

        public async void Init()
        {
            if (await CheckPermisions())
            {
                string path = Path.Combine(FileSystem.AppDataDirectory, "AllSongsData");
                if (File.Exists(path))
                {
                    try
                    {
                        string jsondata = File.ReadAllText(path);
                        AllSongs = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Song>>(jsondata);
                        var list = await ObjectsHelper.playlistManager.GetAllSongs(AllSongs);
                    }
                    catch (Exception t)
                    {

                    }
                }
                else
                {
                    var list = await ObjectsHelper.playlistManager.GetAllSongs(AllSongs);
                }
                string jsondatatowrite = Newtonsoft.Json.JsonConvert.SerializeObject(AllSongs);
                File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, "AllSongsData"), jsondatatowrite);
            }
            else
            {
                await Task.Delay(1000);
                await App.Current.MainPage.DisplayAlert("No Permission", "Please allow permissions to use the app", "ok");
                Init();
            }
        }


        public async Task<bool> CheckPermisions()
        {
            try
            {
                PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                PermissionStatus status2 = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.StorageRead>();
                }
                if (status2 != PermissionStatus.Granted)
                {
                    status2 = await Permissions.RequestAsync<Permissions.StorageWrite>();
                }
                if (status == PermissionStatus.Granted && status2 == PermissionStatus.Granted)
                    return true;

            }
            catch (Exception j)
            {

            }
            return false;

        }
    }
}