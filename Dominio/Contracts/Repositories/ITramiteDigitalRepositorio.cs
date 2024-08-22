using System;
using System.Collections.Generic;

namespace Domain.Contracts.Repositories
{
    public interface ITramiteDigitalRepositorio
    {
        TramiteDigital ObtenerPorNroTramite(int nroTramite);
        IList<TramiteDigital> ObtenerTodosPorNroTramite(int nroTramite);
        void Guardar(TramiteDigital tramiteDigital);
        void Eliminar(TramiteDigital tramiteDigital);
    }
}
