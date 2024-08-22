using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ServiceEnteComunicationException: ServiceEnteException
    {
        public ServiceEnteComunicationException(string mensaje, Exception innerException)
            :base(mensaje, innerException)
        {

        }
    }
}
