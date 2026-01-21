using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EntregaCiclo1.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlX.XDevAPI;
using Newtonsoft.Json;

namespace EntregaCiclo1.Pages
{
    // Clase para los objetos que representan cada item existente en la tienda
    public class ItemInfo
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int id_tipo { get; set; }
        public string tipo { get; set; }
        public string descrip { get; set; }
        public int id_lootbox { get; set; }
    }

    public class LootboxCarrito
    {
        public int id_LB { get; set; }
        public string nombre_LB { get; set; }
        public int cantidad { get; set; }
        public int precio { get; set; }
    }

    public class SorteosTecModel : PageModel
    {
        // Lista para almacenar los items de la tienda
        public List<ItemInfo> Items { get; set; }

        // Lista para almacenar las lootboxes en el carrito
        public List<LootboxCarrito> lootboxesEnCarrito { get; set; }

        private HttpClient client = new HttpClient(); // cliente para usar la API

        public SorteosTecModel()
        {
            client.BaseAddress = new Uri("https://localhost:7222/"); // dirección para las solicitudes
            client.DefaultRequestHeaders.Accept.Clear(); // reiniciar encabezado de la solicitud
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // crear el nuevo encabezado
            Items = new List<ItemInfo>();
            lootboxesEnCarrito = new List<LootboxCarrito>();
        }

        // Función OnGet (solo se ejecuta una vez, al ingresar a la página)
        public async Task<IActionResult> OnGetAsync()
        {
            Items = await RunAsync();

            string itemsJson = JsonConvert.SerializeObject(Items);
            HttpContext.Session.SetString("ItemsEnSesion", itemsJson);

            lootboxesEnCarrito = await RunAsync2();

            string itemsJson2 = JsonConvert.SerializeObject(lootboxesEnCarrito);
            HttpContext.Session.SetString("LootboxesEnCarritoEnSession", itemsJson2);

            return Page();
        }

        private async Task<List<ItemInfo>> RunAsync()
        {
            try
            {
                // solicitud GET a la API
                HttpResponseMessage response = await client.GetAsync("entidades/Usuarios/Items");

                if (response.IsSuccessStatusCode)  // si la solicitud fue exitosa
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();  // leer la respuesta
                    return JsonConvert.DeserializeObject<List<ItemInfo>>(apiResponse); // deserializar la respuesta
                }
                else // si la solicitud no fue exitosa
                {
                    // Manejo adecuado del error
                    throw new HttpRequestException($"Error retrieving data from API. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<ItemInfo>();
            }

        }

        private async Task<List<LootboxCarrito>> RunAsync2()
        {
            try
            {
                // solicitud GET a la API
                HttpResponseMessage response = await client.GetAsync("entidades/Usuarios/LootboxesCarrito");

                if (response.IsSuccessStatusCode)  // si la solicitud fue exitosa
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();  // leer la respuesta
                    return JsonConvert.DeserializeObject<List<LootboxCarrito>>(apiResponse); // deserializar la respuesta
                }
                else // si la solicitud no fue exitosa
                {
                    // Manejo adecuado del error
                    throw new HttpRequestException($"Error retrieving data from API. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<LootboxCarrito>();
            }

        }
    }
}
