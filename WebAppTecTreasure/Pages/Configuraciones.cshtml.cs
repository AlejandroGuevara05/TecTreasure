using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTecTreasure
{
	public class ConfiguracionesModel : PageModel
    {
        public DateTime? FechaMantenimiento { get; set; }
        public string ParametrosJuego { get; set; }
        public string NombreArticulo { get; set; }
        public decimal? PrecioArticulo { get; set; }

        public decimal VentasTotales { get; set; }
        public string ArticuloMasPopular { get; set; }
        public int CantidadArticuloMasPopular { get; set; }

        public void OnGet()
        {
            // Simulando la obtención de datos, reemplaza con la consulta a tu base de datos
            VentasTotales = 1000.5M; // Aquí deberías obtener las ventas totales desde la base de datos
            ArticuloMasPopular = "Sable Láser"; // Aquí el nombre del artículo más vendido
            CantidadArticuloMasPopular = 50; // Aquí la cantidad de veces que se ha vendido el artículo más popular

            // Puedes añadir lógica para obtener más estadísticas según lo necesites
        }

        public void OnPost()
        {
            // Lógica para manejar los valores enviados desde los formularios.
            FechaMantenimiento = DateTime.Parse(Request.Form["mantenimiento"]);
            ParametrosJuego = Request.Form["parametros"];
            NombreArticulo = Request.Form["articulo"];
            PrecioArticulo = decimal.Parse(Request.Form["precio"]);

            // Aquí puedes añadir código para guardar estos valores en la base de datos o realizar otras acciones.
        }
    }
}
