
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 系统模型基类<see cref="IEntityType"/>
    /// </summary>
    public abstract class EntityTypeBase : EntityBase, IEntityType
    {
        private string _code;
        private string _codespace;
        private string _name;

        public string Codespace
        {
            get { return _codespace; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                }
                _codespace = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Code
        {
            get { return _code; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                }
                _code = value;
            }
        }
        public bool IsCatalogued { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid DatabaseId { get; set; }
        // TODO:验证数据库架构
        /// <summary>
        /// 
        /// </summary>
        public string SchemaName { get; set; }
        // TODO:验证数据库表
        /// <summary>
        /// 
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int EditWidth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int EditHeight { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ValidationException("名称是必须的");
                }
                _name = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid DeveloperId { get; set; }
    }
}
