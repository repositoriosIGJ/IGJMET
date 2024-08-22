using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace Utils
{
    public static class OracleCommadHelper
    {
        public static void ExecuteInsert(OracleConnection conn, string tableName, OracleParameter[] parameters)
        {
            StringBuilder sbParamName = new StringBuilder();
            sbParamName.AppendFormat("INSERT INTO {0} (", tableName);

            StringBuilder sbParamCount = new StringBuilder();
            sbParamCount.Append("VALUES (");

            string parameterName;
            for (int i = 0; i < parameters.Length; i++)
            {
                parameterName = parameters[i].ParameterName.Replace("param", "");
                sbParamName.Append(parameterName);
                sbParamName.Append(",");

                sbParamCount.AppendFormat(":{0}", i + 1);
                sbParamCount.Append(",");
            }

            sbParamCount.Remove(sbParamCount.Length - 1, 1);
            sbParamName.Append(")");

            sbParamName.Remove(sbParamName.Length - 1, 1);
            sbParamName.AppendFormat(") {0}", sbParamCount.ToString());

            ExecuteCommand(conn, sbParamName.ToString(), parameters);

        }

        public static void ExecuteCommand(OracleConnection conn, string sql, OracleParameter[] parameters)
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
