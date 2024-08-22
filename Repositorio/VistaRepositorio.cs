using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using DAO;
using DAO.Mappers;
using Domain;
using Domain.Contracts.Repositories;
using Domain.Exceptions;

namespace Repositorio
{
    public class VistaRepositorio: IVistaRepositorio
    {
        public VistaRepositorio()
        {

        }

        public IList<Vista> GetVistas(int nroTramite, int nroCorrelativo)
        {
            using (var context = new MetModel())
            {
                var sql = "SELECT * FROM VISTAREC WHERE VISNROTRAM = :1 AND VISNROCORR = :2";

                OracleParameter[] parameters = new OracleParameter[2];
                parameters[0] = new OracleParameter() { ParameterName = "paramVISNROTRAM", Value = nroTramite };
                parameters[1] = new OracleParameter() { ParameterName = "paramVISNROCORR", Value = nroCorrelativo };

                var listaEntidad = context.Database.SqlQuery<VISTAREC>(sql, parameters).ToList();

                if (listaEntidad.Count == 0)
                    throw new VistaNotExistException("Tramite no apto para Contestacion de Vista");

                return new VistaMapper().Map(listaEntidad);
            }
        }
    }
}
