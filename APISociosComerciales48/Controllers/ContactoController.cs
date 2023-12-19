using APISociosComerciales48.Models;
using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;

namespace APISociosComerciales48.Controllers
{
    public class ContactoController : ApiController
    {
        private CnnDbContext _dbContext;

        public ContactoController()
        {
            _dbContext = new CnnDbContext();
        }

        [HttpPost]
        [Route("api/Login")]
        public IHttpActionResult Login(LoginModel model)
        {
            var usuario = _dbContext._EntidadContactos.FirstOrDefault(u => u.email == model.Email);
            var pswrd = Encriptar(model.Password);
            if (usuario == null || usuario.contrasena != pswrd)
            {
                return BadRequest("Credenciales inválidas");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = "JOFFROYCOMERCIAL2023";
            var key = GenerateHMACSHA256Key(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.email),
                    new Claim("UserName", $"{usuario.nombre} {usuario.apellidos}"),
                    new Claim("Correo", usuario.email),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Token = tokenString,
                Expiration = token.ValidTo,
                UserName = $"{usuario.nombre} {usuario.apellidos}",
                Correo = usuario.email
            });
        }

        [HttpPost]
        [Route("api/Contactos")]
        public IHttpActionResult PostContacto(_EntidadContactos contacto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos del contacto no válidos.");
            }
            string contrasenaEncriptada = Encriptar(contacto.contrasena);
            contacto.contrasena = contrasenaEncriptada;

            _dbContext._EntidadContactos.Add(contacto);
            _dbContext.SaveChanges();
            EnviarCorreo(contacto);

            return Ok("Contacto creado correctamente.");
        }

        [HttpGet]
        [Route("api/Contactos")]
        public IHttpActionResult GetContacto()
        {
            var revisiones = _dbContext.Set<_EntidadContactos>().ToList();
            if (revisiones == null)
            {
                return Ok(new List<_EntidadContactos>());
            }
            return Ok(revisiones);
        }

        [HttpGet]
        [Route("api/Contactos/{id}")]
        public IHttpActionResult GetContacto(int id)
        {
            var contactoExistente = _dbContext._EntidadContactos.FirstOrDefault(p => p.id == id);

            if (contactoExistente == null)
            {
                return NotFound();
            }

            return Ok(contactoExistente);
        }

        [HttpPut]
        [Route("api/Contactos/{id}")]
        public IHttpActionResult PutContacto(int id, _EntidadContactos entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != entidad.id)
            {
                return BadRequest();
            }
            var entidadExistente = _dbContext._EntidadContactos.FirstOrDefault(p => p.id == id);

            if (entidadExistente == null)
            {
                return NotFound();
            }

            foreach (var property in typeof(_EntidadContactos).GetProperties())
            {
                var newValue = property.GetValue(entidad);
                if (newValue != null)
                {
                    if (property.Name == "contrasena")
                    {
                        var nuevaContrasenaEncriptada = Encriptar(newValue.ToString());
                        property.SetValue(entidadExistente, nuevaContrasenaEncriptada);
                    }
                    else
                    {
                        property.SetValue(entidadExistente, newValue);
                    }
                }
            }

            try
            {
                _dbContext.SaveChanges();
                return Ok("Contacto actualizado exitosamente");
            }
            catch (Exception)
            {
                if (!EntidadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError();
                }
            }
        }


        [HttpDelete]
        [Route("api/Contactos/{id}")]
        public IHttpActionResult DeleteContacto(int id)
        {
            var entidadExistente = _dbContext._EntidadContactos.FirstOrDefault(p => p.id == id);

            if (entidadExistente == null)
            {
                return NotFound(); // Devolver un error 404 si la entidad no existe
            }

            try
            {
                _dbContext._EntidadContactos.Remove(entidadExistente);
                _dbContext.SaveChanges();
                return Ok("Entidad eliminada exitosamente");
            }
            catch (Exception)
            {
                return InternalServerError(); // Devolver un error 500 en caso de error en el servidor
            }
        }

        // Método para encriptar la contraseña usando SHA256 (mismo método utilizado anteriormente)
        private string Encriptar(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convertir la contraseña en bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convertir los bytes en una cadena hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private static byte[] GenerateHMACSHA256Key(string secretKey)
        {
            using (var sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider())
            {
                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(secretKey);
                byte[] hashedBytes = sha256.ComputeHash(keyBytes);

                // Seleccionar los primeros 256 bits (32 bytes) como clave
                byte[] key = new byte[32];
                Array.Copy(hashedBytes, key, 32);

                return key;
            }
        }

        private bool EntidadExists(int id)
        {
            return _dbContext._EntidadContactos.Any(e => e.id == id);
        }

        public bool EnviarCorreo(_EntidadContactos contacto)
        {
            try
            {
                MailMessage correo = new MailMessage();
                SmtpClient clienteSmtp = new SmtpClient();

                correo.From = new MailAddress("noreply@joffroy.com", "Joffroy Group");
                correo.To.Add(new MailAddress(contacto.email));
                correo.Subject = "Account confirmation";

                string direccion = "http://partners.joffroy.com/contacto-bienvenida.html?email=" + contacto.email;

                correo.Body = @"
                    <body style='margin:0;background:#7B90B6;'>
                        <!-- Resto del contenido HTML -->
                        <a href='" + direccion + @"' style='background:#1a2bc2; color:white; text-decoration:none; padding: 15px 20px; border-radius: 0.25rem;'>Activate account</a>
                    </body>
                ";
                correo.IsBodyHtml = true;

                clienteSmtp.Host = "email-smtp.us-west-2.amazonaws.com";
                clienteSmtp.Port = 587;
                clienteSmtp.UseDefaultCredentials = false;
                clienteSmtp.Credentials = new NetworkCredential("AKIAUPIQQZEMI5ZAFGZB", "BGQm9+vFhqp1Iv+/rJvvKESGhaUHiNGeCG1K4Hh4hKS5");
                clienteSmtp.EnableSsl = true;

                clienteSmtp.Send(correo);
                return true; // Envío exitoso
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
                return false; // Error al enviar
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _dbContext.Dispose();

            base.Dispose(disposing);
        }

    }
}
