using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class UsuarioMapper : IMapper<Usuario, USUARIO>
    {
        public UsuarioMapper()
        {

        }

        public IList<Usuario> Map(List<USUARIO> listaEntidad)
        {
            IList<Usuario> listaDominio = new List<Usuario>();
            listaEntidad.ForEach(u => listaDominio.Add(Map(u)));
            return listaDominio;
        }

        public Usuario Map(USUARIO entidad)
        {
            Usuario dominio = new Usuario();
            dominio.Id = int.Parse(entidad.USUUSUARIO);
            dominio.Nombre = entidad.USUNOMBRE;
            dominio.Alias = entidad.USUABPM;

            if (entidad.USUFECALT.HasValue)
                dominio.FechaAlta = entidad.USUFECALT.Value;

            if (entidad.USUFECBAJ.HasValue)
                dominio.FechaBaja = entidad.USUFECBAJ.Value;

            return dominio;
        }

        public USUARIO Reverse(Usuario Domain)
        {
            throw new NotImplementedException();
        }

        public void Reverse(USUARIO entidad, Usuario Domain)
        {
            throw new NotImplementedException();
        }
    }
}
