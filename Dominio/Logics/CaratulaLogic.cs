using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Contracts.Repositories;
using Domain.Contracts.Logics;
using Domain.Exceptions;

namespace Domain.Logics
{
    public class CaratulaLogic : ICaratulaLogic
    {
        private readonly IFormularioRepositorio _formularioRepositorio;
        private readonly ITimbradoRepositorio _timbradoRepositorio;
        private readonly ITramiteRepositorio _tramiteRepositorio;
        private readonly ISociedadRepositorio _sociedadRepositorio;
        private readonly IPresentacionTramiteRepositorio _presentacionRepositorio;
        private readonly IOperacionLogic _operacionLogic;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IReimpresionRespositorio _reimpresionRepositorio;

        public CaratulaLogic(IFormularioRepositorio formularioRepositorio, ITramiteRepositorio tramiteRepositorio, ITimbradoRepositorio timbradoRepositorio,
            ISociedadRepositorio sociedadRepositorio, IPresentacionTramiteRepositorio presentacionRepositorio, IOperacionLogic operacionLogic,
            IReimpresionRespositorio reimpresionRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            _formularioRepositorio = formularioRepositorio;
            _timbradoRepositorio = timbradoRepositorio;
            _sociedadRepositorio = sociedadRepositorio;
            _presentacionRepositorio = presentacionRepositorio;
            _tramiteRepositorio = tramiteRepositorio;
            _operacionLogic = operacionLogic;
            _reimpresionRepositorio = reimpresionRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public Caratula GetReportePorOperacion(long codOperacion)
        {
            var operacion = _operacionLogic.GetPorNroOperacion(codOperacion);
            var formulario = _formularioRepositorio.GetFormulario(Convert.ToInt64(operacion.NroFormulario));

            return GenerarCaratula(operacion, formulario.NroCorrelativo);
        }

        public Caratula GetReportePorFormulario(string codFormulario, string usr)
        {
            var formulario = _formularioRepositorio.GetFormulario(codFormulario);
            var timbradoActivo = GetTimbradoActivo(formulario);

            if (timbradoActivo == null)
                throw new TimbradoNotExistException("Timbrado sin procesar en el sistema");

            var operacion = GuardarReimpresion(timbradoActivo.NroTramite, timbradoActivo.NroFormulario, DateTime.Now, usr);
            var nroCorrelativo = (timbradoActivo.NroCorrelativo > 0)? timbradoActivo.NroCorrelativo: formulario.NroCorrelativo;

            return GenerarCaratula(operacion, nroCorrelativo);
        }

        public Caratula GetReportePorTramite(int nroTramite, string usr)
        {
            int nroCorrelativo;
            long nroFormulario = 0;
            DateTime fecha;
            try
            {
                var tramitegratuito = _tramiteRepositorio.GetTramiteGratuito(nroTramite);
                nroCorrelativo = tramitegratuito.NroCorrelativo;
                fecha = tramitegratuito.Fecha;

                if (tramitegratuito == null) {
                    var timbrado = GetTimbradoActivo(nroTramite);
                    if (timbrado == null)
                        throw new TimbradoNotExistException("Nro. de trámite inexistente en el sistema");
                    var formulario = _formularioRepositorio.GetFormulario(timbrado.NroFormulario);
                    nroCorrelativo = (timbrado.NroCorrelativo > 0) ? timbrado.NroCorrelativo : formulario.NroCorrelativo;
                    nroFormulario = timbrado.NroFormulario;
                    fecha = timbrado.FechaCreacion;
                }
               
              
            }
            catch (TramiteNotExistException)
            {
                //Si no existe el timbrado verificamos en tramites gratuitos
               // var tramite = _tramiteRepositorio.GetTramiteGratuito(nroTramite);
               // nroCorrelativo = tramite.NroCorrelativo;
                //fecha = tramite.Fecha;
                var timbrado = GetTimbradoActivo(nroTramite);
                if (timbrado == null)
                    throw new TimbradoNotExistException("Nro. de trámite inexistente en el sistema");
                var formulario = _formularioRepositorio.GetFormulario(timbrado.NroFormulario);
                nroCorrelativo = (timbrado.NroCorrelativo > 0) ? timbrado.NroCorrelativo : formulario.NroCorrelativo;
                nroFormulario = timbrado.NroFormulario;
                fecha = timbrado.FechaCreacion;
              
            }

            var operacion = GuardarReimpresion(nroTramite, nroFormulario, DateTime.Now, usr);

            return GenerarCaratula(operacion, nroCorrelativo);
        }

        private Timbrado GetTimbradoActivo(Formulario formulario)
        {
            var timbrados = _timbradoRepositorio.GetTimbrados(formulario);
            return  timbrados.Where(t => t.EnUso()).FirstOrDefault();
        }

        private Timbrado GetTimbradoActivo(int nroTramite)
        {
            var timbrados = _timbradoRepositorio.GetTimbrados(nroTramite);
            return timbrados.Where(t => t.EnUso()).FirstOrDefault();
        }

        private Caratula GenerarCaratula(Operacion operacion, int nroCorrelativo)
        {
            Sociedad sociedad = null;
            if(nroCorrelativo > 0)
                sociedad = _sociedadRepositorio.GetSociedad(nroCorrelativo);


            int nroPresentacion;
            int nroTramite = Convert.ToInt32(operacion.NroTramite);
            try
            {
                nroPresentacion = _presentacionRepositorio.ObtenerUltimo(nroTramite).NroPresentacion;
            }
            catch (PresentacionNotExistException)
            {
                nroPresentacion = 1;
            }
            
            return new Caratula
            {
                NroTramite = nroTramite,
                NroFormulario = Convert.ToInt64(operacion.NroFormulario),
                NroPresentacion = nroPresentacion,
                Sociedad = sociedad,
                FechaTimbrado = operacion.Fecha,
                Operacion = operacion
            };
        }

        private Operacion GuardarReimpresion(int nroTramite, long nroFormulario, DateTime fecha, string usr)
        {
            Usuario usuario = _usuarioRepositorio.GetUsuario(usr);
            Operacion operacion = new Operacion(nroFormulario, nroTramite, usuario, TipoOperacion.REIMPRESION);

            operacion.NroOperacion = _operacionLogic.GuardarOperacion(operacion);

            Reimpresion reimpresion = new Reimpresion();
            reimpresion.NroTramite = nroTramite;
            reimpresion.IdUsuario = usuario.Id;
            reimpresion.Usuario = usuario.Alias;
            reimpresion.Fecha = DateTime.Now;

            _reimpresionRepositorio.Guardar(reimpresion);

            return operacion;
        }
    }
}
