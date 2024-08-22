using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Domain.Contracts.Repositories;
using Repositorio;
using Utils;

namespace Repostorio.Test
{
    [TestClass]
    public class UsuarioRepositorioTest
    {
        private IUsuarioRepositorio _usuarioRepositorio;

        [TestInitialize]
        public void TestInitialize()
        {
            _usuarioRepositorio = new UsuarioRepositorio(new Logger());
        }

        [TestMethod]
        public void GetUser()
        {
            string operador = "mfederico";

            Usuario usuario = _usuarioRepositorio.GetUsuario(operador);

            Assert.AreEqual(operador, usuario.Alias);

            usuario = _usuarioRepositorio.GetUsuario(operador);

            Assert.AreEqual(operador, usuario.Alias);
        }

    }
}
