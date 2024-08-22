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
    public class SociedadRepositorio : ISociedadRepositorio
    {
        public Sociedad GetSociedad(int nroCorrelativo)
        {
            using(var context = new MetModel())
            {
                var sql = "SELECT * FROM EXPTES WHERE EXPNROCORR = :1";

                OracleParameter[] parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter {ParameterName = "paramEXPNROCORR", Value = nroCorrelativo};

                var entidad = context.Database.SqlQuery<EXPTES>(sql, parameters).SingleOrDefault();

                if(entidad == null)
                    throw new SociedadNotExist("Número correlativo inexistente");

                Sociedad sociedad = new SociedadMapper().Map(entidad);
                sociedad.TipoSociedad = ObtenerTipoSociedad(context, entidad.EXPTIPOSOC);

                return sociedad;
            }
        }

        private TipoSociedad ObtenerTipoSociedad(MetModel context, short idTipoSociedad)
        {
            var entidad = context.TIPOENTIDAD.Where(te => te.IDTIPOENTIDAD == idTipoSociedad).SingleOrDefault();

            if(entidad == null)
                throw new Exception();

            TipoSociedad tipoSociedad = new TipoSociedadMapper().Map(entidad);
            tipoSociedad.Alias = ObtenerAlias(context, idTipoSociedad);

            return tipoSociedad;
        }

        private string ObtenerAlias(MetModel context, short idTipoSociedad)
        {
            var sql = "SELECT TABCONTEN2 FROM TABGEN WHERE TABCLAVE = :1 AND TABTIPOTAB = :2";

            OracleParameter[] parameters = new OracleParameter[2];
            parameters[0] = new OracleParameter {ParameterName = "paramTABCLAVE", Value = idTipoSociedad.ToString("000")};
            parameters[1] = new OracleParameter {ParameterName = "paramTABTIPOTAB", Value = "002"};

            string alias = context.Database.SqlQuery<string>(sql, parameters).SingleOrDefault();

            return alias;
        }

        public IList<Sociedad> BusquedaDeSociedad(string nombre, int codigoTipoSociedad)
        {
            var list = new List<EXPTES>();
            var result = new List<Sociedad>();

            using(var context = new MetModel())
            {
                string sql = "SELECT * FROM EXPTES WHERE EXPTIPOSOC = :1 AND UPPER(EXPRAZONSO) LIKE'%" + nombre.ToUpper() + "%'";

                var parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter() {ParameterName = "paramEXPTIPOSOC", Value = codigoTipoSociedad};

                list = context.Database.SqlQuery<EXPTES>(sql, parameters).ToList();
            }

            foreach(var sociedad in list)
            {
                result.Add(new SociedadMapper().Map(sociedad));
            }

            return result;
        }

        public int GenerarCorrelativo(string usuario, string nombre, string codigoTipoSociedad)
        {
            var idexptes = GenerarIdExptes();
            var idTramSinReg = GenerarIdTramSinReg();

            using(var context = new MetModel())
            {
                //TODO borrar comentarios
                //el correlativo no deberia estar en uso en xpetes, se elimina un registro si es q lo hay
                //LimpiarCorrelativoExptes(nroCorrelativo);  

                //grabo en expete
                var sql = "insert into EXPTES (EXPNROCORR, EXPFECHACC, EXPTIPOSOC, EXPRAZONSO) values (:1, :2, :3, :4)";
                //TODO secuencia de exptes
                //"SELECT nrocorr_seq.nextval  from dual"  

                var parameters = new OracleParameter[4];
                parameters[0] = new OracleParameter() {ParameterName = "paramEXPNROCORR", Value = idexptes};
                parameters[1] = new OracleParameter() {ParameterName = "paramEXPFECHACC", Value = DateTime.Now, OracleDbType = OracleDbType.Date};
                parameters[2] = new OracleParameter() {ParameterName = "paramEXPTIPOSOC", Value = codigoTipoSociedad};
                parameters[3] = new OracleParameter() {ParameterName = "paramEXPRAZONSO", Value = nombre};
                context.Database.ExecuteSqlCommand(sql, parameters);
            }

            using(var context = new MetModel())
            {
                //grabo en correlativos sin registrar, aca se persite la relacion con el tipo de tramite
                var sql = "INSERT INTO COD_TRAM_CORR_SREG (ID,FECHA_ALTA, CODIGO_USUARIO, NUMERO_CORRELATIVO) VALUES (:1, :2, :3, :4)";

                var parameters = new OracleParameter[4];
                parameters[0] = new OracleParameter() {ParameterName = "paramID", Value = idTramSinReg};
                parameters[1] = new OracleParameter() {ParameterName = "paramFECHA_ALTA", Value = DateTime.Now, OracleDbType = OracleDbType.Date};
                parameters[2] = new OracleParameter() {ParameterName = "paramCODIGO_USUARIO", Value = usuario};
                parameters[3] = new OracleParameter() {ParameterName = "paramNUMERO_CORRELATIVO", Value = idexptes};
                context.Database.ExecuteSqlCommand(sql, parameters);

                return idexptes;
            }
        }

        //private void LimpiarCorrelativoExptes(int correlativo)
        //{
        //    using (var context = new MetModel())
        //    {
        //        var sql = "DELETE FROM EXPTES WHERE EXPNROCORR = :1";

        //        var parameters = new OracleParameter[1];
        //        parameters[0] = new OracleParameter { ParameterName = "paramEXPNROCORR", Value = correlativo };

        //        context.Database.ExecuteSqlCommand(sql, parameters);
        //    }
        //}

        public void GenerarCorrelativoExistente(string codigoTramite, string usuario, int correlativo)
        {
            using(var context = new MetModel())
            {
                var id = GenerarIdTramSinReg();

                var sql = "INSERT INTO COD_TRAM_CORR_SREG (ID,FECHA_ALTA, CODIGO_USUARIO, NUMERO_CORRELATIVO, COD_TRAMITE) VALUES (:1, :2, :3, :4, :5)";

                var parameters = new OracleParameter[5];
                parameters[0] = new OracleParameter() {ParameterName = "paramID", Value = id};
                parameters[1] = new OracleParameter() {ParameterName = "paramFECHA_ALTA", Value = DateTime.Now, OracleDbType = OracleDbType.Date};
                parameters[2] = new OracleParameter() {ParameterName = "paramCODIGO_USUARIO", Value = usuario};
                parameters[3] = new OracleParameter() {ParameterName = "paramNUMERO_CORRELATIVO", Value = correlativo};
                parameters[4] = new OracleParameter() {ParameterName = "paramCOD_TRAMITE", Value = codigoTramite};
                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }

        private int GenerarIdTramSinReg()
        {
            using(var context = new MetModel())
            {
                var sql = "SELECT SEQ_CODTRAMSREG.nextval FROM dual";
                var id = context.Database.SqlQuery<int>(sql).SingleOrDefault();

                return id;
            }
        }

        private int GenerarIdExptes()
        {
            using(var context = new MetModel())
            {
                var sql = "SELECT noregis_seq.nextval FROM dual";
                var correlativo = context.Database.SqlQuery<int>(sql).SingleOrDefault();

                return correlativo;
            }
        }

        public void ActualizarCorrelativoSinRegistrar(int nroCorrelativo, int nroTramite, string codigoTipoTramite)
        {
            using(var context = new MetModel())
            {
                var sql = "UPDATE COD_TRAM_CORR_SREG SET NUMERO_TRAMITE =:1 , COD_TRAMITE=:2 WHERE NUMERO_CORRELATIVO =:3 ";

                var parameters = new OracleParameter[3];
                parameters[0] = new OracleParameter {ParameterName = "paramNUMERO_TRAMITE", Value = nroTramite};
                parameters[1] = new OracleParameter {ParameterName = "paramCOD_TRAMITE", Value = codigoTipoTramite};
                parameters[2] = new OracleParameter {ParameterName = "paramNUMERO_CORRELATIVO", Value = nroCorrelativo};

                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }

        public bool ExisteCorrelativoSociedadSinRegistrar(int nroCorrelativo)
        {
            using(var context = new MetModel())
            {
                //Momentaneamente se debe verificar en la tabla de sociedades no registradas la existencia de un Nro. de Tramite
                const string sql = "SELECT COUNT(1) FROM COD_TRAM_CORR_SREG WHERE NUMERO_CORRELATIVO = :1";

                var parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter() {ParameterName = "paramNUMERO_CORRELATIVO", Value = nroCorrelativo};

                int cantidad = context.Database.SqlQuery<int>(sql, parameters).SingleOrDefault();

                return (cantidad > 0);
            }
        }

        public void AgregarCorrelativoSinRegistrar(int nroCorrelativo, string operador, string codigoTipoTramite, int nroTramite)
        {
            using(var context = new MetModel())
            {
                var id = GenerarIdTramSinReg();

                var sql = "INSERT INTO COD_TRAM_CORR_SREG (ID,FECHA_ALTA, CODIGO_USUARIO, NUMERO_CORRELATIVO, COD_TRAMITE, NUMERO_TRAMITE) VALUES (:1, :2, :3, :4, :5, :6)";

                var parameters = new OracleParameter[6];
                parameters[0] = new OracleParameter() {ParameterName = "paramID", Value = id};
                parameters[1] = new OracleParameter() {ParameterName = "paramFECHA_ALTA", Value = DateTime.Now, OracleDbType = OracleDbType.Date};
                parameters[2] = new OracleParameter() {ParameterName = "paramCODIGO_USUARIO", Value = operador};
                parameters[3] = new OracleParameter() {ParameterName = "paramNUMERO_CORRELATIVO", Value = nroCorrelativo};
                parameters[4] = new OracleParameter() {ParameterName = "paramCOD_TRAMITE", Value = codigoTipoTramite};
                parameters[5] = new OracleParameter() {ParameterName = "paramNUMERO_TRAMITE", Value = nroTramite};
                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }
    }
}