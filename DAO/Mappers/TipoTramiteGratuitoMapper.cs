using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class TipoTramiteGratuitoMapper : IMapper<TipoTramiteGratuito, CODIGO_TRAMITES_GRATUITOS>
    {
        public TipoTramiteGratuito Map(CODIGO_TRAMITES_GRATUITOS entidad)
        {
            return new TipoTramiteGratuito()
            {
                Codigo = entidad.CODIGO_TRAMITE,
                Id = entidad.ID,
                Descripcion = entidad.DESCRIPCION
            };

            
        }


        public CODIGO_TRAMITES_GRATUITOS Reverse(TipoTramiteGratuito Domain)
        {
            throw new NotImplementedException();
        }

        public void Reverse(CODIGO_TRAMITES_GRATUITOS entidad, TipoTramiteGratuito Domain)
        {
            throw new NotImplementedException();
        }
    }
}
