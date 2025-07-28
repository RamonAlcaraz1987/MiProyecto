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
    public class Carta
    {
        [Key]
        [Display(Name = "N° de Carta")]
        public int IdCarta { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public int IdCategoria { get; set; }
        public Categoria? Categoria { get; set; }

        [Required]
        [Display(Name = "Tipo 1")]
        public int IdTipo1 { get; set; }
        public Tipo? Tipo1 { get; set; }

        [Display(Name = "Tipo 2")]
        public int IdTipo2 { get; set; }
        public Tipo? Tipo2 { get; set; }

        [Required]
        public int Estado { get; set; }

        [Required]
        public int ValorEstimado { get; set; }

        
        public string Imagen { get; set; }

        [NotMapped]
        public IFormFile ImagenFile { get; set; }

    }
}


