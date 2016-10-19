namespace Dapper.Wrapper
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;

    public class DapperWrapper : IDapperWrapper
    {
        private readonly string connectionString;

        public DapperWrapper(string connectionName)
        {
            if (string.IsNullOrWhiteSpace(connectionName))
            {
                throw new ArgumentException("A connection name must be provided");
            }

            this.connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public DapperWrapper(ConnectionStringSettings connectionSettings)
        {
            if (string.IsNullOrWhiteSpace(connectionSettings.ConnectionString))
            {
                throw new ArgumentException("A connection string must be provided");
            }

            this.connectionString = connectionSettings.ConnectionString;
        }

        private SqlConnection connection => new SqlConnection(this.connectionString);

        public T Get<T>(string sql, object parameters, Func<SqlMapper.GridReader, T> map)
        {
            using (var c = this.connection)
            {
                using (var m = c.QueryMultiple(sql, parameters))
                {
                    return map(m);
                }
            }
        }

        public T Get<T>(string sql, object parameters)
        {
            using (var c = this.connection)
            {
                return c.Query<T>(sql, parameters).FirstOrDefault();
            }
        }

        public List<T> GetList<T>(string sql, object parameters)
        {
            using (var c = this.connection)
            {
                return c.Query<T>(sql, parameters).ToList();
            }
        }

        public void Put(string sql, object parameters)
        {
            using (var c = this.connection)
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                c.Query(sql, parameters).FirstOrDefault();
            }
        }
    }
}
