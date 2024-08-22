using System;
using System.Collections.Generic;
using Domain.Models;

namespace Domain.Contracts.Services
{
    public interface ITramiteService
    {
        TramiteModel IniciarTramitePago(string codFormulario, string operador);
        TramiteModel IniciarTramiteNoPago(int nroCorrelativo, string operador, string CodigoTipoTramite);
        TramiteModel AgregarTimbradoATramite(int nroTramite, string codFormulario, string operador);
        PresentacionModel AgregarPresentacion(int nroTramite, int nroCorrelativo, string operador);
        bool ExisteTramite(int nroTramite);
        PresentacionModel VerificarContestacionVista(int nroTramite, int nroCorrelativo);
        TramiteModel GetTramite(int nroTramite);
        IList<TipoSociedad> GetTipoSociedadList();
        IList<TipoTramite> GetTipoTramite();
        IList<TipoTramiteGratuito> GetTipoTramiteGratuito();
        void VerificarTimbradoVista(int nroTramite, string codigoFormulario, string operador);
        void DesarchivarTramite(int nroTramite, int nroCorrelativo, string operador);

        bool TramiteArchivado(int nroTramite);
    }
}
