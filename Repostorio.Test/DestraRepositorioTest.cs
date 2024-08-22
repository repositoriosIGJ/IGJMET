using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Domain.Contracts.Repositories;
using Repositorio;
using Utils;

namespace Repostorio.Test
{
    [TestClass]
    public class DestraRepositorioTest
    {
        private IDestraRepositorio _repositorio;
        private TrazabilidadTramite _trazabilidad;

        #region Configuracion Inicial

        [TestInitialize]
        public void TestInitialize()
        {
            _repositorio = new DestraRepositorio(new TipoTramiteRepository(), new TramiteRepositorio(new TimbradoRepositorio(new Logger()), new TipoTramiteRepositorio()));

            CrearTrazabilidad();

            LimpiarBD();
            InsertarDatosEnBD();
        }

        private void CrearTrazabilidad()
        {
            _trazabilidad = new TrazabilidadTramite();
            _trazabilidad.NroCorrelativo = 0;
            _trazabilidad.NroTramite = 0;
            _trazabilidad.Destino = "CIC1";
            _trazabilidad.FechaHasta = DateTime.Now;
        }

        //Se persisten los datos en la BD antes de ejecutar los test
        private void InsertarDatosEnBD()
        {

        }

        //Se limpian los datos de la BD antes de la ejecucion de los test
        private void LimpiarBD()
        {
            _repositorio.Eliminar(_trazabilidad);
        }

        #endregion

        [TestMethod]
        public void ObtenerPorTramite()
        {
            _trazabilidad.NroTramite = 0;
            _trazabilidad.NroCorrelativo = 90;

            _repositorio.Guardar(_trazabilidad);

            var resultado = _repositorio.ObtenerPorTramite(_trazabilidad.NroTramite);

            Assert.IsTrue(_trazabilidad.Equals(resultado[0]));
        }
    }
}
