using System;
using Domain;

namespace Domain.Contracts.Repositories
{
    public interface IENTEServiceClient
    {
        Timbrado ConsultarTimbrado(Formulario formulario, long nroTramite, Usuario usuario);
    }
}
