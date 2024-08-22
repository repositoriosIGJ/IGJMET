using Domain.Models;
using System.Collections.Generic;

namespace Domain.Contracts.Services
{
    public interface ISociedadService
    {
        Sociedad GetPorNumeroCorrelativo(int nroCorrelativo);
        SociedadModel GetSociedadModel(int nroCorrelativo);
        IList<Sociedad> BusquedaDeSociedad(string nombre, int codigoTipoSociedad);
        int GenerarCorrelativo(string usuario, string nombre, string codigoTipoSociedad);
    }
}
