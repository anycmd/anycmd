
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 菜单基类
    /// </summary>
    public abstract class MenuBase : EntityBase, IMenu
    {
        private Guid _appsystemId;
        private string _name;

        public Guid AppSystemId
        {
            get { return _appsystemId; }
            set
            {
                if (value == _appsystemId) return;
                if (_appsystemId != Guid.Empty)
                {
                    throw new ValidationException("应用系统不能更改");
                }
                _appsystemId = value;
            }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? ParentId { get; set; }
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
        public string Url { get; set; }
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
