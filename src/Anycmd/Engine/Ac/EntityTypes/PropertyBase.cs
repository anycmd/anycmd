
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 系统字段基类<see cref="IProperty"/>
    /// </summary>
    public abstract class PropertyBase : EntityBase, IProperty
    {
        private string _code;
        private Guid _entityTypeId;
        private string _name;

        protected PropertyBase()
        {
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid EntityTypeId
        {
            get { return _entityTypeId; }
            set
            {
                if (_entityTypeId != value && _entityTypeId != Guid.Empty)
                {
                    throw new AnycmdException("不能更改字段的所属模型");
                }
                _entityTypeId = value;
            }
        }
        public Guid? ForeignPropertyId
        {
            get;
            set;
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
        public string GroupCode { get; set; }
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
        public string GuideWords { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Tooltip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? MaxLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? DicId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDetailsShow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDeveloperOnly { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsInput { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InputType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsTotalLine { get; set; }
    }
}
