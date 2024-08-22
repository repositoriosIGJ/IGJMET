using System;
using System.Data;
using System.Data.Common;
using Oracle.DataAccess.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO;
using DAO.Mappers;
using Domain;
using Domain.Contracts.Common;
using Domain.Contracts.Repositories;
using Domain.Exceptions;

namespace Repositorio
{
    public class TimbradoRepositorio : ITimbradoRepositorio
    {
        private readonly ILogger _logger;

        public TimbradoRepositorio(ILogger logger)
        {
            _logger = logger;
        }

        public IList<Timbrado> GetTimbrados(Formulario formulario)
        {
            using (var context = new MetModel())
            {
                var sql = "SELECT * FROM TIMBRADOS WHERE TIMNROFOR = :1";

                var parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramTIMNROFOR", Value = formulario.NroFormulario };

                var resultado = context.Database.SqlQuery<TIMBRADOS>(sql, parameters).ToList();

                if (resultado.Count == 0)
                    throw new TimbradoNotExistException("Timbrado invalido");

                return new TimbradoMapper().Map(resultado);
            }
        }

        public IList<Timbrado> GetTimbrados(int nroTramite)
        {
            using (var context = new MetModel())
            {
                const string sql = "SELECT * FROM TIMBRADOS WHERE TIMNROTRAM = :1";

                var parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramTIMNROTRAM", Value = nroTramite };

                var resultado = context.Database.SqlQuery<TIMBRADOS>(sql, parameters).ToList();

                return resultado.Count == 0 ? new List<Timbrado>() : new TimbradoMapper().Map(resultado);
            }
        }

        public bool GetNroTramiteExistente(int nroTramite)
        {
            using (var context = new MetModel())
            {
                //Momentaneamente se debe verificar en la tabla timbrados la existencia de un Nro. de Tramite
                var sql = "SELECT COUNT(1) FROM TIMBRADOS WHERE TIMNROTRAM = :1";

                OracleParameter[] parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter() { ParameterName = "paramTIMNROTRAM", Value = nroTramite };

                int cantidad = context.Database.SqlQuery<int>(sql, parameters).SingleOrDefault();

                return (cantidad > 0);
            }
        }

        public int GetNroTramiteExistente(Formulario formulario)
        {
            using (var context = new MetModel())
            {
                var sql = "SELECT TIMNROTRAM FROM TIMBRADOS WHERE TIMNROFOR = :1";

                OracleParameter[] parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramTIMNROFOR", Value = formulario.NroFormulario };

                var nroTramite = context.Database.SqlQuery<int>(sql, parameters).SingleOrDefault();

                return nroTramite;
            }
        }

        public bool Existe(Timbrado timbrado)
        {
            using (var context = new MetModel())
            {
                var sql = "SELECT COUNT(1) FROM TIMBRADOS WHERE TIMNROFOR = :1";

                OracleParameter[] parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter() { ParameterName = "paramTIMNROFOR", Value = timbrado.Formulario.NroFormulario };

                int cantidad = context.Database.SqlQuery<int>(sql, parameters).SingleOrDefault();

                return (cantidad > 0);
            }
        }

        private Timbrado MapTimbrado(TIMBRADOS entidad, Formulario formulario)
        {
            Timbrado dominio = new TimbradoMapper().Map(entidad);
            dominio.Formulario = formulario;
            return dominio;
        }

        public IList<Timbrado> GetAll()
        {
            using (var context = new MetModel())
            {
                var entidadLista = context.Database.SqlQuery<TIMBRADOS>("SELECT * FROM TIMBRADOS").ToList();

                if (entidadLista.Count == 0)
                    throw new Exception();

                return new TimbradoMapper().Map(entidadLista);
            }
        }

        public int ObtenerPagoElectronico()
        {
            using (var context = new MetModel())
            {
                var nroPE = context.Database.SqlQuery<int>("SELECT seq_pago_electr_prueba.nextval FROM dual").SingleOrDefault();

                return nroPE;
            }
        }

        public void Actualizotimbrado(Timbrado timbrado)
        {
            using (var context = new MetModel())
            {
                var sql = "UPDATE TIMBRADOS SET TIMCODRET=:1 WHERE TIMNROFOR=:2 AND  TIMNROTRAM=:3";

                var parameters = new OracleParameter[3];
                parameters[0] = new OracleParameter { ParameterName = "paramTIMCODRET", Value = "FI" };
                parameters[2] = new OracleParameter { ParameterName = "paramTIMNROTRAM", Value = timbrado.NroTramite, OracleDbType = OracleDbType.Int32 };
                parameters[1] = new OracleParameter { ParameterName = "paramTIMNROFOR", Value = timbrado.NroFormulario, OracleDbType = OracleDbType.Int32 };

                context.Database.ExecuteSqlCommand(sql, parameters);
            }}

        public void Guardar(Timbrado timbrado)
        {
            try
            {
                using (var context = new MetModel())
                {

                    //Se realiza el insert a mano ya que no se tienen permisos de escritura en el esquema actual

                    var sql = "INSERT INTO TIMBRADOS (TIMNROTRAM, TIMNROFOR, TIMTIPOFOR, TIMNROCORR, TIMCONTROL, TIMFECTIM, TIMFECACTU, TIMCODRET, TIMCODWEB, TIMMONTO, TIMNCAJA, TIMNROPE, TIMOPERADOR, TIMCODTRAM, TIMFECHACT)";
                    sql += " VALUES (:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11, :12, :13, :14, :15)";

                    OracleParameter[] parameters = new OracleParameter[15];
                    parameters[0] = new OracleParameter { ParameterName = "paramTIMNROTRAM", Value = timbrado.NroTramite };
                    parameters[1] = new OracleParameter { ParameterName = "paramTIMNROFOR", Value = timbrado.Formulario.NroFormulario };
                    parameters[2] = new OracleParameter { ParameterName = "paramTIMTIPOFOR", Value = timbrado.Formulario.TipoFormulario };
                    parameters[3] = new OracleParameter { ParameterName = "paramTIMNROCORR", Value = timbrado.NroCorrelativo };
                    parameters[4] = new OracleParameter { ParameterName = "paramTIMCONTROL", Value = timbrado.Formulario.CodigoBanelco };
                    parameters[5] = new OracleParameter { ParameterName = "paramTIMFECTIM", Value = timbrado.FechaCreacion, OracleDbType = OracleDbType.Date };
                    parameters[6] = new OracleParameter { ParameterName = "paramTIMFECACTU", Value = DateTime.Now, OracleDbType = OracleDbType.Date };

                    String codigo = null;
                    if (timbrado.Estado != Estado.ER_CON)
                        codigo = timbrado.Estado.ToString();

                    var codigoTramite = (timbrado.CodTramite != null) ? timbrado.CodTramite.ToString().PadLeft(5, '0') : null;

                    parameters[7] = new OracleParameter { ParameterName = "paramTIMCODRET", Value = codigo };
                    parameters[8] = new OracleParameter { ParameterName = "paramTIMCODWEB", Value = "1" };
                    parameters[9] = new OracleParameter { ParameterName = "paramTIMMONTO", Value = timbrado.Monto };
                    parameters[10] = new OracleParameter { ParameterName = "paramTIMNCAJA", Value = timbrado.Caja };
                    parameters[11] = new OracleParameter { ParameterName = "paramTIMNROPE", Value = timbrado.NroPagoElectronico };
                    parameters[12] = new OracleParameter { ParameterName = "paramTIMOPERADOR", Value = timbrado.IdUsuario };
                    parameters[13] = new OracleParameter { ParameterName = "paramTIMCODTRAM", Value = codigoTramite };
                    parameters[14] = new OracleParameter { ParameterName = "paramTIMFECHACT", Value = timbrado.FechaOperacion, OracleDbType = OracleDbType.Date };

                    var conn = (OracleConnection)context.Database.Connection;

                    ExecuteOracleCommand(conn, sql, parameters);

                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error al  guardear el timbrado: " + ex.ToString());
                throw new Exception("Ocurrio un error al registrar el tramite");
            }
        }

        public void Eliminar(Timbrado timbrado)
        {
            using (var context = new MetModel())
            {
                var sql = "DELETE FROM TIMBRADOS WHERE TIMNROFOR = :1";

                OracleParameter[] parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramTIMNROFOR", Value = timbrado.Formulario.NroFormulario };

                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }

        private void ExecuteOracleCommand(OracleConnection conn, string sql, OracleParameter[] parameters)
        {
            try
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}
