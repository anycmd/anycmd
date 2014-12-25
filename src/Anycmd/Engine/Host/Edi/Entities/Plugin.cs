
namespace Anycmd.Engine.Host.Edi.Entities
{
    using Model;

    /// <summary>
    /// 插件。命令插件。命令插件实体用于配置插件。
    /// </summary>
    public class Plugin : EntityBase, IAggregateRoot
    {
        public Plugin()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        public Plugin(IPlugin plugin)
        {
            var plugType = plugin.GetType();
            this.Name = plugType.Name;
            this.FullName = plugType.FullName;
            this.AuthorCode = plugin.Author;
            this.Description = plugin.Description;
            base.Id = plugin.Id;
            this.Title = plugin.Title;
            this.IsEnabled = 0;
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 插件标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 类名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类全名。包括命名空间
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 插件作者
        /// </summary>
        public string AuthorCode { get; set; }
        /// <summary>
        /// 有效标记
        /// </summary>
        public int IsEnabled { get; set; }
    }
}
