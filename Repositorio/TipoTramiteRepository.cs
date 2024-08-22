using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using DAO;
using Domain.Contracts.Repositories;

namespace Repositorio
{
    public class TipoTramiteRepository : ITipoTramiteRepository
    {
        public IList<Int32> GetClasificaciones()
        {
            using (var context = new MetModel())
            {
                var tramitesBpm = ConfigurationManager.AppSettings.Get("Tramites.IdentificadorClasificablesPorBPM");
                var sql = String.Format("SELECT CODIGO FROM TRAMITETIPOCLASIFICACION TC WHERE TC.TRAMITETIPOID IN ({0})", tramitesBpm);
                return context.Database.SqlQuery<Int32>(sql).ToList();
            }
        }
    }
}
