using System;
using System.Transactions;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;

namespace Domain.Logics
{
    public class TramiteNuevoGratuitoLogic: TramiteBaseLogic
    {
        private int _nroTramite;
        private ITramiteRepositorio _tramiteRepositorio;
        private IPresentacionTramiteLogic _presentacionLogic;

        public TramiteNuevoGratuitoLogic(Usuario usuario, IOperacionLogic operacionLogic, IPresentacionTramiteLogic presentacionLogic,
            ITramiteRepositorio tramiteRepositorio)
            :base(usuario, operacionLogic)
        {
            _tramiteRepositorio = tramiteRepositorio;
            _presentacionLogic = presentacionLogic;
        }

        public override int Procesar()
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                _nroTramite = _tramiteRepositorio.GenerarNroTramite();

                GenerarTramiteGratuito();

                int nroPresentacion = GenerarNuevaPresentacion();

                GenerarTramiteDigital(nroPresentacion);

                GenerarOperacion();

                scope.Complete();
            }

            return _nroTramite;
        }

        private void GenerarTramiteGratuito()
        {
            TramiteGratuito tramiteGratuito = new TramiteGratuito();
            tramiteGratuito.Fecha = DateTime.Now;
            tramiteGratuito.NroCorrelativo = 0;
            tramiteGratuito.NroTramite = _nroTramite;
            tramiteGratuito.Usuario = _usuario;
            _tramiteRepositorio.GuardarTramiteGratuito(tramiteGratuito);
        }

        private int GenerarNuevaPresentacion()
        {
            PresentacionTramite presentacion = _presentacionLogic.GetUltimaPresentacion(_nroTramite);
            presentacion.Usuario = _usuario;
            presentacion.NroPresentacion++;
            presentacion.Fecha = DateTime.Now;
            _presentacionLogic.GuardarPresentacion(presentacion);
            return presentacion.NroPresentacion;
        }

        private void GenerarTramiteDigital(int nroPresentacion)
        {
            TramiteDigital tramiteDigital = new TramiteDigital();
            tramiteDigital.NroTramite = _nroTramite;
            tramiteDigital.NroPresentacion = nroPresentacion;
            tramiteDigital.NroCorrelativo = 0;
            tramiteDigital.Guid = _usuario.Id.ToString();
            tramiteDigital.Fecha = DateTime.Now;
            _tramiteRepositorio.GuardarDigital(tramiteDigital);
        }

        private void GenerarOperacion()
        {
            Operacion operacion = new Operacion();
            operacion.Fecha = DateTime.Now;
            operacion.NroTramite = _nroTramite;
            operacion.TipoOperacion = TipoOperacion.SIN_TIMBRADO;
            operacion.UsuarioID = _usuario.Id;
            _operacionLogic.GuardarOperacion(operacion);
        }
    }
}
