using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class FormularioNotExistException : GenericException
    {
        public FormularioNotExistException(string mensaje)
            : base(mensaje)
        {

        }
    }
}
