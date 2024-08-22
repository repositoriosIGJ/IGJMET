using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SociedadModel
    {
        public long NroCorrelativo { get; set; }
        public string RazonSocial { get; set; }
        public string Tipo { get; set; }
        public int Resultado { get; set; }
        public string Mensaje { get; set; }
    }
}
