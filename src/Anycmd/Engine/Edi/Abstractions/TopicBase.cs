
namespace Anycmd.Engine.Edi.Abstractions
{
    using Model;
    using System;

    public abstract class TopicBase : EntityBase, ITopic
    {
        private string _code;

        protected TopicBase() { }

        /// <summary>
        /// 编码
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
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAllowed { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 本体主键
        /// </summary>
        public Guid OntologyId { get; set; }
    }
}
