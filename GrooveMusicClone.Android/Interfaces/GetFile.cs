using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GrooveMusicClone.Droid.Interfaces;
using GrooveMusicClone.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(GetFile))]
namespace GrooveMusicClone.Droid.Interfaces
{
    internal class GetFile : IMyfile
    {
        public IList<string> GetFileLocation()
        {
            var files = new List<string>();
            foreach (var dir in RootDirectory())
            {
                if (Directory.Exists(dir))
                {
                    var file = Directory.EnumerateFiles(dir).ToList<string>();
                    file.ForEach(f => { if (f.EndsWith("mp3")) files.Add(f); });
                }
            }
            return files;
        }


        public IList<string> RootDirectory()
        {
            var Pathlist = new List<string>();
            try
            {
                

                Pathlist.Add(Android.OS.Environment.GetExternalStoragePublicDirectory(
                 Android.OS.Environment.DirectoryDocuments).AbsolutePath);
                Pathlist.Add(Android.OS.Environment.GetExternalStoragePublicDirectory(
                          Android.OS.Environment.DirectoryDownloads).AbsolutePath);
                Pathlist.Add(Android.OS.Environment.GetExternalStoragePublicDirectory(
                        Android.OS.Environment.DirectoryMusic).AbsolutePath);
                for (int i = 0;i<Pathlist.Count;i++)
                {
                    var temp = new List<string>();
                    if (Directory.Exists(Pathlist[i]))
                    {
                        temp.AddRange(Directory.EnumerateDirectories(Pathlist[i]).ToList());
                    }
                    Pathlist.AddRange(temp);
                }
                
                return Pathlist;
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
    
