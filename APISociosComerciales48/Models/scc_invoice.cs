using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APISociosComerciales48.Models
{
    public class scc_invoice
    {
        public int id { get; set; }
        public int partner { get; set; }
        public string orderNumber { get; set; }
        public string invoice { get; set; }
        public DateTime date { get; set; }
        public string expire { get; set; }
        public decimal? amount { get; set; }
        public string currency { get; set; }
        public string payments { get; set; }
        public string expense { get; set; }
        public int? company { get; set; }
        public int? branch { get; set; }
        public string concepts { get; set; }
        public string notes { get; set; }
        public decimal? withholding { get; set; }
        public decimal? globaltax { get; set; }
        public string purchase { get; set; }
        public string justification { get; set; }
        public string statusi { get; set; }
        public string statuse { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }
}