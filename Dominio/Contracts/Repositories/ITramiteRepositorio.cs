using System;
using System.Collections.Generic;

namespace Domain.Contracts.Repositories
{
    public interface ITramiteRepositorio
    {
        Tramite GetTramite(int nroTramite);
        Tramite GetTramitePorTimbrado(int nroTramite);
        TramiteGratuito GetTramiteGratuito(int nroTramite);
        bool ExisteTramite(int nroTramite);
        int GenerarNroTramite();
        void GuardarDigital(TramiteDigital tramiteDigital);
        void GuardarTramiteGratuito(TramiteGratuito tramite);
        void GuardarTramite(Tramite tramite);
        IList<TipoSociedad> GetTiposSociedad();
        IList<TipoTramite> GetTipoTramite();
        int GetCodigoTipoSociedadById(int id);
       
    }
}
