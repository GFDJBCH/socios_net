using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APISociosComerciales48.Models
{
    public class scc_documentos
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string nombre_ingles { get; set; }
        public string descripcion { get; set; }
        public string description_english { get; set; }
        public string codigo { get; set; }
        public bool fisica { get; set; }
        public string fisica_periodo { get; set; }
        public string fisica_notificacion { get; set; }
        public bool moral { get; set; }
        public string moral_periodo { get; set; }
        public string moral_notificacion { get; set; }
        public bool? extranjero { get; set; }
        public string extranjero_periodo { get; set; }
        public string extranjero_notificacion { get; set; }
        public string unidades { get; set; }
        public string sucursales { get; set; }
        public string flujos { get; set; }
        public DateTime? fch_creacion { get; set; }
        public DateTime? fch_borrar { get; set; }
    }
}