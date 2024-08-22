using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Domain.Contracts.Repositories;
using Domain.Exceptions;
using Repositorio;

namespace Repostorio.Test
{
    [TestClass]
    public class FormularioRepositorioTest
    {
        IFormularioRepositorio _repositorio;
        Formulario _formulario;

        #region Configuracion Inicial

        [TestInitialize]
        public void TestInitialize()
        {
            _repositorio = new FormularioRepositorio();
            CrearFormulario();

            LimpiarBD();
            InsertarDatosEnBD();
        }

        private void CrearFormulario()
        {
            _formulario = new Formulario("00001031323F.12050701");
            _formulario.NroFormulario = 0;
            _formulario.NroCorrelativo = 0;
            _formulario.IdNomenclador = 0;
            _formulario.IdFirmante = 0;
            _formulario.IdEstado = 0;
        }

        private void InsertarDatosEnBD()
        {
            _repositorio.Guardar(_formulario);
        }

        private void LimpiarBD()
        {
            _repositorio.Eliminar(_formulario);
        }

        #endregion

        #region Unidades de Test

        [TestMethod]
        public void Existe()
        {
            bool existe = _repositorio.Existe(_formulario);
            Assert.IsTrue(existe);
        }

        [TestMethod]
        public void GetFormulario()
        {
            Formulario aux = _repositorio.GetFormulario(_formulario.CodBarra);

            Assert.AreEqual<long>(_formulario.NroFormulario, aux.NroFormulario);
            Assert.AreEqual<DateTime>(_formulario.Fecha, aux.Fecha);
        }

        [TestMethod]
        [ExpectedException(typeof(FormularioNotExistException))]
        public void GetFormularioException()
        {
            string codBarra = _formulario.CodBarra.Replace("F","Z");
            Formulario aux = _repositorio.GetFormulario(codBarra);
        }

        #endregion
    }
}
