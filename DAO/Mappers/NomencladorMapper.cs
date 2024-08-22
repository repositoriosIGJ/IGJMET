using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Common;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class NomencladorMapper: IMapper<Nomenclador, NOMENCLADORTRAMITEBPM>
    {
        public IList<Nomenclador> Map(IList<NOMENCLADORTRAMITEBPM> listaEntidad)
        {
            return listaEntidad.Select(n => Map(n)).ToList();
        }

        public Nomenclador Map(NOMENCLADORTRAMITEBPM entidad)
        {
            Nomenclador dominio = new Nomenclador();
            dominio.IdNomenclador = entidad.IDNOMENCLADOR;

            if (entidad.FECHA_ALTA.HasValue)
                dominio.FechaAlta = entidad.FECHA_ALTA.Value;

            if (entidad.FECHA_BAJA.HasValue)
                dominio.FechaBaja = entidad.FECHA_BAJA.Value;
            
            return dominio;
        }

        public NOMENCLADORTRAMITEBPM Reverse(Nomenclador dominio)
        {
            throw new NotImplementedException();
        }

        public void Reverse(NOMENCLADORTRAMITEBPM entidad, Nomenclador dominio)
        {
            throw new NotImplementedException();
        }
    }
}
