using APISociosComerciales48.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APISociosComerciales48.Controllers
{
    public class ArchivosController : ApiController
    {
        private string CarpetaContenido = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content"));

        [HttpPost]
        [Route("api/Archivos/Subir/{socioid}/{tipo}/{documentoid}")]
        public IHttpActionResult SubirArchivo(string socioid, string tipo, string documentoid, ArchivoModel archivo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Datos del archivo no válidos.");
                }

                // Verificar si se proporcionó un nombre de archivo
                if (string.IsNullOrWhiteSpace(archivo.Nombre))
                {
                    return BadRequest("Se requiere un nombre para el archivo.");
                }

                if (!string.IsNullOrWhiteSpace(archivo.ContenidoBase64))
                {
                    try
                    {
                        string base64String = archivo.ContenidoBase64;

                        // Obtener la extensión del archivo a partir del contenido Base64
                        string extension = base64String.Substring(base64String.IndexOf("/") + 1, base64String.IndexOf(";") - base64String.IndexOf("/") - 1);
                        if (string.IsNullOrEmpty(extension))
                        {
                            // Si no se encuentra la extensión, puedes manejarlo de manera predeterminada o lanzar un error
                            return BadRequest("No se pudo determinar la extensión del archivo.");
                        }

                        // Decodificar el contenido base64
                        byte[] bytesArchivo = Convert.FromBase64String(base64String.Split(',')[1]);

                        // Crear la ruta de destino según el patrón deseado
                        string rutaCarpeta = Path.Combine(CarpetaContenido, socioid, tipo, documentoid);
                        Directory.CreateDirectory(rutaCarpeta); // Crear la carpeta si no existe

                        string nombreArchivo = $"archivo.{extension}"; // Nombre del archivo con la extensión extraída

                        string rutaArchivo = Path.Combine(rutaCarpeta, nombreArchivo);

                        // Guardar el archivo en la carpeta especificada
                        File.WriteAllBytes(rutaArchivo, bytesArchivo);

                        return Ok("Archivo subido correctamente.");
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }


                return BadRequest("No se proporcionó ningún contenido de archivo.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
