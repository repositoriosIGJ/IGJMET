using System;
using System.Transactions;
using Domain.Contracts.Common;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;
using Domain.Exceptions;

namespace Domain.Logics
{
    public class TramiteAgregarTimbradoLogic: TramiteBaseLogic
    {
        private int _nroTramite;
        private string _codFormulario;
        private Formulario _formulario;
        private readonly ITimbradoLogic _timbradoLogic;
        private readonly IFormularioLogic _formularioLogic;
        private readonly ITramiteRepositorio _tramiteRepositorio;
        private readonly IENTEServiceClient _enteRepositorio;
        private readonly ILogger _logger;

        public TramiteAgregarTimbradoLogic(Usuario usuario, int nroTramite, string codFormulario, IOperacionLogic operacionLogic, IFormularioLogic formularioLogic, ITimbradoLogic timbradoLogic, INomencladorLogic nomencladorLogic,
            IPresentacionTramiteLogic presentacionLogic, ITramiteRepositorio tramiteRepositorio, IENTEServiceClient enteRepositorio, IBPMServiceClient bpmRepositorio, ILogger logger)
            : base(usuario, operacionLogic)
        {
            _nroTramite = nroTramite;
            _codFormulario = codFormulario;
            _timbradoLogic = timbradoLogic;
            _formularioLogic = formularioLogic;
            _tramiteRepositorio = tramiteRepositorio;
            _enteRepositorio = enteRepositorio;
            _logger = logger;
        }

        public override int Procesar()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    VerificarDatos();

                    Timbrado timbrado = _enteRepositorio.ConsultarTimbrado(_formulario, _nroTramite, _usuario);
                    timbrado.IdUsuario = _usuario.Id;
                    _timbradoLogic.Guardar(timbrado);

                    _operacionLogic.GuardarOperacion(timbrado, TipoOperacion.AGREGAR_TIMBRADO, _usuario);

                    scope.Complete();
                }

                return _nroTramite;
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
        }

        public void VerificarDatos()
        {
            try
            {
                //Verificación de existencia de formulario digital
                Formulario formulario = _formularioLogic.GetFormulario(_codFormulario);
                Timbrado timbrado = _timbradoLogic.GetUltimoProcesado(_formulario, _nroTramite);

                if (timbrado.EnUso())
                {
                    _logger.Info("El timbrado ya se encuentra utilizado. Nro.: " + _codFormulario);
                    throw new TimbradoAlreadyInProcessException("El timbrado ya se encuentra utilizado");
                }

                _logger.Info("Coninuando el timbrado con estado: " + timbrado.Estado.ToString());
            }
            catch (TimbradoNotExistException)
            {
                //El timbrado no existe, se genera un numero de trámite
                _logger.Info("Generando nuevo timbrado para el tramite: " + _nroTramite);
            }
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
