using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class VistaMapper: IMapper<Vista, VISTAREC>
    {
        public IList<Vista> Map(List<VISTAREC> listaEntidad)
        { 
            IList<Vista> listaDominio = new List<Vista>();

            listaEntidad.ForEach(v => listaDominio.Add(Map(v)));

            return listaDominio;
        }

        public Vista Map(VISTAREC entidad)
        {
            Vista dominio = new Vista();
            dominio.NroCorrelativo = entidad.VISNROCORR;

            if (entidad.VISNROTRAM.HasValue)
                dominio.NroTramite = entidad.VISNROTRAM.Value;

            dominio.Estado = entidad.VISFIN;
            return dominio;
        }

        public VISTAREC Reverse(Vista Domain)
        {
            throw new NotImplementedException();
        }

        public void Reverse(VISTAREC entidad, Vista Domain)
        {
            throw new NotImplementedException();
        }
    }
}
