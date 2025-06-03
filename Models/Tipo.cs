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
    public class Tipo
    {
        [Key]
        public int IdTipo { get; set; }
        public string Descripcion { get; set; }
    }
}