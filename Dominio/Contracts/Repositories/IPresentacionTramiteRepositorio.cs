using System;
using System.Collections.Generic;

namespace Domain.Contracts.Repositories
{
    public interface IPresentacionTramiteRepositorio
    {
        void Guardar(PresentacionTramite presentacion);
        IList<PresentacionTramite> ObtenerTodos(int nroTramite);
        PresentacionTramite ObtenerUltimo(int nroTramite);
        void Eliminar(PresentacionTramite presentacion);
    }
}
