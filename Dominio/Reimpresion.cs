using System;


namespace Domain
{
    public class Reimpresion
    {
        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public string Usuario { get; set; }

        public DateTime Fecha { get; set; }

        public int NroTramite { get; set; }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;

            if (Object.ReferenceEquals(obj, this))
                return true;

            Reimpresion otraReimpresion = (Reimpresion)obj;

            return this.Id == otraReimpresion.Id;
        }
    }
}
