
namespace Anycmd.Engine.Host.Edi.MemorySets
{
    using Bus;
    using Engine.Edi;
    using Engine.Edi.Abstractions;
    using Engine.Edi.Messages;
    using Entities;
    using Exceptions;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Util;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class ProcesseSet : IProcesseSet, IMemorySet
    {
        public static readonly IProcesseSet Empty = new ProcesseSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<Guid, ProcessDescriptor> _dic = new Dictionary<Guid, ProcessDescriptor>();
        private bool _initialized = false;
        private readonly object _locker = new object();

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;

        public Guid Id
        {
            get { return _id; }
        }

        public ProcesseSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (host.Equals(EmptyAcDomain.SingleInstance))
            {
                _initialized = true;
            }
            this._host = host;
            new MessageHandler(this).Register();
        }

        /// <summary>
        /// 根据发送策略名索引发送策略
        /// </summary>
        /// <param name="processId">发送策略名</param>
        /// <exception cref="AnycmdException">当给定名称的发送策略不存在时引发</exception>
        /// <returns></returns>
        /// <exception cref="AnycmdException">当进程标识非法时抛出</exception>
        public ProcessDescriptor this[Guid processId]
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                if (!_dic.ContainsKey(processId))
                {
                    throw new AnycmdException("意外的进程标识");
                }

                return _dic[processId];
            }
        }

        public bool ContainsProcess(Guid processId)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dic.ContainsKey(processId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public bool TryGetProcess(Guid processId, out ProcessDescriptor process)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dic.TryGetValue(processId, out process);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Refresh()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerator<ProcessDescriptor> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dic.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dic.Values.GetEnumerator();
        }

        private void Init()
        {
            if (_initialized) return;
            lock (_locker)
            {
                if (_initialized) return;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _dic.Clear();
                var processes = _host.RetrieveRequiredService<INodeHostBootstrap>().GetProcesses();
                foreach (var process in processes)
                {
                    _dic.Add(process.Id, new ProcessDescriptor(_host, ProcessState.Create(process), process.Id));
                }
                _initialized = true;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        #region MessageHandler
        private class MessageHandler : 
            IHandler<AddProcessCommand>,
            IHandler<UpdateProcessCommand>,
            IHandler<RemoveProcessCommand>,
            IHandler<ChangeProcessCatalogCommand>,
            IHandler<ChangeProcessNetPortCommand>
        {
            private readonly ProcesseSet _set;

            public MessageHandler(ProcesseSet set)
            {
                this._set = set;
            }

            public void Register()
            {
                var messageDispatcher = _set._host.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("messageDispatcher has not be set of host:{0}".Fmt(_set._host.Name));
                }
                messageDispatcher.Register((IHandler<AddProcessCommand>)this);
                messageDispatcher.Register((IHandler<UpdateProcessCommand>)this);
                messageDispatcher.Register((IHandler<RemoveProcessCommand>)this);
                messageDispatcher.Register((IHandler<ChangeProcessCatalogCommand>)this);
                messageDispatcher.Register((IHandler<ChangeProcessNetPortCommand>)this);
            }

            public void Handle(AddProcessCommand message)
            {
                var host = _set._host;
                var processRepository = _set._host.RetrieveRequiredService<IRepository<Process>>();
                if (!message.Input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                if (host.NodeHost.Processs.ContainsProcess(message.Input.Id.Value))
                {
                    throw new ValidationException("给定标识标识的记录已经存在");
                }

                var entity = Process.Create(message.Input);

                lock (_set._locker)
                {
                    if (!_set._dic.ContainsKey(entity.Id))
                    {
                        _set._dic.Add(entity.Id, new ProcessDescriptor(host, ProcessState.Create(entity), entity.Id));
                    }
                    try
                    {
                        processRepository.Add(entity);
                        processRepository.Context.Commit();
                    }
                    catch
                    {
                        if (_set._dic.ContainsKey(entity.Id))
                        {
                            _set._dic.Remove(entity.Id);
                        }
                        processRepository.Context.Rollback();
                        throw;
                    }
                }
                _set._host.PublishEvent(new ProcessAddedEvent(message.UserSession, entity));
                _set._host.CommitEventBus();
            }

            public void Handle(UpdateProcessCommand message)
            {
                var host = _set._host;
                var processRepository = _set._host.RetrieveRequiredService<IRepository<Process>>();
                if (!host.NodeHost.Processs.ContainsProcess(message.Input.Id))
                {
                    throw new NotExistException();
                }
                var entity = processRepository.GetByKey(message.Input.Id);
                if (entity == null)
                {
                    throw new NotExistException();
                }
                var bkState = _set._dic[entity.Id];

                entity.Update(message.Input);

                var newState = new ProcessDescriptor(host, ProcessState.Create(entity), entity.Id);
                bool stateChanged = newState != bkState;
                lock (_set._locker)
                {
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                }
                try
                {
                    processRepository.Update(entity);
                    processRepository.Context.Commit();
                }
                catch
                {
                    if (stateChanged)
                    {
                        Update(bkState);
                    }
                    processRepository.Context.Rollback();
                    throw;
                }
                if (stateChanged)
                {
                    _set._host.PublishEvent(new ProcessUpdatedEvent(message.UserSession, entity));
                    _set._host.CommitEventBus();
                }
            }

            private void Update(ProcessDescriptor state)
            {
                _set._dic[state.Process.Id] = state;
            }

            public void Handle(RemoveProcessCommand message)
            {
                // TODO:处理RemoveProcessCommand命令
            }

            public void Handle(ChangeProcessCatalogCommand message)
            {
                // TODO:处理ChangeProcessCatalogCommand命令
            }

            public void Handle(ChangeProcessNetPortCommand message)
            {
                // TODO:处理ChangeProcessNetPortCommand命令
            }
        }
        #endregion
    }
}