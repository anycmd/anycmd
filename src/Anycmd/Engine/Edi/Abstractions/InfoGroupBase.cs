
namespace Anycmd.Engine.Edi.Abstractions
{
    using Exceptions;
    using Model;
    using System;

    public abstract class InfoGroupBase : EntityBase, IInfoGroup
    {
        private string _code;
        private Guid _ontologyId;

        /// <summary>
        /// 
        /// </summary>
        protected InfoGroupBase()
        {
        }

        #region Public Properties
        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return _code; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ValidationException("编码不能为空");
                }
                value = value.Trim();
                _code = value;
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 本体主键
        /// </summary>
        public Guid OntologyId
        {
            get { return _ontologyId; }
            set
            {
                if (value != _ontologyId)
                {
                    if (_ontologyId != Guid.Empty)
                    {
                        throw new CoreException("不能更改所属本体");
                    }
                    _ontologyId = value;
                }
            }
        }
        #endregion
    }
}
