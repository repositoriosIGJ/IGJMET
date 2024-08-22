using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class GenericException: Exception
    {
        public GenericException()
            : base()
        { 
        
        }

        public GenericException(string mensaje)
            :base(mensaje)
        {

        }

        public GenericException(string mensaje, Exception innerException)
            : base(mensaje, innerException)
        {

        }
    }
}
