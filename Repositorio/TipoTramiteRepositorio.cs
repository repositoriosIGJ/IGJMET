using Oracle.DataAccess.Client;
using System;
using System.Linq;
using DAO;
using DAO.Mappers;
using Domain.Exceptions;
using Domain.Contracts.Repositories;
using Domain;
using System.Collections.Generic;

namespace Repositorio
{
    public class TipoTramiteRepositorio: ITipoTramiteRepositorio
    {
        public TipoTramite GetTipoTramite(int codTramite)
        {
            using (var context = new MetModel())
            {
                var sql = "SELECT * FROM TABGEN WHERE TABTIPOTAB = :1 AND  TABCLAVE = :2";

                OracleParameter[] parameters = new OracleParameter[2];
                parameters[0] = new OracleParameter() { ParameterName = "paramTABTIPOTAB", Value = "001" };
                parameters[1] = new OracleParameter() { ParameterName = "paramTABCLAVE", Value = codTramite.ToString() };

                var entidad = context.Database.SqlQuery<TABGEN>(sql, parameters).SingleOrDefault();

                if (entidad == null)
                    throw new TipoTramiteException("Nro. de trámite inexistente en el sistema");

                return new TipoTramiteMapper().Map(entidad);
            }
        }

        public IList<TipoTramiteGratuito> GetTipoTramiteGratuito()
        {
            var list = new List<CODIGO_TRAMITES_GRATUITOS>();
            var tipoTramiteGratuitoList = new List<TipoTramiteGratuito>();

            using (var context = new MetModel())
            {
                const string sql = "SELECT * FROM CODIGO_TRAMITES_GRATUITOS";
                list = context.Database.SqlQuery<CODIGO_TRAMITES_GRATUITOS>(sql).ToList();
            }

            foreach (var tipoTramite in list)
            {
                tipoTramiteGratuitoList.Add(new TipoTramiteGratuitoMapper().Map(tipoTramite));
            }

            return tipoTramiteGratuitoList;
        }
    }
}
