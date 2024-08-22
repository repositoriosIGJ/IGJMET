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
    public class TramiteRepositorio : ITramiteRepositorio
    {
        private readonly ITimbradoRepositorio _timbradoRepositorio;
        private readonly ITipoTramiteRepositorio _tipoTramiteRepositorio;

        public TramiteRepositorio(ITimbradoRepositorio timbradoRepositorio, ITipoTramiteRepositorio tipoTramiteRepositorio)
        {
            _timbradoRepositorio = timbradoRepositorio;
            _tipoTramiteRepositorio = tipoTramiteRepositorio;
        }

        public Tramite GetTramite(int nroTramite)
        {
            using (var context = new MetModel())
            {
                const string sql = "SELECT * FROM TRAMIT WHERE TRANROTRAM = :1";

                var parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter() { ParameterName = "paramTRANROTRAM", Value = nroTramite };

                var entidades = context.Database.SqlQuery<TRAMIT>(sql, parameters).ToList();

                if (!entidades.Any())
                    throw new TramiteNotExistException("Nro. de tramite inexistente en el sistema");

                var entidad = entidades.Find(x => x.TRADIGITAL == "S") ?? entidades.Find(x => x.TRADIGITAL != "S");

                Tramite tramite = new TramiteMapper().Map(entidad);

                try
                {
                    tramite.Timbrados = _timbradoRepositorio.GetTimbrados(nroTramite);
                }
                catch (TramiteNotExistException)
                {
                    //Tramite sin timbrado
                }

                tramite.TipoTramite = GetTipoTramite(Convert.ToInt32(tramite.CodTramite));
                return tramite;
            }
        }

        public Tramite GetTramitePorTimbrado(int nroTramite)
        {
            IList<Timbrado> timbrados = _timbradoRepositorio.GetTimbrados(nroTramite);

            var tramite = new Tramite(nroTramite)
            {
                Timbrados = timbrados,
                NroCorrelativo = timbrados[0].NroCorrelativo
            };

            tramite.TipoTramite = GetTipoTramite(Convert.ToInt32(tramite.CodTramite));
            return tramite;
        }

        public TramiteGratuito GetTramiteGratuito(int nroTramite)
        {
            using (var context = new MetModel())
            {
                const string sql = "SELECT * FROM TRAMITE_GRATUITO WHERE TGRNROTRAM = :1";

                var parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramTGRNROTRAM", Value = nroTramite };

                var entidad = context.Database.SqlQuery<TRAMITE_GRATUITO>(sql, parameters).SingleOrDefault();

                if (entidad == null)
                    throw new TramiteNotExistException("Nro. de tramite inexistente en el sistema");

                return new TramiteGratuitoMapper().Map(entidad);
            }
        }

        public bool ExisteTramite(int nroTramite)
        {
            using (var context = new MetModel())
            {
                //Momentaneamente se debe verificar en la tabla timbrados la existencia de un Nro. de Tramite
                const string sql = "SELECT COUNT(1) FROM TRAMIT WHERE TRANROTRAM = :1";

                var parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter() { ParameterName = "paramTRANROTRAM", Value = nroTramite };

                int cantidad = context.Database.SqlQuery<int>(sql, parameters).SingleOrDefault();

                return (cantidad > 0);
            }
        }

        public int GenerarNroTramite()
        {
            using (var context = new MetModel())
            {
                var sql = "SELECT SEQ_NROTRAM_GRALDEMO.NEXTVAL FROM DUAL";
                var nroTramite = context.Database.SqlQuery<int>(sql).SingleOrDefault();
                return nroTramite;
            }
        }

        public void GuardarTramite(Tramite tramite)
        {
            using (var context = new MetModel())
            {
                var sql = "INSERT INTO TRAMIT (TRANROCORR, TRACODTRAM, TRAFECHACT, TRANROTRAM) VALUES (:1, :2, :3, :4)";

                var parameters = new OracleParameter[4];
                parameters[0] = new OracleParameter() { ParameterName = "paramTRANROCORR", Value = tramite.NroCorrelativo };
                parameters[1] = new OracleParameter() { ParameterName = "paramTRACODTRAM", Value = tramite.CodTramite };
                parameters[2] = new OracleParameter() { ParameterName = "paramTRAFECHACT", Value = DateTime.Now, OracleDbType = OracleDbType.Date };
                parameters[3] = new OracleParameter() { ParameterName = "paramTRANROTRAM", Value = tramite.NroTramite };

                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }

        public void GuardarDigital(TramiteDigital tramiteDigital)
        {
            using (var context = new MetModel())
            {
                const string sql = "INSERT INTO TRAMITE_DIGITAL (TRDNROCORR, TRDNROTRAM, TRDNROPRES, TRDFECACTU, TRDGUID) VALUES (:1, :2, :3, :4, :5)";

                var parameters = new OracleParameter[5];
                parameters[0] = new OracleParameter() { ParameterName = "paramTRDNROCORR", Value = tramiteDigital.NroCorrelativo };
                parameters[1] = new OracleParameter() { ParameterName = "paramTRDNROTRAM", Value = tramiteDigital.NroTramite };
                parameters[2] = new OracleParameter() { ParameterName = "paramTRDNROPRES", Value = tramiteDigital.NroPresentacion };
                parameters[3] = new OracleParameter() { ParameterName = "paramTRDFECACTU", Value = tramiteDigital.Fecha, OracleDbType = OracleDbType.Date };
                parameters[4] = new OracleParameter() { ParameterName = "paramTRDGUID", Value = tramiteDigital.Guid };

                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }

        public void GuardarTramiteGratuito(TramiteGratuito tramite)
        {
            using (var context = new MetModel())
            {
                const string sql = "INSERT INTO TRAMITE_GRATUITO (TGRNROTRAM, TGRFECHA, TGRUSUARIO, TGRNROCORR, TGRCODTRAM) VALUES (:1, :2, :3, :4, :5)";

                var parameters = new OracleParameter[5];
                parameters[0] = new OracleParameter() { ParameterName = "paramTGRNROTRAM", Value = tramite.NroTramite };
                parameters[1] = new OracleParameter() { ParameterName = "paramTGRFECHA", Value = DateTime.Now, OracleDbType = OracleDbType.Date };
                parameters[2] = new OracleParameter() { ParameterName = "paramTGRUSUARIO", Value = tramite.Usuario.Id };
                parameters[3] = new OracleParameter() { ParameterName = "paramTGRNROCORR", Value = tramite.NroCorrelativo };
                parameters[4] = new OracleParameter() { ParameterName = "paramTGRCODTRAM", Value = tramite.CodTramite };

                context.Database.ExecuteSqlCommand(sql, parameters);
            }
        }

        private TipoTramite GetTipoTramite(int codTramite)
        {
            try
            {
                return _tipoTramiteRepositorio.GetTipoTramite(codTramite);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public IList<TipoSociedad> GetTiposSociedad()
        {
            var list = new List<SOCIEDADTIPO>();
            var tipoSociedadList = new List<TipoSociedad>();

            using (var context = new MetModel())
            {
                const string sql = "SELECT * FROM SOCIEDADTIPO";
                list = context.Database.SqlQuery<SOCIEDADTIPO>(sql).ToList();
            }

            foreach (var sociedadTipo  in list)
            {
                tipoSociedadList.Add(new TipoSociedadMapper().Map(sociedadTipo));
            }

            return tipoSociedadList;
        }

        public int GetCodigoTipoSociedadById(int id)
        {
            using (var context = new MetModel())
            {
                const string sql = "SELECT CODIGO FROM SOCIEDADTIPO WHERE ID = :1";

                var parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter() { ParameterName = "paramID", Value = id };

                return context.Database.SqlQuery<int>(sql, parameters).SingleOrDefault();

            }

        }
    

        public IList<TipoTramite> GetTipoTramite()
        {
            var list = new List<TABGEN>();
            var tipoTramiteList = new List<TipoTramite>();

            using (var context = new MetModel())
            {
                const string sql = "SELECT * FROM TABGEN WHERE TABCLAVE IN ('00170','01270','01320','00810')";
                list = context.Database.SqlQuery<TABGEN>(sql).ToList();
            }

            foreach (var tipoTramite in list)
            {
                tipoTramiteList.Add(new TipoTramiteMapper().Map(tipoTramite));
            }

            return tipoTramiteList;
        }

    }
}
