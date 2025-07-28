using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiProyecto.Models;

namespace MiProyecto.Models
{
    public interface IRepositorioCompra : IRepositorio<Compra>
    { 

        void AgregarCartasACompra(int idCompra, List<int> idsCartas);
        IList<Compra> BuscarPorDNIUsuario(string DNI);
        int ContarTodos();
    }
}