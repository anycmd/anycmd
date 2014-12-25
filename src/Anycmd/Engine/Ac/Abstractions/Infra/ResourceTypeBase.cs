
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 资源基类
    /// </summary>
    public abstract class ResourceTypeBase : EntityBase, IResourceType
    {
        private string _code;
        private string _name;

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

        public Guid AppSystemId { get; set; }

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
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
    }
}
