using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using EntregaCiclo1.Model;

namespace EntregaCiclo1.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string correo_usuario { get; set; }

    [BindProperty]
    public string contrasena_usuario { get; set; }

    private HttpClient client = new HttpClient(); // cliente para usar la API
    public IDictionary<string, string> CorreoContrasena { get; set; }

    public bool cuentaCreada { get; set; }

    public IndexModel()
    {
        client.BaseAddress = new Uri("https://localhost:7222/"); // dirección para las solicitudes
        client.DefaultRequestHeaders.Accept.Clear(); // reiniciar encabezado de la solicitud
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // crear el nuevo encabezado
        CorreoContrasena = new Dictionary<string, string>();
    }

    public void OnGet()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("creacionCuenta")) == false)
        {
            string statusCreacionCuenta = HttpContext.Session.GetString("creacionCuenta");
            cuentaCreada = JsonConvert.DeserializeObject<bool>(statusCreacionCuenta);
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        CorreoContrasena = await RunAsync(); // guardar en el diccionario lo devuelto por la API

        if (correo_usuario == null)
        {
            ViewData["Resultado"] = "Error: no ingresó correo";
        }
        else if (contrasena_usuario == null)
        {
            ViewData["Resultado"] = "Error: no ingresó contraseña";
        }
        else
        {
            if (CorreoContrasena.ContainsKey(correo_usuario))
            {
                string contrasenaGuardada;
                CorreoContrasena.TryGetValue(correo_usuario, out contrasenaGuardada);
                if (contrasena_usuario == contrasenaGuardada)
                {
                    HttpContext.Session.SetString("usuarioEnSesion", correo_usuario);
                    UsuarioInfo user = await NombreYidUsuario();
                    string usuario = JsonConvert.SerializeObject(user);
                    HttpContext.Session.SetString("DatosUsuarioEnSesion", usuario);

                    cuentaCreada = false;
                    string statusCreacionCuenta = JsonConvert.SerializeObject(cuentaCreada);
                    HttpContext.Session.SetString("creacionCuenta", statusCreacionCuenta);

                    Response.Redirect("SorteosTec");
                }
                else
                {
                    ViewData["Resultado"] = "Error: contraseña incorrecta";
                }
            }
            else
            {
                ViewData["Resultado"] = "Error: correo no registrado (registrarse en caso de no tener cuenta)";
            }
        }

        return Page();
    }

    private async Task<IDictionary<string, string>> RunAsync()
    {
        try
        {
            // solicitud GET a la API
            HttpResponseMessage response = await client.GetAsync("entidades/Usuarios/CorreoyContra");

            if (response.IsSuccessStatusCode)  // si la solicitud fue exitosa
            {
                var apiResponse = await response.Content.ReadAsStringAsync();  // leer la respuesta
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse); // deserializar la respuesta
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
            return new Dictionary<string, string>();  // regresar diccionario con los datos
        }

    }

    private async Task<UsuarioInfo> NombreYidUsuario()
    {
        try
        {
            // solicitud GET a la API
            HttpResponseMessage response = await client.GetAsync($"entidades/Usuarios/nombreUsuarioYid?correo={correo_usuario}");

            if (response.IsSuccessStatusCode)  // si la solicitud fue exitosa
            {
                var apiResponse = await response.Content.ReadAsStringAsync();  // leer la respuesta
                return JsonConvert.DeserializeObject<UsuarioInfo>(apiResponse); // deserializar la respuesta
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
            return new UsuarioInfo();
        }

    }

}

