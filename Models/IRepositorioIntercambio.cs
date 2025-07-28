using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiProyecto.Models
{
    public interface IRepositorioIntercambio : IRepositorio<Intercambio>
    {
        int CrearIntercambio(Intercambio intercambio, List<IntercambioCarta> cartasEmisor, List<IntercambioCarta> cartasReceptor);
        bool TieneIntercambioPendiente(int idUsuario);
        bool AceptarIntercambio(int idIntercambio);
        bool CancelarIntercambio(int idIntercambio);
        IList<Intercambio> ObtenerPorUsuario(int idUsuario);
        int ContarTodos();
        List<IntercambioCarta> ObtenerCartasIntercambio(int idIntercambio);
        Intercambio ObtenerPorId(int idIntercambio);
        


    }
}