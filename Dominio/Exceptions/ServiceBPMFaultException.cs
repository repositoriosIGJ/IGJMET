using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ServiceBPMFaultException: GenericException
    {
        public ServiceBPMFaultException(string mensaje, Exception ex)
            :base(mensaje, ex)
        {

        }
    }
}
