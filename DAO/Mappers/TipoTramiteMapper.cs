using System;
using Domain.Contracts.Mappers;
using Domain;

namespace DAO.Mappers
{
    public class TipoTramiteMapper: IMapper<TipoTramite, TABGEN>
    {
        public TipoTramite Map(TABGEN entidad)
        {
            TipoTramite dominio = new TipoTramite();
            dominio.CodTramite = Convert.ToInt32(entidad.TABCLAVE);
            dominio.Descripcion = entidad.TABCONTEN1;

            return dominio;
        }

        public TABGEN Reverse(TipoTramite Domain)
        {
            throw new NotImplementedException();
        }

        public void Reverse(TABGEN entidad, TipoTramite Domain)
        {
            throw new NotImplementedException();
        }
    }
}
