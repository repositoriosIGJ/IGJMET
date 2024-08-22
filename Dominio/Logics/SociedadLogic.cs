using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Exceptions;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;

namespace Domain.Logics
{
    public class SociedadLogic : ISociedadLogic
    {
        private readonly ISociedadRepositorio _sociedadRepositorio;
        private readonly ITramiteLogic _tramiteLogic;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public SociedadLogic(ISociedadRepositorio sociedadRepositorio, ITramiteLogic tramiteLogic, IUsuarioRepositorio usuarioRepositorio)
        {
            _sociedadRepositorio = sociedadRepositorio;
            _tramiteLogic = tramiteLogic;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public Sociedad GetPorNumeroCorrelativo(int nroCorrelativo)
        {
            return _sociedadRepositorio.GetSociedad(nroCorrelativo);
        }

        public Sociedad GetPorTramiteYCorrelativo(int nroTramite, int nroCorrelativo)
        {
            Tramite tramite = _tramiteLogic.GetTramite(nroTramite);

            if(tramite.NroCorrelativo != nroCorrelativo)
                throw new Exception();

            if(!String.IsNullOrEmpty(tramite.Registro))
                throw new Exception();

            return _sociedadRepositorio.GetSociedad(nroCorrelativo);
        }

        public IList<Sociedad> BusquedaDeSociedad(string nombre, int codigoTipoSociedad)
        {
            return _sociedadRepositorio.BusquedaDeSociedad(nombre, codigoTipoSociedad);
        }

        public int GenerarCorrelativo(string usuario, string nombre, string codigoTipoSociedad)
        {
            Usuario user = _usuarioRepositorio.GetUsuario(usuario);
            //si genero correlativo tipo sociedad 160 = NO REGISTRADAS == id =19
            codigoTipoSociedad = _tramiteLogic.GetCodigoTipoSociedadById(19).ToString();

            return _sociedadRepositorio.GenerarCorrelativo(user.Id.ToString(), nombre, codigoTipoSociedad);
        }
    }
}