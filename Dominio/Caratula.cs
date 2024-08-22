using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Caratula
    {
        public Sociedad Sociedad { get; set; }
        public int NroTramite { get; set; }
        public int CodTramite { get; set; }
        public Operacion Operacion { get; set; }
        public int NroPresentacion { get; set; }
        public long NroFormulario { get; set; }
        public DateTime FechaTimbrado { get; set; }
        public bool Urgente { get; set; }
        private string _codigoUnico;
        private string _titulo;

        public string TituloCaratula 
        {
            get
            {
                if (String.IsNullOrEmpty(_titulo))
                    _titulo = GenerarTitulo();
                
                return _titulo;
            }
        }

        public string CodigoUnico
        {
            get 
            {
                if (String.IsNullOrEmpty(_codigoUnico))
                    _codigoUnico = NroTramite.ToString() + NroPresentacion.ToString("0000");
                return _codigoUnico;
            }

        }

        public Caratula()
        {
            
        }

        private string GenerarTitulo()
        {
            switch (Operacion.TipoOperacion)
            {
                case TipoOperacion.CON_TIMBRADO:
                    return "TRAMITE CON TIMBRADO";
                case TipoOperacion.SIN_TIMBRADO:
                    return "TRAMITE SIN TIMBRADO";
                case TipoOperacion.AGREGAR_TIMBRADO:
                    return "AGREGAR TIMBRADO";
                case TipoOperacion.REIMPRESION:
                    return "REIMPRESION";
                case TipoOperacion.VISTAS:
                    return "CONTESTACION DE VISTAS";
                default:
                    return "";
            }
        }
    }
}
