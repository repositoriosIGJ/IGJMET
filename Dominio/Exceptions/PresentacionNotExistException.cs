using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class PresentacionNotExistException : GenericException
    {
        public PresentacionNotExistException(string mensaje)
            :base(mensaje)
        {

        }
    }
}
