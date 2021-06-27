using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace GrooveMusicClone.Models
{
    public class Song
    {
        //public string Album { get; set; }
        //public string Title { get; set; }
        //public string Artist { get; set; }
        public int Index { get; set; }
        //public string Duration { get; set; }
        public byte[] Img { get; set; }
        public string Path { get { return Uri; } set { Uri = value; } }

        public ImageSource imageSource()
        {
            ImageSource source = ImageSource.FromStream(() => new MemoryStream(Img));
            return source;
        }
        public Song(int count)
        {
            Img = new byte[count];
        }




        public ulong Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Album { get; set; }

        public string Genre { get; set; }

        public object Artwork { get; set; }

        public double Duration { get; set; }

        //public DateTime Date { get; set; }

        public string Uri { get; set; }

        public bool HasArtwork
        {
            get
            {
                return Artwork != null && !String.IsNullOrEmpty(Artwork.ToString());
            }
        }

        public Song() { }

        public Song(Song song)
        {
            Id = song.Id;
            Title = song.Title;
            Artist = song.Artist;
            Album = song.Album;
            Genre = song.Genre;
            Artwork = song.Artwork;
            Duration = song.Duration;
            Uri = song.Uri;
        }
    }

}
