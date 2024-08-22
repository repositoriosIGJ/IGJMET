using System;
using System.Collections.Generic;

namespace Domain.Contracts.Repositories
{
    public interface IOperacionRepositorio
    {
        Operacion ObtenerPorNroOperacion(long nroOperacion);
        IList<Operacion> ObtenerPorTramite(int nroTramite);
        long Guardar(Operacion operacion);
        void EliminarPorTramite(int nroTramite);
    }
}
