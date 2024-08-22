using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class TramiteDigitalMapper:IMapper<TramiteDigital, TRAMITE_DIGITAL>
    {
        public IList<TramiteDigital> Map(List<TRAMITE_DIGITAL> entidadLista)
        {
            IList<TramiteDigital> dominioLista = new List<TramiteDigital>();

            entidadLista.ForEach(td => dominioLista.Add(Map(td)));

            return dominioLista;
        }

        public TramiteDigital Map(TRAMITE_DIGITAL entidad)
        {
            TramiteDigital dominio = new TramiteDigital();
            dominio.NroTramite = entidad.TRDNROTRAM;
            dominio.NroCorrelativo = entidad.TRDNROCORR;
            dominio.NroPresentacion = entidad.TRDNROPRES;
            dominio.Fecha = entidad.TRDFECACTU;
            dominio.Guid = entidad.TRDGUID;
            return dominio;
        }

        public TRAMITE_DIGITAL Reverse(TramiteDigital dominio)
        {
            throw new NotImplementedException();
        }

        public void Reverse(TRAMITE_DIGITAL entidad, TramiteDigital dominio)
        {
            throw new NotImplementedException();
        }
    }
}
