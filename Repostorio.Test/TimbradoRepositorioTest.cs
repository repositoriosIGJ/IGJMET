using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Domain;
using Domain.Exceptions;
using Domain.Contracts.Common;
using Domain.Contracts.Repositories;
using Repositorio;
using Utils;

namespace Repostorio.Test
{
    [TestClass]
    public class TimbradoRepositorioTest
    {
        ITimbradoRepositorio _timbradoRepositorio;
        IFormularioRepositorio _formularioRepositorio;
        Formulario _formulario;
        Timbrado _timbrado;

        #region Configuracion Inicial

        [TestInitialize]
        public void TestInitialize()
        {
            ILogger logger = new Logger();
            _timbradoRepositorio = new TimbradoRepositorio(logger);
            _formularioRepositorio = new FormularioRepositorio();
            CrearFormulario();
            CrearTimbrado();

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

        private void CrearTimbrado()
        {
            _timbrado = new Timbrado();
            _timbrado.Formulario = _formulario;
            _timbrado.FechaCreacion = _timbrado.FechaActualizado = DateTime.Now.AddHours(-2);
            _timbrado.Caja = 0;
            _timbrado.Control = "0";
            _timbrado.Monto = 0;
            _timbrado.NroTramite = 0;
            _timbrado.NroPagoElectronico = 0;
            _timbrado.IdUsuario = 0;
        }

        //Se persisten los datos en la BD antes de ejecutar los test
        private void InsertarDatosEnBD()
        {
            _formularioRepositorio.Guardar(_formulario);
        }

        //Se limpian los datos de la BD antes de la ejecucion de los test
        private void LimpiarBD()
        {
            _timbradoRepositorio.Eliminar(_timbrado);

            _formularioRepositorio.Eliminar(_formulario);
        }

        #endregion

        #region Unidades de Test

        [TestMethod]
        public void ObtenerListado()
        {

        }

        [TestMethod]
        public void Existe()
        {
            _timbradoRepositorio.Guardar(_timbrado);

            var retorno = _timbradoRepositorio.Existe(_timbrado);

            Assert.IsTrue(retorno);
        }

        [TestMethod]
        public void ObtenerPorFormulario()
        {
            _timbradoRepositorio.Guardar(_timbrado);

            var timbrados = _timbradoRepositorio.GetTimbrados(_formulario);

            Assert.AreEqual(1, timbrados.Count);

            Assert.IsTrue(_timbrado.Equals(timbrados[0]));
        }

        [TestMethod]
        [ExpectedException(typeof(TimbradoNotExistException))]
        public void ObtenerPorFormularioError()
        {
            var timbrados = _timbradoRepositorio.GetTimbrados(_formulario);

            Assert.IsNotNull(timbrados);
        }

        [TestMethod]
        public void ObtenerUltimoFallido()
        {
            _timbrado.Estado = Estado.ER;
            _timbradoRepositorio.Guardar(_timbrado);

            _timbrado.FechaCreacion = DateTime.Now;
            _timbrado.Estado = Estado.ER;
            _timbradoRepositorio.Guardar(_timbrado);

            var timbrados = _timbradoRepositorio.GetTimbrados(_formulario);

            Assert.AreEqual<int>(2, timbrados.Count);

            var ultimo = timbrados.OrderByDescending(t => t.FechaCreacion).First();

            Assert.IsTrue(_timbrado.Equals(ultimo));
        }

        #endregion
    }
}
