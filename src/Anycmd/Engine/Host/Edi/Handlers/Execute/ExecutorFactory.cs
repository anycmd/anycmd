
namespace Anycmd.Engine.Host.Edi.Handlers.Execute
{
    using Engine.Edi;

    /// <summary>
    /// 命令执行器工厂默认实现
    /// </summary>
    public sealed class ExecutorFactory : IExecutorFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public IExecutor CreateExecutor(ProcessDescriptor process)
        {
            return new DefaultExecutor(process);
        }
    }
}
