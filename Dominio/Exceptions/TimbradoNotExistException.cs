using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class TimbradoNotExistException : GenericException
    {
        public TimbradoNotExistException()
        { 
        
        }

        public TimbradoNotExistException(string mensaje):base(mensaje)
        {

        }

        public TimbradoNotExistException(string mensaje, Exception ex)
            : base(mensaje, ex)
        { 
        
        }
    }
}
