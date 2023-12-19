using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace APISociosComerciales48.Models
{
    public class CnnDbContext: DbContext
    {
        public CnnDbContext() : base("cnnDataBase")
        {
        }
        public DbSet<_EntidadesSocios> _EntidadesSocios { get; set; }
        public DbSet<_EntidadContactos> _EntidadContactos { get; set; }
        public DbSet<_EntidadSocioDocumentos> _EntidadSocioDocumentos { get; set; }
        public DbSet<historial_documento> historial_documento { get; set; }
        public DbSet<scc_documentos> scc_documentos { get; set; }
        public DbSet<scc_invoice> scc_invoice { get; set; }
        public DbSet<scc_scs_dcm_flujo> scc_scs_dcm_flujo { get; set; }
    }
}