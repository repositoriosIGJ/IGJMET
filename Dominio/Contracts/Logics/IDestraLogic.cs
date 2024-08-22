using System;

namespace Domain.Contracts.Logics
{
    public interface IDestraLogic
    {
        bool AptoContestacionVista(int nroTramite);
        bool TramiteArchivado(int nroTramite);

    }
}
