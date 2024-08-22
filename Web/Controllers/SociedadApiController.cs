using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Mvc;
using Domain.Contracts.Common;
using Domain.Contracts.Services;

namespace Web.Controllers
{
    [RoutePrefix("api/Sociedades")]
    public class SociedadApiController : ApiController
    {
        private readonly ISociedadService _sociedadService;
        private readonly ILogger _logger;

        public SociedadApiController()
        {
                
        }

        public SociedadApiController(ISociedadService sociedadService, ILogger logger)
        {
            _sociedadService = sociedadService;
            _logger = logger;
        }

        [GET("{correlativo:int}")]
        public HttpResponseMessage GetSociedad(int correlativo)
        {
            try
            {
                var sociedadModel = _sociedadService.GetSociedadModel(correlativo);
                return Request.CreateResponse(HttpStatusCode.OK, sociedadModel);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Ocurrio un error en el sistema");
            }
        }
    }
}
