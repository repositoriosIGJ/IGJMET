using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class TimbradoMapper : IMapper<Timbrado, TIMBRADOS>
    {
        public IList<Timbrado> Map(List<TIMBRADOS> entidadLista)
        {
            IList<Timbrado> dominioLista = new List<Timbrado>();
            entidadLista.ForEach(t => dominioLista.Add(Map(t)));

            return dominioLista;
        }

        public Timbrado Map(TIMBRADOS entidad)
        {
            var dominio = new Timbrado
            {
                Monto = entidad.TIMMONTO,
                Caja = entidad.TIMNCAJA,
                NroFormulario = entidad.TIMNROFOR,
                FechaCreacion = entidad.TIMFECTIM,
                CodTramite = entidad.TIMCODTRAM != null ? Int32.Parse(entidad.TIMCODTRAM) : (Int32?)null
            };

            if (String.IsNullOrEmpty(entidad.TIMCODRET))
                dominio.Estado = Estado.ER_CON;
            else
                dominio.Estado = (Estado)Enum.Parse(typeof(Estado), entidad.TIMCODRET);

            if (entidad.TIMNROTRAM.HasValue)
                dominio.NroTramite = entidad.TIMNROTRAM.Value;

            if (entidad.TIMFECACTU.HasValue)
                dominio.FechaActualizado = entidad.TIMFECACTU.Value;

            if (entidad.TIMNROCORR.HasValue)
                dominio.NroCorrelativo = entidad.TIMNROCORR.Value;

            if (entidad.TIMFECHACT.HasValue)
                dominio.FechaOperacion = entidad.TIMFECHACT.Value;

            return dominio;
        }

        public TIMBRADOS Reverse(Timbrado dominio)
        {
            var entidad = new TIMBRADOS();
            Reverse(entidad, dominio);
            return entidad;
        }

        public void Reverse(TIMBRADOS entidad, Timbrado dominio)
        {
            entidad.TIMNCAJA = dominio.Caja;
            entidad.TIMCODRET = dominio.Estado.ToString();
            entidad.TIMFECTIM = dominio.FechaCreacion;
            entidad.TIMFECACTU = dominio.FechaActualizado;
            entidad.TIMFECHACT = dominio.FechaOperacion;
        }
    }
}
