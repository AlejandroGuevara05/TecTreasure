using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ServiceREST.Model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ServiceREST.Controllers
{
    [Route("entidades/[controller]")]
    public class UsuariosController : Controller
    {
        // Conexión a base de datos
        private readonly string connectionString = "Server=127.0.0.1;Port=3306;Database=TecTreasureDB;Uid=root;password=;";



        // GET: entidades/usuarios/DatosDemograficosUsuario (obtener datos demograficos de la tabla Usuario)
        [HttpGet("DatosDemograficosUsuario")]
        public IActionResult DatosDemograficosUsuario()
        {
            var listaUsuarios = new List<Dictionary<string, string>>(); // lista para almacenar resultados
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "SELECT id_usuario, nombre, sexo, edad, estado FROM Usuario"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    using (var reader = command.ExecuteReader()) // leer los datos del query
                    {
                        while (reader.Read()) // leer hasta acabarse los resultados recibidos
                        {
                            // almacenar la información de cada usuario en un diccionario (1 usuario = 1 diccionario)
                            var usuario = new Dictionary<string, string>
                            {
                                {"id_usuario", reader["id_usuario"].ToString()},
                                {"nombre", reader["nombre"].ToString()},
                                {"sexo", reader["sexo"].ToString()},
                                {"edad", reader["edad"].ToString()},
                                {"estado", reader["estado"].ToString()}
                            };
                            listaUsuarios.Add(usuario);
                        }
                    }
                }
            }
            return Ok(listaUsuarios); // regresar la lista de diccionarios
        }



        // GET: entidades/usuarios/countByState (para obtener el total de usuarios en cada estado)
        [HttpGet("countByState")]
        public IActionResult GetCountByState()
        {
            var counts = new Dictionary<string, int>(); // diccionario para almacenar los estados con su total de usuarios
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "SELECT estado, COUNT(*) as count FROM Usuario GROUP BY estado"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    using (var reader = command.ExecuteReader()) // leer los datos del query
                    {
                        while (reader.Read()) // leer hasta acabarse los resultados recibidos
                        {
                            var state = reader["estado"].ToString();
                            var count = Convert.ToInt32(reader["count"]);
                            counts.Add(state, count);
                        }
                    }
                }
            }

            return Ok(counts); // regresar el diccionario
        }



        // GET: entidades/usuarios/CorreoyContra (para obtener los correos registrados con sus respectivas contraseñas)
        [HttpGet("CorreoyContra")]
        public IActionResult CorreoyContra()
        {
            var diccionario = new Dictionary<string, string>();  // diccionario para almacenar cada correo con su contraseña
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "SELECT correo,contrasena_web FROM Usuario"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    using (var reader = command.ExecuteReader()) // leer los datos del query
                    {
                        while (reader.Read()) // leer hasta acabarse los resultados recibidos
                        {
                            var mail = reader["correo"].ToString();
                            var contra = reader["contrasena_web"].ToString();
                            diccionario.Add(mail, contra);
                        }
                    }
                }
            }

            return Ok(diccionario); // regresar el diccionario
        }



        // GET: entidades/usuarios/CrearCuenta (para insertar un nuevo registro en la tabla Usuario)
        [HttpPost("CrearCuenta")]
        public IActionResult CrearCuenta([FromBody] Usuarios nuevoUsuario)
        {
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "INSERT INTO Usuario (nombre, sexo, edad, estado, correo, telefono, contrasena_web, activo) VALUES (@nombre, " +
                    "@sexo, @edad, @estado, @correo, @telefono, @contrasena, @activo)"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    command.Parameters.AddWithValue("@nombre", nuevoUsuario.Nombre);
                    command.Parameters.AddWithValue("@sexo", nuevoUsuario.Sexo);
                    command.Parameters.AddWithValue("@edad", nuevoUsuario.Edad);
                    command.Parameters.AddWithValue("@estado", nuevoUsuario.Estado);
                    command.Parameters.AddWithValue("@correo", nuevoUsuario.Correo);
                    command.Parameters.AddWithValue("@telefono", nuevoUsuario.Telefono);
                    command.Parameters.AddWithValue("@contrasena", nuevoUsuario.Contrasena);
                    command.Parameters.AddWithValue("@activo", 0);

                    int rowsAffected = command.ExecuteNonQuery();  // ejecutar el query y obtener el número de filas afectadas

                    if (rowsAffected > 0)
                    {
                        // exitoso
                        return Ok("Usuario agregado exitosamente");
                    }
                    else
                    {
                        // algo salió mal
                        return BadRequest("No se pudo agregar el usuario");
                    }
                }
            }

        }



        // GET: entidades/usuarios/NoRepetirCorreo (para obtener un correo que sea igual al ingresado, de forma que se comprueba si existe)
        [HttpGet("NoRepetirCorreo")]
        public IActionResult NoRepetirCorreo([FromQuery] string correo)
        {
            string mail = null;
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "SELECT correo FROM Usuario WHERE correo = @correo"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    command.Parameters.AddWithValue("@correo", correo);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // si se encuentra un resultado lo asignamos a "mail" (el correo si existe)
                        {
                            mail = reader["correo"].ToString();
                        }
                    }
                }
            }
            return Ok(mail); // regresamos el string (nos importa solamente si es null o no)
        }



        // GET: entidades/usuarios/NoRepetirTelefono (para obtener un telefono que sea igual al ingresado, de forma que se comprueba si existe)
        [HttpGet("NoRepetirTelefono")]
        public IActionResult NoRepetirTelefono([FromQuery] string telefono)
        {
            string phone = null;
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "SELECT telefono FROM Usuario WHERE telefono = @telefono"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    command.Parameters.AddWithValue("@telefono", telefono); 

                    using (var reader = command.ExecuteReader()) // leer los datos del query
                    {
                        if (reader.Read()) // si se encuentra un resultado lo asignamos a "phone" (el telefono ya existe)
                        {
                            phone = reader["telefono"].ToString();
                        }
                    }
                }
            }
            return Ok(phone); // regresamos el string (nos importa solamente si es null o no)
        }



        // GET: entidades/usuarios/DatosAdmin (para obtener la matricula y contraseña de la cuenta de cada admin)
        [HttpGet("DatosAdmin")]
        public IActionResult DatosAdmin()
        {
            var diccionario = new Dictionary<string, string>(); // diccionario para almacenar lo leído
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "SELECT matricula,contrasena_admin FROM Admin"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    using (var reader = command.ExecuteReader()) // leer los datos del query
                    {
                        while (reader.Read()) // leer hasta acabarse los resultados recibidos
                        {
                            var admin_matric = reader["matricula"].ToString();
                            var admin_contra = reader["contrasena_admin"].ToString();
                            diccionario.Add(admin_matric, admin_contra); // meter registro a diccionario
                        }
                    }
                }
            }

            return Ok(diccionario); // regresar el diccionario
        }



        // GET: entidades/usuarios/LootboxesDelUsuario (para obtener las lootboxes que tiene el usuario compradas y no usadas)
        [HttpGet("LootboxesDelUsuario")]
        public IActionResult LootboxesDelUsuario([FromQuery] string correo)
        {
            var listaLootboxes = new List<LootboxInfo>(); // lista para almacenar lo leído
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "SELECT L.id_lootbox, L.nombre, C.id_compra\n" +
                    "FROM Usuario U\n" +
                    "JOIN Compra C ON U.id_usuario = C.id_usuario\n" +
                    "JOIN Lootbox L ON C.id_lootbox = L.id_lootbox\n" +
                    "WHERE U.correo = @correo AND C.usado = 0;"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    command.Parameters.AddWithValue("@correo", correo);
                    using (var reader = command.ExecuteReader()) // leer los datos del query
                    {
                        while (reader.Read()) // leer hasta acabarse los resultados recibidos
                        {
                            var lootboxInfo = new LootboxInfo
                            {
                                id = Convert.ToInt32(reader["id_lootbox"]),
                                nombre = reader["nombre"].ToString(),
                                id_compra = Convert.ToInt32(reader["id_compra"])
                            };

                            listaLootboxes.Add(lootboxInfo);
                        }
                    }
                }
            }

            return Ok(listaLootboxes); // regresar el diccionario
        }

        public class LootboxInfo
        {
            public int id { get; set; }
            public string nombre { get; set; }
            public int id_compra { get; set; }
        }



        // GET: entidades/usuarios/Items (para obtener los items existentes y desplegarlos en tienda)
        [HttpGet("Items")]
        public IActionResult Items()
        {
            var listaItems = new List<ItemInfo>(); // lista para almacenar lo leído
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "SELECT \n\t" +
                    "i.id_item,\n\t" +
                    "i.nombre,\n\t" +
                    "i.id_tipo,\n\t" +
                    "it.nombre AS tipo_nombre,\n\t" +
                    "i.descripcion,\n\t" +
                    "i.id_lootbox\n" +
                    "FROM Item i \n" +
                    "JOIN ItemTipo it ON it.id_tipo = i.id_tipo"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    using (var reader = command.ExecuteReader()) // leer los datos del query
                    {
                        while (reader.Read()) // leer hasta acabarse los resultados recibidos
                        {
                            var itemInfo = new ItemInfo
                            {
                                id = Convert.ToInt32(reader["id_item"]),
                                nombre = reader["nombre"].ToString(),
                                id_tipo = Convert.ToInt32(reader["id_tipo"]),
                                tipo = reader["tipo_nombre"].ToString(),
                                descrip = reader["descripcion"].ToString(),
                                id_lootbox = Convert.ToInt32(reader["id_lootbox"])
                            };
                            listaItems.Add(itemInfo);
                        }
                    }
                }
            }

            return Ok(listaItems); // regresar el diccionario
        }

        public class ItemInfo
        {
            public int id { get; set; }
            public string nombre { get; set; }
            public int id_tipo { get; set; }
            public string tipo { get; set; }
            public string descrip { get; set; }
            public int id_lootbox { get; set; }
        }



        // GET: entidades/usuarios/nombreUsuarioYid (para obtener el nombre del usuario que inicia sesión, y su id)
        [HttpGet("nombreUsuarioYid")]
        public IActionResult nombreUsuarioYid([FromQuery] string correo)
        {
            UsuarioInfo usuario = null;
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "SELECT id_usuario, nombre\n" +
                    "FROM Usuario \n" +
                    "WHERE correo = @correo"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    command.Parameters.AddWithValue("@correo", correo);
                    using (var reader = command.ExecuteReader()) // leer los datos del query
                    {
                        if (reader.Read()) // si se encuentra un resultado
                        {
                            usuario = new UsuarioInfo()
                            {
                                id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                nombre_usuario = reader["nombre"].ToString()
                            };
                        }
                    }
                }
            }

            return Ok(usuario); // regresar el nombre
        }

        public class UsuarioInfo
        {
            public int id_usuario { get; set; }
            public string nombre_usuario { get; set; }
        }



        // GET: entidades/usuarios/MarcarLootboxComoAbierta (para insertar cambiar el estado a usado en la lootbox abierta)
        [HttpPost("MarcarLootboxComoAbierta")]
        public IActionResult MarcarLootboxComoAbierta([FromBody] int compra)
        {
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "UPDATE Compra " +
                    "SET usado = 1 " +
                    "WHERE id_compra = @compra"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    command.Parameters.AddWithValue("@compra", compra);

                    int rowsAffected = command.ExecuteNonQuery();  // ejecutar el query y obtener el número de filas afectadas

                    if (rowsAffected > 0)
                    {
                        // exitoso
                        return Ok("Usuario agregado exitosamente");
                    }
                    else
                    {
                        // algo salió mal
                        return BadRequest("No se pudo agregar el usuario");
                    }
                }
            }

        }



        // GET: entidades/usuarios/AgregarItem (para registrar un nuevo item al usuario en la BD)
        [HttpPost("AgregarItem")]
        public IActionResult AgregarItem([FromBody] ItemRegistro nuevoItem)
        {
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "INSERT INTO UsuarioItem (id_usuario, id_item, cantidad, coordenada_x, coordenada_y)\n" +
                    "VALUES (@usuarioID, @itemID, @cant, @cordX, @cordY)\n" +
                    "ON DUPLICATE KEY UPDATE cantidad = cantidad + 1;\n"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    command.Parameters.AddWithValue("@usuarioID", nuevoItem.id_usuario);
                    command.Parameters.AddWithValue("@itemID", nuevoItem.id_item);
                    command.Parameters.AddWithValue("@cant", nuevoItem.cantidad);
                    command.Parameters.AddWithValue("@cordX", nuevoItem.coordenada_x);
                    command.Parameters.AddWithValue("@cordY", nuevoItem.coordenada_y);

                    int rowsAffected = command.ExecuteNonQuery();  // ejecutar el query y obtener el número de filas afectadas

                    if (rowsAffected > 0)
                    {
                        // exitoso
                        return Ok("Usuario agregado exitosamente");
                    }
                    else
                    {
                        // algo salió mal
                        return BadRequest("No se pudo agregar el usuario");
                    }
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



        // GET: entidades/usuarios/LootboxesCarrito (para obtener todas las lootboxes que hay disponibles para comprar en tienda)
        [HttpGet("LootboxesCarrito")]
        public IActionResult LootboxesCarrito()
        {
            var lootboxesCarrito = new List<LootboxCarrito>(); // lista para almacenar lo leído
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "SELECT id_lootbox, nombre, precio_gemas\n" +
                    "FROM Lootbox\n" +
                    "WHERE status = 1;"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    using (var reader = command.ExecuteReader()) // leer los datos del query
                    {
                        while (reader.Read()) // leer hasta acabarse los resultados recibidos
                        {
                            var lootboxCarrito = new LootboxCarrito
                            {
                                id_LB = Convert.ToInt32(reader["id_lootbox"]),
                                nombre_LB = reader["nombre"].ToString(),
                                cantidad = 0,
                                precio = Convert.ToInt32(reader["precio_gemas"])
                            };

                            lootboxesCarrito.Add(lootboxCarrito);
                        }
                    }
                }
            }

            return Ok(lootboxesCarrito); // regresar el diccionario
        }

        public class LootboxCarrito
        {
            public int id_LB { get; set; }
            public string nombre_LB { get; set; }
            public int cantidad { get; set; }
            public int precio { get; set; }
        }



        // POST: entidades/usuarios/agregarCompra (para insertar las nuevas compras en la tabla Compra)
        [HttpPost("agregarCompra")]
        public IActionResult agregarCompra([FromBody] Compra listaCompras)
        {
            using (var connection = new MySqlConnection(connectionString)) // conexión a bd
            {
                connection.Open();
                var query = "INSERT INTO Compra (id_lootbox, id_usuario, id_tipoPago, fecha_compra, usado) VALUES (@id_L, " +
                    "@id_U, @id_TP, @fecha, @usado)"; // query SQL
                using (var command = new MySqlCommand(query, connection)) // ejecutar el query
                {
                    command.Parameters.AddWithValue("@id_L", listaCompras.id_lootboxComprada);
                    command.Parameters.AddWithValue("@id_U", listaCompras.id_usuarioComprador);
                    command.Parameters.AddWithValue("@id_TP", listaCompras.id_tipoPago);
                    command.Parameters.AddWithValue("@fecha", listaCompras.fechaCompra.Date);
                    command.Parameters.AddWithValue("@usado", listaCompras.usado);

                    int rowsAffected = command.ExecuteNonQuery();  // ejecutar el query y obtener el número de filas afectadas

                    if (rowsAffected > 0)
                    {
                        // exitoso
                        return Ok("Usuario agregado exitosamente");
                    }
                    else
                    {
                        // algo salió mal
                        return BadRequest("No se pudo agregar el usuario");
                    }
                }
            }

        }

        public class Compra
        {
            public int id_lootboxComprada { get; set; }
            public int id_usuarioComprador { get; set; }
            public int id_tipoPago { get; set; }
            public DateTime fechaCompra { get; set; }
            public int usado { get; set; }
        }

        // GET: entidades/usuarios/RangoEdades
        [HttpGet("RangoEdades")]
        public IActionResult RangoEdades()
        {
            var rangosDeEdades = new Dictionary<string, int>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                // Definir rangos de edades aquí. Ejemplo: '<20', '20-29', '30-39', etc.
                var rangos = new List<string> { "<20", "20-29", "30-39", "40-49", "50-59", "60+" };

                foreach (var rango in rangos)
                {
                    var query = "";
                    if (rango == "<20")
                    {
                        query = "SELECT COUNT(*) FROM Usuario WHERE edad < 20";
                    }
                    else if (rango == "60+")
                    {
                        query = "SELECT COUNT(*) FROM Usuario WHERE edad >= 60";
                    }
                    else
                    {
                        var edades = rango.Split('-');
                        query = $"SELECT COUNT(*) FROM Usuario WHERE edad >= {edades[0]} AND edad <= {edades[1]}";
                    }

                    using (var command = new MySqlCommand(query, connection))
                    {
                        var count = Convert.ToInt32(command.ExecuteScalar());
                        rangosDeEdades.Add(rango, count);
                    }
                }
            }

            return Ok(rangosDeEdades);
        }

        //Obtener porcentaje de usuarios que realizaron compras:
        [HttpGet("porcentajeCompras")]
        public IActionResult GetPorcentajeCompras()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT (SELECT COUNT(DISTINCT id_usuario) FROM Compra) * 100.0 / COUNT(*) AS porcentaje_compras FROM Usuario;";
                using (var command = new MySqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar(); // ExecuteScalar devuelve el primer valor de la primera fila
                    if (result != null)
                    {
                        return Ok(new { porcentajeCompras = Convert.ToDouble(result) });
                    }
                    else
                    {
                        return Ok(new { porcentajeCompras = 0 });
                    }
                }
            }
        }

        //Obtener porcentaje genero
        [HttpGet("porcentajePorSexo")]
        public IActionResult GetPorcentajePorSexo()
        {
            var resultado = new List<dynamic>(); // Lista para almacenar los resultados
            using (var connection = new MySqlConnection(connectionString)) // Usando la conexión a la base de datos
            {
                connection.Open();
                var query = "SELECT sexo, COUNT(*) AS conteo, (COUNT(*) / (SELECT COUNT(*) FROM Usuario)) * 100 AS porcentaje FROM Usuario GROUP BY sexo";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) // Leer cada fila
                        {
                            resultado.Add(new
                            {
                                Sexo = reader["sexo"].ToString(),
                                Conteo = Convert.ToInt32(reader["conteo"]),
                                Porcentaje = Convert.ToDouble(reader["porcentaje"])
                            });
                        }
                    }
                }
            }
            return Ok(resultado); // Devolver los resultados
        }
    }
}
