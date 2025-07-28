using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


    namespace MiProyecto.Models
{
    public class IntercambioCarta
    {
        public int IdIntercambio { get; set; }
        public int IdCarta { get; set; }
        public int Cantidad { get; set; }
        public int EsDeEmisor { get; set; } 
        public Carta Carta { get; set; }
    }
}

