using System.CodeDom;
using Domain.Contracts.Common;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;
using Domain.Exceptions;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Domain.Enum;
using Domain.Enums;

namespace Domain.Logics
{
    public class TramiteLogic : ITramiteLogic
    {
        private readonly ITimbradoLogic _timbradoLogic;
        private readonly IOperacionLogic _operacionLogic;
        private readonly IFormularioLogic _formularioLogic;
        private readonly IModuloTramiteLogic _moduloLogic;
        private readonly ITramiteRepositorio _tramiteRepositorio;
        private readonly IPresentacionTramiteLogic _presentacionLogic;
        private readonly INomencladorLogic _nomencladorLogic;
        private readonly IENTEServiceClient _enteRepositorio;
        private readonly IBPMServiceClient _bpmRepositorio;
        private readonly IGestionTramiteServiceClient _tramiteServiceClient;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IDestraRepositorio _trazabilidadRepositorio;
        private readonly ISociedadRepositorio _sociedadRepositorio;
        private readonly ILogger _logger;
        private readonly ITipoTramiteRepositorio _tipoTramiteRepositorio;

        private Usuario _usuario;

        public TramiteLogic(IFormularioLogic formularioLogic, ITimbradoLogic timbradoLogic,
            IOperacionLogic operacionLogic, INomencladorLogic nomencladorLogic,
            IPresentacionTramiteLogic presentacionLogic, ITramiteRepositorio tramiteRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            IENTEServiceClient enteRepositorio, IBPMServiceClient bpmRepositorio, IModuloTramiteLogic moduloLogic,
            IDestraRepositorio trazabilidadRepositorio, ILogger logger, ISociedadRepositorio sociedadRepositorio,
            ITipoTramiteRepositorio tipoTramiteRepositorio, IGestionTramiteServiceClient tramiteServiceClient)
        {
            _timbradoLogic = timbradoLogic;
            _operacionLogic = operacionLogic;
            _formularioLogic = formularioLogic;
            _presentacionLogic = presentacionLogic;
            _nomencladorLogic = nomencladorLogic;
            _tramiteRepositorio = tramiteRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _enteRepositorio = enteRepositorio;
            _moduloLogic = moduloLogic;
            _bpmRepositorio = bpmRepositorio;
            _trazabilidadRepositorio = trazabilidadRepositorio;
            _logger = logger;
            _sociedadRepositorio = sociedadRepositorio;
            _tipoTramiteRepositorio = tipoTramiteRepositorio;
            _tramiteServiceClient = tramiteServiceClient;
        }

        public Tramite GetTramite(int nroTramite)
        {
            Tramite tramite = _tramiteRepositorio.GetTramite(nroTramite);
            return tramite;
        }

        public Tramite GetTramitePorTimbrado(int nroTramite)
        {
            Tramite tramite = _tramiteRepositorio.GetTramitePorTimbrado(nroTramite);
            tramite.Timbrados = _timbradoLogic.GetTimbrados(nroTramite);
            return tramite;
        }

        #region TramitePago

        public Tramite IniciarTramitePago(string codFormulario, string operador)
        {
            //Verificacion del operador
            Usuario usuario = _usuarioRepositorio.GetUsuario(operador);

            Formulario formulario = null;
            int nroTramite;

            try
            {
                //Verificación de existencia de formulario digital
                formulario = _formularioLogic.GetFormulario(codFormulario);
                Timbrado timbrado = _timbradoLogic.GetUltimoProcesado(formulario);

                if (timbrado.EnUso())
                {
                    _logger.Info("El timbrado ya se encuentra utilizado. Nro.: " + codFormulario);
                    throw new TimbradoAlreadyInProcessException("El timbrado ya se encuentra utilizado");
                }

                _logger.Info("Coninuando en estado ER tramite para el timbrado: " + codFormulario);
                nroTramite = timbrado.NroTramite;
            }
            catch (TimbradoNotExistException)
            {
                //El timbrado no existe, se genera un numero de tramite
                _logger.Info("Generando nuevo tramite para el timbrado: " + codFormulario);
                nroTramite = _tramiteRepositorio.GenerarNroTramite();
            }

            Operacion operacion = new Operacion(formulario.NroFormulario, nroTramite, usuario,
                TipoOperacion.CON_TIMBRADO);
            PresentacionTramite presentacion = new PresentacionTramite(nroTramite, usuario, 1);
            presentacion.NroCorrelativo = formulario.NroCorrelativo;

            operacion.NroOperacion = IniciarTransaccion(formulario, nroTramite, operacion, presentacion, usuario,
                string.Empty);

            Tramite tramite = new Tramite
            {
                NroTramite = nroTramite,
                UltimoNroPresentacion = presentacion.NroPresentacion,
                NroCorrelativo = presentacion.NroCorrelativo
            };

            tramite.Operaciones.Add(operacion);

            //TODO. Deshabilitación de la clasificación automatica hasta entrega de las funcionalidades asociadas.
            //_tramiteServiceClient.Clasificar(codFormulario);

            return tramite;
        }

        public Tramite GuardarTramite(string codFormulario, string operador)
        {
            //Verificacion del operador
            _usuario = _usuarioRepositorio.GetUsuario(operador);

            Formulario formulario = null;
            Timbrado timbrado = null;
            Tramite tramite = new Tramite();

            try
            {
                //Verificación de existencia de formulario digital
                formulario = _formularioLogic.GetFormulario(codFormulario);
                timbrado = _timbradoLogic.GetUltimoProcesado(formulario);

                if (timbrado.EnUso())
                {
                    _logger.Info("El timbrado ya se encuentra utilizado. Nro.: " + codFormulario);
                    throw new TimbradoAlreadyInProcessException("El timbrado ya se encuentra utilizado");
                }

                _logger.Info("Coninuando en estado ER tramite para el timbrado: " + codFormulario);
                tramite.NroCorrelativo = timbrado.NroCorrelativo;
                tramite.NroTramite = timbrado.NroTramite;
                tramite.Timbrados.Add(timbrado);
            }
            catch (TimbradoNotExistException)
            {
                //El timbrado no existe, se genera un numero de tramite
                _logger.Info("Generando nuevo tramite para el timbrado: " + codFormulario);
                timbrado = new Timbrado
                {
                    Formulario = formulario,
                    NroCorrelativo = formulario.NroCorrelativo
                };

                tramite.NroCorrelativo = timbrado.NroCorrelativo;
            }

            tramite.Timbrados.Add(timbrado);

            return tramite;
        }

        private void IniciarTransaccion(Tramite tramite)
        {
            try
            {
                var esBPM = false;

                //Se verifica si el monto abonado es adecuado al tipo de tramite a realizar
                VerificarValorTimbrado(tramite.Timbrados[0]);

                //Se inicia la transaccion de persistencia
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    if (tramite.NroTramite == 0)
                    {
                        tramite.NroTramite = _tramiteRepositorio.GenerarNroTramite();
                        tramite.Timbrados[0].NroTramite = tramite.NroTramite;
                    }

                    //Generacion del timbrado
                    _timbradoLogic.Guardar(tramite.Timbrados[0]);

                    //Generación de la operación
                    GenerarOperacion(tramite, _usuario);

                    //Generación de la presentación
                    GenerarPresentacion(tramite, _usuario);

                    //Generación de tramite digital para DIDO
                    GenerarTramiteDigital(tramite, _usuario);

                    //Persistencia del tramite
                    _tramiteRepositorio.GuardarTramite(tramite);

                    // Verificacion de tramite tipo BPM
                    esBPM = _nomencladorLogic.TramiteTipoBPM(tramite.Timbrados[0]);

                    //Generacion de una ruta en destra
                    GenerarRutaDestra(tramite, _usuario, esBPM);

                    scope.Complete();
                }

                if (esBPM)
                    _bpmRepositorio.RegistrarProceso(tramite.NroTramite, 1, tramite.NroCorrelativo,
                        tramite.Timbrados[0].Formulario.Urgente);
            }
            catch (TimbradoInsufficientValueException tivex)
            {
                _logger.Info(tivex.ToString());
                throw new Exception("El valor del timbrado insuficiente");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion TramitePago

        #region TramiteGratuito

        public Tramite IniciarTramiteNoPago(int nroCorrelativo, string operador, string codigoTipoTramite)
        {
            Usuario usuario = _usuarioRepositorio.GetUsuario(operador);

            int nroTramite = _tramiteRepositorio.GenerarNroTramite();
            var actualizarCorrelativos = _sociedadRepositorio.ExisteCorrelativoSociedadSinRegistrar(nroCorrelativo);

            if (actualizarCorrelativos)
            {
                ActualizarCorrelativoSinRegistrar(nroCorrelativo, nroTramite, codigoTipoTramite);
            }
            else
            {
                AgregarCorrelativoSinRegistrar(nroCorrelativo, usuario.Id.ToString(), codigoTipoTramite, nroTramite);
            }

            Operacion operacion = new Operacion(0, nroTramite, usuario, TipoOperacion.SIN_TIMBRADO);

            PresentacionTramite presentacion = new PresentacionTramite(nroTramite, usuario, 1);
            presentacion.NroCorrelativo = nroCorrelativo;

            operacion.NroOperacion = IniciarTransaccion(nroTramite, operacion, presentacion, usuario, codigoTipoTramite);

            Tramite tramite = new Tramite
            {
                NroTramite = nroTramite,
                UltimoNroPresentacion = presentacion.NroPresentacion,
                NroCorrelativo = nroCorrelativo
            };

            tramite.Operaciones.Add(operacion);

            return tramite;
        }

        private void AgregarCorrelativoSinRegistrar(int nroCorrelativo, string operador, string codigoTipoTramite,
            int nroTramite)
        {
            _sociedadRepositorio.AgregarCorrelativoSinRegistrar(nroCorrelativo, operador, codigoTipoTramite, nroTramite);
        }

        public void ActualizarCorrelativoSinRegistrar(int nroCorrelativo, int nroTramite, string codigoTipoTramite)
        {
            _sociedadRepositorio.ActualizarCorrelativoSinRegistrar(nroCorrelativo, nroTramite, codigoTipoTramite);
        }

        #endregion TramiteGratuito

        #region AdicionarTimbrado

        public Tramite AgregarTimbradoATramite(int nroTramite, string codFormulario, string operador)
        {
            Usuario usuario = _usuarioRepositorio.GetUsuario(operador);

            Formulario formulario = null;

            //Version 2.0 se verifica que existe la entidad tramite, momentaneamente no implementado
            //if (!_tramiteRepositorio.ExisteTramite(nroTramite))
            //    throw new TramiteNotExistException("Nro. de tramite inexistente en el sistema");

            try
            {
                //Verificación de existencia de formulario digital
                formulario = _formularioLogic.GetFormulario(codFormulario);

                var timbrado = _timbradoLogic.GetUltimoProcesado(formulario, nroTramite);
            }
            catch (TimbradoNotExistException)
            {
                //El timbrado no existe, se adiciona una timbrado al tramite
            }

            Operacion operacion = new Operacion(formulario.NroFormulario, nroTramite, usuario,
                TipoOperacion.AGREGAR_TIMBRADO);

            operacion.NroOperacion = IniciarTransaccion(formulario, nroTramite, operacion, null, usuario, string.Empty);

            PresentacionTramite presentacion = _presentacionLogic.GetUltimaPresentacion(nroTramite);

            Tramite tramite = new Tramite
            {
                NroTramite = nroTramite,
                NroCorrelativo = presentacion.NroCorrelativo,
                UltimoNroPresentacion = presentacion.NroPresentacion
            };

            tramite.Operaciones.Add(operacion);

            return tramite;
        }

        #endregion AdicionarTimbrado

        public bool ExisteTramite(int nroTramite)
        {
            return _tramiteRepositorio.ExisteTramite(nroTramite);
        }

        private long IniciarTransaccion(int nroTramite, Operacion operacion, PresentacionTramite presentacion,
            Usuario usuario, string codTramite)
        {
            return IniciarTransaccion(null, nroTramite, operacion, presentacion, usuario, codTramite);
        }

        private long IniciarTransaccion(Formulario formulario, int nroTramite, Operacion operacion, PresentacionTramite presentacion, Usuario usuario, string codTramite)
        {
            long nroOperacion = 0;
            Timbrado timbrado = null;
            var requiereTimbrado = RequiereTimbrado(operacion.TipoOperacion);
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    if (requiereTimbrado)
                    {
                        timbrado = _enteRepositorio.ConsultarTimbrado(formulario, nroTramite, usuario);
                        timbrado.IdUsuario = usuario.Id;
                        timbrado.NroCorrelativo = formulario.NroCorrelativo;

                        var ultimoTimbrado = _timbradoLogic.GetUltimoValido(nroTramite);

                        if (ultimoTimbrado != null)
                        {
                            timbrado.FechaOperacion = ultimoTimbrado.FechaOperacion;
                            timbrado.NroCorrelativo = ultimoTimbrado.NroCorrelativo;
                            timbrado.CodTramite = ultimoTimbrado.CodTramite;
                        }

                        _timbradoLogic.Guardar(timbrado);

                        if (operacion.TipoOperacion == TipoOperacion.CON_TIMBRADO)
                        {
                            _presentacionLogic.GuardarPresentacion(presentacion);

                            GenerarTramiteDigital(nroTramite, presentacion, usuario);
                        }
                    }
                    else
                        GenerarTramiteGratuito(nroTramite, presentacion, usuario, codTramite);

                    nroOperacion = _operacionLogic.GuardarOperacion(operacion);

                    scope.Complete();
                }
            }
            catch (ServiceEnteTimeOutException tox)
            {
                //Se persiste el tramite fallido
                CrearTimbradoFallido(formulario, nroTramite, Estado.ER_CON, usuario, operacion);
                throw tox;
            }
            catch (ServiceEnteTimbradoException timx)
            {
                //Se persiste el tramite fallido
                CrearTimbradoFallido(formulario, nroTramite, Estado.ER, usuario, operacion);
                throw timx;
            }
            catch (Exception ex)
            {
                //Se persiste el tramite fallido
                _logger.Error(ex);
                CrearTimbradoFallido(formulario, nroTramite, Estado.ER_CON, usuario, operacion);
                throw ex;
            }

            if (requiereTimbrado && operacion.TipoOperacion == TipoOperacion.CON_TIMBRADO &&
                _nomencladorLogic.TramiteTipoBPM(timbrado))
            {
                _bpmRepositorio.RegistrarProceso(timbrado.NroTramite, presentacion.NroPresentacion,
                    formulario.NroCorrelativo, timbrado.Formulario.Urgente);
            }

            return nroOperacion;
        }

        private bool RequiereTimbrado(TipoOperacion tipoOperacion)
        {
            return (tipoOperacion == TipoOperacion.AGREGAR_TIMBRADO || tipoOperacion == TipoOperacion.CON_TIMBRADO);
        }

        private void VerificarValorTimbrado(Timbrado timbrado)
        {
            var timVer = _enteRepositorio.ConsultarTimbrado(timbrado.Formulario, timbrado.NroFormulario, _usuario);

            timbrado.Monto = timVer.Monto;
            timbrado.Caja = timVer.Caja;

            ModuloTramite modulo = _moduloLogic.GetByFormulario(timbrado.Formulario);

            if (modulo.Cantidad > timbrado.Monto)
                throw new TimbradoInsufficientValueException();
        }

        private void GenerarTramiteGratuito(int nroTramite, PresentacionTramite presentacion, Usuario usuario,
            string codTramite)
        {
            TramiteGratuito tramiteGratuito = new TramiteGratuito();
            tramiteGratuito.Fecha = DateTime.Now;
            tramiteGratuito.NroCorrelativo = presentacion.NroCorrelativo;
            tramiteGratuito.NroTramite = nroTramite;
            tramiteGratuito.Usuario = usuario;
            tramiteGratuito.CodTramite = int.Parse(codTramite);
            _tramiteRepositorio.GuardarTramiteGratuito(tramiteGratuito);
        }

        private long GenerarOperacion(Tramite tramite, Usuario usuario)
        {
            Operacion operacion = new Operacion(tramite.Timbrados[0].Formulario.NroFormulario, tramite.NroTramite,
                usuario, TipoOperacion.CON_TIMBRADO);
            return _operacionLogic.GuardarOperacion(operacion);
        }

        private void GenerarPresentacion(Tramite tramite, Usuario usuario)
        {
            PresentacionTramite presentacion = new PresentacionTramite(tramite.NroTramite, usuario, 1);
            presentacion.NroCorrelativo = tramite.NroCorrelativo;
            _presentacionLogic.GuardarPresentacion(presentacion);
        }

        private void GenerarTramiteDigital(Tramite tramite, Usuario usuario)
        {
            TramiteDigital tramiteDigital = new TramiteDigital();
            tramiteDigital.NroTramite = tramite.NroTramite;
            tramiteDigital.NroPresentacion = 1;
            tramiteDigital.NroCorrelativo = tramite.NroCorrelativo;
            tramiteDigital.Guid = usuario.Id.ToString();
            tramiteDigital.Fecha = DateTime.Now;
            _tramiteRepositorio.GuardarDigital(tramiteDigital);
        }

        private void GenerarTramiteDigital(int nroTramite, PresentacionTramite presentacion, Usuario usuario)
        {
            TramiteDigital tramiteDigital = new TramiteDigital();
            tramiteDigital.NroTramite = nroTramite;
            tramiteDigital.NroPresentacion = presentacion.NroPresentacion;
            tramiteDigital.NroCorrelativo = 0;
            tramiteDigital.Guid = usuario.Id.ToString();
            tramiteDigital.Fecha = DateTime.Now;
            _tramiteRepositorio.GuardarDigital(tramiteDigital);
        }

        private void GenerarRutaDestra(Tramite tramite, Usuario usuario, bool bpm)
        {
            TrazabilidadTramite trazabilidad = new TrazabilidadTramite();
            trazabilidad.FechaTramite = tramite.Fecha;
            trazabilidad.NroTramite = tramite.NroTramite;
            trazabilidad.NroCorrelativo = tramite.NroCorrelativo;
            trazabilidad.Destino = "DMET";
            trazabilidad.Origen = (bpm) ? "RBPM" : "RECU";
            trazabilidad.FechaDesde = DateTime.Now;
            trazabilidad.Usuario = usuario;
            _trazabilidadRepositorio.Guardar(trazabilidad);
        }

        private void CrearTimbradoFallido(Formulario formulario, int nroTramite, Estado codigo, Usuario usuario,
            Operacion operacion)
        {
            if (RequiereTimbrado(operacion.TipoOperacion))
            {
                string msg =
                    string.Format("Creando timbrado fallido. Tramite: {0} formulario: {1} usuario: {2} operacion: {3}",
                        nroTramite, formulario.NroFormulario, usuario.Id, operacion.TipoOperacion);
                _logger.Info(msg);

                Timbrado timbrado = new Timbrado();
                timbrado.Formulario = formulario;
                timbrado.NroTramite = nroTramite;
                timbrado.NroCorrelativo = formulario.NroCorrelativo;
                timbrado.NroPagoElectronico = _timbradoLogic.ObtenerPagoElectronico();
                timbrado.IdUsuario = usuario.Id;
                timbrado.Estado = codigo;
                _timbradoLogic.Guardar(timbrado);
            }
        }

        public IList<TipoSociedad> GetTiposSociedad()
        {
            return _tramiteRepositorio.GetTiposSociedad();
        }

        public int GetCodigoTipoSociedadById(int id)
        {
            return _tramiteRepositorio.GetCodigoTipoSociedadById(id);
        }

        public void VerificarTimbradoVista(int nroTramite, string codigoFormulario, string operador)
        {

            var usuario = _usuarioRepositorio.GetUsuario(operador);

            var formulario = _formularioLogic.GetFormulario(codigoFormulario);

            _enteRepositorio.ConsultarTimbrado(formulario, nroTramite, usuario);

            AgregarTimbradoATramite(nroTramite, codigoFormulario, operador);

            var listaTimbrados = (List<Timbrado>)_timbradoLogic.GetTimbrados(nroTramite);

            var timbrado = listaTimbrados.Single(x => x.NroFormulario == formulario.NroFormulario);

            if (!ConsultoValorFormulario(timbrado))
                throw new TimbradoInsufficientValueException();
        }

        public void DesarchivarTramite(int nroTramite, int nroCorrelativo, string operador)
        {
            var ultimoMovimiento = _trazabilidadRepositorio.ObtenerUltimoEstadoTramite(nroTramite);

            var fecha = DateTime.Now;

            ultimoMovimiento.FechaHasta = fecha;

            _trazabilidadRepositorio.ActualizarUltimoEstado(ultimoMovimiento);


            var destino = EvaluarDestino(ultimoMovimiento);

            var usuario = _usuarioRepositorio.GetUsuario(operador);

            string user = (Convert.ToString(usuario.Id)).PadLeft(2, '0');

            var trazabilidad = new TrazabilidadTramite();
            trazabilidad.FechaTramite = ultimoMovimiento.FechaTramite;
            trazabilidad.CodTramite = ultimoMovimiento.CodTramite;
            trazabilidad.NroTramite = nroTramite;
            trazabilidad.NroCorrelativo = nroCorrelativo;
            trazabilidad.Destino = destino;
            trazabilidad.Origen = ultimoMovimiento.Destino;
            trazabilidad.FechaDesde = DateTime.Now;
            trazabilidad.Usuario = usuario;
            _trazabilidadRepositorio.Guardar(trazabilidad);


        }

        private string EvaluarDestino(TrazabilidadTramite ultimoMovimiento)
        {
            string retornaDestino=null;
            //EnumHelper<Destino>.GetDisplayValue(value);
            Destino destino = (Destino) System.Enum.Parse(typeof(Destino), ultimoMovimiento.Destino);

            switch (destino)
            {
                case Destino.CICR:                   
                case Destino.CICU:
                    retornaDestino = Domain.Enums.EnumUtils.EnumHelper<Destino>.GetDisplayValue(Destino.CIC2);
                    break;
                case Destino.OFE3:
                case Destino.OFER:
                    retornaDestino = Domain.Enums.EnumUtils.EnumHelper<Destino>.GetDisplayValue(Destino.OFE2);
                    break;
                case Destino.DCF3:
                    retornaDestino = Domain.Enums.EnumUtils.EnumHelper<Destino>.GetDisplayValue(Destino.DCF2);
                    break;
            }

            if (retornaDestino==null)
            {
                throw new Exception("No se encuentra destino");
            }
            return retornaDestino;

        }
        public IList<TipoTramite> GetTipoTramite()
        {
            return _tramiteRepositorio.GetTipoTramite();
        }

        public IList<TipoTramiteGratuito> GetTipoTramiteGratuito()
        {
            return _tipoTramiteRepositorio.GetTipoTramiteGratuito();
        }

        public Boolean ConsultoValorFormulario(Timbrado timbrado)
        {

            if (timbrado.Monto >= 100)
            {
                _timbradoLogic.Actualizotimbrado(timbrado);
                return true;

            }
            return false;
        }

    }
}


