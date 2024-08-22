using Domain;
using Domain.Contracts.Logics;
using Domain.Contracts.Services;
using Domain.Models;
using System.Collections.Generic;

namespace Services
{
    public class SociedadService : ISociedadService
    {
        private readonly ISociedadLogic _sociedadlogic;

        public SociedadService(ISociedadLogic sociedadlogic)
        {
            _sociedadlogic = sociedadlogic;
        }

        public Sociedad GetPorNumeroCorrelativo(int nroCorrelativo)
        {
            return _sociedadlogic.GetPorNumeroCorrelativo(nroCorrelativo);
        }

        public SociedadModel GetSociedadModel(int nroCorrelativo)
        {
            var sociedad = _sociedadlogic.GetPorNumeroCorrelativo(nroCorrelativo);
            return new SociedadModel
            {
                NroCorrelativo = sociedad.NroCorrelativo,
                RazonSocial = sociedad.Nombre,
                Tipo = sociedad.TipoSociedad.Descripcion
            };
        }

        public Sociedad GetPorTramiteYCorrelativo(int nroTramite, int nroCorrelativo)
        {
            return null;
        }


        public IList<Sociedad> BusquedaDeSociedad(string nombre, int codigoTipoSociedad)
        {
            return _sociedadlogic.BusquedaDeSociedad(nombre, codigoTipoSociedad);
        }


        public int GenerarCorrelativo(string usuario, string nombre, string codigoTipoSociedad)
        {
            return _sociedadlogic.GenerarCorrelativo(usuario, nombre, codigoTipoSociedad);
        }
    }
}
