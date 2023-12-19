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
    [RoutePrefix("api/Flujo")]
    public class FlujoController : ApiController
    {
        private readonly CnnDbContext _dbContext;

        public FlujoController()
        {
            _dbContext = new CnnDbContext();
        }

        // GET api/Flujo
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetFlujos()
        {
            var flujos = _dbContext.Set<scc_scs_dcm_flujo>().ToList();
            if (flujos == null)
            {
                return Ok(new List<scc_scs_dcm_flujo>());
            }
            return Ok(flujos);
        }

        // GET api/Flujo/5
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetFlujo(int id)
        {
            var flujoExistente = _dbContext.scc_scs_dcm_flujo.FirstOrDefault(p => p.id == id);

            if (flujoExistente == null)
            {
                return NotFound();
            }

            return Ok(flujoExistente);
        }

        // POST api/Flujo
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostFlujo(scc_scs_dcm_flujo flujo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos del comentario no válidos.");
            }
       
            flujo.fecha = DateTime.Now;
            _dbContext.scc_scs_dcm_flujo.Add(flujo);
            _dbContext.SaveChanges();

            return Ok("Comentarios agregados correctamente.");
        }

        // PUT api/Flujo/5
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult PutFlujo(int id, scc_scs_dcm_flujo invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != invoice.id)
            {
                return BadRequest();
            }
            var logExistente = _dbContext.scc_scs_dcm_flujo.FirstOrDefault(p => p.id == id);

            if (logExistente == null)
            {
                return NotFound();
            }

            foreach (var property in typeof(scc_scs_dcm_flujo).GetProperties())
            {
                var newValue = property.GetValue(invoice);
                if (newValue != null && property.Name != "fecha")
                {
                    property.SetValue(logExistente, newValue);
                }
            }


            try
            {
                _dbContext.SaveChanges();
                return Ok("Comentario actualizado exitosamente");
            }
            catch (Exception)
            {
                if (!FlujoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError();
                }
            }
        }

        // DELETE api/Flujo/5
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteFlujo(int id)
        {
            var flujoExistente = _dbContext.scc_scs_dcm_flujo.FirstOrDefault(p => p.id == id);

            if (flujoExistente == null)
            {
                return NotFound();
            }

            try
            {
                _dbContext.scc_scs_dcm_flujo.Remove(flujoExistente);
                _dbContext.SaveChanges();
                return Ok("Comentario eliminado exitosamente");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        private bool FlujoExists(int id)
        {
            return _dbContext.scc_scs_dcm_flujo.Any(e => e.id == id);
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
