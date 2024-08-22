using System;
using Domain.Models;

namespace Domain.Contracts.Services
{
    public interface ICaratulaService
    {
        ReporteModel GetCaratulaPorFormulario(string codFormulario, string usr);
        ReporteModel GetCaratulaPorTramite(int nroTramite, string usr);
        ReporteModel GetCaratulaPorOperacion(long codOepracion);
    }
}
