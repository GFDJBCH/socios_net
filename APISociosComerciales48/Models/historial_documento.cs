using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APISociosComerciales48.Models
{
    public class historial_documento
    {
        [Key]
        public int id { get; set; }

        public int socio { get; set; }
        public int documento { get; set; }
        public int? usuario { get; set; }

        public string comentario { get; set; }

        public string estado { get; set; }

        public string url { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}