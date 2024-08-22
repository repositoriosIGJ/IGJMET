using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public enum Estado
    { 
        OK,
        ERR
    }

    public class ProcesoModel
    {
        public Estado Estado { get; set; }
        public string Descripcion { get; set; }

        public ProcesoModel(Estado estado, string descripcion)
        {
            Estado = estado;
            Descripcion = descripcion;
        }
    }
}
