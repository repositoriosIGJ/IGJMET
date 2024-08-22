using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;
using Domain.Exceptions;

namespace Domain.Logics
{
    public class VistaLogic: IVistaLogic
    {
        private readonly IDestraLogic _destraLogic;
        private readonly ISociedadLogic _sociedadLogic;
        private readonly IVistaRepositorio _vistaRepositorio;
        private readonly ITramiteRepositorio _tramiteRepositorio;

        public VistaLogic(IDestraLogic destraLogic, ISociedadLogic sociedadLogic, IVistaRepositorio vistaRepositorio, ITramiteRepositorio tramiteRepositorio)
        {
            _destraLogic = destraLogic;
            _sociedadLogic = sociedadLogic;
            _vistaRepositorio = vistaRepositorio;
            _tramiteRepositorio = tramiteRepositorio;
        }

        public bool VistaFinalizada(int nroTramite, int nroCorrelativo)
        {
            var vistas = _vistaRepositorio.GetVistas(nroTramite, nroCorrelativo);
            return (vistas.Count(v => v.Estado == "F") > 0);
        }

        public Sociedad VerificarContestacionVista(Tramite tramite)
        {
            if (!(
                    String.IsNullOrEmpty(tramite.Registro) &&
                    VistaFinalizada(tramite.NroTramite, tramite.NroCorrelativo) &&
                    (_destraLogic.AptoContestacionVista(tramite.NroTramite) || ((_destraLogic.TramiteArchivado(tramite.NroTramite))))
                ))
                throw new GenericException("Tramite no apto para Contestacion de Vista");

            if (_destraLogic.TramiteArchivado(tramite.NroTramite))

                throw new GenericException("El tramite se encuentra Archivado, debera presentar formulario desarchivo para contestar vista");


            Sociedad sociedad = _sociedadLogic.GetPorNumeroCorrelativo(tramite.NroCorrelativo);
            return sociedad;
        }

        public bool TramiteArchivado(int nroTramite)
        {
            return _destraLogic.TramiteArchivado(nroTramite);
        }
    }
}
