using System;
using System.Collections.Generic;
using Domain;

namespace Domain.Contracts.Logics
{
    public interface IPresentacionTramiteLogic
    {
        PresentacionTramite GetUltimaPresentacion(int nroTramite);
        void GuardarPresentacion(int nroTramite, string operador, int nroPresentacion);
        PresentacionTramite AsignarNuevaPresentacion(int nroCorrelativo, int nroTramite, string operador);
        void GuardarPresentacion(PresentacionTramite presentacion);
    }
}
