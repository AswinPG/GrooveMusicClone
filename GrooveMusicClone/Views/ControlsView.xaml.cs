using GrooveMusicClone.Helpers;
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
    public partial class ControlsView : Grid
    {
        public static readonly BindableProperty IsOnPlayerProperty =
          BindableProperty.Create(nameof(IsOnPlayer), typeof(bool), typeof(ControlsView), true,propertyChanged: IsOnPlayerChanged);

        private static void IsOnPlayerChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //throw new NotImplementedException();
        }

        public bool IsOnPlayer
        {
            get => (bool)GetValue(IsOnPlayerProperty);
            set => SetValue(IsOnPlayerProperty, value);
        }
        ControlsViewModel _ViewModel;
        public ControlsView()
        {
            InitializeComponent();
            BindingContext = _ViewModel = ObjectsHelper.Controller;

        }
        private void MainSlider_DragCompleted(object sender, EventArgs e)
        {
            _ViewModel.Seek(Convert.ToInt32(MainSlider.Value));
        }
    }
}