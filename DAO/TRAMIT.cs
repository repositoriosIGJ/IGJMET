namespace DAO
{
    using System;
    using System.Collections.Generic;

    public partial class TRAMIT
    {
        public int TRANROCORR { get; set; }
        public string TRACODHABI { get; set; }
        public string TRACODTRAM { get; set; }
        public System.DateTime TRAFECHACT { get; set; }
        public Nullable<System.DateTime> TRAFECHAFT { get; set; }
        public string TRAREGTRAM { get; set; }
        public Nullable<int> TRAREFTRAM { get; set; }
        public Nullable<int> TRANROTRAM { get; set; }
        public Nullable<short> TRAGRPTRAM { get; set; }
        public Nullable<short> TRAFOJAS { get; set; }
        public string TRACONEXPE { get; set; }
        public Nullable<System.DateTime> TRAFECREG { get; set; }
        public Nullable<System.DateTime> TRAFECREGNA { get; set; }
        public string TRADESISTE { get; set; }
        public string TRAREQCONTABLE { get; set; }
        public string TRADIGITAL { get; set; }
    }
}