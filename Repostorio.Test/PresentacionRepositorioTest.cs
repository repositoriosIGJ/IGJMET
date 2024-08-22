using System;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Domain.Contracts.Repositories;
using Repositorio;

namespace Repostorio.Test
{
    [TestClass]
    public class PresentacionRepositorioTest
    {
        private PresentacionTramite _presentacion;

        private IPresentacionTramiteRepositorio _presentacionRepositorio;

        #region Configuracion Inicial

        [TestInitialize]
        public void TestInitialize()
        {
            _presentacionRepositorio = new PresentacionTramiteRepositorio();

            CrearPresentacion();

            LimpiarBD();
            InsertarDatosEnBD();
        }

        private void CrearPresentacion()
        {
            _presentacion = new PresentacionTramite();
            _presentacion.NroTramite = 0;
            _presentacion.Usuario = new Usuario { Id = 9999 };
            _presentacion.Fecha = DateTime.Now;
        }

        //Se persisten los datos en la BD antes de ejecutar los test
        private void InsertarDatosEnBD()
        {

        }

        //Se limpian los datos de la BD antes de la ejecucion de los test
        private void LimpiarBD()
        {
            _presentacionRepositorio.Eliminar(_presentacion);
        }

        #endregion

        [TestMethod]
        public void GuardarPrimeraPresentacion()
        {
            _presentacion.NroPresentacion = 1;

            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                _presentacionRepositorio.Guardar(_presentacion);

                scope.Complete();
            }
            
            var listado = _presentacionRepositorio.ObtenerTodos(_presentacion.NroTramite);

            Assert.AreEqual(1, listado.Count);

            Assert.IsTrue(_presentacion.Equals(listado[0]));
        }
    }
}
