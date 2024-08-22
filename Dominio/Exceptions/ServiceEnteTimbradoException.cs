using System;

namespace Domain.Exceptions
{
    public class ServiceEnteTimbradoException: ServiceEnteException
    {
        public ServiceEnteTimbradoException(string mensaje)
            :this(mensaje, null)
        { 
        
        }

        public ServiceEnteTimbradoException(string mensaje, Exception innerException)
            : base(mensaje, innerException)
        {

        }
    }
}
