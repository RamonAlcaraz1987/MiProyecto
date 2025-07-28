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
    public class Pack
    {
        [Key]
        public int IdPack { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [RegularExpression("^(Basico|Raro|Epico|Jumbo)$", ErrorMessage = "El nombre debe ser uno de los siguientes: Basico, Raro, Epico, Jumbo.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public int Precio { get; set; }

        [Required(ErrorMessage = "El total de cartas es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El total de cartas debe ser mayor a 0.")]
        public int TotalCartas { get; set; }

        [Required(ErrorMessage = "La probabilidad de rara es obligatoria.")]
        [Range(0, double.MaxValue, ErrorMessage = "La probabilidad de rara debe ser mayor o igual a 0.")]
        public decimal RaraChance { get; set; }

        [Required(ErrorMessage = "La probabilidad de épica es obligatoria.")]
        [Range(0, double.MaxValue, ErrorMessage = "La probabilidad de épica debe ser mayor o igual a 0.")]
        public decimal EpicaChance { get; set; }

        [Required(ErrorMessage = "La probabilidad de legendaria es obligatoria.")]
        [Range(0, double.MaxValue, ErrorMessage = "La probabilidad de legendaria debe ser mayor o igual a 0.")]
        public decimal LegendariaChance { get; set; }

        [Required(ErrorMessage = "Las raras garantizadas son obligatorias.")]
        [Range(0, int.MaxValue, ErrorMessage = "Las raras garantizadas deben ser mayores o iguales a 0.")]
        public int RaraGar { get; set; }

        [Required(ErrorMessage = "Las épicas garantizadas son obligatorias.")]
        [Range(0, int.MaxValue, ErrorMessage = "Las épicas garantizadas deben ser mayores o iguales a 0.")]
        public int EpicaGar { get; set; }

        [Required(ErrorMessage = "Las legendarias garantizadas son obligatorias.")]
        [Range(0, int.MaxValue, ErrorMessage = "Las legendarias garantizadas deben ser mayores o iguales a 0.")]
        public int LegGar { get; set; }

        public string Imagen { get; set; }

        [Required(ErrorMessage = "La leyenda es obligatoria.")]
        public string Leyenda { get; set; }

        [NotMapped]
        public IFormFile ImagenFile { get; set; }
    }
}