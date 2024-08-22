using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Domain.Contracts.Repositories;
using Repositorio;

namespace Repostorio.Test
{
    [TestClass]
    public class SociedadRepositorioTest
    {
        private readonly ISociedadRepositorio _repositorio;

        public SociedadRepositorioTest()
        {
            _repositorio = new SociedadRepositorio();
        }

        [TestMethod]
        public void ObtenerSociedad()
        {
            Sociedad sociedad = _repositorio.GetSociedad(1504714);

            Assert.IsNotNull(sociedad);
        }
    }
}
