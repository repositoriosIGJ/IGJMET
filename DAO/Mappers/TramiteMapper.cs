using System;
using System.Collections.Generic;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class TramiteMapper:IMapper<Tramite,TRAMIT>
    {
        public Tramite Map(TRAMIT entidad)
        {
            Tramite dominio = new Tramite();

            if (entidad.TRANROTRAM.HasValue)
                dominio.NroTramite = entidad.TRANROTRAM.Value;
            
            dominio.NroCorrelativo = entidad.TRANROCORR;
            dominio.Registro = entidad.TRAREGTRAM;
            dominio.CodTramite = entidad.TRACODTRAM;
            dominio.Fecha = entidad.TRAFECHACT;
            dominio.TipoBPM = (entidad.TRADIGITAL == "S");

            return dominio;
        }

        public TRAMIT Reverse(Tramite dominio)
        {
            throw new NotImplementedException();
        }

        public void Reverse(TRAMIT entidad, Tramite dominio)
        {
            throw new NotImplementedException();
        }
    }
}
