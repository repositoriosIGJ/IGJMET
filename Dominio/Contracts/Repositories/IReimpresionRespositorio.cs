using System;
using Domain;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.Repositories
{
    public interface IReimpresionRespositorio
    {
        Reimpresion ObtenerPorId(int idReimpresion);
        void Guardar(Reimpresion reimpresion);
        void Eliminar(Reimpresion reimpresion);
    }
}
