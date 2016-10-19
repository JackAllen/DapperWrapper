namespace Dapper.Wrapper
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    public static class Extensions
    {
        public static SqlMapper.ICustomQueryParameter ToSingleTypeTvp<T>(this List<T> objects, string name, string columnName)
        {
            if (!objects.Any())
            {
                return null;
            }

            var dt = new DataTable { Columns = { new DataColumn(columnName, typeof(T)) } };

            foreach (var obj in objects)
            {
                dt.Rows.Add(obj);
            }

            return dt.AsTableValuedParameter(name);
        }

        public static SqlMapper.ICustomQueryParameter ToTvp<T>(this T obj, string name)
        {
            return new List<T> { obj }.ToTvp(name);
        }

        public static SqlMapper.ICustomQueryParameter ToTvp<T>(this List<T> objects, string name)
        {
            if (!objects.Any())
            {
                return null;
            }

            var dapperProperties = GetDapperProperties(typeof(T));
            var columns = GetDataColumns(dapperProperties);

            var table = new DataTable();

            table.Columns.AddRange(columns);

            var objectValues = objects.Select(s =>
                dapperProperties.ToDictionary(
                    dp => dp.Name, 
                    dp => s.GetType().GetProperty(dp.PropertyName).GetValue(s, null)
            )).ToList();

            foreach (var values in objectValues)
            {
                var row = table.NewRow();

                foreach (var v in values)
                {
                    row[v.Key] = v.Value;
                }

                table.Rows.Add(row);
            }

            return table.AsTableValuedParameter(name);
        }

        private static DataColumn[] GetDataColumns(IEnumerable<DapperProperty> dapperProperties)
        {
            return dapperProperties.Select(p => new DataColumn
            {
                DataType = p.DataType,
                ColumnName = p.Name,
                MaxLength = p.MaxLength
            }).ToArray();
        }

        private static List<DapperProperty> GetDapperProperties(Type obj)
        {
            var properties = obj
                    .GetProperties()
                    .Where(p => Attribute.IsDefined(p, typeof(TvpPropertyAttribute)))
                    .ToList();

            return properties.Select(p =>
            {
                var attr = p.GetCustomAttribute<TvpPropertyAttribute>();

                return new DapperProperty
                {
                    Name = attr.Name ?? p.Name,
                    PropertyName = p.Name,
                    MaxLength = attr._maxLength ?? -1,
                    DataType = p.PropertyType
                };
            }).ToList();
        }
    }

    public class DapperProperty
    {
        public string PropertyName { get; set; }

        public Type DataType { get; set; }

        public string Name { get; set; }

        public int MaxLength { get; set; }
    }
}
