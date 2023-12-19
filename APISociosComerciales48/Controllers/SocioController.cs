using APISociosComerciales48.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace APISociosComerciales48.Controllers
{
    public class SocioController : ApiController
    {
        private CnnDbContext _dbContext;

        public SocioController()
        {
            _dbContext = new CnnDbContext();
        }

        [HttpGet]
        [Route("api/Socios")]
        public IHttpActionResult GetSocio()
        {
            var revisiones = _dbContext.Set<_EntidadesSocios>().ToList();
            if (revisiones == null)
            {
                return Ok(new List<_EntidadesSocios>());
            }
            return Ok(revisiones);
        }
        
        [HttpPost]
        [Route("api/Socios")]
        public IHttpActionResult PostSocio(_EntidadesSocios entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos de la entidad no válidos.");
            }

            _dbContext._EntidadesSocios.Add(entidad);
            _dbContext.SaveChanges();

            return Ok("Entidad creada correctamente.");
        }
        
        [HttpPut]
        [Route("api/Socios/{id}")]
        public IHttpActionResult PutSocio(int id, _EntidadesSocios entidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != entidad.id)
            {
                return BadRequest();
            }
            var entidadExistente = _dbContext._EntidadesSocios.FirstOrDefault(p => p.id == id);


            if (entidadExistente == null)
            {
                return NotFound();
            }

            foreach (var property in typeof(_EntidadesSocios).GetProperties())
            {
                var newValue = property.GetValue(entidad);
                if (newValue != null)
                {
                    property.SetValue(entidadExistente, newValue);
                }
            }
            try
            {
                _dbContext.SaveChanges();
                return Ok("Entidad actualizada exitosamente");
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
        [Route("api/Socios/{id}")]
        public IHttpActionResult DeleteSocio(int id)
        {
            var entidadExistente = _dbContext._EntidadesSocios.FirstOrDefault(p => p.id == id);

            if (entidadExistente == null)
            {
                return NotFound(); // Devolver un error 404 si la entidad no existe
            }

            try
            {
                _dbContext._EntidadesSocios.Remove(entidadExistente);
                _dbContext.SaveChanges();
                return Ok("Entidad eliminada exitosamente");
            }
            catch (Exception)
            {
                return InternalServerError(); // Devolver un error 500 en caso de error en el servidor
            }
        }


        private bool EntidadExists(int id)
        {
            return _dbContext._EntidadesSocios.Any(e => e.id == id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _dbContext.Dispose();

            base.Dispose(disposing);
        }
    }
}
