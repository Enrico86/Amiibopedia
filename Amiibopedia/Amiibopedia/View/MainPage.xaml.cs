using Amiibopedia.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Amiibopedia
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPageViewModel ViewModel { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel = new MainPageViewModel();
            await ViewModel.LoadCharacters();
            this.BindingContext = ViewModel;

        }

        private async void ViewCell_Appearing(object sender, EventArgs e)
        {
            var cell = sender as ViewCell;
            var view = cell.View;
            view.TranslationX = -100;
            view.Opacity = 0;

            await Task.WhenAny<bool>
                (
                    view.TranslateTo(0, 0, 500, Easing.Linear),
                    view.FadeTo(1, 500, Easing.SinIn)
                );
            
        }
    }
}
