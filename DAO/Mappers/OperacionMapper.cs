using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class OperacionMapper: IMapper<Operacion, OPERACION>
    {
        public IList<Operacion> Map(List<OPERACION> listaEntidad)
        {
            IList<Operacion> listaDominio = new List<Operacion>();

            listaEntidad.ForEach(o => listaDominio.Add(Map(o)));

            return listaDominio;
        }

        public Operacion Map(OPERACION entidad)
        {
            Operacion dominio = new Operacion();
            dominio.NroOperacion = (long)entidad.OPERACIONID;
            dominio.Fecha = entidad.FECHA;
            dominio.UsuarioID = entidad.USUARIOID;
            dominio.NroTramite = entidad.NROTRAMITE;

            if (entidad.NROFORMULARIO.HasValue)
                dominio.NroFormulario = entidad.NROFORMULARIO.Value;

            if (entidad.TIPOOPERACIONID.HasValue)
                dominio.TipoOperacion = (TipoOperacion)entidad.TIPOOPERACIONID;

            return dominio;
        }

        public OPERACION Reverse(Operacion dominio)
        {
            OPERACION entidad = new OPERACION();
            Reverse(entidad, dominio);
            return entidad;
        }

        public void Reverse(OPERACION entidad, Operacion dominio)
        {
            entidad.FECHA = dominio.Fecha;
            entidad.NROFORMULARIO = dominio.NroFormulario;
            entidad.NROTRAMITE = dominio.NroTramite;
            entidad.TIPOOPERACIONID = Convert.ToDecimal(dominio.TipoOperacion);
            entidad.USUARIOID = dominio.UsuarioID;
        }
    }
}
