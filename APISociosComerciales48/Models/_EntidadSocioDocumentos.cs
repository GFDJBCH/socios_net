using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APISociosComerciales48.Models
{
    public class _EntidadSocioDocumentos
    {
        [Key]
        public int id { get; set; }

        public int socio { get; set; }
        public int documento { get; set; }

        [MaxLength(100)]
        public string nombre { get; set; }

        [DataType(DataType.Date)]
        public DateTime? emision { get; set; }

        [DataType(DataType.Date)]
        public DateTime? vigencia { get; set; }

        [MaxLength(255)]
        public string url { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime fch_creacion { get; set; }

        [MaxLength(50)]
        public string estado { get; set; }
    }
}