using APISociosComerciales48.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APISociosComerciales48.Controllers
{
    [RoutePrefix("api/Documento")]
    public class DocumentoController : ApiController
    {
        private readonly CnnDbContext _dbContext;

        public DocumentoController()
        {
            _dbContext = new CnnDbContext();
        }

        // GET api/Documento
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetDocumentos()
        {
            var documentos = _dbContext.scc_documentos.ToList();
            if (documentos == null)
            {
                return Ok(new List<object>());
            }

            var parsedDocumentos = documentos.Select(d => new
            {
                d.id,
                d.nombre,
                d.nombre_ingles,
                d.descripcion,
                d.description_english,
                d.codigo,
                d.fisica,
                d.fisica_periodo,
                d.fisica_notificacion,
                d.moral,
                d.moral_periodo,
                d.moral_notificacion,
                d.extranjero,
                d.extranjero_periodo,
                d.extranjero_notificacion,
                unidades = JArray.Parse(d.unidades),
                sucursales = JArray.Parse(d.sucursales),
                flujos = JArray.Parse(d.flujos),
                d.fch_creacion,
                d.fch_borrar
            }).ToList();

            // Devolver los campos seleccionados como JSON
            return Content(HttpStatusCode.OK, parsedDocumentos, Configuration.Formatters.JsonFormatter);
        }

        // GET api/Documento/5
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetDocumento(int id)
        {
            var documento = _dbContext.scc_documentos.FirstOrDefault(h => h.id == id);
            if (documento == null)
            {
                return NotFound();
            }

            var parsedDocumento = new
            {
                documento.id,
                documento.nombre,
                documento.nombre_ingles,
                documento.descripcion,
                documento.description_english,
                documento.codigo,
                documento.fisica,
                documento.fisica_periodo,
                documento.fisica_notificacion,
                documento.moral,
                documento.moral_periodo,
                documento.moral_notificacion,
                documento.extranjero,
                documento.extranjero_periodo,
                documento.extranjero_notificacion,
                unidades = JArray.Parse(documento.unidades),
                sucursales = JArray.Parse(documento.sucursales),
                flujos = JArray.Parse(documento.flujos),
                documento.fch_creacion,
                documento.fch_borrar
            };

            return Ok(parsedDocumento);
        }

        // POST api/Documento
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostDocumento(scc_documentos documento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos del documento no válidos.");
            }
            bool codigoUnico = !_dbContext.scc_documentos.Any(d => d.codigo == documento.codigo);
            if (!codigoUnico)
            {
                return BadRequest("El código ya está en uso.");
            }
            documento.fch_creacion = DateTime.Now;
            _dbContext.scc_documentos.Add(documento);
            _dbContext.SaveChanges();

            return Ok("Documento creado correctamente.");
        }

        // PUT api/Documento/5
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult PutDocumento(int id, scc_documentos documento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != documento.id)
            {
                return BadRequest();
            }
            var logExistente = _dbContext.scc_documentos.FirstOrDefault(p => p.id == id);

            if (logExistente == null)
            {
                return NotFound();
            }

            bool codigoUnico = !_dbContext.scc_documentos.Any(d => d.id != id && d.codigo == documento.codigo);
            if (!codigoUnico)
            {
                return BadRequest("El código ya está en uso por otro documento.");
            }

            foreach (var property in typeof(scc_documentos).GetProperties())
            {
                var newValue = property.GetValue(documento);
                if (newValue != null && property.Name != "created_at")
                {
                    property.SetValue(logExistente, newValue);
                }
            }


            try
            {
                _dbContext.SaveChanges();
                return Ok("Documento actualizado exitosamente");
            }
            catch (Exception)
            {
                if (!DocumentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError();
                }
            }
        }

        // PUT api/Documento/Eliminar/5
        [HttpPut]
        [Route("Eliminar/{id}")]
        public IHttpActionResult PutEliminarDocumento(int id)
        {
            var documento = _dbContext.scc_documentos.FirstOrDefault(h => h.id == id);
            if (documento == null)
            {
                return NotFound();
            }

            documento.fch_borrar = DateTime.Now;
            try
            {
                _dbContext.SaveChanges();
                return Ok("Documento eliminado lógicamente.");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // DELETE api/HistorialDocumento/5
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteDocumento(int id)
        {
            var documentoExistente = _dbContext.scc_documentos.FirstOrDefault(p => p.id == id);

            if (documentoExistente == null)
            {
                return NotFound();
            }

            try
            {
                _dbContext.scc_documentos.Remove(documentoExistente);
                _dbContext.SaveChanges();
                return Ok("Documento eliminado exitosamente");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        private bool DocumentExists(int id)
        {
            return _dbContext.scc_documentos.Any(e => e.id == id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
