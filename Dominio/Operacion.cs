using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public enum TipoOperacion
    {
        CON_TIMBRADO = 1,
        AGREGAR_TIMBRADO = 2,
        SIN_TIMBRADO = 3,
        VISTAS = 4,
        REIMPRESION = 5
    }

    public class Operacion
    {
        public long NroOperacion { get; set; }

        public TipoOperacion TipoOperacion { get; set; }

        public decimal NroFormulario { get; set; }

        public decimal NroTramite { get; set; }

        public DateTime Fecha { get; set; }

        public decimal UsuarioID { get; set; }

        public Operacion()
        {
            
        }

        public Operacion(Timbrado timbrado, Usuario usuario, TipoOperacion tipoOperacion)
            :this(timbrado.Formulario.NroFormulario, timbrado.NroTramite, usuario, tipoOperacion)
        {

        }

        public Operacion(long nroFormulario, int nroTramite, Usuario usuario, TipoOperacion tipoOperacion)
        {
            NroFormulario = nroFormulario;
            NroTramite = nroTramite;
            UsuarioID = usuario.Id;
            TipoOperacion = tipoOperacion;
            Fecha = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;

            if (Object.ReferenceEquals(obj, this))
                return true;

            Operacion otraOperacion = (Operacion)obj;

            return this.NroFormulario == otraOperacion.NroFormulario
                && this.NroOperacion == otraOperacion.NroOperacion
                && this.NroTramite == otraOperacion.NroTramite
                && this.UsuarioID == otraOperacion.UsuarioID
                && this.Fecha.ToString("dd/MM/yy hh:mm:ss") == otraOperacion.Fecha.ToString("dd/MM/yy hh:mm:ss");
        }
    }
}
