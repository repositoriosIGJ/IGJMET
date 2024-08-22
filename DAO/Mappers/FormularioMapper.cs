using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class FormularioMapper: IMapper<Formulario, TRANSACCION>
    {
        public Formulario Map(TRANSACCION entidad)
        {
            Formulario dominio = new Formulario();
            dominio.NroFormulario = entidad.IDTRANSACCION;
            dominio.Urgente = Convert.ToBoolean(entidad.URGENTE);
            dominio.Fecha = entidad.FECHAALTA;
            dominio.CodBarra = entidad.CODBARRA;
            dominio.IdEstado = entidad.IDESTADO;
            dominio.IdFirmante = entidad.IDFIRMANTE;
            dominio.IdNomenclador = entidad.IDNOMENCLADOR;

            if(entidad.NROCORRELATIVO.HasValue)
                dominio.NroCorrelativo = entidad.NROCORRELATIVO.Value;

            return dominio;
        }

        public TRANSACCION Reverse(Formulario dominio)
        {
            TRANSACCION entidad = new TRANSACCION();
            entidad.IDTRANSACCION = dominio.NroFormulario;
            entidad.CODBARRA = dominio.CodBarra;
            entidad.FECHAALTA = dominio.Fecha;
            entidad.FECHAINGRESO = dominio.Fecha;
            entidad.URGENTE = Convert.ToDecimal(dominio.Urgente);
            entidad.IDNOMENCLADOR = dominio.IdNomenclador;
            entidad.NROCORRELATIVO = dominio.NroCorrelativo;
            entidad.IDESTADO = dominio.IdEstado;
            entidad.IDFIRMANTE = dominio.IdFirmante;
            return entidad;
        }

        public void Reverse(TRANSACCION entidad, Formulario dominio)
        {
            throw new NotImplementedException();
        }
    }
}
