using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class TramiteNotExistException : GenericException
    {
        public TramiteNotExistException()
        {

        }

        public TramiteNotExistException(string mensaje)
            :base(mensaje)
        {

        }
    }
}
