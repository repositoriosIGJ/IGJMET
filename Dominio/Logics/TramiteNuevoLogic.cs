using Domain.Contracts.Common;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace Domain.Logics
{
    public class TramiteNuevoLogic : TramiteBaseLogic
    {
        private string _codFormulario;
        private Formulario _formulario;
        private int _nroTramite;
        private readonly ITimbradoLogic _timbradoLogic;
        private readonly IFormularioLogic _formularioLogic;
        private readonly IPresentacionTramiteLogic _presentacionLogic;
        private readonly INomencladorLogic _nomencladorLogic;
        private readonly ITramiteRepositorio _tramiteRepositorio;
        private readonly IENTEServiceClient _enteRepositorio;
        private readonly IBPMServiceClient _bpmRepositorio;
        private readonly ILogger _logger;

        public TramiteNuevoLogic(Usuario usuario, string codFormulario, IOperacionLogic operacionLogic, IFormularioLogic formularioLogic, ITimbradoLogic timbradoLogic, INomencladorLogic nomencladorLogic,
            IPresentacionTramiteLogic presentacionLogic, ITramiteRepositorio tramiteRepositorio, IENTEServiceClient enteRepositorio, IBPMServiceClient bpmRepositorio,
            ILogger logger)
            : base(usuario, operacionLogic)
        {
            _codFormulario = codFormulario;
            _timbradoLogic = timbradoLogic;
            _formularioLogic = formularioLogic;
            _presentacionLogic = presentacionLogic;
            _nomencladorLogic = nomencladorLogic;
            _tramiteRepositorio = tramiteRepositorio;
            _enteRepositorio = enteRepositorio;
            _bpmRepositorio = bpmRepositorio;
            _logger = logger;
        }

        public override int Procesar()
        {
            VerficarDatos();
            var timbrado = _enteRepositorio.ConsultarTimbrado(_formulario, _nroTramite, _usuario);
            PresentacionTramite presentacion;
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    timbrado.IdUsuario = _usuario.Id;
                    _timbradoLogic.Guardar(timbrado);

                    presentacion = GenerarNuevaPresentacion();
                    _presentacionLogic.GuardarPresentacion(presentacion);

                    TramiteDigital tramiteDigital = new TramiteDigital();
                    tramiteDigital.NroTramite = _nroTramite;
                    tramiteDigital.NroPresentacion = presentacion.NroPresentacion;
                    tramiteDigital.NroCorrelativo = _formulario.NroCorrelativo;
                    tramiteDigital.Guid = _usuario.Id.ToString();
                    tramiteDigital.Fecha = DateTime.Now;
                    _tramiteRepositorio.GuardarDigital(tramiteDigital);

                    _operacionLogic.GuardarOperacion(timbrado, TipoOperacion.CON_TIMBRADO, _usuario);

                    scope.Complete();
                }
            }
            catch (ServiceEnteTimeOutException tox)
            {
                //Se persiste el tramite fallido
                CrearTimbradoFallido(Estado.ER_CON);
                throw tox;
            }
            catch (ServiceEnteTimbradoException timx)
            {
                //Se persiste el tramite fallido
                CrearTimbradoFallido(Estado.ER);
                throw timx;
            }
            catch (Exception ex)
            {
                //Se persiste el tramite fallido
                CrearTimbradoFallido(Estado.ER_CON);
                throw ex;
            }

            if (_nomencladorLogic.TramiteTipoBPM(timbrado))
            {
                _bpmRepositorio.RegistrarProceso(timbrado.NroTramite, presentacion.NroPresentacion, presentacion.NroCorrelativo, timbrado.Formulario.Urgente);
            }

            return _nroTramite;
        }

        private void VerficarDatos()
        {
            try
            {
                //Verificación de existencia de formulario digital
                _formulario = _formularioLogic.GetFormulario(_codFormulario);
                Timbrado timbrado = _timbradoLogic.GetUltimoProcesado(_formulario);

                if (timbrado.EnUso())
                {
                    _logger.Info("El timbrado ya se encuentra utilizado. Nro.: " + _codFormulario);
                    throw new TimbradoAlreadyInProcessException("El timbrado ya se encuentra utilizado");
                }

                _logger.Info("Coninuando en estado ER tramite para el timbrado: " + _codFormulario);
                _nroTramite = timbrado.NroTramite;
            }
            catch (TimbradoNotExistException)
            {
                //El timbrado no existe, se genera un numero de trámite
                _logger.Info("Generando nuevo tramite para el timbrado: " + _codFormulario);
                _nroTramite = _tramiteRepositorio.GenerarNroTramite();
            }
        }

        private PresentacionTramite GenerarNuevaPresentacion()
        {
            PresentacionTramite presentacion = _presentacionLogic.GetUltimaPresentacion(_nroTramite);
            presentacion.Usuario = _usuario;
            presentacion.NroPresentacion++;
            presentacion.Fecha = DateTime.Now;
            return presentacion;
        }

        private void CrearTimbradoFallido(Estado codigo)
        {
            Timbrado timbrado = new Timbrado();
            timbrado.Formulario = _formulario;
            timbrado.NroTramite = _nroTramite;
            timbrado.NroPagoElectronico = _timbradoLogic.ObtenerPagoElectronico();
            timbrado.IdUsuario = _usuario.Id;
            timbrado.Estado = codigo;

            _timbradoLogic.Guardar(timbrado);
        }
    }
}