using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Logics
{
    public interface IModuloTramiteLogic
    {
        ModuloTramite GetByFormulario(Formulario formulario);
    }
}
