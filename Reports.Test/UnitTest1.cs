using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;

namespace Reports.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CrearReporte()
        {
            Caratula caratula = new Caratula()
            {
                NroFormulario = 1031351,
                NroTramite = 9000507,
                NroPresentacion = 1,
                Sociedad = new Sociedad { Nombre = "Gasoleros", NroCorrelativo = 465578, TipoSociedad = new TipoSociedad { Descripcion = "SRL" }, },
                Operacion = new Operacion { TipoOperacion= TipoOperacion.CON_TIMBRADO }
            };

            CaratulaReporte reporte = new CaratulaReporte();
            //reporte.CrearReporte(caratula);
        }
    }
}
