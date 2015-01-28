
namespace Anycmd.Engine.Host.Edi.Handlers.Execute
{
    using Engine.Edi;
    using Exceptions;
    using Host;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// 命令执行器。一个执行器对应一个系统线程和一个“数据交换进程”。
    /// <remarks>
    /// 数据交换进程与系统进程不同，数据交换进程分四类：分发器、执行器、Mis系统、Web服务系统
    /// </remarks>
    /// </summary>
    public sealed class DefaultExecutor : IExecutor
    {
        #region private fields
        private readonly Guid _id = Guid.NewGuid();
        private Thread _workThread = null;
        #endregion

        #region 事件
        /// <summary>
        /// 开始工作前事件
        /// </summary>
        public event EventHandler<EventArgs> Starting;
        /// <summary>
        /// 执行前事件
        /// </summary>
        public event EventHandler<ExecutingEventArgs> Executing;
        /// <summary>
        /// 执行后事件
        /// </summary>
        public event EventHandler<ExecutedEventArgs> Executed;
        /// <summary>
        /// 停止工作前事件
        /// </summary>
        public event EventHandler<StoppingEventArgs> Stopping;
        /// <summary>
        /// 停止工作后事件
        /// </summary>
        public event EventHandler<EventArgs> Stoped;
        /// <summary>
        /// 睡眠前事件
        /// </summary>
        public event EventHandler<SleepingEventArgs> Sleepping;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> Waked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ExceptionEventArgs> Error;
        #endregion

        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        public DefaultExecutor(ProcessDescriptor process)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process");
            }
            this.Process = process;
            this.Ontology = Process.Ontology;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 
        /// </summary>
        public ProcessDescriptor Process
        {
            get;
            private set;
        }

        private OntologyDescriptor Ontology { get; set; }

        /// <summary>
        /// 数据发送器标识
        /// </summary>
        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// 查看发送者的运行状态
        /// </summary>
        public bool IsRuning
        {
            get;
            private set;
        }

        /// <summary>
        /// 查看执行者是否在睡觉
        /// <para>当执行者未加载到新的待执行命令消息的时候执行者会把该属性置为True</para>
        /// <para>当执行者在睡觉的时候调用Stop方法将直接停止执行者线程，否则直到当前处理周期完成后结束线程</para>
        /// </summary>
        public bool IsSleeping
        {
            get;
            private set;
        }

        /// <summary>
        /// 递增计数命令消息成功发送数
        /// </summary>
        public int SucessCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 递增计数命令消息失败发送数
        /// </summary>
        public int FailCount
        {
            get;
            private set;
        }
        #endregion

        #region Start
        /// <summary>
        /// 开始工作
        /// </summary>
        public void Start()
        {
            if (!IsRuning)
            {
                OnStarting(new EventArgs());
                IsRuning = true;
                _workThread = new Thread(Run);
                // 必须是前台线程
                _workThread.IsBackground = false;
                _workThread.Start();
            }
        }
        #endregion

        #region Stop
        /// <summary>
        /// 停止工作
        /// </summary>
        public void Stop()
        {
            if (IsRuning)
            {
                var e = new StoppingEventArgs();
                OnStopping(e);
                if (!e.Canceled)
                {
                    IsRuning = false;
                    bool wait = true;
                    while (wait)
                    {
                        if (!IsSleeping)
                        {
                            wait = true;
                        }
                        else
                        {
                            wait = false;
                            OnStoped(new EventArgs());
                        }
                    }
                }
            }
        }
        #endregion

        #region private methods
        #region Run
        /// <summary>
        /// 干活
        /// </summary>
        private void Run()
        {
            try
            {
                while (true)
                {
                    if (!IsRuning)
                    {
                        return;
                    }
                    if (IsSleeping)
                    {
                        OnWaked(new EventArgs());
                    }

                    IList<MessageEntity> toExecutes = this.Ontology.MessageProvider.GetTopNCommands(MessageTypeKind.Received, this.Ontology, this.Ontology.Ontology.ExecutorLoadCount, "CreateOn", "asc"); ;
                    IList<MessageContext> descriptors = new List<MessageContext>();
                    if (toExecutes == null || toExecutes.Count == 0)
                    {
                        Sleep(new SleepingEventArgs(this.Ontology.Ontology.ExecutorSleepTimeSpan));
                        continue;
                    }
                    else
                    {
                        foreach (var item in toExecutes)
                        {
                            var context = new MessageContext(this.Process.AcDomain, item);
                            var eArgs = new ExecutedEventArgs(context);
                            OnExecuting(eArgs);
                            MessageHandler.Instance.Response(context);
                            if (context.Result.Status == (int)Status.ExecuteOk)
                            {
                                SucessCount++;
                            }
                            else
                            {
                                FailCount++;
                            }

                            eArgs.ExecutedOn = DateTime.Now;
                            OnExecuted(eArgs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnError(new ExceptionEventArgs(ex) { ExceptionHandled = false });
            }
        }
        #endregion

        #region Sleep
        private void Sleep(SleepingEventArgs e)
        {
            OnSleepping(e);
            if (e.Canceled) return;
            IsSleeping = true;
            Thread.Sleep(e.SleepTimeSpan);
        }
        #endregion

        #region 事件包装
        private void OnStarting(EventArgs e)
        {
            if (Starting != null)
            {
                Starting(this, e);
            }
        }

        private void OnExecuting(ExecutingEventArgs e)
        {
            if (Executing != null)
            {
                Executing(this, e);
            }
        }

        private void OnExecuted(ExecutedEventArgs e)
        {
            if (Executed != null)
            {
                Executed(this, e);
            }
        }

        private void OnStopping(StoppingEventArgs e)
        {
            if (Stopping != null)
            {
                Stopping(this, e);
            }
        }

        private void OnStoped(EventArgs e)
        {
            if (_workThread != null)
            {
                IsRuning = false;
                _workThread.Join(1000);
                if (Stoped != null)
                {
                    Stoped(this, e);
                }
            }
        }

        private void OnSleepping(SleepingEventArgs e)
        {
            if (Sleepping != null)
            {
                Sleepping(this, e);
            }
        }

        private void OnWaked(EventArgs e)
        {
            this.IsSleeping = false;
            if (Waked != null)
            {
                Waked(this, e);
            }
        }

        private void OnError(ExceptionEventArgs e)
        {
            if (Error != null)
            {
                Error(this, e);
            }
        }
        #endregion
        #endregion
    }
}
