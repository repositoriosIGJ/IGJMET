using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Formulario
    {
        private string _tipoFormulario;

        public long NroFormulario { get; set; }

        public int NroCorrelativo { get; set; }

        public string CodBarra { get; set; }

        private string _codBanelco { get; set; }

        public string TipoDecodificado { get; set; }

        public string CodControl { get; set; }

        public DateTime Fecha { get; set; }

        public bool Urgente { get; set; }

        public long IdNomenclador { get; set; }

        public short IdEstado { get; set; }

        public int IdFirmante { get; set; }

        public string TipoFormulario
        {
            get
            {
                if (string.IsNullOrEmpty(_tipoFormulario))
                    _tipoFormulario = CodBarra.Substring(11, 2);
                
                return _tipoFormulario;
            }

            set
            {
                _tipoFormulario = value;
            }
        }

        public string CodigoBarraDecodificado
        {
            get { return ObtenerCodigoBarraCodificado(); }
        }

        public string CodigoBanelco
        {
            get
            {
                if (string.IsNullOrEmpty(_codBanelco))
                    _codBanelco = CodBarra.Substring(1, 10);

                return _codBanelco;
            }

            set
            {
                _codBanelco = value;
            }
        }

        public Formulario()
        {

        }

        public Formulario(string codigoBarra)
        {
            CodBarra = codigoBarra;
            ObtenerDatosFormulario(codigoBarra);
        }

        public void ObtenerDatosFormulario(string codigoBarra)
        {
            var dato = CodBarra.Substring(13, 6);
            Fecha = DateTime.ParseExact(dato, "yyMMdd", null);
            dato = CodBarra.Substring(19, 2);
            if (string.Compare(dato, "01") == 0)
                Urgente = true;

            CodControl = codigoBarra.Substring(1, 10);
        }

        private string ObtenerCodigoBarraCodificado()
        {
            string codigo = CodBarra.Replace(TipoFormulario, TipoDecodificado);
            //El del codigo de barra se elimina el dia de la fecha en formato yyMMdd
            return codigo.Remove(17, 2);
            //return codigo.Substring(0, 17) + string.Format("{0:00}", Urgente);
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;

            if (Object.ReferenceEquals(obj, this))
                return true;

            var otroFormulario = (Formulario)obj;

            return this.CodBarra == otroFormulario.CodBarra
                && this.NroFormulario == otroFormulario.NroFormulario
                && this.TipoFormulario == otroFormulario.TipoFormulario
                && this.Urgente == otroFormulario.Urgente
                && this.Fecha == otroFormulario.Fecha;
        }

        public static long GetNroFormulario(string codigoFormulario)
        {

            return long.Parse(codigoFormulario.Substring(0, 11));
        }
    }
}
