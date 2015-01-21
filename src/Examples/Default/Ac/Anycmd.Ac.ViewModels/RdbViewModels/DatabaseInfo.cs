
namespace Anycmd.Ac.ViewModels.RdbViewModels
{
    using Engine.Rdb;
    using System;

    /// <summary>
    /// 关系数据库详细展示模型
    /// </summary>
    public sealed class DatabaseInfo
    {
        public static DatabaseInfo Create(RDatabase database)
        {
            if (database == null)
            {
                return null;
            }
            return new DatabaseInfo
            {
                CatalogName = database.CatalogName,
                CreateBy = database.CreateBy,
                CreateOn = database.CreateOn,
                CreateUserId = database.CreateUserId,
                DataSource = database.DataSource,
                Description = database.Description,
                Id = database.Id,
                IsTemplate = database.IsTemplate,
                Profile = database.Profile,
                RdbmsType = database.RdbmsType,
                UserId = database.UserId
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RdbmsType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsTemplate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CatalogName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Profile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
