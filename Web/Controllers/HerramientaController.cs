using System.Web.Mvc;
using Domain.Contracts.Common;
using Domain.Contracts.Services;
using Domain.Contracts.Repositories;
using Domain.Models;

namespace Web.Controllers
{
    public class HerramientaController : MetController
    {
        //
        // GET: /Herramienta/
        private readonly IHerramientaService _herramientaService;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public HerramientaController(IHerramientaService herramientaService, IUsuarioRepositorio usuarioRepositorio, ILogger logger)
            :base(logger)
        {
            _herramientaService = herramientaService;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public ActionResult VerificarCaratula()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult VerificarCaratula(VerificarCaratulaModel model)
        {
            model = _herramientaService.VerificarCaratula(model.CodigoSeguridad);
            return View(model);
        }

        public ActionResult Usuarios()
        {
            var usuarios = _usuarioRepositorio.GetAvailables();
            return View(usuarios);
        }

    }
}
