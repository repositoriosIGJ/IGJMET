using System;
using System.Collections.Generic;
using Domain;

namespace Domain.Contracts.Logics
{
    public interface IOperacionLogic
    {
        Operacion GetPorNroOperacion(long nroOperacion);
        Operacion GetPorNroTramite(int nroTramite);
        long GuardarOperacion(Timbrado timbrado, TipoOperacion tipoOperacion, Usuario usuario);
        long GuardarOperacion(Operacion operacion);
    }
}
