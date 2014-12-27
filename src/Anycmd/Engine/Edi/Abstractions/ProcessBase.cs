
namespace Anycmd.Engine.Edi.Abstractions
{
    using Exceptions;
    using Model;
    using System;
    using Util;

    /// <summary>
    /// 进程
    /// </summary>
    public abstract class ProcessBase : EntityBase, IProcess
    {
        private string _type;
        private Guid _ontologyId;

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type
        {
            get { return _type; }
            set
            {
                if (value == _type) return;
                if (_type != null)
                {
                    throw new AnycmdException("不能更改进程类型");
                }
                else if (value == null)
                {
                    throw new AnycmdException("必须指定进程类型");
                }
                ProcessType processType;
                if (!value.TryParse(out processType))
                {
                    throw new AnycmdException("非法的进程类型");
                }
                _type = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int NetPort { get; set; }
        /// <summary>
        /// 有效标记
        /// </summary>
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid OntologyId
        {
            get { return _ontologyId; }
            set
            {
                if (value == _ontologyId) return;
                if (_ontologyId != Guid.Empty)
                {
                    throw new AnycmdException("不能更改关联本体");
                }
                _ontologyId = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrganizationCode { get; set; }
    }
}
