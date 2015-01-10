
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 定义进程集合。
    /// <remarks>
    /// 注意：基础库的进程指的是命令接收服务器、命令执行器、命令分发器、数据交换平台Mis系统这基类进程。
    /// </remarks>
    /// </summary>
    public interface IProcesseSet : IEnumerable<ProcessDescriptor>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        ProcessDescriptor this[Guid processId] { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        bool ContainsProcess(Guid processId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        bool TryGetProcess(Guid processId, out ProcessDescriptor process);
    }
}
