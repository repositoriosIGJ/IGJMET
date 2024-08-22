using System;
using System.Collections.Generic;

namespace Domain.Contracts.Repositories
{
    public interface INomencladorRepositorio
    {
        IList<Nomenclador> GetNomenclados();
        Nomenclador Get(long Id);
    }
}
