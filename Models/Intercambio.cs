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
    public class Intercambio
    {
        public int IdIntercambio { get; set; }
        [Required]
        public int IdUsuarioEmisor { get; set; }
        [Required]
        public int IdUsuarioReceptor { get; set; }
        [Required]
        public int IdColeccionEmisor { get; set; }
        [Required]
        public int IdColeccionReceptor { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public int Estado { get; set; }
        public Coleccion ColeccionEmisor { get; set; }
        public Coleccion ColeccionReceptor { get; set; }
        public Usuario Emisor { get; set; }
        public Usuario Receptor { get; set; }
        [NotMapped]
        public List<IntercambioCarta> Cartas { get; set; } = new List<IntercambioCarta>();
        
    }


    public class IntercambioViewModel
    {
        public int IdColeccionEmisor { get; set; }
        public int IdColeccionReceptor { get; set; }
        public List<IntercambioCartaViewModel> CartasEmisor { get; set; }
        public List<IntercambioCartaViewModel> CartasReceptor { get; set; }
    }

    public class IntercambioCartaViewModel
    {
        public int IdCarta { get; set; }
        public int Cantidad { get; set; }
    }

    public class IntercambioEditViewModel
{
    [Required]
    public int IdIntercambio { get; set; }

    [Required]
    public DateTime Fecha { get; set; }
}


}