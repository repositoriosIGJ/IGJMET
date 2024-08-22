using System;
using System.Collections.Generic;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class TrazabilidadTramiteMapper:IMapper<TrazabilidadTramite, DESTRA>
    {
        public IList<TrazabilidadTramite> Map(List<DESTRA> listaEntidad)
        {
            IList<TrazabilidadTramite> listaDominio = new List<TrazabilidadTramite>();

            listaEntidad.ForEach(tt => listaDominio.Add(Map(tt)));

            return listaDominio;
        }

        public TrazabilidadTramite Map(DESTRA entidad)
        {
             var dominio = new TrazabilidadTramite
             {
                 NroTramite = entidad.DRTNROTRAM,
                 CodTramite = entidad.DTRCODTRAM,
                 NroCorrelativo = entidad.DTRNROCORR,
                 Origen = entidad.DTRDESTANT,
                 Destino = entidad.DTRCODDEST,
                 FechaDesde = entidad.DTRFECHART,
                 FechaTramite = entidad.DTRFECHACT

             };

            if (entidad.DTRFECHAST.HasValue)
                dominio.FechaHasta = entidad.DTRFECHAST.Value;

            return dominio;
        }

        public DESTRA Reverse(TrazabilidadTramite dominio)
        {
            throw new NotImplementedException();
        }

        public void Reverse(DESTRA entidad, TrazabilidadTramite dominio)
        {
            throw new NotImplementedException();
        }
    }
}
