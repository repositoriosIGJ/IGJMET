using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Tramite
    {
        public int NroTramite { get; set; }

        public int NroCorrelativo { get; set; }

        public string CodTramite { get; set; }

        public string Registro { get; set; }

        public DateTime Fecha { get; set; }

        public IList<PresentacionTramite> Presentaciones { get; set; }

        public int UltimoNroPresentacion { get; set; }

        public IList<Timbrado> Timbrados { get; set; }

        public IList<VistaTramite> Vistas { get; set; }

        public IList<Operacion> Operaciones { get; set; }

        public Nomenclador Nomenclador { get; set; }

        public TipoTramite TipoTramite { get; set; }

        public bool TipoBPM { get; set; }

        public Tramite():this(0)
        {

        }

        public Tramite(int nroTramite)
        {
            NroTramite = nroTramite;
            Fecha = DateTime.Now;
            
            Timbrados = new List<Timbrado>();
            Presentaciones = new List<PresentacionTramite>();
            Vistas = new List<VistaTramite>();
            Operaciones = new List<Operacion>();
        }
    }
}
