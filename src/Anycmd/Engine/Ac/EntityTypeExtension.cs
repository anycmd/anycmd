
namespace Anycmd.Engine.Ac
{
    using Exceptions;
    using Model;
    using Rdb;
    using System;
    using System.Data.SqlClient;

    public static class EntityTypeExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="propertyCode"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool TryGetProperty(this EntityTypeState entityType, string propertyCode, out PropertyState property)
        {
            return entityType.AcDomain.EntityTypeSet.TryGetProperty(entityType, propertyCode, out property);
        }

        /// <summary>
        /// 从给定的实体类型所代表的实体集中读取给定标识的实体记录。
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DicReader GetData(this EntityTypeState entityType, Guid id)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }
            if (entityType == EntityTypeState.Empty || entityType.AcDomain == null)
            {
                throw new InvalidOperationException();
            }
            // TODO:不能假定实体集是持久在关系数据库中的，这里需要提取策略。根据该实体类型上的不同配置以不同的策略访问不同的数据库。
            RdbDescriptor db;
            if (!entityType.AcDomain.Rdbs.TryDb(entityType.DatabaseId, out db))
            {
                throw new AnycmdException("意外的实体类型数据库标识" + entityType.Code);
            }
            if (string.IsNullOrEmpty(entityType.TableName))
            {
                throw new AnycmdException(entityType.Name + "未配置对应的数据库表");
            }
            var sql = "select * from " + string.Format("[{0}]", entityType.TableName) + " as a where Id=@Id";
            using (var reader = db.ExecuteReader(sql, new SqlParameter("Id", id)))
            {
                if (reader.Read())
                {
                    var dic = new DicReader(entityType.AcDomain);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dic.Add(reader.GetName(i), reader.GetValue(i));
                    }
                    return dic;
                }
            }
            return null;
        }
    }
}
