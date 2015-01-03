
namespace Anycmd.Util
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    public static class DataTableExtension
    {
        /// <summary>
        /// DataTable To IList<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> ToList<T>(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return null;
            return (from DataRow row in dt.Rows select row.ToEntity<T>()).ToList();
        }

        /// <summary>
        /// DataRow To T
        /// </summary>
        public static T ToEntity<T>(this DataRow row)
        {
            var objType = typeof(T);
            var obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in row.Table.Columns)
            {
                var property =
                    objType.GetProperty(column.ColumnName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property == null || !property.CanWrite)
                {
                    continue;
                }
                var value = row[column.ColumnName];
                if (value == DBNull.Value) value = null;

                property.SetValue(obj, value, null);

            }
            return obj;
        }


        /// <summary>
        /// List To DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            try
            {
                var objType = typeof(T);
                var dataTable = new DataTable(objType.Name);
                if (list == null || list.Count <= 0) return dataTable;
                var properties = TypeDescriptor.GetProperties(objType);
                foreach (PropertyDescriptor property in properties)
                {
                    var propertyType = property.PropertyType;

                    //nullables must use underlying types
                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        propertyType = Nullable.GetUnderlyingType(propertyType);
                    //enums also need special treatment
                    if (propertyType.IsEnum)
                        propertyType = Enum.GetUnderlyingType(propertyType); //probably Int32

                    dataTable.Columns.Add(property.Name, propertyType);
                }

                foreach (T li in list)
                {
                    var row = dataTable.NewRow();
                    foreach (PropertyDescriptor property1 in properties)
                    {
                        row[property1.Name] = property1.GetValue(li) ?? DBNull.Value; //can't use null
                    }
                    dataTable.Rows.Add(row);

                }
                return dataTable;
            }
            catch
            {
                return null;
            }
        }
    }
}
