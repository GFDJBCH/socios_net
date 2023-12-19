using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APISociosComerciales48.Models
{
    public class _EntidadesSocios
    {
        public int id { get; set; }

        [MaxLength]
        public string nbr_negocio { get; set; }

        [MaxLength(200)]
        public string prs_nombre { get; set; }

        [MaxLength(200)]
        public string prs_apellidos { get; set; }

        [MaxLength]
        public string compania { get; set; }

        [MaxLength]
        public string rgm_capital { get; set; }

        [MaxLength]
        public string nbr_comercial { get; set; }

        [MaxLength]
        public string correo { get; set; }

        [MaxLength(20)]
        public string telefonoPublico { get; set; }

        [MaxLength]
        public string telefono { get; set; }

        [MaxLength]
        public string pagina { get; set; }

        [MaxLength]
        public string sectorCliente { get; set; }

        [MaxLength(4)]
        public string operaciones { get; set; }

        [MaxLength]
        public string capacidad { get; set; }

        public int? numero { get; set; }

        [MaxLength]
        public string idioma { get; set; }

        [MaxLength]
        public string calle { get; set; }

        [MaxLength]
        public string nmr_interno { get; set; }

        [MaxLength]
        public string nmr_externo { get; set; }

        [MaxLength]
        public string cdg_postal { get; set; }

        [MaxLength]
        public string colonia { get; set; }

        [MaxLength]
        public string pais { get; set; }

        [MaxLength]
        public string estado { get; set; }

        [MaxLength]
        public string ciudad { get; set; }

        [MaxLength]
        public string tp_proveedor { get; set; }

        [MaxLength]
        public string tax_id { get; set; }

        [MaxLength]
        public string lna_negocio { get; set; }

        [MaxLength]
        public string confianza { get; set; }

        [MaxLength]
        public string tipo_operacion { get; set; }

        [MaxLength]
        public string nacionalidad { get; set; }

        [MaxLength]
        public string dias_de_credito { get; set; }

        [MaxLength]
        public string limite_de_credito { get; set; }

        [MaxLength]
        public string sucursales { get; set; }

        [MaxLength]
        public string areas { get; set; }

        [MaxLength]
        public string justificacion { get; set; }

        [MaxLength]
        public string iso { get; set; }

        [MaxLength]
        public string seguridad { get; set; }

        [MaxLength]
        public string actividad { get; set; }

        [MaxLength]
        public string tipoEmpresa { get; set; }

        [MaxLength]
        public string sectorEmpresa { get; set; }

        [MaxLength(200)]
        public string correoPublico { get; set; }

        public int? Tipo { get; set; }

        public DateTime fch_creacion { get; set; }

        public bool activo { get; set; }
    }
}