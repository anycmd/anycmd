
namespace Anycmd.Engine.Host.Edi.Handlers.Distribute
{
    using Engine.Edi;
    using Exceptions;
    using Host;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// 命令分发器。一个分发器对应一个系统线程。
    /// <remarks>
    /// 数据交换进程与系统进程不同，数据交换进程分四类：分发器、执行器、Mis系统、Web服务系统
    /// </remarks>
    /// </summary>
    public sealed class DefaultDispatcher : IDispatcher
    {
        #region private fields
        private readonly Guid _id = Guid.NewGuid();
        private Thread _workThread = null;
        private readonly IAcDomain _acDomain;
        #endregion

        #region 事件
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> Starting;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<StoppingEventArgs> Stopping;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> Stoped;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<DistributingEventArgs> Distributing;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<DistributedEventArgs> Distributed;
        /// <summary>
        /// 
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
        public DefaultDispatcher(ProcessDescriptor process)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process");
            }
            this._acDomain = process.AcDomain;
            this.Process = process;
            this.Ontology = process.Ontology;
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

        /// <summary>
        /// 查看发送者的运行状态
        /// </summary>
        public bool IsRuning
        {
            get;
            private set;
        }

        /// <summary>
        /// 查看发送者是否在睡觉
        /// <para>当发送者未加载到新的待发送命令消息的时候发送者会把该属性置为True</para>
        /// <para>当发送者在睡觉的时候调用Stop方法将直接停止发送者线程，否则直到当前处理周期完成后结束线程</para>
        /// </summary>
        public bool IsSleeping
        {
            get;
            private set;
        }
        #endregion

        #region Start
        /// <summary>
        /// 
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
        /// 
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

                    IList<MessageEntity> toDistributes = this.Ontology.MessageProvider.GetTopNCommands(MessageTypeKind.Distribute,
                        this.Ontology, this.Ontology.Ontology.DispatcherLoadCount, "CreateOn", "asc");

                    if (toDistributes == null || toDistributes.Count == 0)
                    {
                        Sleep(new SleepingEventArgs(this.Ontology.Ontology.DispatcherSleepTimeSpan));
                        continue;
                    }
                    else
                    {
                        if (toDistributes.Count <= 0) continue;
                        // 根据ClientID分组命令消息，即把应发送到相同节点的命令消息分在同一组
                        var commandGroups = toDistributes.GroupBy(a => a.ClientId.ToLower());
                        foreach (var g in commandGroups)
                        {
                            foreach (var item in g)
                            {
                                NodeDescriptor responder;
                                _acDomain.NodeHost.Nodes.TryGetNodeById(item.ClientId, out responder);
                                var eArg = new DistributedEventArgs(new DistributeContext(item, responder));
                                OnSending(eArg);
                                MessageRequester.Instance.Request(eArg.Context);
                                var result = eArg.Context.Result;

                                // 处理发送结果
                                int stateCode = result.Status;
                                if (stateCode >= 200 && stateCode < 400)
                                {
                                    SucessCount += 1;
                                }
                                else
                                {
                                    FailCount += 1;
                                    if (stateCode == (int)Status.InternalServerError || stateCode < 200)
                                    {
                                        OnError(new ExceptionEventArgs(
                                            new AnycmdException(
                                                stateCode + result.ReasonPhrase + result.Description))
                                        {
                                            ExceptionHandled = false
                                        });
                                        Sleep(new SleepingEventArgs(this.Ontology.Ontology.DispatcherSleepTimeSpan));
                                    }
                                }
                                eArg.DistributedOn = DateTime.Now;
                                OnSended(eArg);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnError(new ExceptionEventArgs(ex) { ExceptionHandled = false });
                throw;
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

        #region 事件包装方法
        /// <summary>
        /// 开始
        /// </summary>
        private void OnStarting(EventArgs e)
        {
            if (Starting != null)
            {
                Starting(this, e);
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        private void OnStopping(StoppingEventArgs e)
        {
            if (Stopping != null)
            {
                Stopping(this, e);
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
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

        /// <summary>
        /// 睡眠
        /// </summary>
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

        private void OnSending(DistributingEventArgs e)
        {
            if (Distributing != null)
            {
                Distributing(this, e);
            }
        }

        private void OnSended(DistributedEventArgs e)
        {
            if (Distributed != null)
            {
                Distributed(this, e);
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
