using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Servicio;

namespace Servicio.Test
{
    [TestClass]
    public class ENTEProxyTest
    {
        private ENTEServiceClient _proxy;
        private Formulario _formulario;

        [TestInitialize]
        public void TestInitialize()
        {
            _proxy = new ENTEServiceClient();

            CrearFormulario();
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

        [TestMethod]
        public void ConsultarTimbrado()
        {
            var resultado = _proxy.ConsultarTimbrado(_formulario, 0);

            Assert.AreEqual<Estado>(Estado.OK, resultado.Estado);
        }
    }
}
