using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;

namespace Domain.Logics
{
    public class OperacionLogic: IOperacionLogic
    {
        IOperacionRepositorio _operacionRepositorio;

        public OperacionLogic(IOperacionRepositorio operacionRepositorio)
        {
            _operacionRepositorio = operacionRepositorio;
        }

        public Operacion GetPorNroOperacion(long nroOperacion)
        {
            return _operacionRepositorio.ObtenerPorNroOperacion(nroOperacion);
        }

        public Operacion GetPorNroTramite(int nroTramite)
        {
            var operaciones = _operacionRepositorio.ObtenerPorTramite(nroTramite);
            return operaciones.OrderBy(o => o.Fecha).FirstOrDefault();
        }

        public long GuardarOperacion(Timbrado timbrado, TipoOperacion tipoOperacion, Usuario usuario)
        {
            Operacion operacion = new Operacion(timbrado, usuario, tipoOperacion);
            return GuardarOperacion(operacion);
        }

        public long GuardarOperacion(Operacion operacion)
        {
           return _operacionRepositorio.Guardar(operacion);
        }
    }
}
