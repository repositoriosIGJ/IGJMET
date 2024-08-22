using System;
using Domain;

namespace Domain.Contracts.Repositories
{
    public interface IBPMServiceClient
    {
        void RegistrarProceso(long nroTramite, int nroPresentacion, int nroCorrelativo, bool urgente);
    }
}
