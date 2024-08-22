using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class VistaNotExistException:GenericException
    {
        public VistaNotExistException()
            :base()
        {

        }

        public VistaNotExistException(string msg)
            :base(msg)
        {

        }
    }
}
