using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebAppTecTreasure.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace WebAppTecTreasure.Pages
{
    public class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is bool && (bool)value;
        }
    }

    public class RegistroModel : PageModel
    {

        [BindProperty]
        [Required(ErrorMessage = "Es necesario confirmar este campo para crear una cuenta.")]
        [MustBeTrue(ErrorMessage = "Es necesario confirmar este campo para crear una cuenta.")]
        public bool confirmacion_edad { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "El nombre no debe contener números ni caracteres especiales.")]
        public string nombre_registro { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string sexo_registro { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Range(18, 100, ErrorMessage = "Debes ser mayor de 18 años.")]
        public int? edad_registro { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string estado_registro { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(outlook\.com|gmail\.com|hotmail\.com|tec\.mx)$", ErrorMessage = "El correo no tiene un formato válido.")]
        public string correo_registro { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [RegularExpression(@"^(\+?52\s?1?\s?)?(\d{10})$", ErrorMessage = "El número de teléfono no es válido.")]         
        public string telefono_registro { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [RegularExpression(@"^.{8,}$", ErrorMessage = "La contraseña debe ser de al menos 8 caracteres.")]
        public string contrasena_registro { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string contrasena_registro_confirm { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Es necesario confirmar este campo para crear una cuenta.")]
        [MustBeTrue(ErrorMessage = "Es necesario confirmar este campo para crear una cuenta.")]
        public bool confirmacion_terminos { get; set; }

        public bool cuentaCreada { get; set; }



        private HttpClient client = new HttpClient(); // cliente para usar la API
        public string CorreoNoRepetir { get; set; }
        public string TelefonoNoRepetir { get; set; }

        public RegistroModel()
        {
            client.BaseAddress = new Uri("https://localhost:7222/"); // dirección para las solicitudes
            client.DefaultRequestHeaders.Accept.Clear(); // reiniciar encabezado de la solicitud
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // crear el nuevo encabezado
        }

        public void OnGet()
        {
            cuentaCreada = false;
            string statusCreacionCuenta = JsonConvert.SerializeObject(cuentaCreada);
            HttpContext.Session.SetString("creacionCuenta", statusCreacionCuenta);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (contrasena_registro != contrasena_registro_confirm)
            {
                ModelState.AddModelError("contrasena_registro_confirm", "La contraseña no coincide.");
            }

            CorreoNoRepetir = await RunAsync(); // guardar en variable el string devuelto por la API
            TelefonoNoRepetir = await RunAsyncTelefono(); // guardar en variable el string devuelto por la API

            if (!string.IsNullOrEmpty(CorreoNoRepetir))
            {
                ModelState.AddModelError("correo_registro", "El correo ya está registrado.");
            }

            if (!string.IsNullOrEmpty(TelefonoNoRepetir))
            {
                ModelState.AddModelError("telefono_registro", "El teléfono ya está registrado.");
            }

            if (ViewData.ModelState.GetFieldValidationState("confirmacion_edad") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
                ViewData.ModelState.GetFieldValidationState("nombre_registro") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
                ViewData.ModelState.GetFieldValidationState("sexo_registro") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
                ViewData.ModelState.GetFieldValidationState("edad_registro") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
                ViewData.ModelState.GetFieldValidationState("estado_registro") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
                ViewData.ModelState.GetFieldValidationState("correo_registro") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
                ViewData.ModelState.GetFieldValidationState("telefono_registro") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
                ViewData.ModelState.GetFieldValidationState("contrasena_registro") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
                ViewData.ModelState.GetFieldValidationState("contrasena_registro_confirm") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
                ViewData.ModelState.GetFieldValidationState("confirmacion_terminos") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
            {
                try
                {
                    await AgregarUsuarioAsync(); // agregar registro a BD usando la API

                    cuentaCreada = true;
                    string statusCreacionCuenta = JsonConvert.SerializeObject(cuentaCreada);
                    HttpContext.Session.SetString("creacionCuenta", statusCreacionCuenta);

                    return RedirectToPage("/Index");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Page();
                }
            }
            
            return Page();
        }          

        private async Task<string> RunAsync()
        {
            try
            {
                // solicitud GET a la API
                HttpResponseMessage response = await client.GetAsync($"entidades/Usuarios/NoRepetirCorreo?correo={correo_registro}");

                if (response.IsSuccessStatusCode)  // si la solicitud fue exitosa
                {
                    var apiResponse = await response.Content.ReadAsStringAsync(); // leer la respuesta
                    return apiResponse; // asumiendo que la API devuelve el correo si existe, de lo contrario una cadena vacía
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
                return null;
            }
        }

        private async Task<string> RunAsyncTelefono()
        {
            try
            {
                // solicitud GET a la API
                HttpResponseMessage response = await client.GetAsync($"entidades/Usuarios/NoRepetirTelefono?telefono={telefono_registro}");

                if (response.IsSuccessStatusCode) // si la solicitud fue exitosa
                {
                    var apiResponse = await response.Content.ReadAsStringAsync(); // leer la respuesta
                    return apiResponse; // asumiendo que la API devuelve el correo si existe, de lo contrario una cadena vacía
                }
                else
                {
                    // Manejo adecuado del error
                    throw new HttpRequestException($"Error retrieving data from API. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        // para agregar un nuevo usuario a través de una solicitud POST a la API
        private async Task AgregarUsuarioAsync()
        {
            // objeto Usuarios con los datos proporcionados en la página de registro
            var nuevoUsuario = new Usuarios
            {
                Nombre = nombre_registro,
                Sexo = sexo_registro,
                Edad = edad_registro,
                Estado = estado_registro,
                Correo = correo_registro,
                Telefono = telefono_registro,
                Contrasena = contrasena_registro
            };

            var jsonBody = JsonConvert.SerializeObject(nuevoUsuario);  // serializar el objeto nuevoUsuario a formato JSON
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json"); // StringContent con el cuerpo JSON de la solicitud

            // solicitud POST a la API con la info del nuevo usuario
            HttpResponseMessage response = await client.PostAsync("entidades/Usuarios/CrearCuenta", content);

            if (!response.IsSuccessStatusCode) // en caso de error,
            {
                throw new HttpRequestException($"Error al agregar usuario. Status Code: {response.StatusCode}");
            }

        }

    }
}
