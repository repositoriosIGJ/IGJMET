using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using Domain.Contracts.Common;
using Domain.Contracts.Repositories;
using Domain.Exceptions;
using Gateway.BPM;
using Gateway.DTO;
using Gateway.TramiteService;
using Newtonsoft.Json;

namespace Gateway
{
    public class GestionTramiteServiceClient : IGestionTramiteServiceClient
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;
        public GestionTramiteServiceClient(ILogger logger)
        {
            _logger = logger;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

            #region 
            /*
             
             var handler = new HttpClientHandler
                          {
                              UseDefaultCredentials = true,
                              Credentials = CredentialCache.DefaultCredentials
                          };

            _client = HttpClientFactory.Create(handler);
            _client.DefaultRequestHeaders.Add("X-CustomHeader", "value");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get("baseAddress") + "/");
             
            */
            #endregion
        }

        public void Clasificar(String codigoBarra)
        {
            try
            {
                using (var tramiteServiceClient = new TramiteServiceClient())
                {
                    tramiteServiceClient.Clasificar(codigoBarra);
                }
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            /*
            
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
            HttpContent content = new ObjectContent<TramiteDTO>(new TramiteDTO {CodigoBarra = codigoBarra}, jsonFormatter);
            var response = _client.PostAsync("Tramites", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                _logger.Error("CodigoBarra: " + codigoBarra + " no pudo ser clasificado");
            }
             
            */
        }
    }
}
