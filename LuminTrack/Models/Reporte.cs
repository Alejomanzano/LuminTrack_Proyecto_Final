using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LuminTrack.Models
{
    public class Reporte
    {
        public int Id { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public string Categoria { get; set; }

        // SOLO si Categoria = "Otros"
        [StringLength(300)]
        public string OtraCategoria { get; set; }

        [Required]
        public string Parroquia { get; set; }

        [Required]
        [StringLength(10)]
        public string CodigoPostal { get; set; }

        public string FotoURL { get; set; }

        public int PrioridadIA { get; set; } = 0;

        public string Estado { get; set; } = "Enviado";

        
        public string UsuarioEmail { get; set; }

    }
}