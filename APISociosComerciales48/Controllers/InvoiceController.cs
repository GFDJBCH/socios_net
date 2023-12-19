using APISociosComerciales48.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;

namespace APISociosComerciales48.Controllers
{
    [RoutePrefix("api/Factura")]
    public class InvoiceController : ApiController
    {
        private readonly CnnDbContext _dbContext;

        public InvoiceController()
        {
            _dbContext = new CnnDbContext();
        }

        // GET api/Factura
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetFacturas()
        {
            var facturas = _dbContext.scc_invoice.ToList();
            if (facturas == null)
            {
                return Ok(new List<object>());
            }

            var parsedFacturas = facturas.Select(d => new
            {
                d.id,
                d.partner,
                d.orderNumber,
                d.invoice,
                d.date,
                d.expire,
                d.amount,
                d.currency,
                d.payments,
                d.expense,
                d.company,
                d.branch,
                concepts = JArray.Parse(d.concepts),
                d.notes,
                d.withholding,
                d.globaltax,
                d.purchase,
                d.justification,
                d.statusi,
                d.statuse,
                d.created_at,
                d.updated_at,
                d.deleted_at
            }).ToList();

            // Devolver los campos seleccionados como JSON
            return Content(HttpStatusCode.OK, parsedFacturas, Configuration.Formatters.JsonFormatter);
        }

        // GET api/Factura/5
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetFactura(int id)
        {
            var factura = _dbContext.scc_invoice.FirstOrDefault(h => h.id == id);
            if (factura == null)
            {
                return NotFound();
            }

            var parsedFactura = new
            {
                factura.id,
                factura.partner,
                factura.orderNumber,
                factura.invoice,
                factura.date,
                factura.expire,
                factura.amount,
                factura.currency,
                factura.payments,
                factura.expense,
                factura.company,
                factura.branch,
                concepts = JArray.Parse(factura.concepts),
                factura.notes,
                factura.withholding,
                factura.globaltax,
                factura.purchase,
                factura.justification,
                factura.statusi,
                factura.statuse,
                factura.created_at,
                factura.updated_at,
                factura.deleted_at
            };

            return Ok(parsedFactura);
        }

        // POST api/Factura
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostFactura(scc_invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos de la factura no válidos.");
            }
            /*bool codigoUnico = !_dbContext.scc_invoice.Any(d => d.orderNumber == invoice.orderNumber);
            if (!codigoUnico)
            {
                return BadRequest("El código ya está en uso.");
            }*/
            invoice.created_at = DateTime.Now;
            invoice.updated_at = DateTime.Now;
            _dbContext.scc_invoice.Add(invoice);
            _dbContext.SaveChanges();

            return Ok("Factura creada correctamente.");
        }

        // PUT api/Factura/5
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult PutFactura(int id, scc_invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != invoice.id)
            {
                return BadRequest();
            }
            var logExistente = _dbContext.scc_invoice.FirstOrDefault(p => p.id == id);

            if (logExistente == null)
            {
                return NotFound();
            }

            /*bool codigoUnico = !_dbContext.scc_invoice.Any(d => d.id != id && d.orderNumber == invoice.orderNumber);
            if (!codigoUnico)
            {
                return BadRequest("El código ya está en uso por otra factura.");
            }*/
            invoice.updated_at = DateTime.Now;
            foreach (var property in typeof(scc_invoice).GetProperties())
            {
                var newValue = property.GetValue(invoice);
                if (newValue != null && property.Name != "created_at")
                {
                    property.SetValue(logExistente, newValue);
                }
            }


            try
            {
                _dbContext.SaveChanges();
                return Ok("Factura actualizada exitosamente");
            }
            catch (Exception)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError();
                }
            }
        }

        // PUT api/Factura/Eliminar/5
        [HttpPut]
        [Route("Eliminar/{id}")]
        public IHttpActionResult PutEliminarFactura(int id)
        {
            var documento = _dbContext.scc_invoice.FirstOrDefault(h => h.id == id);
            if (documento == null)
            {
                return NotFound();
            }

            documento.deleted_at = DateTime.Now;
            try
            {
                _dbContext.SaveChanges();
                return Ok("Factura eliminada lógicamente.");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // DELETE api/Factura/5
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteFactura(int id)
        {
            var facturaExistente = _dbContext.scc_invoice.FirstOrDefault(p => p.id == id);

            if (facturaExistente == null)
            {
                return NotFound();
            }

            try
            {
                _dbContext.scc_invoice.Remove(facturaExistente);
                _dbContext.SaveChanges();
                return Ok("Factura eliminada exitosamente");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        private bool InvoiceExists(int id)
        {
            return _dbContext.scc_invoice.Any(e => e.id == id);
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
