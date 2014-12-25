
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Engine.Edi;
    using Exceptions;
    using Execute;
    using Host;
    using System;

    /// <summary>
    /// 命令执行者
    /// </summary>
    public interface IExecutor
    {
        #region 事件
        /// <summary>
        /// 开始工作前事件
        /// </summary>
        event EventHandler<EventArgs> Starting;
        /// <summary>
        /// 执行前事件
        /// </summary>
        event EventHandler<ExecutingEventArgs> Executing;
        /// <summary>
        /// 执行后事件
        /// </summary>
        event EventHandler<ExecutedEventArgs> Executed;
        /// <summary>
        /// 停止工作前事件
        /// </summary>
        event EventHandler<StoppingEventArgs> Stopping;
        /// <summary>
        /// 停止工作后事件
        /// </summary>
        event EventHandler<EventArgs> Stoped;
        /// <summary>
        /// 睡眠前事件
        /// </summary>
        event EventHandler<SleepingEventArgs> Sleepping;
        /// <summary>
        /// 醒来时
        /// </summary>
        event EventHandler<EventArgs> Waked;
        /// <summary>
        /// 发生了异常时
        /// </summary>
        event EventHandler<ExceptionEventArgs> Error;
        #endregion

        #region 属性
        /// <summary>
        /// 进程
        /// </summary>
        ProcessDescriptor Process { get; }
        /// <summary>
        /// 执行者标识
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 获取运行状态，返回True表示运行中
        /// </summary>
        bool IsRuning { get; }
        /// <summary>
        /// 获取休眠状态，返回True表示休眠中
        /// </summary>
        bool IsSleeping { get; }
        /// <summary>
        /// 递增计数命令消息成功发送数
        /// </summary>
        int SucessCount { get; }
        /// <summary>
        /// 递增计数命令消息失败发送数
        /// </summary>
        int FailCount { get; }
        #endregion

        /// <summary>
        /// 启动命令执行者
        /// </summary>
        void Start();

        /// <summary>
        /// 停止命令执行者
        /// </summary>
        void Stop();
    }
}
