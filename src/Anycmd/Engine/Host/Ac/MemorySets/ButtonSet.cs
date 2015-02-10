
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Bus;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Abstractions.Infra;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Infra;
    using Exceptions;
    using Host;
    using Infra;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Util;

    internal sealed class ButtonSet : IButtonSet, IMemorySet
    {
        public static readonly IButtonSet Empty = new ButtonSet(EmptyAcDomain.SingleInstance);
        private static readonly object Locker = new object();

        private readonly Dictionary<Guid, ButtonState> _dicById = new Dictionary<Guid, ButtonState>();
        private readonly Dictionary<string, ButtonState> _dicByCode = new Dictionary<string, ButtonState>(StringComparer.OrdinalIgnoreCase);
        private bool _initialized;
        private readonly IAcDomain _acDomain;

        private readonly Guid _id = Guid.NewGuid();

        public Guid Id
        {
            get { return _id; }
        }

        internal ButtonSet(IAcDomain acDomain)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            if (acDomain.Equals(EmptyAcDomain.SingleInstance))
            {
                _initialized = true;
            }
            _acDomain = acDomain;
            new MessageHandler(this).Register();
        }

        public bool ContainsButton(Guid buttonId)
        {
            if (!_initialized)
            {
                Init();
            }
            if (buttonId == Guid.Empty)
            {
                throw new ArgumentException("传入的buttonId不应为Guid.Empty。");
            }

            return _dicById.ContainsKey(buttonId);
        }

        public bool ContainsButton(string buttonCode)
        {
            if (!_initialized)
            {
                Init();
            }
            if (string.IsNullOrEmpty(buttonCode))
            {
                throw new ArgumentNullException("buttonCode");
            }

            return _dicByCode.ContainsKey(buttonCode);
        }

        public bool TryGetButton(Guid buttonId, out ButtonState button)
        {
            if (!_initialized)
            {
                Init();
            }
            if (buttonId == Guid.Empty)
            {
                throw new ArgumentException("传入的buttonId不应为Guid.Empty。");
            }

            return _dicById.TryGetValue(buttonId, out button);
        }

        public bool TryGetButton(string buttonCode, out ButtonState button)
        {
            if (!_initialized)
            {
                Init();
            }
            if (string.IsNullOrEmpty(buttonCode))
            {
                throw new ArgumentNullException("buttonCode");
            }

            return _dicByCode.TryGetValue(buttonCode, out button);
        }

        internal void Refresh()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }

        private void Init()
        {
            if (_initialized) return;
            lock (Locker)
            {
                if (_initialized) return;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _dicById.Clear();
                _dicByCode.Clear();
                var buttons = _acDomain.RetrieveRequiredService<IOriginalHostStateReader>().GetAllButtons().ToList();
                foreach (var button in buttons)
                {
                    if (_dicById.ContainsKey(button.Id))
                    {
                        throw new AnycmdException("意外重复的按钮标识");
                    }
                    if (_dicByCode.ContainsKey(button.Code))
                    {
                        throw new AnycmdException("意外重复的按钮编码");
                    }
                    var buttonState = ButtonState.Create(button);
                    _dicById.Add(button.Id, buttonState);
                    _dicByCode.Add(button.Code, buttonState);
                }
                _initialized = true;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        public IEnumerator<ButtonState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.Values.GetEnumerator();
        }

        #region MessageHandler
        private class MessageHandler :
            IHandler<UpdateButtonCommand>,
            IHandler<AddButtonCommand>,
            IHandler<ButtonAddedEvent>,
            IHandler<ButtonUpdatedEvent>,
            IHandler<RemoveButtonCommand>,
            IHandler<ButtonRemovedEvent>
        {
            private static readonly object MessageLocker = new object();
            private readonly ButtonSet _set;

            internal MessageHandler(ButtonSet set)
            {
                _set = set;
            }

            public void Register()
            {
                var messageDispatcher = _set._acDomain.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("AcDomain对象'{0}'尚未设置MessageDispatcher。".Fmt(_set._acDomain.Name));
                }
                messageDispatcher.Register((IHandler<AddButtonCommand>)this);
                messageDispatcher.Register((IHandler<ButtonAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateButtonCommand>)this);
                messageDispatcher.Register((IHandler<ButtonUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveButtonCommand>)this);
                messageDispatcher.Register((IHandler<ButtonRemovedEvent>)this);
            }

            public void Handle(AddButtonCommand message)
            {
                Handle(message.AcSession, message.Input, isCommand: true);
            }

            public void Handle(ButtonAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateButtonAddedEvent))
                {
                    return;
                }
                Handle(message.AcSession, message.Output, isCommand: false);
            }

            private void Handle(IAcSession acSession, IButtonCreateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var dicById = _set._dicById;
                var dicByCode = _set._dicByCode;
                var buttonRepository = acDomain.RetrieveRequiredService<IRepository<Button>>();
                if (!input.Id.HasValue)
                {
                    throw new AnycmdException("标识是必须的");
                }
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                Button entity;
                lock (MessageLocker)
                {
                    if (acDomain.ButtonSet.ContainsButton(input.Id.Value))
                    {
                        throw new AnycmdException("给定标识的记录已经存在" + input.Id);
                    }
                    if (acDomain.ButtonSet.ContainsButton(input.Code))
                    {
                        throw new ValidationException("重复的按钮编码");
                    }

                    entity = Button.Create(input);

                    var buttonState = ButtonState.Create(entity);
                    if (!dicById.ContainsKey(entity.Id))
                    {
                        dicById.Add(entity.Id, buttonState);
                    }
                    if (!dicByCode.ContainsKey(entity.Code))
                    {
                        dicByCode.Add(entity.Code, buttonState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            buttonRepository.Add(entity);
                            buttonRepository.Context.Commit();
                        }
                        catch
                        {
                            if (dicById.ContainsKey(entity.Id))
                            {
                                dicById.Remove(entity.Id);
                            }
                            if (dicByCode.ContainsKey(entity.Code))
                            {
                                dicByCode.Remove(entity.Code);
                            }
                            buttonRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateButtonAddedEvent(acSession, entity, input));
                }
            }

            private class PrivateButtonAddedEvent : ButtonAddedEvent, IPrivateEvent
            {
                internal PrivateButtonAddedEvent(IAcSession acSession, ButtonBase source, IButtonCreateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(UpdateButtonCommand message)
            {
                Handle(message.AcSession, message.Input, isCommand: true);
            }

            public void Handle(ButtonUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateButtonUpdatedEvent))
                {
                    return;
                }
                Handle(message.AcSession, message.Input, isCommand: false);
            }

            private void Handle(IAcSession acSession, IButtonUpdateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var buttonRepository = acDomain.RetrieveRequiredService<IRepository<Button>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                ButtonState bkState;
                if (!acDomain.ButtonSet.TryGetButton(input.Id, out bkState))
                {
                    throw new NotExistException("意外的按钮标识" + input.Id);
                }
                Button entity;
                var stateChanged = false;
                lock (MessageLocker)
                {
                    ButtonState oldState;
                    if (!acDomain.ButtonSet.TryGetButton(input.Id, out oldState))
                    {
                        throw new NotExistException("意外的按钮标识" + input.Id);
                    }
                    ButtonState button;
                    if (acDomain.ButtonSet.TryGetButton(input.Code, out button) && button.Id != input.Id)
                    {
                        throw new ValidationException("重复的按钮编码");
                    }
                    entity = buttonRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }

                    entity.Update(input);

                    var newState = ButtonState.Create(entity);
                    stateChanged = bkState != newState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            buttonRepository.Update(entity);
                            buttonRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            buttonRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateButtonUpdatedEvent(acSession, entity, input));
                }
            }

            private void Update(ButtonState state)
            {
                var dicById = _set._dicById;
                var dicByCode = _set._dicByCode;
                var oldKey = dicById[state.Id].Code;
                var newKey = state.Code;
                dicById[state.Id] = state;
                // 如果按钮编码改变了
                if (!dicByCode.ContainsKey(newKey))
                {
                    dicByCode.Remove(oldKey);
                    dicByCode.Add(newKey, state);
                }
                else
                {
                    dicByCode[newKey] = state;
                }
            }

            private class PrivateButtonUpdatedEvent : ButtonUpdatedEvent, IPrivateEvent
            {
                internal PrivateButtonUpdatedEvent(IAcSession acSession, ButtonBase source, IButtonUpdateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(RemoveButtonCommand message)
            {
                Handle(message.AcSession, message.EntityId, isCommand: true);
            }

            public void Handle(ButtonRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateButtonRemovedEvent))
                {
                    return;
                }
                Handle(message.AcSession, message.Source.Id, isCommand: false);
            }

            private void Handle(IAcSession acSession, Guid buttonId, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var dicById = _set._dicById;
                var dicByCode = _set._dicByCode;
                var buttonRepository = acDomain.RetrieveRequiredService<IRepository<Button>>();
                ButtonState bkState;
                if (!acDomain.ButtonSet.TryGetButton(buttonId, out bkState))
                {
                    return;
                }
                if (acDomain.UiViewSet.GetUiViewButtons().Any(a => a.ButtonId == buttonId))
                {
                    throw new ValidationException("按钮关联界面视图后不能删除");
                }
                Button entity;
                lock (MessageLocker)
                {
                    ButtonState state;
                    if (!acDomain.ButtonSet.TryGetButton(buttonId, out state))
                    {
                        return;
                    }
                    entity = buttonRepository.GetByKey(buttonId);
                    if (entity == null)
                    {
                        return;
                    }
                    if (dicById.ContainsKey(bkState.Id))
                    {
                        if (isCommand)
                        {
                            acDomain.MessageDispatcher.DispatchMessage(new ButtonRemovingEvent(acSession, entity));
                        }
                        dicById.Remove(bkState.Id);
                        if (dicByCode.ContainsKey(bkState.Code))
                        {
                            dicByCode.Remove(bkState.Code);
                        }
                    }
                    if (isCommand)
                    {
                        try
                        {
                            buttonRepository.Remove(entity);
                            buttonRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!dicById.ContainsKey(bkState.Id))
                            {
                                dicById.Add(bkState.Id, bkState);
                            }
                            if (!dicByCode.ContainsKey(bkState.Code))
                            {
                                dicByCode.Add(bkState.Code, bkState);
                            }
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateButtonRemovedEvent(acSession, entity));
                }
            }

            private class PrivateButtonRemovedEvent : ButtonRemovedEvent, IPrivateEvent
            {
                internal PrivateButtonRemovedEvent(IAcSession acSession, ButtonBase source)
                    : base(acSession, source)
                {

                }
            }
        }
        #endregion
    }
}
