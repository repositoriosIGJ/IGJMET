using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using Domain.Contracts.Common;
using Domain.Common;
using Domain.Exceptions;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services;
using Gateway.ENTE;
using Gateway.DTO;
using Gateway.Mappers;
using System.ServiceModel;
using System.Security.Principal;
using Utils;

namespace Gateway
{
    public class ENTEServiceClient: IENTEServiceClient
    {
        private Formulario _formularioProxy;
        private long _nroTramite;
        private Usuario _usuario;
        private readonly ILogger _logger;

        public ENTEServiceClient(ILogger logger)
        {
            _logger = logger;
        }

        public Timbrado ConsultarTimbrado(Formulario formulario, long nroTramite, Usuario usuario)
        {
            _usuario = usuario;
            _formularioProxy = formulario;
            _nroTramite = nroTramite;

            switch (Config.StringConfig(ConfigName.PLATAFORM))
            {
                case ConfigName.PROD:
                    return ConsumirServicio(new ENTE.Prod.ServiceEnteSoapClient());
                case ConfigName.TEST:
                    return ConsumirServicio(new ENTE.WS_RespuestaEnteSoapClient());
                default:
                    return null;
            }

        }

        private Timbrado ConsumirServicio(dynamic proxy)
        {
            string respuesta, consulta;

            using (proxy)
            {
                try
                {
                    consulta = FormatoConsultaWS();
                    respuesta = proxy.ConsultaTimbrado(consulta);
                }
                catch (TimeoutException timeoutEx)
                {
                    _logger.Error(timeoutEx);
                    throw new ServiceEnteTimeOutException("Tiempo de espera agotado conectandose al servicio ENTE de cooperación", timeoutEx);
                }
                catch (FaultException faultEx)
                {
                    _logger.Error(faultEx);
                    throw new ServiceEnteComunicationException("Error conectandose al servicio ENTE de cooperación", faultEx);
                }
                catch (CommunicationException commEx)
                {
                    _logger.Error(commEx);
                    throw new ServiceEnteComunicationException("Error de comunicacion con el servicio ENTE de cooperación", commEx);
                }
            }

            TimbradoDTO dto = new TimbradoDTO(respuesta);

            if (dto.CodRetorno == Codigo.ER)
            {
                _logger.Error("ERROR - WS ENTE. Parametros WS: " + consulta + "\nRepuesta: " + respuesta);
                throw new ServiceEnteTimbradoException("Formulario no reconocido como pago por el Ente de Cooperación");
            }

            Timbrado timbrado = new TimbradoMapper().Map(dto);
            timbrado.NroTramite = (int)_nroTramite;
            timbrado.Formulario = _formularioProxy;
            return timbrado; 
        }

        private string FormatoConsultaWS()
        { 
            //Formato de parametros para consultar al ENTE
            StringBuilder sb = new StringBuilder();
            sb.Append(_formularioProxy.CodigoBarraDecodificado);
            sb.AppendFormat("{0:0000}", _usuario.Id);
            sb.AppendFormat("{0:ddMMyyHHmmss}", DateTime.Now);
            sb.AppendFormat("{0:0000000}", _nroTramite);
            return sb.ToString();
        }
    }
}
