using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebAppTecTreasure.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlX.XDevAPI;
using Newtonsoft.Json;

namespace WebAppTecTreasure.Pages
{
    public class Compra
    {
        public int id_lootboxComprada { get; set; }
        public int id_usuarioComprador { get; set; }
        public int id_tipoPago { get; set; }
        public DateTime fechaCompra { get; set; }
        public int usado { get; set; }
    }

	public class CarritoModel : PageModel
    {
        // Lista para almacenar las lootboxes en el carrito
        public List<LootboxCarrito> lootboxesEnCarrito { get; set; }

        public bool compraRealizada { get; set; }

        private HttpClient client = new HttpClient(); // cliente para usar la API

        // Constructor de la clase
        public CarritoModel()
        {
            client.BaseAddress = new Uri("https://localhost:7222/"); // dirección para las solicitudes
            client.DefaultRequestHeaders.Accept.Clear(); // reiniciar encabezado de la solicitud
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // crear el nuevo encabezado
        }

        public void OnGet()
        {
            string Lootboxes = HttpContext.Session.GetString("LootboxesEnCarritoEnSession");
            lootboxesEnCarrito = JsonConvert.DeserializeObject<List<LootboxCarrito>>(Lootboxes);

            compraRealizada = false;
        }

        public IActionResult OnPostDelete(int lootboxId)
        {
            string Lootboxes = HttpContext.Session.GetString("LootboxesEnCarritoEnSession");
            lootboxesEnCarrito = JsonConvert.DeserializeObject<List<LootboxCarrito>>(Lootboxes);

            foreach (var lootbox in lootboxesEnCarrito)
            {
                if(lootbox.id_LB == lootboxId)
                {
                    lootbox.cantidad = 0;
                }
            }

            Lootboxes = JsonConvert.SerializeObject(lootboxesEnCarrito);
            HttpContext.Session.SetString("LootboxesEnCarritoEnSession", Lootboxes);

            return Page();
        }

        public async Task<IActionResult> OnPostComprar()
        {
            string Lootboxes = HttpContext.Session.GetString("LootboxesEnCarritoEnSession");
            lootboxesEnCarrito = JsonConvert.DeserializeObject<List<LootboxCarrito>>(Lootboxes);

            string datosUsuario = HttpContext.Session.GetString("DatosUsuarioEnSesion");
            UsuarioInfo user = JsonConvert.DeserializeObject<UsuarioInfo>(datosUsuario);

            foreach (var lootbox in lootboxesEnCarrito)
            {
                if (lootbox.cantidad > 0)
                {
                    for (var i = 0; i < lootbox.cantidad; i++)
                    {
                        var compra = new Compra
                        {
                            id_lootboxComprada = lootbox.id_LB,
                            id_usuarioComprador = user.id_usuario,
                            id_tipoPago = 1,
                            fechaCompra = DateTime.Now,
                            usado = 0
                        };
                        await AgregarCompra(compra);
                    }
                    lootbox.cantidad = 0;
                }
            }

            Lootboxes = JsonConvert.SerializeObject(lootboxesEnCarrito);
            HttpContext.Session.SetString("LootboxesEnCarritoEnSession", Lootboxes);

            compraRealizada = true;

            return Page();
        }

        private async Task AgregarCompra(Compra compra)
        {
            var jsonBody = JsonConvert.SerializeObject(compra);  // serializar el objeto a formato JSON
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json"); // StringContent con el cuerpo JSON de la solicitud

            // solicitud POST a la API con la info del nuevo usuario
            HttpResponseMessage response = await client.PostAsync("entidades/Usuarios/agregarCompra", content);

            if (!response.IsSuccessStatusCode) // en caso de error,
            {
                throw new HttpRequestException($"Error al agregar usuario. Status Code: {response.StatusCode}");
            }

        }

    }
}
