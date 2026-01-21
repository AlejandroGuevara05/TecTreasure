using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EntregaCiclo1.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace EntregaCiclo1.Pages
{
    // Clase para los objetos que representan cada lootbox del usuario
    public class LootboxInfo
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int id_compra { get; set; }
    }

    public class UsuarioInfo
    {
        public int id_usuario { get; set; }
        public string nombre_usuario { get; set; }
    }

    public class LootboxesModel : PageModel
    {
        // String para almacenar el nombre del usuario que ingresó
        public string usuarioNombre { get; set; }

        // String para almacenar el correo del usuario que ingresó
        public string usuario { get; set; }

        private HttpClient client = new HttpClient(); // cliente para usar la API

        // Lista para almacenar objetos de la clase LootboxInfo
        public List<LootboxInfo> LootboxesUsuario { get; set; }

        // Diccionario para el estado de las animaciones
        public Dictionary<int, bool> DicActivarAnimacion { get; set; } = new Dictionary<int, bool>();

        // Diccionario para saber si ya se ejecutó la animación
        public Dictionary<int, bool> nuevoDiccionario { get; set; } = new Dictionary<int, bool>();

        // Lista para almacenar los items de la tienda
        public List<ItemInfo> Items { get; set; }

        // Objeto para almacenar el item que se ganó el usuario en la lootbox
        public ItemInfo premio { get; set; }

        // Constructor de la clase
        public LootboxesModel()
        {
            client.BaseAddress = new Uri("https://localhost:7222/"); // dirección para las solicitudes
            client.DefaultRequestHeaders.Accept.Clear(); // reiniciar encabezado de la solicitud
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // crear el nuevo encabezado
            LootboxesUsuario = new List<LootboxInfo>();
            DicActivarAnimacion = new Dictionary<int, bool>();
            nuevoDiccionario = new Dictionary<int, bool>();
            Items = new List<ItemInfo>();
        }

        // Función OnGet (solo se ejecuta una vez, al ingresar a la página)
        public async Task<IActionResult> OnGetAsync()
        {

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("usuarioEnSesion")) == false)
            {
                usuario = HttpContext.Session.GetString("usuarioEnSesion");
            }

            string datosUsuario = HttpContext.Session.GetString("DatosUsuarioEnSesion");
            UsuarioInfo user = JsonConvert.DeserializeObject<UsuarioInfo>(datosUsuario);
            usuarioNombre = user.nombre_usuario;

            LootboxesUsuario = await RunAsync();
            string lootboxes = JsonConvert.SerializeObject(LootboxesUsuario);
            HttpContext.Session.SetString("Lootboxes", lootboxes);

            int counter = 0;
            foreach (var lootbox in LootboxesUsuario)
            {
                counter = counter + 1;
                int clave = counter;
                bool activacion = false;
                DicActivarAnimacion.Add(clave, activacion);
                nuevoDiccionario.Add(clave, false);
            }
            string diccionarioJson = JsonConvert.SerializeObject(DicActivarAnimacion);
            HttpContext.Session.SetString("miDiccionarioEnSesion", diccionarioJson);

            string diccionarioJson2 = JsonConvert.SerializeObject(nuevoDiccionario);
            HttpContext.Session.SetString("miDiccionarioEnSesion2", diccionarioJson2);

            string itemsJson = HttpContext.Session.GetString("ItemsEnSesion");
            Items = JsonConvert.DeserializeObject<List<ItemInfo>>(itemsJson);

            return Page();
        }

        // Función OnPost (se ejecuta cuando se manda a llamar desde el html)
        public async Task<IActionResult> OnPostAsync(int lootboxId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("usuarioEnSesion")) == false)
            {
                usuario = HttpContext.Session.GetString("usuarioEnSesion");
            }

            string datosUsuario = HttpContext.Session.GetString("DatosUsuarioEnSesion");
            UsuarioInfo user = JsonConvert.DeserializeObject<UsuarioInfo>(datosUsuario);
            usuarioNombre = user.nombre_usuario;

            string lootboxes = HttpContext.Session.GetString("Lootboxes");
            LootboxesUsuario = JsonConvert.DeserializeObject<List<LootboxInfo>>(lootboxes);

            string diccionarioJson = HttpContext.Session.GetString("miDiccionarioEnSesion");
            DicActivarAnimacion = JsonConvert.DeserializeObject<Dictionary<int, bool>>(diccionarioJson);

            if (DicActivarAnimacion.ContainsKey(lootboxId))
            {
                DicActivarAnimacion[lootboxId] = true;
            }

            string diccionarioModificadoJson = JsonConvert.SerializeObject(DicActivarAnimacion);
            HttpContext.Session.SetString("miDiccionarioEnSesion", diccionarioModificadoJson);

            string diccionarioJson2 = HttpContext.Session.GetString("miDiccionarioEnSesion2");
            nuevoDiccionario = JsonConvert.DeserializeObject<Dictionary<int, bool>>(diccionarioJson2);

            string itemsJson = HttpContext.Session.GetString("ItemsEnSesion");
            Items = JsonConvert.DeserializeObject<List<ItemInfo>>(itemsJson);

            premioLootbox(lootboxId);

            int lootbox_id_compra = LootboxesUsuario[lootboxId - 1].id_compra;
            await MarcarLootboxComoAbiertaAsync(lootbox_id_compra);

            ItemRegistro itemParaRegistrar = new ItemRegistro();
            itemParaRegistrar.id_usuario = user.id_usuario;
            itemParaRegistrar.id_item = premio.id;
            itemParaRegistrar.cantidad = 1;
            itemParaRegistrar.coordenada_x = 0;
            itemParaRegistrar.coordenada_y = 0;

            await AgregarItemAUsuarioAsync(itemParaRegistrar);

            return Page();
        }
        
        public void IngresarDato(int lootboxId)
        {
            string diccionarioJson2 = HttpContext.Session.GetString("miDiccionarioEnSesion2");
            nuevoDiccionario = JsonConvert.DeserializeObject<Dictionary<int, bool>>(diccionarioJson2);

            if (nuevoDiccionario.ContainsKey(lootboxId))
            {
                nuevoDiccionario[lootboxId] = true;
            }

            diccionarioJson2 = JsonConvert.SerializeObject(nuevoDiccionario);
            HttpContext.Session.SetString("miDiccionarioEnSesion2", diccionarioJson2);
        }

        private void premioLootbox(int lootboxId)
        {
            int lootboxAbiertaID = LootboxesUsuario[lootboxId - 1].id;
            int premioTipo;

            Random random = new Random();
            int randomNum = random.Next(1, 101);

            if (randomNum < 3) // 2% de probabilidad
            {
                premioTipo = 4; // El cuatro es el id que tienen asignados los premios de tipo Boleto
            }
            else if (randomNum < 21) // 18% de probabilidad
            {
                premioTipo = 3; // El tres es el id que tienen asignados los premios de tipo Descuento (boleto)
            }
            else if (randomNum < 51) // 30% de probabilidad
            {
                premioTipo = 1; // El uno es el id que tienen asignados los premios de tipo Skin
            }
            else // 50% de probabilidad
            {
                premioTipo = 2; // El dos es el id que tienen asignados los premios de tipo ObjetoCasa
            }

            foreach (var item in Items)
            {
                if(premioTipo == item.id_tipo && lootboxAbiertaID == item.id_lootbox)
                {
                    premio = item;
                }
            }
        }

        private async Task<List<LootboxInfo>> RunAsync()
        {
            try
            {
                // solicitud GET a la API
                HttpResponseMessage response = await client.GetAsync($"entidades/Usuarios/LootboxesDelUsuario?correo={usuario}");

                if (response.IsSuccessStatusCode)  // si la solicitud fue exitosa
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();  // leer la respuesta
                    return JsonConvert.DeserializeObject<List<LootboxInfo>>(apiResponse); // deserializar la respuesta
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
                return new List<LootboxInfo>();
            }

        }

        // para marcar la lootbox como abierta en la BD (no aparecerá más para abrir en la página)
        private async Task MarcarLootboxComoAbiertaAsync(int id_compra)
        {
            var jsonBody = JsonConvert.SerializeObject(id_compra);  // serializar el id de la compra a formato JSON
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json"); // StringContent con el cuerpo JSON de la solicitud

            HttpResponseMessage response = await client.PostAsync("entidades/Usuarios/MarcarLootboxComoAbierta", content);

            if (!response.IsSuccessStatusCode) // en caso de error,
            {
                throw new HttpRequestException($"Error al agregar usuario. Status Code: {response.StatusCode}");
            }
        }

        // para agregar un nuevo item al usuario a través de una solicitud POST a la API
        private async Task AgregarItemAUsuarioAsync(ItemRegistro itemParaRegistrar)
        {
            var jsonBody = JsonConvert.SerializeObject(itemParaRegistrar);  // serializar el objeto nuevoUsuario a formato JSON
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json"); // StringContent con el cuerpo JSON de la solicitud

            // solicitud POST a la API con la info del nuevo usuario
            HttpResponseMessage response = await client.PostAsync("entidades/Usuarios/AgregarItem", content);

            if (!response.IsSuccessStatusCode) // en caso de error,
            {
                throw new HttpRequestException($"Error al agregar usuario. Status Code: {response.StatusCode}");
            }

        }

    }

    public class ItemRegistro
    {
        public int id_usuario { get; set; }
        public int id_item { get; set; }
        public int cantidad { get; set; }
        public decimal coordenada_x { get; set; }
        public decimal coordenada_y { get; set; }
    }
}