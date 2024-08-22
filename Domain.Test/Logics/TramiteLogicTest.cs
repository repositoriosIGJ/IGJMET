using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Domain.Logics;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;
using Repositorio;

namespace Domain.Test.Logics
{
    [TestClass]
    public class TramiteLogicTest
    {
        Formulario _formulario;
        Timbrado _timbrado;
        INomencladorRepositorio _nomencladorRepositorio;
        INomencladorLogic _nomencladorLogic;

        [TestInitialize]
        public void Init()
        {
            //Formulario urgente con nomenclador BPM
            _formulario = new Formulario("00001188837J.12122901");
            _formulario.IdNomenclador = 442;
            _timbrado = new Timbrado { Formulario = _formulario };
            _nomencladorRepositorio = new NomencladorRepositorio();
            _nomencladorLogic = new NomencladorLogic(_nomencladorRepositorio);
        }

        [TestMethod]
        public void VerificarNomenclador()
        {
            var isBPM = _nomencladorLogic.TramiteTipoBPM(_timbrado);
            Assert.IsFalse(isBPM);
        }
    }
}
