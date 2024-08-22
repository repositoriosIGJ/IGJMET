using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum Destino
    {
        [Display(Name = "CICR")]
        CICR,
        [Display(Name = "CICU")]
        CICU,
        [Display(Name = "CIC2")]
        CIC2,
        [Display(Name = "OFE3")]
        OFE3,
        [Display(Name = "OFER")]
        OFER,
        [Display(Name = "OFE2")]
        OFE2,
        [Display(Name = "DCF3")]
        DCF3,
        [Display(Name = "DCF2")]
        DCF2


    }
}

