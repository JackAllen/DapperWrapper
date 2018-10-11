namespace Dapper.Wrapper
{
    using System;
    using System.Collections.Generic;

    public interface IDapperWrapper
    {
        T Get<T>(string sql, object parameters, Func<SqlMapper.GridReader, T> mapping);

        T Get<T>(string sql, object parameters);

        List<T> GetList<T>(string sql, object parameters);

        void Put(string sql, object parameters);
    }
}
