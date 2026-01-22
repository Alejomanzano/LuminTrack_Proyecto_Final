using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LuminTrack.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Rol { get; set; }

        

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}