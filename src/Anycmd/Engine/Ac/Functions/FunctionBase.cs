
namespace Anycmd.Engine.Ac.Functions
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 函数、功能、操作、逻辑单元，基类。<see cref="IFunction"/>
    /// </summary>
    public abstract class FunctionBase : EntityBase, IFunction
    {
        private Guid _resourceTypeId;
        private string _code;

        public Guid? Guid { get; set; }

        public int IsEnabled { get; set; }
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

        public bool IsManaged { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid ResourceTypeId
        {
            get { return _resourceTypeId; }
            set
            {
                if (value == _resourceTypeId) return;
                if (value == System.Guid.Empty)
                {
                    throw new AnycmdException("必须指定资源");
                }
                if (_resourceTypeId != System.Guid.Empty)
                {
                    throw new AnycmdException("所属资源不能修改");
                }
                _resourceTypeId = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid DeveloperId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
    }
}
