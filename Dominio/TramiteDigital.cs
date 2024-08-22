using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TramiteDigital
    {
        public int NroTramite { get; set; }

        public int NroPresentacion { get; set; }

        public int NroCorrelativo { get; set; }

        public DateTime Fecha { get; set; }

        public string Guid { get; set; }

        public TramiteDigital()
        {

        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;

            if (Object.ReferenceEquals(obj, this))
                return true;

            var otroTramiteDigital = (TramiteDigital)obj;

            return this.NroTramite == otroTramiteDigital.NroTramite
                && this.NroCorrelativo == otroTramiteDigital.NroCorrelativo
                && this.NroPresentacion == otroTramiteDigital.NroPresentacion
                && this.Guid == otroTramiteDigital.Guid
                && this.Fecha.ToString("dd/MM/yyyy hh:mm:ss") == otroTramiteDigital.Fecha.ToString("dd/MM/yyyy hh:mm:ss");
        }
    }
}
