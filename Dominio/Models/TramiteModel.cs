using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public enum TipoTramiteModel
    {
        Pago,
        NoPago
    }

    public class TramiteModel
    {
        public TramiteModel() 
        {
            TipoSociedadList = new List<TipoSociedad>();
        }

        public string CodigoFormulario { get; set; }

        public long NroFormulario { get; set; }

        public int Resultado { get; set; }

        public string Mensaje { get; set; }

        public int NroTramite { get; set; }

        public int NroPresentacion { get; set; }

        public int NroCorrelativo { get; set; }

        public string CodTramite { get; set; }

        public Sociedad Sociedad { get; set; }

        public TipoTramiteModel Tipo { get; set; }

        public long NroOperacion { get; set; }

        public IList<Timbrado> Timbrados { get; set; }

        public TipoTramite TipoTramite { get; set; }

        public bool Imprime { get; set; }
        public IList<TipoSociedad> TipoSociedadList { get; set; }
        public int SelectedTipoSociedad { get; set; }
        public int SelectedTipoTramiteId { get; set; }
        public string SelectedTipoSociedadCode { get; set; }
        public IList<TipoTramite> TipoTramiteList {get; set;}
        public string CodigoTipoDeTramite { get; set; }
    }
}
