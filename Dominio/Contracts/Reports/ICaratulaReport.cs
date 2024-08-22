using System.IO;

namespace Domain.Contracts.Reports
{
    public interface ICaratulaReport
    {
        Stream GetReporteCaratula(Caratula caratula);
    }
}
