using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class TRAMITE_GRATUITO
    {
        public int TGRNROTRAM { get; set; }
        public DateTime TGRFECHA { get; set; }
        public int TGRUSUARIO { get; set; }
        public Nullable<int> TGRNROCORR { get; set; }
        public Nullable<DateTime> TGRFECHACT { get; set; }
        public string TGRCODTRAM { get; set; }
    }
}