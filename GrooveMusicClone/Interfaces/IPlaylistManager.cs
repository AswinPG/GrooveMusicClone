using GrooveMusicClone.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace GrooveMusicClone.Interfaces
{
    public interface IPlaylistManager
    {
        Task AddToPlaylist(Playlist playlist, Song song);

        Playlist CreatePlaylist(string name);

        IList<Playlist> GetPlaylists();

        Task<IList<Song>> GetPlaylistSongs(ulong playlistId);

        Task<IList<Song>> GetAllSongs();
        Task<IList<Song>> GetAllSongs(ObservableCollection<Song> songs);
    }
}
