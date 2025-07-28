using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiProyecto.Models;

namespace MiProyecto.Models
{
    public interface IRepositorioCarta: IRepositorio<Carta>
    {
        public int ContarTodos();
        public Carta ObtenerPorNombre(string Nombre);
        public IList<Carta> ObtenerPortada();
        IList<Carta> BuscarPorNombre(string Nombre);
        public List<int> ObtenerCartasAleatorias(int idCategoria, int cantidad);
        public IList<Carta> ObtenerPorIds(IList<int> ids);
        IList<Coleccion> BuscarPorNombre(string nombre, int idUsuarioLogueado, bool esAdmin);
    }
}