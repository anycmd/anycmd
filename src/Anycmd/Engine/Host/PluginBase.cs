
namespace Anycmd.Engine.Host
{
    using System;

    /// <summary>
    /// 表示插件基类
    /// </summary>
    public abstract class PluginBase : IPlugin
    {
        private BuiltInResourceKind _resourceType = BuiltInResourceKind.Plugin;

        /// <summary>
        /// 
        /// </summary>
        protected PluginBase() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">验证器标识</param>
        /// <param name="title">验证器标题</param>
        /// <param name="author">验证器作者</param>
        /// <param name="description">验证器描述</param>
        protected PluginBase(Guid id, string title, string author, string description)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Author = author;
            this.IsGlobal = false;
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
        public bool IsGlobal { get; protected set; }

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

        public abstract void Register(IAcDomain nodeHost);
    }
}
