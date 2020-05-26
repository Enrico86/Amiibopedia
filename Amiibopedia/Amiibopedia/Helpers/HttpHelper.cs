using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Amiibopedia.Helpers
{
    public class HttpHelper<T>
    //Lo primero a hacer es decir que la clase va a recibir un generico que representa nuestro modelo de datos sobre el que vamos a trabajar
    {
        public async Task <T> GetRestServiceDataAsync (string serviceAddress)
            //Creamos un método asincrono (que devolverá una tarea) que se encargará de coger los datos que nos envíe el servicio rest
            //Tendrá que recibir por parametro la Url del servicio que queramos consumir

        {
            var client = new HttpClient();
            //Creamos el clásico cliente (que consume el servicio) como una nueva instancia de la clase HttpClient.
            client.BaseAddress = new Uri(serviceAddress);
            //Definimos la propiedad BaseAddress como una nueva instancia de la clase Uri, pasandole por parametro la url del servicio
            var response = await client.GetAsync(client.BaseAddress);
            //Esta variable es la respuesta: en ella almacenamos lo que nuestro client coja (reciba) del BaseAddress
            response.EnsureSuccessStatusCode();
            //Invocamos este método para controlar que la llamada al servicio se haya realizado con éxito
            var jsonResult = await response.Content.ReadAsStringAsync();
            //creamos una variable resultado (todavía es un archivo Json) en la que almacenamos lo que hemos leido como string en el contenido 
            //de la respuesta que nos ha dado el servicio.
            var result = JsonConvert.DeserializeObject<T>(jsonResult);
            //Con la clase JsonConvert (del paquete NuGet que hemos instalado, por lo que tendremos que añadir la directiva using correspondiente)
            //deserializamos el resultado que todavía era un archivo Json en el generico <T>, el mismo generico que recibe la clase.
            return result;
            //Devolvemos el resultado deserializado
        }
    }
}
