using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;


namespace Domain.Logics
{
    public class ModuloTramiteLogic: IModuloTramiteLogic
    {
        //private readonly Modu

        private readonly IModuloRepositorio _moduloRepositorio;
        private readonly ISociedadRepositorio _sociedadRepositorio;

        public ModuloTramite GetByFormulario(Formulario formulario)
        {
            Sociedad sociedad = _sociedadRepositorio.GetSociedad(formulario.NroCorrelativo);
            ModuloTramite modulo = _moduloRepositorio.GetModulo(formulario.IdNomenclador, sociedad.TipoSociedad.Id);
            return modulo;
        }
    }
}
