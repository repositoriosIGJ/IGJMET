using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;

namespace Domain.Logics
{
    public class DestraLogic : IDestraLogic
    {
        private string[] _destinos;

        private readonly IDestraRepositorio _destraRepositorio;

        public DestraLogic(IDestraRepositorio destraRepositorio)
        {
            _destraRepositorio = destraRepositorio;
            // _destinos = new string[6]{ "CIC2", "CICU", "DCF6", "CICR", "OFE2", "OFE3" };
            _destinos = new string[3] {"CIC2", "DCF6", "OFE2"};
        }

        public bool AptoContestacionVista(int nroTramite)
        {
            var trazabilidad = _destraRepositorio.ObtenerUltimoEstadoTramite(nroTramite);
            _destinos = new string[7] {"CIC2", "CICU", "DCF6", "CICR", "OFE2", "OFE3", "DMEV"};
            return _destinos.Contains(trazabilidad.Destino);
        }

        private bool ValidacionDestino(string destino)
        {
            return _destinos.Contains(destino);
        }

        public bool TramiteArchivado(int nroTramite)
        {
            var trazabilidad = _destraRepositorio.ObtenerUltimoEstadoTramite(nroTramite);
            _destinos = new string[4] {"CICU", "CICR", "OFE3", "OFER"};
            return _destinos.Contains(trazabilidad.Destino);
        }
    }
}