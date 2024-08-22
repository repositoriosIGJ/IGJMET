using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class TrazabilidadNotExistException: GenericException
    {
        public TrazabilidadNotExistException()
            :base()
        {

        }

        public TrazabilidadNotExistException(string msg)
            :base(msg)
        {

        }
    }
}
