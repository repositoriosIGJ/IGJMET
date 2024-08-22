using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Contracts.Logics;
using Domain.Logics;
using Web.Controllers;
using Moq;

namespace Web.Tests.Controllers
{
    [TestClass]
    public class TramiteControllerTest
    {
        private readonly ITramiteLogic _tramiteLogic;
        private readonly TramiteController _controller;

        public TramiteControllerTest()
        {

        }

        [TestMethod]
        public void IniciarTramitePago()
        {

        }
    }
}
