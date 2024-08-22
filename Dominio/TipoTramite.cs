using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TipoTramite
    {
        public int CodTramite { get; set; }
        public string Descripcion { get; set; }

        public TipoTramite()
        {

        }

        public TipoTramite(string descripcion)
        {
            Descripcion = descripcion;
        }
    }
}
