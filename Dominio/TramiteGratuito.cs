using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TramiteGratuito
    {
        public int NroTramite { get; set; }
        public int NroCorrelativo { get; set; }
        public int CodTramite { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime Fecha { get; set; }
    }
}
