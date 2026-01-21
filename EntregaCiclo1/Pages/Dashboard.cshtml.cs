using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;


namespace EntregaCiclo1.Pages
{
    public class DashboardModel : PageModel
    {
        private HttpClient client = new HttpClient(); // cliente para usar la API
        public IDictionary<string, int> EstadosConConteo { get; set; }
        public IList<dynamic> PorcentajesPorSexo { get; set; } // Cambiado a IList<dynamic> para ajustarse a tu estructura de datos
        public double PorcentajeCompras { get; set; }
        public IDictionary<string, int> RangosDeEdadConConteo { get; set; } // Propiedad para almacenar la distribución de usuarios por rango de edad
        public int TotalUsuarios { get; set; } // Total de usuarios

        public bool cuentaCreada { get; set; }

        public DashboardModel()
        {
            client.BaseAddress = new Uri("https://localhost:7222/"); // dirección para las solicitudes
            client.DefaultRequestHeaders.Accept.Clear();  // reiniciar encabezado de la solicitud
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // crear el nuevo encabezado
            EstadosConConteo = new Dictionary<string, int>();
            PorcentajesPorSexo = new List<dynamic>();
            RangosDeEdadConConteo = new Dictionary<string, int>();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            EstadosConConteo = await RunAsync();  //Agregado
            TotalUsuarios = EstadosConConteo.Values.Sum(); // Calcular el total de usuarios
            RangosDeEdadConConteo = await ObtenerDistribucionPorEdadAsync(); //Agregado
            PorcentajesPorSexo = await GetPorcentajesPorSexoAsync() ?? new List<dynamic>();
            return Page();
        }

        // Método para obtener la distribución por edad
        public async Task<IDictionary<string, int>> ObtenerDistribucionPorEdadAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("entidades/Usuarios/RangoEdades");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Dictionary<string, int>>(apiResponse);
                }
                else
                {
                    throw new HttpRequestException($"Error retrieving data from API. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Dictionary<string, int>();
            }
        }


        private async Task<IDictionary<string, int>> RunAsync()
        {
            try
            {
                // solicitud GET a la API
                HttpResponseMessage response = await client.GetAsync("entidades/Usuarios/countByState");

                if (response.IsSuccessStatusCode) // si la solicitud fue exitosa
                {
                    var apiResponse = await response.Content.ReadAsStringAsync(); // leer la respuesta
                    return JsonConvert.DeserializeObject<Dictionary<string, int>>(apiResponse); // deserializar la respuesta
                }
                else // si la solicitud no fue exitosa
                {
                    throw new HttpRequestException($"Error retrieving data from API. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Dictionary<string, int>();  // regresar diccionario con los datos
            }
        }

        private async Task<IList<dynamic>> GetPorcentajesPorSexoAsync()
        {
            var response = await client.GetAsync("https://localhost:7222/entidades/Usuarios/porcentajePorSexo");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var porcentajes = JsonConvert.DeserializeObject<IList<dynamic>>(apiResponse);

                // Agrega esto para la depuración
                Console.WriteLine("Porcentajes por sexo recibidos:");
                foreach (var p in porcentajes)
                {
                    Console.WriteLine($"Sexo: {p.Sexo}, Conteo: {p.Conteo}, Porcentaje: {p.Porcentaje}");
                }

                return porcentajes;
            }
            return new List<dynamic>();
        }

        private async Task<double> GetPorcentajeComprasAsync()
        {
            var response = await client.GetAsync("https://localhost:7222/entidades/Usuarios/porcentajeCompras");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var porcentajeCompras = JsonConvert.DeserializeObject<Dictionary<string, double>>(apiResponse);
                return porcentajeCompras["porcentajeCompras"];
            }
            return 0;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            // Intenta obtener los porcentajes por sexo al cargar la página.
            EstadosConConteo = await RunAsync();  //Agregado
            TotalUsuarios = EstadosConConteo.Values.Sum(); // Calcular el total de usuarios
            RangosDeEdadConConteo = await ObtenerDistribucionPorEdadAsync(); //Agregado
            PorcentajesPorSexo = await GetPorcentajesPorSexoAsync() ?? new List<dynamic>();
            PorcentajeCompras = await GetPorcentajeComprasAsync();

            return Page();
        }

    }
}