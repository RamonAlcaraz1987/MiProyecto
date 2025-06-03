using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiProyecto.Models;

namespace MiProyecto.Models
{
    public interface IRepositorioUsuario: IRepositorio<Usuario>
    {
        Usuario ObtenerPorEmail(string email);
    }
}