using System;
using System.Collections.Generic;
using System.Linq;
using Oracle.DataAccess.Client;
using DAO;
using DAO.Mappers;
using Domain;
using Domain.Contracts.Repositories;
using Domain.Exceptions;

namespace Repositorio
{
    public class TramiteDigitalRepositorio: ITramiteDigitalRepositorio
    {
        public TramiteDigital ObtenerPorNroTramite(int nroTramite)
        {
            throw new NotImplementedException();
        }

        public IList<TramiteDigital> ObtenerTodosPorNroTramite(int nroTramite)
        {
            using (var context = new MetModel())
            {
                var sql = "SELECT * FROM TRAMITE_DIGITAL WHERE TRDNROTRAM = :1";

                OracleParameter[] parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramTRDNROTRAM", Value = nroTramite };

                var listado = context.Database.SqlQuery<TRAMITE_DIGITAL>(sql, parameters).ToList();

                return new TramiteDigitalMapper().Map(listado);
            }
        }

        public void Guardar(TramiteDigital tramiteDigital)
        {
            using (var context = new MetModel())
            {
                //Se realiza el insert a mano ya que no se tienen permisos de escritura en el esquema actual
                var sql = "INSERT INTO TRAMITE_DIGITAL (TRDNROCORR, TRDNROTRAM, TRDNROPRES, TRDFECACTU, TRDGUID)";
                sql += " VALUES (:1, :2, :3, :4, :5)";

                OracleParameter[] parameters = new OracleParameter[5];
                parameters[0] = new OracleParameter { ParameterName = "paramTRDNROCORR", Value = tramiteDigital.NroCorrelativo };
                parameters[1] = new OracleParameter { ParameterName = "paramTRDNROTRAM", Value = tramiteDigital.NroTramite };
                parameters[2] = new OracleParameter { ParameterName = "paramTRDNROPRES", Value = tramiteDigital.NroPresentacion };
                parameters[3] = new OracleParameter { ParameterName = "paramTRDFECACTU", Value = tramiteDigital.Fecha, OracleDbType = OracleDbType.Date };
                parameters[4] = new OracleParameter { ParameterName = "paramTRDGUID", Value = tramiteDigital.Guid };

                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }

        public void Eliminar(TramiteDigital tramiteDigital)
        {
            using (var context = new MetModel())
            {
                var sql = "DELETE FROM TRAMITE_DIGITAL WHERE TRDNROTRAM = :1";

                OracleParameter[] parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramTRDNROTRAM", Value = tramiteDigital.NroTramite };

                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }
    }
}
