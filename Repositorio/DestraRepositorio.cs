using DAO;
using DAO.Mappers;
using Domain;
using Domain.Contracts.Repositories;
using Domain.Exceptions;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositorio
{
    public class DestraRepositorio : IDestraRepositorio
    {
        private readonly ITipoTramiteRepository _tipoTramiteRepository;
        private readonly ITramiteRepositorio _tramiteRepositorio;

        public DestraRepositorio(ITipoTramiteRepository tipoTramiteRepository, ITramiteRepositorio tramiteRepositorio)
        {
            _tipoTramiteRepository = tipoTramiteRepository;
            _tramiteRepositorio = tramiteRepositorio;
        }

        public IList<TrazabilidadTramite> ObtenerPorTramite(int nroTramite)
        {
            using (var context = new MetModel())
            {
                var sql = "SELECT * FROM DESTRA WHERE DTRNROTRAM = :1";

                if (IsBpm(nroTramite))
                    sql += " AND DTRCODTRAM IN (" + GetClasificacionesJoin() + ")";

                var parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter() { ParameterName = "paramDTRNROTRAM", Value = nroTramite };

                var resultado = context.Database.SqlQuery<DESTRA>(sql, parameters).ToList();

                if (resultado.Count == 0)
                    throw new TrazabilidadNotExistException("No se pudo obtener el estado del tramite");

                return new TrazabilidadTramiteMapper().Map(resultado);
            }
        }

        public TrazabilidadTramite ObtenerUltimoEstadoTramite(int nroTramite)
        {
            using (var context = new MetModel())
            {
                var sql = "SELECT * FROM DESTRA WHERE DTRNROTRAM = :1 AND DTRFECHAST IS NULL";

                if (IsBpm(nroTramite))
                    sql += " AND DTRCODTRAM IN (" + GetClasificacionesJoin() + ")";

                var parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter() { ParameterName = "paramDTRNROTRAM", Value = nroTramite };

                var resultado = context.Database.SqlQuery<DESTRA>(sql, parameters).ToList();

                if (!resultado.Any())
                    throw new TrazabilidadNotExistException("No se pudo obtener el estado del tramite");

                resultado.First().DRTNROTRAM = nroTramite;
                return new TrazabilidadTramiteMapper().Map(resultado.First());
            }
        }

        public void Guardar(TrazabilidadTramite trazabilidad)
        {
            using (var context = new MetModel())
            {
                var sql = "INSERT INTO DESTRA (DTRNROCORR, DTRNROTRAM, DTRCODDEST, DTRFECHART, DTRFECHACT, DTRCODTRAM, DTRUSUDEST, DTRDESTANT)";
                sql += " VALUES (:1, :2, :3, :4, :5, :6, :7, :8)";

                var parameters = new OracleParameter[8];
                parameters[0] = new OracleParameter { ParameterName = "paramDTRNROCORR", Value = trazabilidad.NroCorrelativo };
                parameters[1] = new OracleParameter { ParameterName = "paramDTRNROTRAM", Value = trazabilidad.NroTramite };
                parameters[2] = new OracleParameter { ParameterName = "paramDTRCODDEST", Value = trazabilidad.Destino };
                parameters[3] = new OracleParameter { ParameterName = "paramDTRFECHART", Value = trazabilidad.FechaDesde, OracleDbType = OracleDbType.Date };
                parameters[4] = new OracleParameter { ParameterName = "paramDTRFECHACT", Value = trazabilidad.FechaTramite.Date, OracleDbType = OracleDbType.Date };
                parameters[5] = new OracleParameter { ParameterName = "paramDTRCODTRAM", Value = trazabilidad.CodTramite };
                parameters[6] = new OracleParameter { ParameterName = "paramDTRUSUDEST", Value = trazabilidad.Usuario.Id };
                parameters[7] = new OracleParameter { ParameterName = "paramDTRDESTANT", Value = trazabilidad.Origen };

                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }

        public void ActualizarUltimoEstado(TrazabilidadTramite trazabilidad)
        {
            using (var context = new MetModel())
            {
                var sql = "UPDATE DESTRA SET DTRFECHAST=:1 WHERE DTRNROTRAM=:2 AND DTRFECHAST IS NULL";

                if (IsBpm(trazabilidad.NroTramite))
                    sql += " AND DTRCODTRAM IN (" + GetClasificacionesJoin() + ")";

                var parameters = new OracleParameter[2];
                parameters[0] = new OracleParameter { ParameterName = "paramDTRFECHAST", Value = trazabilidad.FechaHasta, OracleDbType = OracleDbType.Date };
                parameters[1] = new OracleParameter { ParameterName = "paramDTRNROTRAM", Value = trazabilidad.NroTramite };

                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }

        public void Eliminar(TrazabilidadTramite trazabilidad)
        {
            using (var context = new MetModel())
            {
                var sql = "DELETE FROM DESTRA WHERE DTRNROTRAM = :1";

                if (IsBpm(trazabilidad.NroTramite))
                    sql += " AND DTRCODTRAM IN (" + GetClasificacionesJoin() + ")";

                var parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramDTRNROTRAM", Value = trazabilidad.NroTramite };

                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }

        private String GetClasificacionesJoin()
        {
            return String.Join(",", _tipoTramiteRepository.GetClasificaciones().ToArray());
        }

        private bool IsBpm(Int32 numeroTramite)
        {
            try
            {
                var tramite = _tramiteRepositorio.GetTramite(numeroTramite);
                return tramite != null && tramite.TipoBPM;
            }
            catch (TramiteNotExistException ex)
            {

                return false;
            }
           
            
        }

        private DESTRA GetDestraValido(IList<DESTRA> resultados)
        {
            DESTRA destraValido = null;

            foreach (var result in resultados)
            {
                if (_tipoTramiteRepository.GetClasificaciones().Contains(Int32.Parse(result.DTRCODTRAM)))
                    destraValido = result;
            }

            return destraValido ?? (destraValido = resultados.FirstOrDefault());
        }
    }
}