namespace DAO
{
    using System;
    using System.Collections.Generic;

    public partial class PRESENTACION_TRAMITE
    {
        public int PRTNROTRAM { get; set; }
        public short PRTNROPRES { get; set; }
        public System.DateTime PRTFECHAPRES { get; set; }
        public string PRTUSUACTU { get; set; }
        public Nullable<int> PRTNROCORR { get; set; }
        public string PRTCODTRAM { get; set; }
        public Nullable<System.DateTime> PRTFECHACT { get; set; }
        public Nullable<short> PRTACTUACION { get; set; }
    }
}