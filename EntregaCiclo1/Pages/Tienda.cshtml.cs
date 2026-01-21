using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace EntregaCiclo1.Pages
{
    public class TiendaModel : PageModel
    {
        // Lista para almacenar las lootboxes en el carrito
        public List<LootboxCarrito> lootboxesEnCarrito { get; set; }

        public bool lootboxAgregadaACarrito { get; set; }

        public void OnGet()
        {
            lootboxAgregadaACarrito = false;
        }

        public async Task<IActionResult> OnPostAsync(int lootboxSeleccionada)
        {
            string Lootboxes = HttpContext.Session.GetString("LootboxesEnCarritoEnSession");
            lootboxesEnCarrito = JsonConvert.DeserializeObject<List<LootboxCarrito>>(Lootboxes);

            int cantidadSeleccionada = lootboxesEnCarrito[lootboxSeleccionada - 1].cantidad;
            cantidadSeleccionada = cantidadSeleccionada + 1;

            lootboxesEnCarrito[lootboxSeleccionada - 1].cantidad = cantidadSeleccionada;

            string itemsJson = JsonConvert.SerializeObject(lootboxesEnCarrito);
            HttpContext.Session.SetString("LootboxesEnCarritoEnSession", itemsJson);

            lootboxAgregadaACarrito = true;

            return Page();
        }
    }
}
