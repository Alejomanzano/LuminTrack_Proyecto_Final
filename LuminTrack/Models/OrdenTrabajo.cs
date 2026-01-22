using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LuminTrack.Models
{
    public class OrdenTrabajo
    {
        public int Id { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public string TecnicoEmail { get; set; }
        public int? ReporteId { get; set; }
        public int? LuminariaId { get; set; }
        public string FotoEvidenciaURL { get; set; }
    }
}