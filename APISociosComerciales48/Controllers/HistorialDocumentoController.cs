using APISociosComerciales48.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APISociosComerciales48.Controllers
{
    [RoutePrefix("api/HistorialDocumento")]
    public class HistorialDocumentoController : ApiController
    {
        private readonly CnnDbContext _dbContext;

        public HistorialDocumentoController()
        {
            _dbContext = new CnnDbContext();
        }

        // GET api/HistorialDocumento
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllHistorialDocumentos()
        {
            var historialDocumentos = _dbContext.Set<historial_documento>().ToList();
            if (historialDocumentos == null)
            {
                return Ok(new List<historial_documento>());
            }
            return Ok(historialDocumentos);
        }

        // GET api/HistorialDocumento/5
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetHistorialDocumento(int id)
        {
            var historialDocumento = _dbContext.historial_documento.FirstOrDefault(h => h.id == id);
            if (historialDocumento == null)
            {
                return NotFound();
            }
            return Ok(historialDocumento);
        }

        // POST api/HistorialDocumento
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostHistorialDocumento(historial_documento historialDocumento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos de la asignación no válidos.");
            }
            historialDocumento.created_at = DateTime.Now;
            historialDocumento.updated_at = DateTime.Now;
            _dbContext.historial_documento.Add(historialDocumento);
            _dbContext.SaveChanges();

            return Ok("Log creado correctamente.");
        }

        // PUT api/HistorialDocumento/5
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult PutHistorialDocumento(int id, historial_documento historialDocumento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != historialDocumento.id)
            {
                return BadRequest();
            }
            var logExistente = _dbContext.historial_documento.FirstOrDefault(p => p.id == id);

            if (logExistente == null)
            {
                return NotFound();
            }

            foreach (var property in typeof(historial_documento).GetProperties())
            {
                var newValue = property.GetValue(historialDocumento);
                if (newValue != null && property.Name != "created_at")
                {
                    if (property.Name == "updated_at")
                    {
                        logExistente.updated_at = DateTime.Now;
                    }
                    else
                    {
                        property.SetValue(logExistente, newValue);
                    }
                }
            }


            try
            {
                _dbContext.SaveChanges();
                return Ok("Log actualizado exitosamente");
            }
            catch (Exception)
            {
                if (!LogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError();
                }
            }
        }

        // DELETE api/HistorialDocumento/5
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteHistorialDocumento(int id)
        {
            var logExistente = _dbContext.historial_documento.FirstOrDefault(p => p.id == id);

            if (logExistente == null)
            {
                return NotFound();
            }

            try
            {
                _dbContext.historial_documento.Remove(logExistente);
                _dbContext.SaveChanges();
                return Ok("Log eliminado exitosamente");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        private bool LogExists(int id)
        {
            return _dbContext.historial_documento.Any(e => e.id == id);
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
