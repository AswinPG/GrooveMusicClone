using GrooveMusicClone.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GrooveMusicClone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongsListPage : ContentPage
    {
        public SongsListPage()
        {
            InitializeComponent();
            
            BindingContext = new SongsListViewModel();
        }

        private void MainCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}