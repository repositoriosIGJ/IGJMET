using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public enum Estado
    {
        OK,
        ER,
        ER_CON,
        FI
    }

    public class Timbrado
    {
        public string Control { get; set; }
        public decimal Monto { get; set; }
        public short Caja { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizado { get; set; }
        public DateTime FechaOperacion { get; set; }
        public Formulario Formulario { get; set; }
        public Estado Estado { get; set; }
        public int NroTramite { get; set; }
        public int? CodTramite { get; set; }
        public int NroCorrelativo { get; set; }
        public int NroPagoElectronico { get; set; }
        public long NroFormulario { get; set; }
        public int IdUsuario { get; set; }
        public Timbrado()
        {
            Formulario = new Formulario();
        }

        public bool EnUso()
        {
            return (Estado == Domain.Estado.OK || Estado == Domain.Estado.FI);
        }

        public bool SinProcesar()
        {
            return (Estado == Domain.Estado.ER);
        }

        public string MostrarEstado()
        {
            if (this.EnUso())
                return "Procesado";
            return "Error";
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;

            if (Object.ReferenceEquals(obj, this))
                return true;

            Timbrado otroTimbrado = (Timbrado)obj;

            return this.Estado == otroTimbrado.Estado
                && this.FechaCreacion.ToString("dd/MM/yyyy hh:mm:ss") == otroTimbrado.FechaCreacion.ToString("dd/MM/yyyy hh:mm:ss")
                && this.NroTramite == otroTimbrado.NroTramite;
        }

        public static Timbrado ObtenerUltimoProcesado(IList<Timbrado> timbrados)
        {
            var ultimo = timbrados.OrderByDescending(t => t.FechaCreacion).SingleOrDefault();

            if (ultimo == null)
                throw new Exception();

            return ultimo;
        }
    }
}
