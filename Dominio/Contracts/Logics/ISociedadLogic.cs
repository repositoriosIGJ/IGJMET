using System.Collections.Generic;
namespace Domain.Contracts.Logics
{
    public interface ISociedadLogic
    {
        Sociedad GetPorNumeroCorrelativo(int nroCorrelativo);

        IList<Sociedad> BusquedaDeSociedad(string nombre, int codigoTipoSociedad);

        int GenerarCorrelativo(string usuario, string nombre, string codigoTipoSociedad);
    }
}
