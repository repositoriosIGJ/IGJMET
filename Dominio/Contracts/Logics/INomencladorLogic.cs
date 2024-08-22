using System;
using System.Collections.Generic;
using Domain;

namespace Domain.Contracts.Logics
{
    public interface INomencladorLogic
    {
        bool TramiteTipoBPM(Timbrado timbrado);
    }
}
