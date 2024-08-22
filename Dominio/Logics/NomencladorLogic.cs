using Domain;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;
using Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Domain.Logics
{
    public class NomencladorLogic : INomencladorLogic
    {
        private readonly INomencladorRepositorio _nomencladorRepositorio;

        public NomencladorLogic(INomencladorRepositorio nomencladorRepositorio)
        {
            _nomencladorRepositorio = nomencladorRepositorio;
        }

        public bool TramiteTipoBPM(Timbrado timbrado)
        {
            try
            {
                Nomenclador nomenclador = _nomencladorRepositorio.Get(timbrado.Formulario.IdNomenclador);

                if (nomenclador != null)
                    return true;
            }
            catch (NomencladorNotExistException)
            {
            }

            return false;
        }
    }
}