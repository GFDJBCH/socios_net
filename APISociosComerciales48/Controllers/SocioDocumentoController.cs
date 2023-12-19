using APISociosComerciales48.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace APISociosComerciales48.Controllers
{
    public class SocioDocumentoController : ApiController
    {
        private CnnDbContext _dbContext;

        public SocioDocumentoController()
        {
            _dbContext = new CnnDbContext();
        }

        [HttpPost]
        [Route("api/SocioDocumentos")]
        public IHttpActionResult PostSocioDocumento(_EntidadSocioDocumentos sociodocumento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos de la asignación no válidos.");
            }

            _dbContext._EntidadSocioDocumentos.Add(sociodocumento);
            _dbContext.SaveChanges();

            return Ok("Asignación creada correctamente.");
        }

        [HttpGet]
        [Route("api/SocioDocumentos")]
        public IHttpActionResult GetSocioDocumento()
        {
            var socioDocumentos = _dbContext.Set<_EntidadSocioDocumentos>().ToList();
            if (socioDocumentos == null)
            {
                return Ok(new List<_EntidadSocioDocumentos>());
            }
            return Ok(socioDocumentos);
        }

        [HttpGet]
        [Route("api/SocioDocumentos/{id}")]
        public IHttpActionResult GetSocioDocumento(int id)
        {
            var socioDocumentosExistente = _dbContext._EntidadSocioDocumentos.FirstOrDefault(p => p.id == id);

            if (socioDocumentosExistente == null)
            {
                return NotFound();
            }

            return Ok(socioDocumentosExistente);
        }

        [HttpPut]
        [Route("api/SocioDocumentos/{id}")]
        public IHttpActionResult PutSocioDocumento(int id, _EntidadSocioDocumentos sociodocumento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != sociodocumento.id)
            {
                return BadRequest();
            }
            var socioDocumentosExistente = _dbContext._EntidadSocioDocumentos.FirstOrDefault(p => p.id == id);

            if (socioDocumentosExistente == null)
            {
                return NotFound();
            }

            foreach (var property in typeof(_EntidadSocioDocumentos).GetProperties())
            {
                var newValue = property.GetValue(sociodocumento);
                if (newValue != null)
                {
                    property.SetValue(socioDocumentosExistente, newValue);
                }
            }

            try
            {
                _dbContext.SaveChanges();
                return Ok("Asignación actualizada exitosamente");
            }
            catch (Exception)
            {
                if (!SocioDocumentoExists(id))
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
        [Route("api/SocioDocumentos/{id}")]
        public IHttpActionResult DeleteSocioDocumento(int id)
        {
            var socioDocumentosExistente = _dbContext._EntidadSocioDocumentos.FirstOrDefault(p => p.id == id);

            if (socioDocumentosExistente == null)
            {
                return NotFound();
            }

            try
            {
                _dbContext._EntidadSocioDocumentos.Remove(socioDocumentosExistente);
                _dbContext.SaveChanges();
                return Ok("Asignación eliminada exitosamente");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        private bool SocioDocumentoExists(int id)
        {
            return _dbContext._EntidadSocioDocumentos.Any(e => e.id == id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _dbContext.Dispose();

            base.Dispose(disposing);
        }
    }
}
