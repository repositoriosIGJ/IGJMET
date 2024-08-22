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
    public class PresentacionTramiteRepositorio: IPresentacionTramiteRepositorio
    {
        public void Guardar(PresentacionTramite presentacion)
        {
            using (var context = new MetModel())
            {
                //Se realiza el insert a mano ya que no se tienen permisos de escritura en el esquema actual
                var sql = "INSERT INTO PRESENTACION_TRAMITE (PRTNROTRAM, PRTNROPRES, PRTFECHAPRES, PRTUSUACTU, PRTNROCORR, PRTCODTRAM, PRTFECHACT)";
                sql += " VALUES (:1, :2, :3, :4, :5, :6, :7)";

                OracleParameter[] parameters = new OracleParameter[7];
                parameters[0] = new OracleParameter { ParameterName = "paramPRTNROTRAM", Value = presentacion.NroTramite };
                parameters[1] = new OracleParameter { ParameterName = "paramPRTNROPRES", Value = presentacion.NroPresentacion};
                parameters[2] = new OracleParameter { ParameterName = "paramPRTFECHAPRES", Value = DateTime.Now, OracleDbType = OracleDbType.Date};
                parameters[3] = new OracleParameter { ParameterName = "paramPRTUSUACTU", Value = presentacion.Usuario.Id };
                parameters[4] = new OracleParameter { ParameterName = "paramPRTNROCORR", Value = presentacion.NroCorrelativo };
                parameters[5] = new OracleParameter { ParameterName = "paramPRTCODTRAM", Value = presentacion.CodTramite };
                parameters[6] = new OracleParameter { ParameterName = "paramPRTFECHACT", Value = presentacion.Fecha };

                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }

        public IList<PresentacionTramite> ObtenerTodos(int nroTramite)
        {
            using (var context = new MetModel())
            {
                
                var sql = "SELECT * FROM PRESENTACION_TRAMITE WHERE PRTNROTRAM = :1";

                OracleParameter[] parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramPRTNROTRAM", Value = nroTramite };

                var listado = context.Database.SqlQuery<PRESENTACION_TRAMITE>(sql, parameters).ToList();

                return new PresentacionTramiteMapper().Map(listado);
            }
        }

        public PresentacionTramite ObtenerUltimo(int nroTramite)
        {
            using (var context = new MetModel())
            {
                var sql = "SELECT * FROM (SELECT * FROM PRESENTACION_TRAMITE WHERE PRTNROTRAM = :1 ORDER BY PRTNROPRES DESC ) WHERE ROWNUM = 1";

                OracleParameter[] parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramPRTNROTRAM", Value = nroTramite };

                var ultimo = context.Database.SqlQuery<PRESENTACION_TRAMITE>(sql, parameters).SingleOrDefault();

                if (ultimo == null)
                    throw new PresentacionNotExistException("El tramite no posee presentaciones previas");

                return new PresentacionTramiteMapper().Map(ultimo);
            }
        }

        public void Eliminar(PresentacionTramite presentacion)
        {
            using (var context = new MetModel())
            {
                
                var sql = "DELETE FROM PRESENTACION_TRAMITE WHERE PRTNROTRAM = :1";

                OracleParameter[] parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramPRTNROTRAM", Value = presentacion.NroTramite };

                 context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }
    }
}
