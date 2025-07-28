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
    public class Coleccion
    {
        [Key]
        public int IdColeccion { get; set; }
        
        [Required]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        [Required]
        public String Nombre { get; set; }

        [Required]
        public int EsPublica { get; set; }
        
        [Required]
        public int Estado { get; set; }

        public List<Carta> Cartas { get; set; }
        
    }
}