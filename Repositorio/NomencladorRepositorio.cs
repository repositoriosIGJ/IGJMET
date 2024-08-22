using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using DAO;
using DAO.Mappers;
using Domain;
using Domain.Exceptions;
using Domain.Contracts.Repositories;

namespace Repositorio
{
    public class NomencladorRepositorio: INomencladorRepositorio
    {
        public IList<Nomenclador> GetNomenclados()
        {
            using (var context = new MetModel())
            {
                //var sql = "SELECT * FROM NOMENCLADORTRAMITEBPM";

                //var resultado = context.Database.SqlQuery<NOMENCLADORTRAMITEBPM>(sql).ToList();

                var resultado = context.Set<NOMENCLADORTRAMITEBPM>().ToList();

                if (resultado.Count == 0)
                    throw new NomencladorNotExistException();

                return new NomencladorMapper().Map(resultado);
            }
        }

        public Nomenclador Get(long id)
        {
            using (var context = new MetModel())
            {
                var sql = "SELECT * FROM NOMENCLADORTRAMITEBPM WHERE IDNOMENCLADOR = :1";

                OracleParameter[] parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramIDNOMENCLADOR", Value = id };

                var resultado = context.Database.SqlQuery<NOMENCLADORTRAMITEBPM>(sql, parameters).SingleOrDefault();

                if (resultado == null)
                    throw new NomencladorNotExistException();

                return new NomencladorMapper().Map(resultado);
            }
        }
    }
}
