
namespace Anycmd.Util
{
    using System;
    using System.Data;

    public static class DataRecordExtension
    {
        #region GetNullableString
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetNullableString(this IDataRecord record, string name)
        {
            string value = null;
            var obj = record.GetValue(record.GetOrdinal(name));
            if (obj != DBNull.Value)
            {
                value = (string)obj;
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public static string GetNullableString(this IDataRecord record, int ordinal)
        {
            string value = null;
            var obj = record.GetValue(ordinal);
            if (obj != DBNull.Value)
            {
                value = (string)obj;
            }
            return value;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string[] GetNullableStringArray(this IDataRecord record, string name)
        {
            var obj = record.GetValue(record.GetOrdinal(name));
            if (obj != DBNull.Value)
            {
                var value = (string)obj;
                return value.Split(',');
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public static string[] GetNullableStringArray(this IDataRecord record, int ordinal)
        {
            var obj = record.GetValue(ordinal);
            if (obj != DBNull.Value)
            {
                var value = (string)obj;
                return value.Split(',');
            }
            else
            {
                return null;
            }
        }

        #region GetNullableInt32

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? GetNullableInt32(this IDataRecord record, string name, int? defaultValue = null)
        {
            int? value = null;
            var obj = record.GetValue(record.GetOrdinal(name));
            if (obj != DBNull.Value)
            {
                value = (int)obj;
            }
            else
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue;
                }
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? GetNullableInt32(this IDataRecord record, int ordinal, int? defaultValue = null)
        {
            int? value = null;
            var obj = record.GetValue(ordinal);
            if (obj != DBNull.Value)
            {
                value = (int)obj;
            }
            else
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue;
                }
            }
            return value;
        }

        public static int GetInt32(this IDataRecord record, int ordinal, int? defaultValue = null)
        {
            if (!defaultValue.HasValue)
            {
                defaultValue = int.MinValue;
            }
            int? value = defaultValue;
            var obj = record.GetValue(ordinal);
            if (obj != DBNull.Value)
            {
                value = (int)obj;
            }
            return value.Value;
        }
        #endregion

        #region GetNullableInt64
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Int64? GetNullableInt64(this IDataRecord record, string name)
        {
            Int64? value = null;
            var obj = record.GetValue(record.GetOrdinal(name));
            if (obj != DBNull.Value)
            {
                value = (Int64)obj;
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public static Int64? GetNullableInt64(this IDataRecord record, int ordinal)
        {
            Int64? value = null;
            var obj = record.GetValue(ordinal);
            if (obj != DBNull.Value)
            {
                value = (Int64)obj;
            }
            return value;
        }
        #endregion

        #region GetNullableDateTime

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? GetNullableDateTime(this IDataRecord record, string name, DateTime? defaultValue = null)
        {
            DateTime? value = null;
            var obj = record.GetValue(record.GetOrdinal(name));
            if (obj != DBNull.Value)
            {
                value = (DateTime)obj;
            }
            else
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue;
                }
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="ordinal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? GetNullableDateTime(this IDataRecord record, int ordinal, DateTime? defaultValue = null)
        {
            DateTime? value = null;
            var obj = record.GetValue(ordinal);
            if (obj != DBNull.Value)
            {
                value = (DateTime)obj;
            }
            else
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue;
                }
            }
            return value;
        }

        public static DateTime GetDateTime(this IDataRecord record, int ordinal, DateTime? defaultValue = null)
        {
            if (!defaultValue.HasValue)
            {
                defaultValue = SystemTime.MinDate;
            }
            DateTime? value = defaultValue;
            var obj = record.GetValue(ordinal);
            if (obj != DBNull.Value)
            {
                value = (DateTime)obj;
            }
            return value.Value;
        }
        #endregion

        #region GetNullableBoolean
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool? GetNullableBoolean(this IDataRecord record, string name)
        {
            bool? value = null;
            var obj = record.GetValue(record.GetOrdinal(name));
            if (obj != DBNull.Value)
            {
                value = (bool)obj;
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public static bool? GetNullableBoolean(this IDataRecord record, int ordinal)
        {
            bool? value = null;
            var obj = record.GetValue(ordinal);
            if (obj != DBNull.Value)
            {
                value = (bool)obj;
            }
            return value;
        }
        #endregion

        #region GetNullableGuid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Guid? GetNullableGuid(this IDataRecord record, string name)
        {
            Guid? value = null;
            var obj = record.GetValue(record.GetOrdinal(name));
            if (obj != DBNull.Value)
            {
                value = (Guid)obj;
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public static Guid? GetNullableGuid(this IDataRecord record, int ordinal)
        {
            Guid? value = null;
            var obj = record.GetValue(ordinal);
            if (obj != DBNull.Value)
            {
                value = (Guid)obj;
            }
            return value;
        }
        #endregion
    }
}
