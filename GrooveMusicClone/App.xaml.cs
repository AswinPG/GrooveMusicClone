using GrooveMusicClone.Helpers;
using GrooveMusicClone.Models;
using GrooveMusicClone.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GrooveMusicClone
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ObjectsHelper.InitObjects();
            MainPage = new NavigationPage(new SongsListPage());
            QueueData.Init();
            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
