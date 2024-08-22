using System;
using Domain.Exceptions;

namespace Domain.Exceptions
{
    public class TipoTramiteException : GenericException
    {
        public TipoTramiteException()
        {

        }

        public TipoTramiteException(string msg)
            :base(msg)
        {

        }
    }
}
