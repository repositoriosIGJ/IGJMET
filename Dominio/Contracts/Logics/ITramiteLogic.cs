using System;
using System.Collections.Generic;
using Domain.Models;

namespace Domain.Contracts.Logics
{
    public interface ITramiteLogic
    {
        Tramite GetTramite(int nroTramite);
        Tramite GetTramitePorTimbrado(int nroTramite);
        Tramite IniciarTramitePago(string codFormulario, string operador);
        Tramite IniciarTramiteNoPago(int nroCorrelativo, string operador, string codigoTipoTramite);
        Tramite AgregarTimbradoATramite(int nroTramite, string codFormulario, string operador);
        bool ExisteTramite(int nroTramite);
        IList<TipoSociedad> GetTiposSociedad();
        IList<TipoTramite> GetTipoTramite();
        IList<TipoTramiteGratuito> GetTipoTramiteGratuito();
        int GetCodigoTipoSociedadById(int id);
        void VerificarTimbradoVista(int nroTramite, string codigoFormulario, string operador);
        void DesarchivarTramite(int nroTramite, int nroCorrelativo, string operador);
    }
}
