using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Logics
{
    public interface ICaratulaLogic
    {
        Caratula GetReportePorFormulario(string codFormulario, string usr);
        Caratula GetReportePorOperacion(long codOperacion);
        Caratula GetReportePorTramite(int nroTramite, string usr);
    }
}
