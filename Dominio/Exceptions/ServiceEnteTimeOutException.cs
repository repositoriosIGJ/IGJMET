using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ServiceEnteTimeOutException: ServiceEnteException
    {
        public ServiceEnteTimeOutException(string mensaje, Exception innerException)
            : base(mensaje, innerException)
        {
            
        }
    }
}