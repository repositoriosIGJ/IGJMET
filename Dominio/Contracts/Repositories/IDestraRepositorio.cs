using System;
using System.Collections.Generic;

namespace Domain.Contracts.Repositories
{
    public interface IDestraRepositorio
    {
        IList<TrazabilidadTramite> ObtenerPorTramite(int nroTramite);
        TrazabilidadTramite ObtenerUltimoEstadoTramite(int nroTramite);
        void Guardar(TrazabilidadTramite trazabilidad);
        void ActualizarUltimoEstado(TrazabilidadTramite trazabilidad);
        void Eliminar(TrazabilidadTramite trazabilidad);
    }
}
