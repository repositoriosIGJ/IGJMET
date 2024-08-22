using System;
using System.Text;
using System.Web.Mvc;
using Domain.Contracts.Common;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Domain.Models;
using System.Web.Helpers;
using Utils;

namespace Web.Controllers
{
    public class ReporteController : MetController
    {
        private readonly ICaratulaService _caratulaService;
        private readonly IReporteService _reporteService;
        private readonly ITramiteService _tramiteService;
        private readonly ISociedadService _sociedadService;

        public ReporteController(ICaratulaService caratulaService, IReporteService reporteService, ITramiteService tramiteService, 
            ISociedadService sociedadService, ILogger logger)
            :base(logger)
        {
            _caratulaService = caratulaService;
            _reporteService = reporteService;
            _tramiteService = tramiteService;
            _sociedadService = sociedadService;
        }

        public ActionResult TramitePago(string operacion, double? timestamp, string token)
        {
            //var validToken = new object[1] {String.Format("{0}:{1}", operacion, timestamp)};
            //if (!ValidateToken(validToken, token) && !ValidateTimestamp(timestamp))
            //    return Json("Eror de datos ingresados", JsonRequestBehavior.AllowGet);
            try
            {
                var usr = GetOperador();
                var st = _reporteService.GetCaratula(long.Parse(operacion), usr);
                return File(st, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json("Error de datos ingresados", JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Report/ReimprimirCaratula
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ReimprimirCaratula (ReporteModel model)
        {
            switch (model.TipoReporte)
            {
                case TipoReporte.Formulario:
                    model = _caratulaService.GetCaratulaPorFormulario(model.CodigoFormulario, GetOperador());
                    break;
                case TipoReporte.Tramite:
                    model = _caratulaService.GetCaratulaPorTramite(model.NroTramite, GetOperador());
                    break;
                default:
                    model.Resultado = 1;
                    model.Mensaje = "Debe seleccionar una opcion para reimprimir";
                    break;
            }

            return View(model);
        }

        //
        // GET: /Report/ReimprimirCaratula
        //[AuthorizeGroupAttribute]
        public ActionResult ReimprimirCaratula()
        {
            return View();
        }


        public ActionResult ValidarReimpresion(string tipo, string valor)
        {
            ReporteModel model;
            if (ValidateToken(Request.Headers["RequestVerificationToken"]))
            {
                model = tipo == "formulario" ? _caratulaService.GetCaratulaPorFormulario(valor, GetOperador()) : _caratulaService.GetCaratulaPorTramite(int.Parse(valor), GetOperador());
            }
            else
                model = new ReporteModel()
                {
                    Resultado = 1,
                    Mensaje = "Datos malignos"
                };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Tramites()
        {
            return View();
        }

        //
        // POST: /Report/Tramites

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Tramites(TramiteModel tramite)
        {
            tramite = _tramiteService.GetTramite(tramite.NroTramite);
            try
            {
                tramite.Sociedad = _sociedadService.GetPorNumeroCorrelativo(tramite.NroCorrelativo);
            }
            catch (SociedadNotExist)
            {
                //No existe la sociedad
            }
            
            return View(tramite);
        }

        protected override bool ValidateToken(object[] requestParams, string token)
        {
            var validToken = requestParams[0].ToString();
            var signature = HashCode.GetHashCodeFromString(validToken);
            return String.Compare(token, signature, StringComparison.Ordinal) == 0;
        }
    }
}
