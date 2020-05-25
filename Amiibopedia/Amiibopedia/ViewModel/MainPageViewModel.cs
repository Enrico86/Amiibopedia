using Amiibopedia.Helpers;
using Amiibopedia.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace Amiibopedia.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<Character> Characters { get; set; }
        public ICommand SearchCommand { get; set; }

        private ObservableCollection<Amiibo> _amiibos;
        public ObservableCollection<Amiibo> Amiibos
        {
            get => _amiibos; set
            {
                _amiibos = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel()
        {
            SearchCommand = new Command(async (searchTerm) =>
            {
                IsBusy = true;
                var character = searchTerm as Character;
                if (character != null)
                {
                    string url = $"https://www.amiiboapi.com/api/amiibo/?character={character.name}";
                    var service = new HttpHelper<Amiibos>();
                    var amiibos = await service.GetRestServiceDataAsync(url);
                    Amiibos = new ObservableCollection<Amiibo>(amiibos.amiibo);
                }
                IsBusy = false;
            });
        }
        public async Task LoadCharacters()
        {
            var url = "https://www.amiiboapi.com/api/character/";
            var service = new HttpHelper<Characters>();
            var characters = await service.GetRestServiceDataAsync(url);
            Characters = new ObservableCollection<Character>(characters.amiibo);
        }
    }
}
