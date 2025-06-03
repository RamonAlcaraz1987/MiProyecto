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

    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DNI { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Clave { get; set; }
        [Required]
        public string Rol { get; set; }
        [Required]
        public int PuntosVirtuales { get; set; }
        
        public string Avatar { get; set; }
        [NotMapped]
        public IFormFile AvatarFile { get; set; }
    }



}