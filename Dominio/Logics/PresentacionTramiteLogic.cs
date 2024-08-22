using Domain;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace Domain.Logics
{
    public class PresentacionTramiteLogic : IPresentacionTramiteLogic
    {
        private readonly IPresentacionTramiteRepositorio _presentacionRepositorio;
        private readonly IOperacionRepositorio _operacionRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITramiteRepositorio _tramiteRepositorio;
        private readonly IBPMServiceClient _bpmClient;
        private readonly IDestraRepositorio _trazabilidadRepositorio;
        private readonly ITimbradoLogic _timbradoLogic;

        public PresentacionTramiteLogic(IPresentacionTramiteRepositorio presentacionRepositorio, IUsuarioRepositorio usuarioRepositorio,
            IOperacionRepositorio operacionRepositorio, ITramiteRepositorio tramiteRepositorio, IBPMServiceClient bpmClient, 
            IDestraRepositorio trazabilidadRepositorio,ITimbradoLogic timbradoLogic)
        {
            _presentacionRepositorio = presentacionRepositorio;
            _operacionRepositorio = operacionRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _tramiteRepositorio = tramiteRepositorio;
            _bpmClient = bpmClient;
            _trazabilidadRepositorio = trazabilidadRepositorio;
            _timbradoLogic = timbradoLogic;
        }

        public PresentacionTramite GetUltimaPresentacion(int nroTramite)
        {
            return _presentacionRepositorio.ObtenerUltimo(nroTramite);
        }

        public PresentacionTramite AsignarNuevaPresentacion(int nroCorrelativo, int nroTramite, string operador)
        {
            Usuario usuario = _usuarioRepositorio.GetUsuario(operador);
            Tramite tramite = _tramiteRepositorio.GetTramite(nroTramite);

            PresentacionTramite presentacion = GetUltimaPresentacion(nroTramite);
            presentacion.Usuario = usuario;
            presentacion.NroCorrelativo = nroCorrelativo;
            presentacion.NroPresentacion++;
            presentacion.CodTramite = tramite.CodTramite;
            presentacion.Fecha = tramite.Fecha;

            var timbrado = _timbradoLogic.GetTimbradoActivo(nroTramite);

            Operacion operacion = new Operacion(timbrado.NroFormulario, nroTramite, usuario, TipoOperacion.VISTAS);

            operacion.NroOperacion = IniciarTransaccion(presentacion, operacion, tramite.TipoBPM);

            presentacion.Operacion = operacion;

            return presentacion;
        }

        private long IniciarTransaccion(PresentacionTramite presentacion, Operacion operacion, bool esBPM)
        {
            long nroOperacion = 0;

            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                GuardarPresentacion(presentacion);

                nroOperacion = _operacionRepositorio.Guardar(operacion);

                if (esBPM)
                    GenerarRutaDestra(presentacion);

                scope.Complete();
            }

            if(esBPM)
                _bpmClient.RegistrarProceso(presentacion.NroTramite, presentacion.NroPresentacion, presentacion.NroCorrelativo, false);

            return nroOperacion;
        }

        public void GuardarPresentacion(int nroTramite, string operador, int nroPresentacion)
        {
            Usuario usuario = _usuarioRepositorio.GetUsuario(operador);
            GuardarPresentacion(new PresentacionTramite(nroTramite, usuario, nroPresentacion));
        }

        public void GuardarPresentacion(PresentacionTramite presentacion)
        {
            _presentacionRepositorio.Guardar(presentacion);
        }

        private void GenerarRutaDestra(PresentacionTramite presentacion)
        {
            TrazabilidadTramite trazabilidad = _trazabilidadRepositorio.ObtenerUltimoEstadoTramite(presentacion.NroTramite);
            trazabilidad.FechaHasta = DateTime.Now;

            //Se debe actualizar la ruta actual antes de generar una nueva trazabilidad del tramite
            _trazabilidadRepositorio.ActualizarUltimoEstado(trazabilidad);

            //Se inserta una nueva trazabilidad
            trazabilidad.FechaTramite = presentacion.Fecha;
            trazabilidad.Origen = trazabilidad.Destino;
            trazabilidad.Destino = "DMET";
            trazabilidad.FechaDesde = DateTime.Now;
            trazabilidad.Usuario.Id = 9999;
            _trazabilidadRepositorio.Guardar(trazabilidad);
        }
    }
}
