using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace WebAppTecTreasure.Pages;

public class InicioAdmins : PageModel
{
    [BindProperty]
    public string admin_matricula { get; set; }

    [BindProperty]
    public string admin_contrase { get; set; }

    private HttpClient client = new HttpClient(); // cliente para usar la API
    public IDictionary<string, string> LogInAdmin { get; set; }

    public bool cuentaCreada { get; set; }

    public InicioAdmins()
    {
        client.BaseAddress = new Uri("https://localhost:7222/"); // dirección para las solicitudes
        client.DefaultRequestHeaders.Accept.Clear();  // reiniciar encabezado de la solicitud
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // crear el nuevo encabezado
        LogInAdmin = new Dictionary<string, string>();
    }

    public void OnGet()
    {
        cuentaCreada = false;
        string statusCreacionCuenta = JsonConvert.SerializeObject(cuentaCreada);
        HttpContext.Session.SetString("creacionCuenta", statusCreacionCuenta);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        LogInAdmin = await RunAsync(); // guardar en el diccionario lo devuelto por la API

        if (admin_matricula == null)
        {
            ViewData["Mensaje"] = "Error: no ingresó matrícula de administrador";
        }
        else
        {
            if(admin_contrase == null)
            {
                ViewData["Mensaje"] = "Error: no ingresó contraseña";
            }
            else
            {
                if (LogInAdmin.ContainsKey(admin_matricula))
                {
                    string contrasenaAdmin;
                    LogInAdmin.TryGetValue(admin_matricula, out contrasenaAdmin);
                    if (admin_contrase == contrasenaAdmin)
                    {
                        Response.Redirect("Dashboard");
                    }
                    else
                    {
                        ViewData["Mensaje"] = "Error: contraseña incorrecta";
                    }
                }
                else
                {
                    ViewData["Mensaje"] = "Error: administrador no registrado";
                }
            }
        }

        return Page();
    }

    private async Task<IDictionary<string, string>> RunAsync()
    {
        try
        {
            // solicitud GET a la API
            HttpResponseMessage response = await client.GetAsync("entidades/Usuarios/DatosAdmin");

            if (response.IsSuccessStatusCode) // si la solicitud fue exitosa
            {
                var apiResponse = await response.Content.ReadAsStringAsync(); // leer la respuesta
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse); // deserializar la respuesta
            }
            else // si la solicitud no fue exitosa
            {
                throw new HttpRequestException($"Error retrieving data from API. Status Code: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new Dictionary<string, string>(); // regresar diccionario con los datos
        }
    }
}