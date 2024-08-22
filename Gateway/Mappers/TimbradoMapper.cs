using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using Domain.Contracts.Mappers;
using Gateway.DTO;

namespace Gateway.Mappers
{
    public class TimbradoMapper: IMapper<Timbrado, TimbradoDTO>
    {
        public Timbrado Map(TimbradoDTO entidad)
        {
            Timbrado dominio = new Timbrado();
            Map(dominio, entidad);
            return dominio;
        }

        public void Map(Timbrado dominio, TimbradoDTO entidad)
        {
            dominio.Caja = entidad.Caja;
            dominio.FechaCreacion = entidad.FechaPago;
            dominio.Monto = entidad.Monto;
            dominio.NroPagoElectronico = entidad.NroPagoElectronico;
            dominio.FechaCreacion = entidad.FechaPago;
        }

        public TimbradoDTO Reverse(Timbrado dominio)
        {
            throw new NotImplementedException();
        }

        public void Reverse(TimbradoDTO entidad, Timbrado dominio)
        {
            throw new NotImplementedException();
        }
    }
}
