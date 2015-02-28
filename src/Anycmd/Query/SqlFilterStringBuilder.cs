
namespace Anycmd.Query
{
    using Engine.Rdb;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Util;

    /// <summary>
    /// 将数据查询筛选器转化为sql查询条件字符串
    /// </summary>
    public sealed class SqlFilterStringBuilder : ISqlFilterStringBuilder
    {
        #region GetSqlFilterString

        /// <summary>
        /// 将给定的数据查询筛选器列表转化为原生sql查询条件字符串返回
        /// </summary>
        /// <param name="rdb"></param>
        /// <param name="filters">数据查询筛选器</param>
        /// <param name="alias">别名，空字符串或null表示无别名</param>
        /// <param name="prams"></param>
        /// <returns></returns>
        public string FilterString(RdbDescriptor rdb, List<FilterData> filters, string alias, out List<DbParameter> prams)
        {
            bool useAlias = !string.IsNullOrEmpty(alias);
            prams = new List<DbParameter>();
            if (filters == null || filters.Count == 0)
                return string.Empty;
            var result = new StringBuilder();
            //type=string
            #region string
            var stringList = from f in filters where f.type == "string" select f;
            int i = 0;
            foreach (var filter in stringList)
            {
                var comparision = GetComparison(filter.comparison);
                result.Append(alias, useAlias).Append(".", useAlias).Append(filter.field);
                if (comparision == "like")
                {
                    result.Append(" ").Append("like").Append(" @").Append(filter.field).Append(i).Append("");
                    string value = string.Empty;
                    if (filter.value != null)
                    {
                        value = filter.value.ToString();
                    }
                    if (!value.StartsWith("%") && !value.EndsWith("%"))
                    {
                        value = "%" + value + "%";
                    }
                    prams.Add(CreateParameter(rdb, filter.field + i.ToString(CultureInfo.InvariantCulture), value));
                }
                else
                {
                    result.Append(comparision).Append("@").Append(filter.field).Append(i);
                    prams.Add(CreateParameter(rdb, filter.field + i.ToString(CultureInfo.InvariantCulture), filter.value));
                }
                result.Append(" and ");
                i++;
            }
            #endregion
            //type=guid
            #region guid
            var guidList = from f in filters where f.type == "guid" select f;
            i = 0;
            foreach (var filter in guidList)
            {
                var comparision = GetComparison(filter.comparison);
                result.Append(alias, useAlias).Append(".", useAlias).Append(filter.field);
                result.Append(comparision).Append("@").Append(filter.field).Append(i);
                if (filter.value.GetType() != typeof(Guid))
                {
                    throw new ValidationException("'" + filter.value + "'不是Guid类型的");
                }
                prams.Add(CreateParameter(rdb, filter.field + i.ToString(CultureInfo.InvariantCulture), filter.value));
                result.Append(" and ");
                i++;
            }
            #endregion
            //type=boolean
            #region boolean
            i = 0;
            var booleanList = from f in filters where f.type == "boolean" select f;
            foreach (var filter in booleanList)
            {
                var v = filter.value.ToString();
                bool b;
                if (!bool.TryParse(v, out b))
                {
                    throw new ValidationException("'" + filter.value + "'不是boolean类型的");
                }
                result.Append(alias, useAlias).Append(".", useAlias).Append(filter.field).Append("=@").Append(filter.field).Append(i.ToString(CultureInfo.InvariantCulture)).Append(" and ");
                var p = CreateParameter(rdb, filter.field + i.ToString(CultureInfo.InvariantCulture), v);
                prams.Add(p);
                i++;
            }
            #endregion
            //type=null
            #region null
            i = 0;
            var nullList = from f in filters where f.type == "null" select f;
            foreach (var filter in nullList)
            {
                result.Append(alias, useAlias).Append(".", useAlias).Append(filter.field).Append(GetComparison(filter.comparison)).Append(" and ");
                i++;
            }
            #endregion
            //type=numeric
            #region numeric
            i = 0;
            var numericList = from f in filters where f.type == "numeric" group f by f.field into g select g;
            foreach (var g in numericList)
            {
                result.Append("( ");
                string iiStr = string.Empty;
                int j = 0;
                foreach (var filter in g)
                {
                    iiStr += alias + "." + filter.field + GetComparison(filter.comparison) + "@" + filter.field + i.ToString(CultureInfo.InvariantCulture) + j.ToString(CultureInfo.InvariantCulture) + " and ";
                    if (filter.value.GetType() != typeof(int))
                    {
                        throw new ValidationException("'" + filter.value + "'不是int类型的");
                    }
                    prams.Add(CreateParameter(rdb, filter.field + i.ToString(CultureInfo.InvariantCulture) + j.ToString(CultureInfo.InvariantCulture), filter.value));
                    j++;
                }
                result.Append(iiStr.Substring(0, iiStr.Length - " and".Length));
                result.Append(" )");
                result.Append(" and ");
                i++;
            }
            #endregion
            //type=date
            #region date
            i = 0;
            var dateList = from f in filters where f.type == "date" group f by f.field into g select g;
            foreach (var g in dateList)
            {
                int j = 0;
                result.Append("( ");
                string iiStr = string.Empty;
                foreach (var filter in g)
                {
                    iiStr += alias + "." + filter.field + GetComparison(filter.comparison) + " @" + filter.field + i.ToString(CultureInfo.InvariantCulture) + j.ToString(CultureInfo.InvariantCulture) + ")" + " and ";
                    prams.Add(CreateParameter(rdb, filter.field + i.ToString(CultureInfo.InvariantCulture) + j.ToString(CultureInfo.InvariantCulture), filter.value));
                    j++;
                }
                result.Append(iiStr.Substring(0, iiStr.Length - " and".Length));
                result.Append(" )");
                result.Append(" and ");
                i++;
            }
            #endregion
            //type=list  :["1","2"]
            #region list
            i = 0;
            var listList = from f in filters where f.type == "list" select f;
            foreach (var filter in listList)
            {
                result.Append(alias, useAlias).Append(".", useAlias).Append(filter.field).Append(" in @").Append(filter.field).Append(i.ToString(CultureInfo.InvariantCulture)).Append(" and ");
                prams.Add(CreateParameter(rdb, filter.field + i.ToString(CultureInfo.InvariantCulture), filter.value.ToString().Replace("[", "( ").Replace("]", " )").Replace("\"", "'")));
                i++;
            }
            #endregion

            return result.ToString().Substring(0, result.Length - " and".Length);
        }
        #endregion

        private static string GetComparison(string comparison)
        {
            string res = string.Empty;
            switch (comparison)
            {
                case "lt":
                    res = "<";
                    break;
                case "gt":
                    res = ">";
                    break;
                case "eq":
                    res = "=";
                    break;
                case "like":
                    res = "like";
                    break;
                case "isnull":
                    res = " is null ";
                    break;
                case "isnotnull":
                    res = " is not null ";
                    break;
            }
            return res;
        }

        private static DbParameter CreateParameter(RdbDescriptor db, string parameterName, object value)
        {
            var p = db.CreateParameter();
            p.ParameterName = parameterName;
            p.Value = value;

            return p;
        }
    }
}
