using System;

namespace Domain.Models
{
    public class PresentacionModel
    {
        public int Resultado { get; set; }
        public string Mensaje { get; set; }
        public int NroTramite { get; set; }
        public string CodTramite { get; set; }
        public int NroPresentacion { get; set; }
        public int NroCorrelativo { get; set; }
        public string NombreSociedad { get; set; }
        public string TipoSociedad { get; set; }
        public long NroOperacion { get; set; }
        public Boolean Desarchivar { get; set; }
        public String CodigoFormulario { get; set; }
    }
}
