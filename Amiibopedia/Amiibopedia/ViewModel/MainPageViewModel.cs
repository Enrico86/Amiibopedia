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
        //Esta propiedad ObservableCollection será la lista de objetos Character en la que pondremos los datos que recibamos desde la API
        //después de haberlos pedidos y deserializados.
        public ICommand SearchCommand { get; set; }
        //Primero creamos una propiedad ICommand que tendrá el mismo nombre que hemos asignado al comando de nuestra SearchBar

        private ObservableCollection<Amiibo> _amiibos;
        //Luego creamos una lista (ObservableCollection) de objetos Amiibo (acabamos de ver que la respuesta de la API es un archivo Json en la que
        //tenemos un array de objetos Amiibo). Con el HttpHelper acordemonos que deserializaremos el archivo Json convirtiendola en un 
        //generico, que en nuestro caso será un Amiibos (array de objetos Amiibo)
        public ObservableCollection<Amiibo> Amiibos
        {
            get => _amiibos; set
            {
                _amiibos = value;
                OnPropertyChanged();
                //Con el método OnPropertyChanged estamos avisando el aplicación de cuando nuestra ObservableCollection sufra un cambio (que
                //será cuando reciba nuevos datos desde la API). De esta forma podremos hacer que se visualice correctamente la información en 
                //nuestra App.
            }
        }

        public MainPageViewModel()
        {
            SearchCommand = new Command(async (searchTerm) =>
            //Y en el contructor de nuestro MainPageViewModel instanciamos nuestra propiedad SearchCommand como una instancia de la clase Command
            //que irá a ejecutar una tarea. El comando recibirá una expresión lambda con parametro el searchTerm 
            //(es decir lo que el usuario introduzca en la SearchBar del View. En el cuerpo de la expresión lambda tenemos que analizar lo que 
            //va a pasar cuando el usuario ejecute este comando (es decir cuando busque algo): y nosotros necesitamos coger lo que ha introducido 
            //el usuario y pasar este parametro al servicio Rest para que nos devuelva información del conjunto de Amiibos para ese Character.
            //Primero pero tendremos que crear una nueva clase a partir del código Json que nos devolverá la API cuando consumamos el servicio Rest.
            
            {
                IsBusy = true;
                //Definimos que sí (true) estamos ocupados en este momento, que empieza la llamada al servicio
                var character = searchTerm as Character;
                //Entonces primero creamos una variable y le asignamos lo que haya puesto en la SearchBar el usuario, pero antes lo
                //convertimos en un objeto Character (con la palabra reservada "as")
                
                if (character != null)
                //Si nuestra variable es diferente de null
                {
                    string url = $"https://www.amiiboapi.com/api/amiibo/?character={character.name}";
                    //Creamos una Url dinamica concatenando la parte estatica de la url y la propiedad name (que es un string) de nuestra variable 
                    //character (que al ser una instancia de Character puede acceder a las propiedades de esa clase)
                    var service = new HttpHelper<Amiibos>();
                    //Con nuestro HttpHelper (que ya hemos dicho que como se definió como generico puede ser cualquier clase de objetos), creamos
                    //el servicio
                    var amiibos = await service.GetRestServiceDataAsync(url);
                    //Esperamos a que se invoque y se lleve a cabo el método GetRestServiceDataAsync (que recibe por parametro la url dinamica que hemos 
                    //construido hace un momento). Con este método estamos deserializando el archivo Json.
                    Amiibos = new ObservableCollection<Amiibo>(amiibos.amiibo);
                    //Instanciamos la propiedad Amiibos (que hemos definido al principio de esta clase) como una nueva instancia de una 
                    //ObservableCollection de objetos Amiibo, y cogerá los datos de la variable amiibos (Json deserializado), en concreto dentro de la
                    //propiedad amiibo.
                }
                IsBusy = false;
                //Definimpos que ya no estamos ocupados ahora que se ha recibido respuesta y se ha generado la ObservableCollection que se mostrará 
                //en la ListView de nuestra MainPage.
            });
        }
        public async Task LoadCharacters()
        {
            var url = "https://www.amiiboapi.com/api/character/";
            //Creamos una variable url a la que vamos a hacer la llamada para cargar los personajes en nuestra lista. Es una url estatica, 
            //ya que no cambia y siempre será ésta. Podríamos no definirla estatica si por ejemplo quisieramos dejar al usuario la posibilidad
            //de escoger si quiere cargar en la lista los personajes, o el Id, etc.
            var service = new HttpHelper<Characters>();
            //Creamos el servicio para llevar a cabo su consumo. Es una instancia de la clase HttpHelper, y aquí vemos como esta instancia
            //puede recibir un genérico (es decir cualquier tipo de dato). En este caso desde la API recibireDmos un objeto de la clase Characters
            //que será un array (su nombre es amiibo) en el que estarán todos los objetos Character.
            var characters = await service.GetRestServiceDataAsync(url);
            //Finalmente con la variable service de HttpHelper podemos invocar el método GetRestServiceDataAsync que habíamos creado en la clase
            //HttpHelper. Este método tiene que recibir un string como url, y es la url que hemos definido antes (url de los personajes)
            Characters = new ObservableCollection<Character>(characters.amiibo);
            //Instanciamos la ObservableCollection que habíamos declarado antes y vamos a almacenar en esta lista lo que le pasemos por parámetro,
            //que es al array amiibo que hemos almacenado en la variable characters como parte de la información recibida desde la API.  
        }
    }
}
