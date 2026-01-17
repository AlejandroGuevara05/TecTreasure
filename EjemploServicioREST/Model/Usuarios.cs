using System;
namespace EjemploServicioREST.Model
{
    public class Usuarios
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        public int? Edad { get; set; }
        public string Estado { get; set; }
        public string Correo { get; set; }
        public int MaxPuntuacion { get; set; }
        public string Telefono { get; set; }
        public string Contrasena { get; set; }

        public Usuarios()
        {

        }
    }
}
