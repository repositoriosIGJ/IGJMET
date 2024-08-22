using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Domain.Contracts.Repositories;
using Repositorio;

namespace Repostorio.Test
{
    [TestClass]
    public class TramiteRepositorioTest
    {
        private ITramiteRepositorio _repositorio;

        
        private void Init()
        {
            _repositorio = new TramiteRepositorio(null, null);
        }

        [TestMethod]
        public void ObtenerNuevoTramite()
        {
            Init();
            var nroTramite = _repositorio.GenerarNroTramite();
            Assert.AreNotEqual(0, nroTramite);
        }
    }
}
