
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Distribute;
    using Engine.Edi;
    using Exceptions;
    using Host;
    using System;

    /// <summary>
    /// 命令分发者
    /// </summary>
    public interface IDispatcher
    {
        #region 事件
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs> Starting;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<StoppingEventArgs> Stopping;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs> Stoped;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<DistributingEventArgs> Distributing;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<DistributedEventArgs> Distributed;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<SleepingEventArgs> Sleepping;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs> Waked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<ExceptionEventArgs> Error;
        #endregion

        #region 属性
        /// <summary>
        /// 
        /// </summary>
        ProcessDescriptor Process { get; }
        /// <summary>
        /// 数据发送器标识
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 返回发送者的运行状态
        /// </summary>
        bool IsRuning { get; }
        /// <summary>
        /// 查看发送者是否在睡觉
        /// <para>当发送者未加载到新的待发送命令消息的时候发送者会把该属性置为True</para>
        /// </summary>
        bool IsSleeping { get; }
        /// <summary>
        /// 递增计数命令消息失败发送数
        /// </summary>
        int FailCount { get; }
        /// <summary>
        /// 递增计数命令消息成功发送数
        /// </summary>
        int SucessCount { get; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        void Start();
        /// <summary>
        /// 
        /// </summary>
        void Stop();
    }
}
