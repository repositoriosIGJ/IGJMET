using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Domain.Contracts.Repositories;
using Repositorio;

namespace Repostorio.Test
{
    [TestClass]
    public class TramiteDigitalRepositorioTest
    {
        private TramiteDigital _tramiteDigital;

        private ITramiteDigitalRepositorio _tramiteDigitalRepositorio;

        #region Configuracion Inicial

        [TestInitialize]
        public void TestInitialize()
        {
            _tramiteDigitalRepositorio = new TramiteDigitalRepositorio();

            CrearTramiteDigital();

            LimpiarBD();
            InsertarDatosEnBD();
        }

        private void CrearTramiteDigital()
        {
            _tramiteDigital = new TramiteDigital();
            _tramiteDigital.NroCorrelativo = 0;
            _tramiteDigital.Fecha = DateTime.Now;
        }

        //Se persisten los datos en la BD antes de ejecutar los test
        private void InsertarDatosEnBD()
        {

        }

        //Se limpian los datos de la BD antes de la ejecucion de los test
        private void LimpiarBD()
        {
            _tramiteDigitalRepositorio.Eliminar(_tramiteDigital);
        }

        #endregion

        [TestMethod]
        public void GuardarTramiteDigital()
        {
            _tramiteDigital.NroTramite = 0;
            _tramiteDigital.NroPresentacion = 1;

            _tramiteDigitalRepositorio.Guardar(_tramiteDigital);

            var listado = _tramiteDigitalRepositorio.ObtenerTodosPorNroTramite(_tramiteDigital.NroTramite);

            Assert.AreEqual<int>(1, listado.Count);

            Assert.IsTrue(_tramiteDigital.Equals(listado[0]));
        }
    }
}
