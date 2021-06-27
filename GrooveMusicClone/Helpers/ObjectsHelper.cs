using GrooveMusicClone.Interfaces;
using GrooveMusicClone.Models;
using GrooveMusicClone.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GrooveMusicClone.Helpers
{
    public static class ObjectsHelper
    {
        public static IPlaylistManager playlistManager;
        public static ControlsViewModel Controller;
        public static List<Song> AllSongs;
        public static Song CurrentSong;
        public static void InitObjects()
        {
            playlistManager = DependencyService.Get<IPlaylistManager>();
            Controller = new ControlsViewModel();
        }
        public static void Shuffle()
        {

        }
        
    }
}
