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
    public class Coleccion_Carta
    {
        [Key]
        public int IdColeccion{ get; set; }
       
        [Key]
        public int IdCarta { get; set; }

        public int Cantidad { get; set; }
    }
}