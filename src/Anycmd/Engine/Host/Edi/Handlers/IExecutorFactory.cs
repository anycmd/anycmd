
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Engine.Edi;

    /// <summary>
    /// 命令执行者工厂
    /// </summary>
    public interface IExecutorFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        IExecutor CreateExecutor(ProcessDescriptor process);
    }
}
