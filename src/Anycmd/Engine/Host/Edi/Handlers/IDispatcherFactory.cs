
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Engine.Edi;

    /// <summary>
    /// 命令分发器工厂。
    /// </summary>
    public interface IDispatcherFactory
    {
        /// <summary>
        /// 根据给定的进程配置信息创建并返回一个分发器。
        /// </summary>
        /// <param name="process">进程描述对象，进程描述对象上可以读取进程配置信息。</param>
        /// <returns></returns>
        IDispatcher CreateDispatcher(ProcessDescriptor process);
    }
}
