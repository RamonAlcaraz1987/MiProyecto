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
    public class Compra_Carta{
        [Key]
        public int IdCompra { get; set; }
        [Key]
        public int IdCarta { get; set; }

        [Required]
        public int Cantidad { get; set; }
       
    }
    
}