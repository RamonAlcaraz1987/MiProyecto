using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MiProyecto.Models
{
    public class LoginView
    {
       
       
        [DataType(DataType.EmailAddress)]
        public string Usuario { get; set; }
        
        
        [DataType(DataType.Password)]
        public string Clave { get; set; }
    }
}