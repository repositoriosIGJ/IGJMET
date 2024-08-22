using System;
using System.Web.Mvc;
using Domain.Contracts.Common;
using Domain.Exceptions;
using Domain.Models;
using Domain.Contracts.Services;
using System.Web.Helpers;

namespace Web.Controllers
{
    public class SociedadController : MetController
    {
        private readonly ISociedadService _sociedadService;

        public SociedadController(ISociedadService sociedadService, ILogger logger)
            :base(logger)
        {
            _sociedadService = sociedadService;

        }

        //
        // GET: /ValidarSociedad/

        public JsonResult ValidarSociedad(string nroCorrelativo)
        {
            var model = new SociedadModel
            {
                Resultado = 1
            };

            if (!ValidateToken(Request.Headers["RequestVerificationToken"]))
            {
                model.Mensaje = "Datos ingresados malignos";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            int correlativo;
            if (!int.TryParse(nroCorrelativo, out correlativo)) 
                return Json(model, JsonRequestBehavior.AllowGet);
            try
            {
                var sociedad = _sociedadService.GetPorNumeroCorrelativo(correlativo);
                model.Resultado = 0;
                model.NroCorrelativo = sociedad.NroCorrelativo;
                model.RazonSocial = sociedad.Nombre;
                model.Tipo = sociedad.TipoSociedad.Descripcion;
            }
            catch (SociedadNotExist)
            {
                model.Mensaje = "Nro. correlativo inexistente";
            }
            catch(Exception ex)
            {
                _logger.Error(ex.ToString());
                model.Mensaje = "Ocurrió un error en la operación";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
