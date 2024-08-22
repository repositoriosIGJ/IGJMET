using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Contracts.Services
{
    public interface IReporteService
    {
        ReporteModel GetPorFormulario(string codigoFormulario);
        ReporteModel GetPorFormularioYTramite(string codigoFormulario, int nroTramite);
        Stream GetCaratula(long nroOperacion, string usr);
        Stream GetCaratula(int nroCorrealtivo, long nroFormulario, int nroTramite, int nroPresentacion, long nroOperacion, string usr);
    }
}
