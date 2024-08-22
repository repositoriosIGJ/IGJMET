using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Sociedad
    {
        public long NroCorrelativo { get; set; }

        public string Nombre { get; set; }

        public TipoSociedad TipoSociedad { get; set; }

        public List<Tramite> Tramites { get; set; }

        public Sociedad()
        {
            Tramites = new List<Tramite>();
        }
    }
}
