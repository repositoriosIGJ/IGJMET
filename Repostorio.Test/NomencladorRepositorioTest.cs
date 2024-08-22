using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Domain.Contracts.Repositories;
using Repositorio;

namespace Repostorio.Test
{
    [TestClass]
    public class NomencladorRepositorioTest
    {
        private INomencladorRepositorio _respositorio;

        private void Init()
        {
            _respositorio = new NomencladorRepositorio();
        }

        [TestMethod]
        public void ObtenerNomenclados()
        {
            Init();
            IList<Nomenclador> resultado = _respositorio.GetNomenclados();
            Assert.IsNotNull(resultado);
            Assert.AreNotEqual(0, resultado.Count);
        }
    }
}
