using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace APISociosComerciales48.Models
{
    public class _EntidadContactos
    {
        public int id { get; set; }

        public int socio { get; set; }

        [MaxLength]
        public string nombre { get; set; }

        [MaxLength]
        public string apellidos { get; set; }

        [MaxLength]
        public string email { get; set; }

        private string _contrasena;

        [MaxLength]
        public string contrasena { get; set; }

        [MaxLength]
        public string telefono { get; set; }

        [MaxLength]
        public string puesto { get; set; }

        [MaxLength]
        public string activo { get; set; }

        public _EntidadContactos()
        {
            activo = "sin confirmar";
        }

        // Método para encriptar la contraseña usando SHA256
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

    }
}