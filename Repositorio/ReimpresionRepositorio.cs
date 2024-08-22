using System;
using System.Linq;
using Oracle.DataAccess.Client;
using DAO;
using DAO.Mappers;
using Domain;
using Domain.Contracts.Common;
using Domain.Contracts.Repositories;

namespace Repositorio
{
    public class ReimpresionRepositorio: IReimpresionRespositorio
    {
        private readonly ILogger _logger;

        public ReimpresionRepositorio(ILogger logger)
        {
            _logger = logger;
        }

        public Reimpresion ObtenerPorId(int idReimpresion)
        {
            using (var context = new MetModel())
            {
                var entidad = context.REIMPRESIONES.SingleOrDefault(r => r.ID_REIMPRESION == idReimpresion);

                if (entidad == null)
                    throw new Exception();

                return new ReimpresionMapper().Map(entidad);
            }
        }

        public void Guardar(Reimpresion reimpresion)
        {
            try
            {
                using (var context = new MetModel())
                {
                    reimpresion.Id = ObtenerNroReimpresion(context);

                    var sql = "INSERT INTO REIMPRESIONES (ID_REIMPRESION, ID_USUARIO, USUARIO, FECHA, TRAMITE)";
                    sql += " VALUES (:1, :2, :3, :4, :5)";

                    var parameters = new OracleParameter[5];
                    parameters[0] = new OracleParameter { ParameterName = "paramID_REIMPRESION", Value = reimpresion.Id };
                    parameters[1] = new OracleParameter { ParameterName = "paramID_USUARIO", Value = reimpresion.IdUsuario };
                    parameters[2] = new OracleParameter { ParameterName = "paramUSUARIO", Value = reimpresion.Usuario };
                    parameters[3] = new OracleParameter { ParameterName = "paramFECHA", Value = reimpresion.Fecha.ToString("dd/MM/yy") };
                    parameters[4] = new OracleParameter { ParameterName = "paramTRAMITE", Value = reimpresion.NroTramite };

                    context.Database.ExecuteSqlCommand(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private int ObtenerNroReimpresion(MetModel context)
        {
            const string sql = "SELECT seq_reimpresion.nextval FROM dual";
            return context.Database.SqlQuery<int>(sql).SingleOrDefault();
        }

        public void Eliminar(Reimpresion reimpresion)
        {
            using (var context = new MetModel())
            {
                var entidad = context.REIMPRESIONES.SingleOrDefault(r => r.ID_REIMPRESION == reimpresion.Id);

                if (entidad == null)
                    throw new Exception(); ;

                context.REIMPRESIONES.Remove(entidad);
                context.SaveChanges();
            }
        }
    }
}
