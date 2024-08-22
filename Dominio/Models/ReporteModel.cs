using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public enum TipoReporte
    { 
        Formulario,
        Tramite
    }

    public class ReporteModel
    {
        public TipoReporte TipoReporte { get; set; }
        public int Resultado { get; set; }
        public string Mensaje { get; set; }
        public int NroTramite { get; set; }
        public string CodigoFormulario { get; set; }
        public long NroFormulario { get; set; }
        public int NroPresentacion { get; set; }
        public bool Urgente { get; set; }
        public long NroCorrelativo { get; set; }
        public string TipoSocietario { get; set; }
        public string Denominacion { get; set; }
        public long NroOperacion { get; set; }
    }
}
