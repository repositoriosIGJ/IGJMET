//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAO
{
    using System;
    using System.Collections.Generic;
    
    public partial class TRANSACCION
    {
        public long IDTRANSACCION { get; set; }
        public short IDESTADO { get; set; }
        public int IDFIRMANTE { get; set; }
        public Nullable<int> NROCORRELATIVO { get; set; }
        public string MOTIVO { get; set; }
        public System.DateTime FECHAALTA { get; set; }
        public Nullable<System.DateTime> FECHAINGRESO { get; set; }
        public long IDNOMENCLADOR { get; set; }
        public decimal URGENTE { get; set; }
        public string CODBARRA { get; set; }
        public byte[] IMAGENCODBARRA { get; set; }
    }
}
