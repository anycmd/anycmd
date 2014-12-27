
namespace Anycmd.Engine.Info
{
    using Host;
    using Model;
    using System;

    /// <summary>
    /// 信息验证器抽象基类
    /// </summary>
    public abstract class InfoRuleBase : DisposableObject, IWfResource
    {
        private BuiltInResourceKind _resourceType = BuiltInResourceKind.InfoCheck;

        /// <summary>
        /// 
        /// </summary>
        protected InfoRuleBase() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="description"></param>
        protected InfoRuleBase(Guid id, string title, string author, string description)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Author = author;
        }

        /// <summary>
        /// 验证器标识
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// 验证器标题
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// 验证器描述
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// 验证器作者
        /// </summary>
        public string Author { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return Title; }
            protected set
            {
                Title = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BuiltInResourceKind BuiltInResourceKind
        {
            get { return _resourceType; }
            protected set
            {
                _resourceType = value;
            }
        }
    }
}
