using GrooveMusicClone.Models;
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

            MainPage = new NavigationPage(new MainPage());
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
