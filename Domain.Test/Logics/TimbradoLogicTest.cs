using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain;
using Domain.Contracts.Repositories;
using Domain.Logics;
using Repositorio;

namespace Domain.Test
{
    [TestClass]
    public class TimbradoLogicTest
    {
        private readonly Mock<ITimbradoRepositorio> _timbradoRepositorio = new Mock<ITimbradoRepositorio>();
        private readonly Mock<IFormularioRepositorio> _formularioRepositorio = new Mock<IFormularioRepositorio>();
        //private readonly ITimbradoRepositorio _timbradoRepositorio = new TimbradoRepositorio();

        [TestInitialize]
        public void TestInitialize()
        {
            _timbradoRepositorio
                .Setup(s => s.GetTimbrados(It.IsAny<Formulario>()));
        }

        [TestMethod]
        public void GetUltimoProcesado()
        {
            var timbradoLogic = new TimbradoLogic(_timbradoRepositorio.Object, _formularioRepositorio.Object);
            var formulario = GetFormulario();
            var timbrado = timbradoLogic.GetUltimoProcesado(formulario);

            Assert.AreEqual(formulario.NroFormulario, timbrado.Formulario.NroFormulario);

        }

        private IList<Timbrado> GetTimbrados()
        {
            IList<Timbrado> timbrados = new List<Timbrado>();
            timbrados.Add(GetTimbrado());
            return timbrados;
        }

        private Timbrado GetTimbrado()
        {
            return new Timbrado() 
            { 
                Caja = 1,
                Monto = 10,
                Control = "",
                Estado = Estado.OK,
                FechaCreacion = DateTime.Now,
                FechaActualizado = DateTime.Now,
                IdUsuario = 0,
                NroPagoElectronico = 0,
                NroTramite = 0,
                Formulario = GetFormulario()
            };
        }

        private Formulario GetFormulario()
        {
            return new Formulario()
            {
                NroFormulario = 0,
                CodBarra = "",
                Urgente = false,
                TipoFormulario = "F."
            };
        }
    }
}
