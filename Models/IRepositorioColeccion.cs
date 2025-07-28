using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiProyecto.Models;

namespace MiProyecto.Models
{
    public interface IRepositorioColeccion : IRepositorio<Coleccion>
    {
        IList<Coleccion> ObtenerTodosFiltrados(int pagina, int tamPagina, int idUsuarioLogueado, bool esAdmin);
        int ContarTodos(int idUsuarioLogueado, bool esAdmin);
        IList<Coleccion> BuscarPorDNIUsuario(string dni);
        void AgregarCartasAColeccion(List<int> idsCartas, int idColeccion);
        IList<Coleccion> BuscarPorIdUsuario(int idUsuario);
    }
}