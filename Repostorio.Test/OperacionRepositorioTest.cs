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
    public class OperacionRepositorioTest
    {
        private Formulario _formulario;
        private Timbrado _timbrado;
        private Operacion _operacion;

        private IFormularioRepositorio _formularioRepositorio;
        private ITimbradoRepositorio _timbradoRepositorio;
        private IOperacionRepositorio _operacionRepositorio;

        #region Configuracion Inicial

        [TestInitialize]
        public void TestInitialize()
        {
            ILogger logger = new Logger();
            _timbradoRepositorio = new TimbradoRepositorio(logger);
            _formularioRepositorio = new FormularioRepositorio();
            _operacionRepositorio = new OperacionRepositorio();

            CrearFormulario();
            CrearTimbrado();
            CrearOperacion();

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

        private void CrearOperacion()
        {
            _operacion = new Operacion();
            _operacion.NroTramite = _timbrado.NroTramite;
            _operacion.Fecha = DateTime.Now;
            _operacion.UsuarioID = 0;
        }

        //Se persisten los datos en la BD antes de ejecutar los test
        private void InsertarDatosEnBD()
        {
            
        }

        //Se limpian los datos de la BD antes de la ejecucion de los test
        private void LimpiarBD()
        {
            _operacionRepositorio.EliminarPorTramite(_timbrado.NroTramite);
        }

        #endregion

        [TestMethod]
        public void GuardarOperacionConTimbrado()
        {
            _operacion.TipoOperacion = TipoOperacion.CON_TIMBRADO;
            _operacion.NroFormulario = _formulario.NroFormulario;
            _operacionRepositorio.Guardar(_operacion);

            var resultado = _operacionRepositorio.ObtenerPorTramite(_timbrado.NroTramite);

            Assert.IsTrue(_operacion.Equals(resultado[0]));
        }

        [TestMethod]
        public void GuardarOperacionSinTimbrado()
        {
            _operacion.TipoOperacion = TipoOperacion.SIN_TIMBRADO;
            _operacion.NroFormulario = 0;
            _operacionRepositorio.Guardar(_operacion);

            var resultado = _operacionRepositorio.ObtenerPorTramite(_timbrado.NroTramite);

            Assert.IsTrue(_operacion.Equals(resultado[0]));
        }
    }
}
