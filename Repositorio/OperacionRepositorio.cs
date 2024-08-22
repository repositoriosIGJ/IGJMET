using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using DAO;
using DAO.Mappers;
using Domain;
using Domain.Contracts.Repositories;

namespace Repositorio
{
    public class OperacionRepositorio: IOperacionRepositorio
    {
        public Operacion ObtenerPorNroOperacion(long nroOperacion)
        {
            using (var context = new MetModel())
            {
                var operacion = context.OPERACION.Where(o => o.OPERACIONID == nroOperacion).SingleOrDefault();

                if (operacion == null)
                    throw new Exception();

                return new OperacionMapper().Map(operacion);
            }
        }

        public IList<Operacion> ObtenerPorTramite(int nroTramite)
        {
            using (var context = new MetModel())
            {
                var operacion = context.OPERACION.Where(o => o.NROTRAMITE == nroTramite).ToList();

                if (operacion.Count == 0)
                    throw new Exception();

                return new OperacionMapper().Map(operacion);
            }
        }

        public long Guardar(Operacion operacion)
        {
            using (var context = new MetModel())
            {
                OPERACION entidad = new OperacionMapper().Reverse(operacion);

                entidad.OPERACIONID = GenerarNuevaOperacion(context);

                context.OPERACION.Add(entidad);

                context.SaveChanges();

                return (long)entidad.OPERACIONID;
            }
        }

        private decimal GenerarNuevaOperacion(MetModel context)
        {
            var sql = "SELECT SEQ_operacion.Nextval FROM DUAL";

            decimal nroOperacion = context.Database.SqlQuery<decimal>(sql).SingleOrDefault();

            return nroOperacion;
        }

        public void EliminarPorTramite(int nroTramite)
        {
            using (var context = new MetModel())
            {
                var sql = "DELETE FROM OPERACION WHERE NROTRAMITE = :1";

                OracleParameter[] parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramNROTRAMITE", Value = nroTramite };

                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }
    }
}
