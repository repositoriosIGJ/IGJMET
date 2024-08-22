using System;

namespace Domain.Exceptions
{
    public class TimbradoAlreadyInProcessException : GenericException
    {
        public TimbradoAlreadyInProcessException(string mensaje)
            :base(mensaje)
        {
                
        }
    }
}
