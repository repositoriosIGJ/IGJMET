using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Domain.Contracts.Common;
using Domain.Contracts.Repositories;
using Repositorio;
using Utils;

namespace Repostorio.Test
{
    [TestClass]
    public class ReimpresionRepositorioTest
    {
        private IReimpresionRespositorio _repositorio;
        private Reimpresion _reimpresion;

        #region Configuracion Inicial

        [TestInitialize]
        public void TestInitialize()
        {
            ILogger logger = new Logger();
            _repositorio = new ReimpresionRepositorio(logger);

            CrearReimpresion();

            LimpiarBD();
            InsertarDatosEnBD();
        }

        private void CrearReimpresion()
        {
            _reimpresion = new Reimpresion();
            _reimpresion.IdUsuario = 0;
            _reimpresion.Usuario = "pepe";
            _reimpresion.Fecha = DateTime.Now;
        }

        //Se persisten los datos en la BD antes de ejecutar los test
        private void InsertarDatosEnBD()
        {

        }

        //Se limpian los datos de la BD antes de la ejecucion de los test
        private void LimpiarBD()
        {
            _repositorio.Eliminar(_reimpresion);
        }

        #endregion


        [TestMethod]
        public void Guardar()
        {
            _reimpresion.NroTramite = 0;
            _repositorio.Guardar(_reimpresion);

            var resultado = _repositorio.ObtenerPorId(_reimpresion.Id);

            Assert.IsTrue(_reimpresion.Equals(resultado));
        }
    }
}
