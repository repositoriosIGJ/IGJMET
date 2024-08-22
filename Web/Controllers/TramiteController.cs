using Domain.Contracts.Common;
using Domain.Contracts.Services;
using Domain.Models;
using System;
using System.Web.Helpers;
using System.Web.Mvc;
using Web.Filters;
using System.Collections.Generic;
using Domain;

namespace Web.Controllers
{
    public class TramiteController : MetController
    {
        private readonly ITramiteService _tramiteService;
        private readonly ISociedadService _sociedadService;

        public TramiteController(ITramiteService tramiteService, ILogger logger, ISociedadService sociedadService)
            : base(logger)
        {
            _tramiteService = tramiteService;
            _sociedadService = sociedadService;
        }

        public JsonResult ConsultarTramite(string nroTramite)
        {
            var tramit = 0;
            if (!int.TryParse(nroTramite, out tramit))
                return Json(false, JsonRequestBehavior.AllowGet);
            var existe = _tramiteService.ExisteTramite(tramit);
            return Json(existe, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Tramite/TramitePago

        public ActionResult TramitePago()
        {
            return View();
        }

        //
        // POST: /Tramite/TramitePago

        [ValidateAntiForgeryToken]
        [HttpPost, ValidateInput(false)]
        public ActionResult TramitePago(TramiteModel model)
        {
            model = _tramiteService.IniciarTramitePago(model.CodigoFormulario.Trim(), GetOperador());
            return View(model);
        }

        //
        // GET: /Tramite/TramiteNoPago

        public ActionResult TramiteNoPago()
        {
            Inicializar();
            return View();
        }

        public void Inicializar()
        {
            ViewBag.TiposSociedad = _tramiteService.GetTipoSociedadList();
            ViewBag.TiposTramite = _tramiteService.GetTipoTramiteGratuito();
        }


        //
        // POST: /Tramite/TramiteNoPago

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TramiteNoPago(TramiteModel model)
        {
            model = _tramiteService.IniciarTramiteNoPago(model.NroCorrelativo, GetOperador(), model.CodigoTipoDeTramite);

            return View(model);
        }

        //GET: /Tramite/AgregarTimbrado

        public ActionResult AgregarTimbrado()
        {
            return View();
        }

        //
        // POST: /Tramite/AgregarTimbrado

        [ValidateAntiForgeryToken]
        [HttpPost, ValidateInput(false)]
        public ActionResult AgregarTimbrado(TramiteModel model)
        {
            var newModel = _tramiteService.AgregarTimbradoATramite(model.NroTramite, model.CodigoFormulario.Trim(), GetOperador());
            newModel.Imprime = model.Imprime;
            return View(newModel);
        }

        public ActionResult RecepcionVista()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecepcionVista(PresentacionModel model)
        {
            model = _tramiteService.AgregarPresentacion(model.NroTramite, model.NroCorrelativo, GetOperador());

            return View(model);
        }

        public JsonResult ValidarVista(PresentacionModel model)
        {
            if (ValidateToken(Request.Headers["RequestVerificationToken"]))
            {

                try
                {
                    if (model.Desarchivar)
                    {
                        var valido = _tramiteService.TramiteArchivado(model.NroTramite);
                        if (valido)
                        {
                            _tramiteService.VerificarTimbradoVista(model.NroTramite, model.CodigoFormulario, GetOperador());
                            _tramiteService.DesarchivarTramite(model.NroTramite, model.NroCorrelativo, GetOperador());
                        }
                        else
                        {
                            throw new Exception(
                                "El tramite no se encuentra archivado");
                        }
                    }


                    model = _tramiteService.VerificarContestacionVista(model.NroTramite, model.NroCorrelativo);
                }
                catch (Exception ex)
                {
                    model.Resultado = 1;
                    model.Mensaje = ex.Message;
                }
            }
            else
            {
                model.Resultado = 1;
                model.Mensaje = "Datos invalidos";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Buscar(string nombre, int codigoTipoSociedad)
        {
            var result = _sociedadService.BusquedaDeSociedad(nombre, codigoTipoSociedad);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerarCorrelativo(string nombre, string codigoTipoSociedad)
        {
            //traer datos sociedad y generar registro en exptes

            var result = _sociedadService.GenerarCorrelativo(GetOperador(), nombre, codigoTipoSociedad);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
