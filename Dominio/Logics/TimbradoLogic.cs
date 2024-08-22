using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;
using Domain.Exceptions;

namespace Domain.Logics
{
    public class TimbradoLogic : ITimbradoLogic
    {
        private readonly ITimbradoRepositorio _timbradoRepositorio;
        private readonly IFormularioRepositorio _formularioRepositorio;

        #region public methods

        public TimbradoLogic(ITimbradoRepositorio timbradoRepositorio, IFormularioRepositorio formularioRepositorio)
        {
            _timbradoRepositorio = timbradoRepositorio;
            _formularioRepositorio = formularioRepositorio;
        }

        public void AgregarTimbrado(string codFormulario)
        {
        }

        public void Guardar(Timbrado timbrado)
        {
            _timbradoRepositorio.Guardar(timbrado);
        }

        public Timbrado GetUltimoProcesado(Formulario formulario)
        {
            var timbrados = _timbradoRepositorio.GetTimbrados(formulario);
            var ultimo = timbrados.OrderByDescending(t => t.FechaCreacion).First();
            ultimo.Formulario = formulario;
            return ultimo;
        }

        public Timbrado GetUltimoProcesado(Formulario formulario, int nroTramite)
        {
            var timbrados = _timbradoRepositorio.GetTimbrados(nroTramite);

            if (timbrados.Count == 0)
                throw new TramiteNotExistException("Nro. de tramite inexistente en el sistema");

            if (UtilizadoEnARGA(formulario))
                throw new TimbradoAlreadyInProcessException("El timbrado ya se encuentra utilizado");

            timbrados = timbrados.Where(t => t.NroFormulario == formulario.NroFormulario).ToList();

            if (timbrados.Count == 0)
                throw new TimbradoNotExistException("Timbrado invalido");

            var ultimo = timbrados.OrderByDescending(t => t.FechaCreacion).First();
            ultimo.Formulario = formulario;
            return ultimo;
        }

        public Timbrado GetUltimoValido(Int32 nroTramite)
        {
            var timbrados = _timbradoRepositorio.GetTimbrados(nroTramite);
            return timbrados.Where(x => x.Estado == Estado.FI || x.Estado == Estado.OK).OrderByDescending(o => o.FechaCreacion).FirstOrDefault();
        }

        public IList<Timbrado> GetTimbrados(int nroTramite)
        {
            var timbrados = _timbradoRepositorio.GetTimbrados(nroTramite);

            if (timbrados.Count == 0)
                throw new TramiteNotExistException("Nro. de tramite inexistente en el sistema");

            foreach (var timbrado in timbrados)
            {
                timbrado.Formulario = _formularioRepositorio.GetFormulario(timbrado.NroFormulario);
            }

            return timbrados;
        }

        public bool UtilizadoEnARGA(Formulario formulario)
        {
            try
            {
                var timbrados = _timbradoRepositorio.GetTimbrados(formulario);
                return timbrados.Count(t => t.EnUso()) > 0;
            }
            catch (TimbradoNotExistException)
            {
                return false;
            }
        }

        public Timbrado GetTimbradoActivo(int nroTramite)
        {
            var timbrados = _timbradoRepositorio.GetTimbrados(nroTramite);

            if (timbrados.Count == 0)
                throw new TramiteNotExistException("Nro. de tramite inexistente en el sistema");

            return TimbradoDeAlta(timbrados);
        }

        public Timbrado GetTimbradoActivo(Formulario formulario)
        {
            var timbrados = _timbradoRepositorio.GetTimbrados(formulario);
            return TimbradoDeAlta(timbrados);
        }

        public int ObtenerPagoElectronico()
        {
            return _timbradoRepositorio.ObtenerPagoElectronico();
        }

        public void Actualizotimbrado(Timbrado timbrado)
        {
            _timbradoRepositorio.Actualizotimbrado(timbrado);

        }

        #endregion

        private static Timbrado TimbradoDeAlta(IList<Timbrado> timbrados)
        {
            return timbrados.FirstOrDefault(t => t.EnUso() && t.FechaCreacion == timbrados.Where(tim => tim.EnUso()).Min(ti => ti.FechaCreacion));
        }


    }
}