using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class PresentacionTramite
    {
        public int NroTramite { get; set; }

        public string CodTramite { get; set; }

        public int NroCorrelativo { get; set; }

        public int NroPresentacion { get; set; }

        public Usuario Usuario { get; set; }

        public DateTime Fecha { get; set; }

        public Operacion Operacion { get; set; }
 
        public PresentacionTramite()
        {
            Usuario = new Usuario();
        }

        public PresentacionTramite(int nroTramite, Usuario usuario, int nroPresentacion)
        {
            NroPresentacion = nroPresentacion;
            NroTramite = nroTramite;
            Usuario = usuario;
            Fecha = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;

            if (Object.ReferenceEquals(obj, this))
                return true;

            var otraPresentacion = (PresentacionTramite)obj;

            return this.NroTramite == otraPresentacion.NroTramite
                && this.NroPresentacion == otraPresentacion.NroPresentacion
                && this.Fecha.ToString("dd/MM/yyyy hh:mm:ss") == otraPresentacion.Fecha.ToString("dd/MM/yyyy hh:mm:ss")
                && this.Usuario == otraPresentacion.Usuario;
        } 
    }
}
