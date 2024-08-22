using System;
using System.Collections.Generic;

namespace Domain.Contracts.Repositories
{
    public interface IVistaRepositorio
    {
        IList<Vista> GetVistas(int nroTramite, int nroCorrelativo);
    }
}
