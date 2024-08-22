using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TrazabilidadTramite
    {
        public int NroTramite { get; set; }

        public string CodTramite { get; set; }

        public int NroCorrelativo { get; set; }

        public string Origen { get; set; }

        public string Destino { get; set; }

        public DateTime FechaTramite { get; set; }

        public DateTime FechaDesde { get; set; }

        public DateTime FechaHasta { get; set; }

        public Usuario Usuario { get; set; }

        public TrazabilidadTramite()
        {
            Usuario = new Usuario();
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;

            if (Object.ReferenceEquals(obj, this))
                return true;

            TrazabilidadTramite trazabilidad = (TrazabilidadTramite)obj;

            return this.NroTramite == trazabilidad.NroTramite
                && this.NroCorrelativo == trazabilidad.NroCorrelativo
                && this.Destino == trazabilidad.Destino
                && this.FechaHasta.ToString("dd/MM/yyyy hh:mm:ss") == trazabilidad.FechaHasta.ToString("dd/MM/yyyy hh:mm:ss");
        }
    }
}