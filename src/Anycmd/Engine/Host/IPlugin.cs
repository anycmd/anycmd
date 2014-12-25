
namespace Anycmd.Engine.Host
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是插件。
    /// </summary>
    public interface IPlugin : IWfResource
    {
        /// <summary>
        /// 插件标识
        /// </summary>
        new Guid Id { get; }

        /// <summary>
        /// 插件标题
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 插件描述
        /// </summary>
        new string Description { get; }

        /// <summary>
        /// 插件作者。如xuexs
        /// </summary>
        string Author { get; }

        /// <summary>
        /// 注册插件
        /// </summary>
        /// <param name="nodeHost">数据交换宿主。与NodeHost.Instance效果相同</param>
        void Register(IAcDomain nodeHost);
    }
}
