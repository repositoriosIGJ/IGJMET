using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Domain.Contracts.Services
{
    public interface ITimbradoService
    {
        Timbrado GetPorFormulario(Formulario formulario);
    }
}
