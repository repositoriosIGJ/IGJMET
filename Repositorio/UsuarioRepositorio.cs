using Domain.Contracts.Common;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using DAO;
using DAO.Mappers;
using Domain;
using Domain.Common;
using Domain.Contracts.Repositories;
using Domain.Exceptions;

namespace Repositorio
{
    public class UsuarioRepositorio: IUsuarioRepositorio
    {
        private readonly ILogger _logger;

        public UsuarioRepositorio(ILogger logger)
        {
            this._logger = logger;
        }

        public Usuario GetUsuario(string nombreUsuario)
        {
            var usuario = UserCache.Get(nombreUsuario);
            if (usuario != null)
                return usuario;
            try
            {
                usuario = GetUsuarioFromDB(nombreUsuario);
                UserCache.Add(usuario);
            }
            catch (UserNotExistException uex)
            {
                var msg = String.Format("No se encontro el usuario {0}", nombreUsuario);
                this._logger.Error(msg);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex);
            }

            return usuario;
        }

        private Usuario GetUsuarioFromDB(string nombreUsuario)
        {
            using (var context = new MetModel())
            {
                const string sql = "SELECT USUUSUARIO FROM USUARIO WHERE UPPER(USUABPM) = :1";

                var parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramUSUABPM", Value = nombreUsuario.ToUpper() };

                var id = context.Database.SqlQuery<string>(sql, parameters).FirstOrDefault();

                if (id == null)
                    throw new UserNotExistException();

                var idUsuario = Convert.ToInt32(id);

                return new Usuario { Alias = nombreUsuario, Id = idUsuario };
            }
        }

        public IList<Usuario> GetAvailables()
        {
            using (var context = new MetModel())
            {
                const string sql = "SELECT USUUSUARIO, USUNOMBRE, USUABPM FROM USUARIO WHERE USUDESTINO = :1 AND USUFECBAJ IS NULL";

                var parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramUSUDESTINO", Value = "DME1"};

                var usuarios = context.Database.SqlQuery<USUARIO>(sql, parameters).ToList();
                return new UsuarioMapper().Map(usuarios);
            }
        }
    }
}
