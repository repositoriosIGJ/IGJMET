using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public abstract class ServiceEnteException : GenericException
    {
        public ServiceEnteException(string mensaje, Exception innerException)
            :base(mensaje, innerException)
        {

        }
    }
}
