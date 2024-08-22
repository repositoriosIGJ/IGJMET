using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class SociedadMapper: IMapper<Sociedad, EXPTES>
    {
        public Sociedad Map(EXPTES entidad)
        {
            Sociedad dominio = new Sociedad();
            dominio.NroCorrelativo = entidad.EXPNROCORR;
            dominio.Nombre = entidad.EXPRAZONSO;
            return dominio;
        }

        public EXPTES Reverse(Sociedad dominio)
        {
            throw new NotImplementedException();
        }

        public void Reverse(EXPTES entidad, Sociedad dominio)
        {
            throw new NotImplementedException();
        }
    }
}
