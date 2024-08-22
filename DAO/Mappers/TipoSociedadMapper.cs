using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class TipoSociedadMapper: IMapper<TipoSociedad, TIPOENTIDAD>
    {
        public TipoSociedad Map(TIPOENTIDAD entidad)
        {
            TipoSociedad dominio = new TipoSociedad();
            dominio.Descripcion = entidad.DESCRIPCION;
            return dominio;
        }

        public TipoSociedad Map(SOCIEDADTIPO entidad)
        {
            TipoSociedad dominio = new TipoSociedad();
            dominio.Descripcion = entidad.DESCRIPCION;
            dominio.Alias = entidad.ABREVIACION;
            dominio.Id = entidad.ID;
            dominio.Codigo = entidad.CODIGO;

            return dominio;
        }

        public TIPOENTIDAD Reverse(TipoSociedad dominio)
        {
            throw new NotImplementedException();
        }

        public void Reverse(TIPOENTIDAD entidad, TipoSociedad dominio)
        {
            throw new NotImplementedException();
        }
    }
}
