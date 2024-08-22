using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class TramiteGratuitoMapper : IMapper<TramiteGratuito, TRAMITE_GRATUITO>
    {
        public TramiteGratuito Map(TRAMITE_GRATUITO entidad)
        {
            TramiteGratuito dominio = new TramiteGratuito();
            dominio.NroTramite = entidad.TGRNROTRAM;
            dominio.Fecha = entidad.TGRFECHA;

            if(entidad.TGRNROCORR.HasValue)
                dominio.NroCorrelativo = entidad.TGRNROCORR.Value;

            if(!string.IsNullOrEmpty(entidad.TGRCODTRAM))
                dominio.CodTramite = Convert.ToInt32(entidad.TGRCODTRAM);

            return dominio;
        }

        public TRAMITE_GRATUITO Reverse(TramiteGratuito Domain)
        {
            throw new NotImplementedException();
        }

        public void Reverse(TRAMITE_GRATUITO entidad, TramiteGratuito Domain)
        {
            throw new NotImplementedException();
        }
    }
}