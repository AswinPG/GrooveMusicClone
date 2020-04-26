using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrooveMusicClone.Models
{
    public static class QueueData
    {
        public static List<Song> CurrentList { get; set; }
        //public static IList<string> CurrentQueue { get; set; }
        public static ISimpleAudioPlayer player { get; set; }
        public static PlayerPage Player { get; set; }
        public static MusicPage musicPage { get; set; }
        public static void Init()
        {
            CurrentList = new List<Song> { };
            //CurrentQueue = new List<string> { };
            player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
        }
    }
}
