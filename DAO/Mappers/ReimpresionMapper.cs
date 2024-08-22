using System;
using Domain;
using Domain.Contracts.Mappers;

namespace DAO.Mappers
{
    public class ReimpresionMapper: IMapper<Reimpresion, REIMPRESIONES>
    {
        public Reimpresion Map(REIMPRESIONES entidad)
        {
            Reimpresion dominio = new Reimpresion();
            dominio.Id = entidad.ID_REIMPRESION;
            dominio.Usuario = entidad.USUARIO;
            dominio.Fecha = DateTime.ParseExact(entidad.FECHA, "dd/MM/yy", null);

            if(entidad.TRAMITE.HasValue)
                dominio.NroTramite = entidad.TRAMITE.Value;

            if(entidad.ID_USUARIO.HasValue)
                dominio.IdUsuario = entidad.ID_USUARIO.Value;

            return dominio;
        }

        public REIMPRESIONES Reverse(Reimpresion dominio)
        {
            REIMPRESIONES entidad = new REIMPRESIONES();
            entidad.ID_REIMPRESION = dominio.Id;
            entidad.TRAMITE = dominio.NroTramite;
            entidad.FECHA = dominio.Fecha.ToString("dd/MM/yy");
            entidad.ID_USUARIO = dominio.IdUsuario;
            entidad.USUARIO = dominio.Usuario;
            return entidad;
        }

        public void Reverse(REIMPRESIONES entidad, Reimpresion dominio)
        {
            throw new NotImplementedException();
        }
    }
}
