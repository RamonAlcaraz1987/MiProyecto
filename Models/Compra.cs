using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MiProyecto.Models
{
    public class Compra
    {
        [Key]
        public int IdCompra { get; set; }

        [Required]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        [Required]
        public int IdPack { get; set; }
        public Pack Pack { get; set; } 

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public int Estado { get; set; }

        public List<Carta> Cartas { get; set; } 
    }
}
   
