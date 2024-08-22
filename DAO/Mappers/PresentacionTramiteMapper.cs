using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class PresentacionTramiteMapper: IMapper<PresentacionTramite, PRESENTACION_TRAMITE>
    {
        public IList<PresentacionTramite> Map(List<PRESENTACION_TRAMITE> entidadLista)
        {
            IList<PresentacionTramite> dominioLista = new List<PresentacionTramite>();

            entidadLista.ForEach(p => dominioLista.Add(Map(p)));

            return dominioLista;
        }

        public PresentacionTramite Map(PRESENTACION_TRAMITE entidad)
        {
            PresentacionTramite dominio = new PresentacionTramite();
            dominio.NroTramite = entidad.PRTNROTRAM;
            dominio.NroPresentacion = entidad.PRTNROPRES;
            dominio.Fecha = entidad.PRTFECHAPRES;
            dominio.CodTramite = entidad.PRTCODTRAM;

            if(entidad.PRTNROCORR.HasValue)
                dominio.NroCorrelativo = entidad.PRTNROCORR.Value;

            return dominio;
        }

        public PRESENTACION_TRAMITE Reverse(PresentacionTramite dominio)
        {
            throw new NotImplementedException();
        }

        public void Reverse(PRESENTACION_TRAMITE entidad, PresentacionTramite dominio)
        {
            throw new NotImplementedException();
        }
    }
}
