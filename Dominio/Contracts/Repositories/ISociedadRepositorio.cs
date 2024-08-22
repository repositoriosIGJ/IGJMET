using System;
using System.Collections.Generic;

namespace Domain.Contracts.Repositories
{
    public interface ISociedadRepositorio
    {
        Sociedad GetSociedad(int nroCorrelativo);

        IList<Sociedad> BusquedaDeSociedad(string nombre, int codigoTipoSociedad);

        int GenerarCorrelativo(string usuario, string nombre, string codigoTipoSociedad);

        void ActualizarCorrelativoSinRegistrar(int nroCorrelativo, int nroTramite, string codigoTipoTramite);

        bool ExisteCorrelativoSociedadSinRegistrar(int nroCorrelativo);

        void AgregarCorrelativoSinRegistrar(int nroCorrelativo, string operador, string codigoTipoTramite, int nroTramite);
    }
}
