using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiProyecto.Models
{
    public interface IRepositorio<T> {


        int Alta(T p);
        int Baja(int d);
        int Modificacion(T p);

        IList<T> ObtenerTodos(int pagina, int tamPagina);

        T ObtenerPorId(int id);


    }
}