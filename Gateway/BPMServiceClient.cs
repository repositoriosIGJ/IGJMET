using System.Threading;
using Domain;
using Domain.Contracts.Common;
using Domain.Contracts.Repositories;
using Domain.Exceptions;
using Gateway.BPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Web;
using Gateway.Extensions;
using Gateway.Queue;
using Utils;

namespace Gateway
{
    public class BPMServiceClient : IBPMServiceClient
    {
        private readonly ILogger _logger;

        public BPMServiceClient(ILogger logger)
        {
            _logger = logger;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
        }

        public void RegistrarProceso(long nroTramite, int nroPresentacion, int nroCorrelativo, bool urgente)
        {
            var userId = HttpContext.Current.User.Identity.Name;
            var definitionId = String.Empty;

            var variables = new Dictionary<string, object>
                                    {
                                        {"numeroTramite", nroTramite},
                                        {"numeroPresentacion", nroPresentacion.ToString("0000")},
                                        {"numeroCorrelativo", nroCorrelativo},
                                        {"urgenciaTramite", (urgente ? "Urgente" : "")},
                                        {"observadoPreca", ""},
                                        {"observacionRevisor", ""},
                                        {"observadoRNS", ""},
                                        {"observadoRegistral", ""},
                                        {"requiereRevisionContable", "NO"},
                                        {"requiereRevisionLegal", "NO"},
                                        {"nombreJefeRevisor", ""},
                                        {"observacionDirector", ""},
                                        {"prontoDespacho", ""}
                                    };

            try
            {
                using (var proxy = new ProcessManagementServiceInterfaceClient())
                {
                    var process = proxy.getProcessDefinitionsForUser(userId).ToList().FindAll(s => s.id.StartsWith("IGJ.ProcesoRegistralV"));
                    definitionId = process.First(l => int.Parse(l.id.Split('V').Last()) == process.Max(p => int.Parse(p.id.Split('V').Last()))).id;

                    proxy.newInstanceWithVars(definitionId, variables.ToBpmPairs(), userId);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                
                SendMessage(definitionId, userId, variables.ToPairs(), TimeSpan.FromSeconds(5), retryCount: 3);
            }
        }

        private void SendMessage(String definitionId, String userId, Pair[] variables, TimeSpan sleepPeriod, int retryCount = 1)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        var nroTramite = variables.First(x => x.Key == "numeroTramite").Value;
                        var nroPresentacion = variables.First(x => x.Key == "numeroPresentacion").Value;

                        _logger.Info(String.Format("Agregando trámite nº:{0} presentación nº:{1} a la cola de mensajes", nroTramite, nroPresentacion));

                        using (var queueService = new QueueServiceClient())
                        {
                            queueService.PublishMessage(QueueOperationType.BpmCreateProcess, 0, definitionId, userId, variables);
                        }

                        break;
                    }
                    catch
                    {
                        if (--retryCount == 0) throw;

                        Thread.Sleep(sleepPeriod);
                    }
                }
            }
            catch (Exception serviceEx)
            {
                _logger.Error(serviceEx);
            }
        }
    }
}