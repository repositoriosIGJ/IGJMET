using Domain;
using Domain.Contracts.Common;
using Domain.Contracts.Logics;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Domain.Models;
using System;
using System.Collections.Generic;
using Utils;

namespace Services
{
    public class TramiteService : ITramiteService
    {
        private readonly ITramiteLogic _tramiteLogic;
        private readonly IPresentacionTramiteLogic _presentacionLogic;
        private readonly IVistaLogic _vistaLogic;
        private readonly ILogger _logger;

        public TramiteService(ITramiteLogic tramiteLogic, IVistaLogic vistaLogic, IPresentacionTramiteLogic presentacionLogic, ILogger logger)
        {
            _tramiteLogic = tramiteLogic;
            _vistaLogic = vistaLogic;
            _presentacionLogic = presentacionLogic;
            _logger = logger;
        }

        public TramiteModel GetTramite(int nroTramite)
        {
            Tramite tramite = null;
            try
            {
                tramite = _tramiteLogic.GetTramite(nroTramite);
            }
            catch (TramiteNotExistException)
            {
                //Si no existe en la entidad Tramite se busca por los timbrados asociados al numero de tramite
                tramite = GetTramitePorTimbrado(nroTramite);
            }

            if (tramite == null)
                return new TramiteModel
                {
                    Resultado = 1,
                    Mensaje = "Tramite inexistente",
                    NroTramite = nroTramite
                };

            return new TramiteModel
            {
                Resultado = 0,
                NroTramite = nroTramite,
                NroCorrelativo = tramite.NroCorrelativo,
                CodTramite = tramite.CodTramite,
                Timbrados = tramite.Timbrados,
                TipoTramite = tramite.TipoTramite
            };
        }

        public Tramite GetTramitePorTimbrado(int nroTramite)
        {
            try
            {
                return _tramiteLogic.GetTramitePorTimbrado(nroTramite);
            }
            catch (TramiteNotExistException)
            {
                return null;
            }
        }

        public TramiteModel IniciarTramitePago(string codFormulario, string operador)
        {
            try
            {
                //int nroTramite = _tramiteLogic.IniciarTramitePago(codFormulario, operador);
                Tramite tramite = _tramiteLogic.IniciarTramitePago(codFormulario, operador);

                return new TramiteModel
                {
                    Resultado = 0,
                    Mensaje = "Tramite ingresado correctamente",
                    NroTramite = tramite.NroTramite,
                    CodigoFormulario = codFormulario,
                    NroFormulario = Formulario.GetNroFormulario(codFormulario),
                    NroPresentacion = tramite.UltimoNroPresentacion,
                    NroCorrelativo = tramite.NroCorrelativo,
                    Tipo = TipoTramiteModel.Pago,
                    NroOperacion = tramite.Operaciones[0].NroOperacion,
                };
            }
            catch (FormatException fex)
            {
                _logger.Error(fex);
                return new TramiteModel { Resultado = 1, Mensaje = "Codigo de formulario invalido", CodigoFormulario = codFormulario };
            }
            catch (GenericException gex)
            {
                _logger.Error(gex);
                return new TramiteModel { Resultado = 1, Mensaje = gex.Message, CodigoFormulario = codFormulario };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new TramiteModel { Resultado = 1, Mensaje = "Se produjo un error en la operación", CodigoFormulario = codFormulario };
            }
        }

        public TramiteModel IniciarTramiteNoPago(int nroCorrelativo, string operador, string codigoTipoTramite)
        {
            codigoTipoTramite = "0";
            try
            {
                Tramite tramite = _tramiteLogic.IniciarTramiteNoPago(nroCorrelativo, operador, codigoTipoTramite);

                return new TramiteModel
                {
                    Resultado = 0,
                    Mensaje = "Tramite ingresado correctamente",
                    NroTramite = tramite.NroTramite,
                    NroPresentacion = tramite.UltimoNroPresentacion,
                    NroCorrelativo = tramite.NroCorrelativo,
                    Tipo = TipoTramiteModel.NoPago,
                    NroOperacion = tramite.Operaciones[0].NroOperacion
                };
            }
            catch (GenericException gex)
            {
                _logger.Error(gex);
                return new TramiteModel { Resultado = 1, Mensaje = gex.Message };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new TramiteModel { Resultado = 1, Mensaje = "Ocurrio un error al generar un tramite gratuito." };
            }
        }

        public TramiteModel AgregarTimbradoATramite(int nroTramite, string codFormulario, string operador)
        {
            try
            {
                Tramite tramite = _tramiteLogic.AgregarTimbradoATramite(nroTramite, codFormulario, operador);
                return new TramiteModel
                {
                    Resultado = 0,
                    Mensaje = "Tramite ingresado correctamente",
                    NroTramite = nroTramite,
                    CodigoFormulario = codFormulario,
                    NroFormulario = Formulario.GetNroFormulario(codFormulario),
                    NroCorrelativo = tramite.NroCorrelativo,
                    NroPresentacion = tramite.UltimoNroPresentacion,
                    Tipo = TipoTramiteModel.Pago,
                    NroOperacion = tramite.Operaciones[0].NroOperacion
                };
            }
            catch (FormatException fex)
            {
                _logger.Error(fex);
                return new TramiteModel { Resultado = 1, Mensaje = "Codigo de formulario invalido", CodigoFormulario = codFormulario };
            }
            catch (GenericException gex)
            {
                _logger.Error(gex);
                return new TramiteModel { Resultado = 1, Mensaje = gex.Message, CodigoFormulario = codFormulario };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new TramiteModel { Resultado = 1, Mensaje = "Se produjo un error en la operación", NroTramite = nroTramite, CodigoFormulario = codFormulario };
            }
        }

        public PresentacionModel VerificarContestacionVista(int nroTramite, int nroCorrelativo)
        {
            PresentacionModel model;
            try
            {
                Tramite tramite = _tramiteLogic.GetTramite(nroTramite);
                if (tramite.NroCorrelativo != nroCorrelativo)
                    throw new GenericException("El tramite no se encuentra relacionado con el Nro. Correlativo ingresado");

                Sociedad sociedad = _vistaLogic.VerificarContestacionVista(tramite);

                model = new PresentacionModel
                {
                    NombreSociedad = sociedad.Nombre,
                    TipoSociedad = sociedad.TipoSociedad.Descripcion,
                    NroCorrelativo = tramite.NroCorrelativo,
                    NroTramite = tramite.NroTramite,
                    Resultado = 0,
                };
            }
            catch (GenericException gex)
            {
                _logger.Error(gex);

                model = new PresentacionModel
                {
                    Resultado = 1,
                    Mensaje = gex.Message,
                    NroTramite = nroTramite,
                    NroCorrelativo = nroCorrelativo
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                model = new PresentacionModel
                {
                    Resultado = 1,
                    Mensaje = "Se produjo un error en la operación",
                    NroTramite = nroTramite,
                    NroCorrelativo = nroCorrelativo
                };
            }

            return model;
        }

        public PresentacionModel AgregarPresentacion(int nroTramite, int nroCorrelativo, string operador)
        {
            PresentacionModel model;
            try
            {
                PresentacionTramite presentacion = _presentacionLogic.AsignarNuevaPresentacion(nroCorrelativo, nroTramite, operador);
                model = new PresentacionModel
                {
                    Resultado = 0,
                    Mensaje = "Contestación realizada con éxito",
                    NroTramite = nroTramite,
                    NroCorrelativo = nroCorrelativo,
                    NroPresentacion = presentacion.NroPresentacion,
                    NroOperacion = presentacion.Operacion.NroOperacion
                };
            }
            catch (GenericException gex)
            {
                _logger.Error(gex);

                model = new PresentacionModel
                {
                    Resultado = 1,
                    Mensaje = gex.Message,
                    NroTramite = nroTramite,
                    NroCorrelativo = nroCorrelativo,
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                model = new PresentacionModel
                {
                    Resultado = 1,
                    Mensaje = "Se produjo un error en la operación",
                    NroTramite = nroTramite,
                    NroCorrelativo = nroCorrelativo
                };
            }

            return model;
        }

        public bool ExisteTramite(int nroTramite)
        {
            return _tramiteLogic.ExisteTramite(nroTramite);
        }

        public IList<TipoSociedad> GetTipoSociedadList() 
        {
            IList<TipoSociedad> tipoSociedadLista = _tramiteLogic.GetTiposSociedad();
            return tipoSociedadLista;
        }

        public IList<TipoTramite> GetTipoTramite()
        {
            return _tramiteLogic.GetTipoTramite();
        }

        public IList<TipoTramiteGratuito> GetTipoTramiteGratuito()
        {
            return _tramiteLogic.GetTipoTramiteGratuito();
        }

        public void VerificarTimbradoVista(int nroTramite, string codigoFormulario, string operador)
        {
             _tramiteLogic.VerificarTimbradoVista(nroTramite, codigoFormulario, operador);
        }
        public void DesarchivarTramite(int nroTramite, int nroCorrelativo, string operador)
        {
             _tramiteLogic.DesarchivarTramite(nroTramite, nroCorrelativo,operador);
        }
        public bool TramiteArchivado(int nroTramite)
        {
            return _vistaLogic.TramiteArchivado(nroTramite);
        }
    }
}
