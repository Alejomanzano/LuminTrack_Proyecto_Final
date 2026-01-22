using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace LuminTrack.Models
{
    public class Luminaria
    {
        public int Id { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        [StringLength(50)]
        public string CodigoLuminaria { get; set; }

        [Required]
        public double AlturaPoste { get; set; }

        [Required]
        public int Potencia { get; set; }

        public bool TienePanelSolar { get; set; }

        [Required]
        public string Estado { get; set; }

        public string Ubicacion { get; set; }
    }
}