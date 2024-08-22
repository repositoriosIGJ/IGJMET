using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ServiceBPMComunicationException : GenericException
    {
        public ServiceBPMComunicationException(string mensaje, Exception ex)
            :base(mensaje, ex)
        {

        }
    }
}
