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
            //Instanciamos nuestro ViewModel
            await ViewModel.LoadCharacters();
            //Con la instancia del ViewModel invocamos el método LoadCharacters para cargar todos los personajes en nuestra
            //ObservableCollection (Characters)
            this.BindingContext = ViewModel;
            //Y ahora le decimos que el BindingContext del View es nuestro ViewModel. Es importante el orden, ya que si definimos
            //antes el BindingContext y luego invocamos el método LoadCharacters, si necesitara más adelante visualizar estos elementos
            //en nuestro View tendría problemas, ya que cuando se establezca el BindingContext todavía no habría nada en nuestra 
            //ObservableCollection.

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
