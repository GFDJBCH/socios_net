using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APISociosComerciales48.Models
{
    public class scc_scs_dcm_flujo
    {
        public int id { get; set; }
        public int scs_documentos { get; set; }
        public int usuarios { get; set; }
        public string comentario { get; set; }
        public DateTime fecha { get; set; }
        public string estado { get; set; }
    }
}