
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Bus;
    using Engine.Ac;
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

    internal sealed class UiViewSet : IUiViewSet, IMemorySet
    {
        public static readonly IUiViewSet Empty = new UiViewSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<FunctionState, UiViewState> _viewDicByFunction = new Dictionary<FunctionState, UiViewState>();
        private readonly Dictionary<Guid, UiViewState> _viewDicById = new Dictionary<Guid, UiViewState>();
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;
        private readonly UiViewButtonSet _viewButtonSet;

        public Guid Id
        {
            get { return _id; }
        }

        internal UiViewSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
            _viewButtonSet = new UiViewButtonSet(host);
            new MessageHandler(this).Register();
        }

        public bool TryGetUiView(FunctionState function, out UiViewState view)
        {
            if (!_initialized)
            {
                Init();
            }

            return _viewDicByFunction.TryGetValue(function, out view);
        }

        public bool TryGetUiView(Guid viewId, out UiViewState view)
        {
            if (!_initialized)
            {
                Init();
            }
            return _viewDicById.TryGetValue(viewId, out view);
        }

        public IReadOnlyList<UiViewButtonState> GetUiViewButtons(UiViewState view)
        {
            if (!_initialized)
            {
                Init();
            }
            return _viewButtonSet.GetUiViewButtons(view);
        }

        public IEnumerable<UiViewButtonState> GetUiViewButtons()
        {
            if (!_initialized)
            {
                Init();
            }
            return _viewButtonSet;
        }

        internal void Refresh()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }

        public IEnumerator<UiViewState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _viewDicById.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _viewDicById.Values.GetEnumerator();
        }

        private void Init()
        {
            if (_initialized) return;
            lock (this)
            {
                if (_initialized) return;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _viewDicByFunction.Clear();
                _viewDicById.Clear();
                var views = _host.RetrieveRequiredService<IOriginalHostStateReader>().GetAllUiViews();
                foreach (var view in views)
                {
                    var viewState = UiViewState.Create(_host, view);
                    _viewDicById.Add(view.Id, viewState);
                    FunctionState function;
                    if (!_host.FunctionSet.TryGetFunction(view.Id, out function))
                    {
                        throw new NotExistException("意外的功能标识" + view.Id);
                    }
                    if (!_viewDicByFunction.ContainsKey(function))
                    {
                        _viewDicByFunction.Add(function, viewState);
                    }
                }
                _initialized = true;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        #region MessageHandler
        private class MessageHandler:
            IHandler<AddUiViewCommand>,
            IHandler<UiViewUpdatedEvent>,
            IHandler<RemoveUiViewCommand>,
            IHandler<FunctionUpdatedEvent>,
            IHandler<FunctionRemovingEvent>,
            IHandler<FunctionRemovedEvent>, 
            IHandler<UiViewAddedEvent>, 
            IHandler<UpdateUiViewCommand>, 
            IHandler<UiViewRemovedEvent>
        {
            private readonly UiViewSet _set;

            internal MessageHandler(UiViewSet set)
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
                messageDispatcher.Register((IHandler<AddUiViewCommand>)this);
                messageDispatcher.Register((IHandler<UiViewAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateUiViewCommand>)this);
                messageDispatcher.Register((IHandler<UiViewUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveUiViewCommand>)this);
                messageDispatcher.Register((IHandler<UiViewRemovedEvent>)this);
                messageDispatcher.Register((IHandler<FunctionUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<FunctionRemovingEvent>)this);
                messageDispatcher.Register((IHandler<FunctionRemovedEvent>)this);
            }

            public void Handle(FunctionUpdatedEvent message)
            {
                var host = _set._host;
                var viewDicByFunction = _set._viewDicByFunction;
                FunctionState newKey;
                if (!host.FunctionSet.TryGetFunction(message.Source.Id, out newKey))
                {
                    throw new AnycmdException("意外的功能标识" + message.Source.Id);
                }
                var oldKey = viewDicByFunction.Keys.FirstOrDefault(a => a.Id == newKey.Id);
                if (oldKey != null && !viewDicByFunction.ContainsKey(newKey))
                {
                    viewDicByFunction.Add(newKey, viewDicByFunction[oldKey]);
                    viewDicByFunction.Remove(oldKey);
                }
            }

            public void Handle(FunctionRemovingEvent message)
            {
                var host = _set._host;
                var viewDicById = _set._viewDicById;
                if (viewDicById.ContainsKey(message.Source.Id))
                {
                    host.Handle(new RemoveUiViewCommand(message.Source.Id));
                }
            }

            public void Handle(FunctionRemovedEvent message)
            {
                var viewDicByFunction = _set._viewDicByFunction;
                var key = viewDicByFunction.Keys.FirstOrDefault(a => a.Id == message.Source.Id);
                if (key != null)
                {
                    viewDicByFunction.Remove(key);
                }
            }

            public void Handle(AddUiViewCommand message)
            {
                this.Handle(message.Input, true);
            }

            public void Handle(UiViewAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateUiViewAddedEvent))
                {
                    return;
                }
                this.Handle(message.Output, false);
            }

            private void Handle(IUiViewCreateIo input, bool isCommand)
            {
                var host = _set._host;
                var viewDicByFunction = _set._viewDicByFunction;
                var viewDicById = _set._viewDicById;
                var viewRepository = host.RetrieveRequiredService<IRepository<UiView>>();
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                UiView entity;
                lock (this)
                {
                    FunctionState function;
                    if (!host.FunctionSet.TryGetFunction(input.Id.Value, out function))
                    {
                        throw new ValidationException("意外的功能标识，界面视图首先是个功能。请先添加界面视图对应的功能记录。");
                    }
                    UiViewState view;
                    if (host.UiViewSet.TryGetUiView(input.Id.Value, out view))
                    {
                        throw new ValidationException("给定标识的界面视图已经存在");
                    }

                    entity = UiView.Create(input);

                    var state = UiViewState.Create(host, entity);
                    if (!viewDicById.ContainsKey(state.Id))
                    {
                        viewDicById.Add(state.Id, state);
                    }
                    if (!viewDicByFunction.ContainsKey(function))
                    {
                        viewDicByFunction.Add(function, state);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            viewRepository.Add(entity);
                            viewRepository.Context.Commit();
                        }
                        catch
                        {
                            if (viewDicById.ContainsKey(entity.Id))
                            {
                                viewDicById.Remove(entity.Id);
                            }
                            if (viewDicByFunction.ContainsKey(function))
                            {
                                viewDicByFunction.Remove(function);
                            }
                            viewRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateUiViewAddedEvent(entity, input));
                }
            }

            private class PrivateUiViewAddedEvent : UiViewAddedEvent
            {
                internal PrivateUiViewAddedEvent(UiViewBase source, IUiViewCreateIo input)
                    : base(source, input)
                {

                }
            }
            public void Handle(UpdateUiViewCommand message)
            {
                this.Handle(message.Output, true);
            }

            public void Handle(UiViewUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateUiViewUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.Input, false);
            }

            private void Handle(IUiViewUpdateIo input, bool isCommand)
            {
                var host = _set._host;
                var viewDicByFunction = _set._viewDicByFunction;
                var viewDicById = _set._viewDicById;
                var viewRepository = host.RetrieveRequiredService<IRepository<UiView>>();
                UiViewState bkState;
                if (!host.UiViewSet.TryGetUiView(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                UiView entity;
                var stateChanged = false;
                lock (bkState)
                {
                    UiViewState state;
                    if (!host.UiViewSet.TryGetUiView(input.Id, out state))
                    {
                        throw new NotExistException();
                    }
                    entity = viewRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }

                    entity.Update(input);

                    var newState = UiViewState.Create(host, entity);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            viewRepository.Update(entity);
                            viewRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            viewRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateUiViewUpdatedEvent(entity, input));
                }
            }

            private void Update(UiViewState state)
            {
                var host = _set._host;
                var viewDicByFunction = _set._viewDicByFunction;
                var viewDicById = _set._viewDicById;
                FunctionState function;
                host.FunctionSet.TryGetFunction(state.Id, out function);
                viewDicById[state.Id] = state;
                viewDicByFunction[function] = state;
            }

            private class PrivateUiViewUpdatedEvent : UiViewUpdatedEvent
            {
                internal PrivateUiViewUpdatedEvent(UiViewBase source, IUiViewUpdateIo input)
                    : base(source, input)
                {

                }
            }
            public void Handle(RemoveUiViewCommand message)
            {
                this.Handle(message.EntityId, true);
            }

            public void Handle(UiViewRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateUiViewRemovedEvent))
                {
                    return;
                }
                this.Handle(message.Source.Id, false);
            }

            private void Handle(Guid viewId, bool isCommand)
            {
                var host = _set._host;
                var viewDicByFunction = _set._viewDicByFunction;
                var viewDicById = _set._viewDicById;
                var viewRepository = host.RetrieveRequiredService<IRepository<UiView>>();
                var viewButtonRepository = host.RetrieveRequiredService<IRepository<UiViewButton>>();
                UiViewState bkState;
                if (!host.UiViewSet.TryGetUiView(viewId, out bkState))
                {
                    return;
                }
                UiView entity;
                lock (bkState)
                {
                    UiViewState state;
                    if (!host.UiViewSet.TryGetUiView(viewId, out state))
                    {
                        return;
                    }
                    FunctionState function;
                    if (!host.FunctionSet.TryGetFunction(viewId, out function))
                    {
                        throw new NotExistException("意外的功能标识" + viewId);
                    }
                    entity = viewRepository.GetByKey(viewId);
                    if (entity == null)
                    {
                        return;
                    }
                    foreach (var viewButton in viewButtonRepository.AsQueryable().Where(a => a.UiViewId == entity.Id).ToList())
                    {
                        viewButtonRepository.Remove(viewButton);
                    }
                    if (viewDicById.ContainsKey(bkState.Id))
                    {
                        if (isCommand)
                        {
                            host.MessageDispatcher.DispatchMessage(new UiViewRemovingEvent(entity));
                        }
                        viewDicById.Remove(bkState.Id);
                    }
                    if (viewDicByFunction.ContainsKey(function))
                    {
                        viewDicByFunction.Remove(function);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            viewButtonRepository.Context.Commit();
                            viewRepository.Remove(entity);
                            viewRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!viewDicById.ContainsKey(bkState.Id))
                            {
                                viewDicById.Add(bkState.Id, bkState);
                            }
                            if (!viewDicByFunction.ContainsKey(function))
                            {
                                viewDicByFunction.Add(function, bkState);
                            }
                            viewButtonRepository.Context.Rollback();
                            viewRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateUiViewRemovedEvent(entity));
                }
            }

            private class PrivateUiViewRemovedEvent : UiViewRemovedEvent
            {
                internal PrivateUiViewRemovedEvent(UiViewBase source)
                    : base(source)
                {

                }
            }
        }
        #endregion

        // 内部类
        #region UIViewButtonSet
        /// <summary>
        /// 界面视图菜单上下文
        /// </summary>
        private sealed class UiViewButtonSet : IEnumerable<UiViewButtonState>
        {
            private readonly Dictionary<UiViewState, List<UiViewButtonState>> _viewButtonsByUiView = new Dictionary<UiViewState, List<UiViewButtonState>>();
            private readonly Dictionary<Guid, UiViewButtonState> _viewButtonDicById = new Dictionary<Guid, UiViewButtonState>();
            private bool _initialized = false;
            private readonly IAcDomain _host;

            internal UiViewButtonSet(IAcDomain host)
            {
                this._host = host;
                new UiViewButtonMessageHandler(this).Register();
            }

            public IReadOnlyList<UiViewButtonState> GetUiViewButtons(UiViewState view)
            {
                if (!_initialized)
                {
                    Init();
                }
                if (!_viewButtonsByUiView.ContainsKey(view))
                {
                    return new List<UiViewButtonState>();
                }

                return _viewButtonsByUiView[view];
            }

            public IEnumerator<UiViewButtonState> GetEnumerator()
            {
                if (!_initialized)
                {
                    Init();
                }
                return ((IEnumerable<UiViewButtonState>) _viewButtonDicById.Values).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                if (!_initialized)
                {
                    Init();
                }
                return _viewButtonDicById.Values.GetEnumerator();
            }

            private void Init()
            {
                if (_initialized) return;
                lock (this)
                {
                    if (_initialized) return;
                    _viewButtonsByUiView.Clear();
                    _viewButtonDicById.Clear();
                    var allUiViewButtons = _host.RetrieveRequiredService<IOriginalHostStateReader>().GetAllUiViewButtons();
                    foreach (var viewButton in allUiViewButtons)
                    {
                        var viewButtonState = UiViewButtonState.Create(_host, viewButton);
                        if (!_viewButtonDicById.ContainsKey(viewButton.Id))
                        {
                            _viewButtonDicById.Add(viewButton.Id, viewButtonState);
                        }
                        if (!_viewButtonsByUiView.ContainsKey(viewButtonState.UiView))
                        {
                            _viewButtonsByUiView.Add(viewButtonState.UiView, new List<UiViewButtonState>());
                        }
                        _viewButtonsByUiView[viewButtonState.UiView].Add(viewButtonState);
                    }
                    foreach (var item in _viewButtonsByUiView)
                    {
                        item.Value.Sort(new UiViewButtonCompare());
                    }
                    _initialized = true;
                }
            }

            #region UiViewButtonMessageHandler
            private class UiViewButtonMessageHandler:
                IHandler<UiViewButtonAddedEvent>,
                IHandler<UiViewButtonUpdatedEvent>,
                IHandler<RemoveUiViewButtonCommand>,
                IHandler<UiViewButtonRemovedEvent>,
                IHandler<UiViewUpdatedEvent>,
                IHandler<FunctionRemovingEvent>,
                IHandler<AddUiViewButtonCommand>,
                IHandler<UpdateUiViewButtonCommand>,
                IHandler<UiViewRemovingEvent>,
                IHandler<UiViewRemovedEvent>
            {
                private readonly UiViewButtonSet _set;

                internal UiViewButtonMessageHandler(UiViewButtonSet set)
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
                    messageDispatcher.Register((IHandler<AddUiViewButtonCommand>)this);
                    messageDispatcher.Register((IHandler<UiViewButtonAddedEvent>)this);
                    messageDispatcher.Register((IHandler<UpdateUiViewButtonCommand>)this);
                    messageDispatcher.Register((IHandler<UiViewButtonUpdatedEvent>)this);
                    messageDispatcher.Register((IHandler<RemoveUiViewButtonCommand>)this);
                    messageDispatcher.Register((IHandler<UiViewButtonRemovedEvent>)this);
                    messageDispatcher.Register((IHandler<UiViewUpdatedEvent>)this);
                    messageDispatcher.Register((IHandler<UiViewRemovingEvent>)this);
                    messageDispatcher.Register((IHandler<UiViewRemovedEvent>)this);
                    messageDispatcher.Register((IHandler<FunctionRemovingEvent>)this);
                }

                public void Handle(UiViewUpdatedEvent message)
                {
                    var host = _set._host;
                    var viewButtonsByUiView = _set._viewButtonsByUiView;
                    UiViewState newKey;
                    if (!host.UiViewSet.TryGetUiView(message.Source.Id, out newKey))
                    {
                        throw new AnycmdException("意外的界面视图标识" + message.Source.Id);
                    }
                    var oldKey = viewButtonsByUiView.Keys.FirstOrDefault(a => a.Id == newKey.Id);
                    if (oldKey != null && !viewButtonsByUiView.ContainsKey(newKey))
                    {
                        viewButtonsByUiView.Add(newKey, viewButtonsByUiView[oldKey]);
                        viewButtonsByUiView.Remove(oldKey);
                    }
                }

                public void Handle(UiViewRemovingEvent message)
                {
                    var host = _set._host;
                    var viewButtonsByUiView = _set._viewButtonsByUiView;
                    UiViewState key;
                    if (!host.UiViewSet.TryGetUiView(message.Source.Id, out key))
                    {
                        throw new AnycmdException("意外的界面视图标识" + message.Source.Id);
                    }
                    if (viewButtonsByUiView.ContainsKey(key))
                    {
                        var viewButtonIds = new HashSet<Guid>();
                        foreach (var item in viewButtonsByUiView[key])
                        {
                            viewButtonIds.Add(item.Id);
                        }
                        foreach (var viewButtonId in viewButtonIds)
                        {
                            host.Handle(new RemoveUiViewButtonCommand(viewButtonId));
                        }
                        viewButtonsByUiView.Remove(key);
                    }
                }

                public void Handle(UiViewRemovedEvent message)
                {
                    var viewButtonsByUiView = _set._viewButtonsByUiView;
                    var key = viewButtonsByUiView.Keys.FirstOrDefault(a => a.Id == message.Source.Id);
                    if (key != null)
                    {
                        viewButtonsByUiView.Remove(key);
                    }
                }

                public void Handle(FunctionRemovingEvent message)
                {
                    var host = _set._host;
                    var viewButtonDicById = _set._viewButtonDicById;
                    var viewButtonIds = new HashSet<Guid>();
                    foreach (var item in viewButtonDicById.Values)
                    {
                        if (item.FunctionId.HasValue && item.FunctionId.Value == message.Source.Id)
                        {
                            viewButtonIds.Add(item.Id);
                        }
                    }
                    foreach (var viewButtonId in viewButtonIds)
                    {
                        host.Handle(new RemoveUiViewButtonCommand(viewButtonId));
                    }
                }

                public void Handle(AddUiViewButtonCommand message)
                {
                    this.Handle(message.Input, true);
                }

                public void Handle(UiViewButtonAddedEvent message)
                {
                    if (message.GetType() == typeof(PrivateUiViewButtonAddedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Output, false);
                }

                private void Handle(IUiViewButtonCreateIo input, bool isCommand)
                {
                    var host = _set._host;
                    var viewButtonsByUiView = _set._viewButtonsByUiView;
                    var viewButtonDicById = _set._viewButtonDicById;
                    var viewButtonRepository = host.RetrieveRequiredService<IRepository<UiViewButton>>();
                    if (!input.Id.HasValue)
                    {
                        throw new ValidationException("标识是必须的");
                    }
                    UiViewButton entity;
                    lock (this)
                    {
                        ButtonState button;
                        if (!host.ButtonSet.TryGetButton(input.ButtonId, out button))
                        {
                            throw new ValidationException("按钮不存在");
                        }
                        UiViewState view;
                        if (!host.UiViewSet.TryGetUiView(input.UiViewId, out view))
                        {
                            throw new ValidationException("界面视图不存在");
                        }
                        if (input.FunctionId.HasValue)
                        {
                            FunctionState function;
                            if (!host.FunctionSet.TryGetFunction(input.FunctionId.Value, out function))
                            {
                                throw new ValidationException("托管功能不存在");
                            }
                        }
                        if (host.UiViewSet.GetUiViewButtons().Any(a => a.Id == input.Id.Value))
                        {
                            throw new ValidationException("给定标识的界面视图按钮已经存在");
                        }

                        entity = UiViewButton.Create(input);

                        var state = UiViewButtonState.Create(host, entity);
                        if (!viewButtonsByUiView.ContainsKey(view))
                        {
                            viewButtonsByUiView.Add(view, new List<UiViewButtonState>());
                        }
                        if (!viewButtonsByUiView[view].Contains(state))
                        {
                            viewButtonsByUiView[view].Add(state);
                        }
                        if (!viewButtonDicById.ContainsKey(state.Id))
                        {
                            viewButtonDicById.Add(state.Id, state);
                        }
                        if (isCommand)
                        {
                            try
                            {
                                viewButtonRepository.Add(entity);
                                viewButtonRepository.Context.Commit();
                            }
                            catch
                            {
                                if (viewButtonsByUiView.ContainsKey(view))
                                {
                                    if (viewButtonsByUiView[view].Any(a => a.Id == entity.Id))
                                    {
                                        var item = viewButtonsByUiView[view].First(a => a.Id == entity.Id);
                                        viewButtonsByUiView[view].Remove(item);
                                    }
                                }
                                if (viewButtonDicById.ContainsKey(entity.Id))
                                {
                                    viewButtonDicById.Remove(entity.Id);
                                }
                                viewButtonRepository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateUiViewButtonAddedEvent(entity, input));
                    }
                }

                private class PrivateUiViewButtonAddedEvent : UiViewButtonAddedEvent
                {
                    internal PrivateUiViewButtonAddedEvent(UiViewButtonBase source, IUiViewButtonCreateIo input) : base(source, input) { }
                }
                public void Handle(UpdateUiViewButtonCommand message)
                {
                    this.Handle(message.Output, true);
                }

                public void Handle(UiViewButtonUpdatedEvent message)
                {
                    if (message.GetType() == typeof(PrivateUiViewButtonUpdatedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Input, false);
                }

                private void Update(UiViewButtonState state)
                {
                    var host = _set._host;
                    var viewButtonsByUiView = _set._viewButtonsByUiView;
                    var viewButtonDicById = _set._viewButtonDicById;
                    var oldState = viewButtonDicById[state.Id];
                    viewButtonDicById[state.Id] = state;
                    foreach (var item in viewButtonsByUiView)
                    {
                        if (item.Value.Contains(oldState))
                        {
                            item.Value.Remove(oldState);
                            item.Value.Add(state);
                        }
                    }
                }

                private void Handle(IUiViewButtonUpdateIo input, bool isCommand)
                {
                    var host = _set._host;
                    var viewButtonRepository = host.RetrieveRequiredService<IRepository<UiViewButton>>();
                    var bkState = host.UiViewSet.GetUiViewButtons().FirstOrDefault(a => a.Id == input.Id);
                    if (bkState == null)
                    {
                        throw new NotExistException();
                    }
                    if (input.FunctionId.HasValue)
                    {
                        FunctionState function;
                        if (!host.FunctionSet.TryGetFunction(input.FunctionId.Value, out function))
                        {
                            throw new ValidationException("非法的托管功能标识" + input.FunctionId);
                        }
                    }
                    UiViewButton entity;
                    var stateChanged = false;
                    lock (bkState)
                    {
                        if (host.UiViewSet.GetUiViewButtons().All(a => a.Id != input.Id))
                        {
                            throw new NotExistException();
                        }
                        entity = viewButtonRepository.GetByKey(input.Id);
                        if (entity == null)
                        {
                            throw new NotExistException();
                        }

                        entity.Update(input);

                        var newState = UiViewButtonState.Create(host, entity);
                        stateChanged = newState != bkState;
                        if (stateChanged)
                        {
                            Update(newState);
                        }
                        if (isCommand)
                        {
                            try
                            {
                                viewButtonRepository.Update(entity);
                                viewButtonRepository.Context.Commit();
                            }
                            catch
                            {
                                if (stateChanged)
                                {
                                    Update(bkState);
                                }
                                viewButtonRepository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand && stateChanged)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateUiViewButtonUpdatedEvent(entity, input));
                    }
                }

                private class PrivateUiViewButtonUpdatedEvent : UiViewButtonUpdatedEvent
                {
                    internal PrivateUiViewButtonUpdatedEvent(UiViewButtonBase source, IUiViewButtonUpdateIo input)
                        : base(source, input)
                    {

                    }
                }
                public void Handle(RemoveUiViewButtonCommand message)
                {
                    this.Handle(message.EntityId, true);
                }

                public void Handle(UiViewButtonRemovedEvent message)
                {
                    if (message.GetType() == typeof(PrivateUiViewButtonRemovedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Source.Id, false);
                }

                private void Handle(Guid viewButtonId, bool isCommand)
                {
                    var host = _set._host;
                    var viewButtonsByUiView = _set._viewButtonsByUiView;
                    var viewButtonDicById = _set._viewButtonDicById;
                    var viewButtonRepository = host.RetrieveRequiredService<IRepository<UiViewButton>>();
                    var bkState = host.UiViewSet.GetUiViewButtons().FirstOrDefault(a => a.Id == viewButtonId);
                    if (bkState == null)
                    {
                        return;
                    }
                    UiViewButton entity;
                    lock (bkState)
                    {
                        if (host.UiViewSet.GetUiViewButtons().All(a => a.Id != viewButtonId))
                        {
                            return;
                        }
                        entity = viewButtonRepository.GetByKey(viewButtonId);
                        if (entity == null)
                        {
                            return;
                        }
                        if (viewButtonDicById.ContainsKey(bkState.Id))
                        {
                            if (viewButtonsByUiView.ContainsKey(bkState.UiView) && viewButtonsByUiView[bkState.UiView].Any(a => a.Id == bkState.Id))
                            {
                                var item = viewButtonsByUiView[bkState.UiView].First(a => a.Id == bkState.Id);
                                viewButtonsByUiView[bkState.UiView].Remove(item);
                            }
                            viewButtonDicById.Remove(bkState.Id);
                        }
                        if (isCommand)
                        {
                            try
                            {
                                viewButtonRepository.Remove(entity);
                                viewButtonRepository.Context.Commit();
                            }
                            catch
                            {
                                if (!viewButtonDicById.ContainsKey(bkState.Id))
                                {
                                    if (!viewButtonsByUiView.ContainsKey(bkState.UiView))
                                    {
                                        viewButtonsByUiView.Add(bkState.UiView, new List<UiViewButtonState>());
                                    }
                                    if (viewButtonsByUiView[bkState.UiView].All(a => a.Id != bkState.Id))
                                    {
                                        viewButtonsByUiView[bkState.UiView].Add(bkState);
                                    }
                                    viewButtonDicById.Add(bkState.Id, bkState);
                                }
                                viewButtonRepository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateUiViewButtonRemovedEvent(entity));
                    }
                }

                private class PrivateUiViewButtonRemovedEvent : UiViewButtonRemovedEvent
                {
                    internal PrivateUiViewButtonRemovedEvent(UiViewButtonBase source)
                        : base(source)
                    {

                    }
                }
            }
            #endregion

            private class UiViewButtonCompare : IComparer<UiViewButtonState>
            {
                public int Compare(UiViewButtonState x, UiViewButtonState y)
                {
                    return x.Button.SortCode.CompareTo(y.Button.SortCode);
                }
            }
        }
        #endregion
    }
}