using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Contracts.Repositories
{
    public interface IGestionTramiteServiceClient
    {
        void Clasificar(String codigoBarra);
    }
}
