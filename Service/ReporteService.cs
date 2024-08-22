using Domain;
using Domain.Contracts.Common;
using Domain.Contracts.Logics;
using Domain.Contracts.Reports;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Domain.Models;
using System;
using System.IO;

namespace Services
{
    public class ReporteService : IReporteService
    {
        private readonly IOperacionLogic _operacionLogic;
        private readonly IFormularioLogic _formularioLogic;
        private readonly ITimbradoLogic _timbradoLogic;
        private readonly ISociedadLogic _sociedadLogic;
        private readonly ITramiteRepositorio _tramiteRepositorio;
        private readonly ICaratulaReport _caratulaReport;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPresentacionTramiteRepositorio _presentacionRepositorio;
        private readonly ILogger _logger;

        public ReporteService(IFormularioLogic formularioLogic, ITimbradoLogic timbradoLogic, IOperacionLogic operacionLogic,
            ISociedadLogic sociedadLogic, ICaratulaReport caratulaReport, ITramiteRepositorio tramiteRepositorio,
            IUsuarioRepositorio usuarioRepositorio, IPresentacionTramiteRepositorio presentacionRepositorio, ILogger logger)
        {
            _operacionLogic = operacionLogic;
            _formularioLogic = formularioLogic;
            _timbradoLogic = timbradoLogic;
            _sociedadLogic = sociedadLogic;
            _caratulaReport = caratulaReport;
            _usuarioRepositorio = usuarioRepositorio;
            _presentacionRepositorio = presentacionRepositorio;
            _tramiteRepositorio = tramiteRepositorio;
            _logger = logger;
        }

        public ReporteModel GetPorFormulario(string codigoFormulario)
        {
            ReporteModel model;

            try
            {
                Formulario formulario = _formularioLogic.GetFormulario(codigoFormulario);
                Timbrado timbrado = _timbradoLogic.GetUltimoProcesado(formulario);
                Sociedad sociedad = _sociedadLogic.GetPorNumeroCorrelativo(formulario.NroCorrelativo);

                model = new ReporteModel
                {
                    Resultado = 0,
                    NroFormulario = formulario.NroFormulario,
                    Urgente = formulario.Urgente,
                    NroTramite = timbrado.NroTramite,
                    NroCorrelativo = sociedad.NroCorrelativo,
                    Denominacion = sociedad.Nombre,
                    TipoSocietario = sociedad.TipoSociedad.Descripcion
                };
            }
            catch (GenericException gex)
            {
                _logger.Error(gex);

                model = new ReporteModel
                {
                    Resultado = 1,
                    Mensaje = gex.Message
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                model = new ReporteModel
                {
                    Resultado = 1,
                    Mensaje = "Ocurrio un error en la operación"
                };
            }

            return model;
        }

        public ReporteModel GetPorTramite(int nroTramite)
        {
            return null;
        }

        public ReporteModel GetPorFormularioYTramite(string codigoFormulario, int nroTramite)
        {
            throw new NotImplementedException();
        }

        public Stream GetCaratula(long nroOperacion, string usr)
        {
            Operacion operacion = _operacionLogic.GetPorNroOperacion(nroOperacion);
            Caratula caratula = new Caratula();
            Sociedad sociedad = null;

            try
            {
                Formulario formulario = _formularioLogic.GetFormulario(Int64.Parse(operacion.NroFormulario.ToString()));
                var presentacion = _presentacionRepositorio.ObtenerUltimo((int)operacion.NroTramite);

                if (presentacion.NroCorrelativo > 0)
                    sociedad = _sociedadLogic.GetPorNumeroCorrelativo(presentacion.NroCorrelativo);
                else if (formulario != null && formulario.NroCorrelativo > 0)
                    sociedad = _sociedadLogic.GetPorNumeroCorrelativo(formulario.NroCorrelativo);

                bool urgente = false;
                if (operacion.NroFormulario > 0)
                    urgente = _formularioLogic.EsUrgente((long)operacion.NroFormulario);

                int codTramite = 0;
                if (int.TryParse(presentacion.CodTramite, out codTramite))
                    caratula.CodTramite = codTramite;

                caratula.NroFormulario = (long)operacion.NroFormulario;
                caratula.NroTramite = presentacion.NroTramite;
                caratula.NroPresentacion = presentacion.NroPresentacion;
                caratula.Sociedad = sociedad;
                caratula.Operacion = operacion;
                caratula.Urgente = urgente;
            }
            catch (PresentacionNotExistException)
            {
                GetCaratulaTramiteGratuito(operacion, caratula);
            }
            catch (FormularioNotExistException)
            {
                GetCaratulaTramiteGratuito(operacion, caratula);
            }

            return _caratulaReport.GetReporteCaratula(caratula);
        }

        public Stream GetCaratula(int nroCorrelativo, long nroFormulario, int nroTramite, int nroPresentacion, long nroOperacion, string usr)
        {
            Operacion operacion = _operacionLogic.GetPorNroOperacion(nroOperacion);
            Formulario formulario = _formularioLogic.GetFormulario(Int64.Parse(operacion.NroFormulario.ToString()));
            Sociedad sociedad = null;
            bool urgente = false;

            if (nroCorrelativo > 0)
                sociedad = _sociedadLogic.GetPorNumeroCorrelativo(nroCorrelativo);
            else if (formulario != null && formulario.NroCorrelativo > 0)
                sociedad = _sociedadLogic.GetPorNumeroCorrelativo(formulario.NroCorrelativo);

            if (nroFormulario > 0)
                urgente = _formularioLogic.EsUrgente(nroFormulario);

            Caratula caratula = new Caratula()
            {
                NroFormulario = nroFormulario,
                NroTramite = nroTramite,
                NroPresentacion = nroPresentacion,
                Sociedad = sociedad,
                Operacion = operacion,
                Urgente = urgente,
            };

            return _caratulaReport.GetReporteCaratula(caratula);
        }

        private void GetCaratulaTramiteGratuito(Operacion operacion, Caratula caratula)
        {
            Sociedad sociedad;
            //Se verifica que sea un tramite gratuito ya que no posee presentaciones
            TramiteGratuito tramite = _tramiteRepositorio.GetTramiteGratuito((int)operacion.NroTramite);
            sociedad = _sociedadLogic.GetPorNumeroCorrelativo(tramite.NroCorrelativo);
            caratula.NroTramite = tramite.NroTramite;
            caratula.Operacion = operacion;
            caratula.Sociedad = sociedad;
            caratula.CodTramite = tramite.CodTramite;
        }
    }
}