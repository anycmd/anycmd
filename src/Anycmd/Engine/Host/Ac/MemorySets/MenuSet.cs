
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

    internal sealed class MenuSet : IMenuSet, IMemorySet
    {
        public static readonly IMenuSet Empty = new MenuSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<Guid, MenuState> _menuById = new Dictionary<Guid, MenuState>();
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;

        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        internal MenuSet(IAcDomain host)
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

        public bool TryGetMenu(Guid menuId, out MenuState menu)
        {
            if (!_initialized)
            {
                Init();
            }
            return _menuById.TryGetValue(menuId, out menu);
        }

        public IEnumerator<MenuState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _menuById.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _menuById.Values.GetEnumerator();
        }

        private void Init()
        {
            if (_initialized) return;
            lock (this)
            {
                if (_initialized) return;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _menuById.Clear();
                var menus = _host.RetrieveRequiredService<IOriginalHostStateReader>().GetAllMenus().OrderBy(a => a.SortCode);
                foreach (var menu in menus)
                {
                    _menuById.Add(menu.Id, MenuState.Create(_host, menu));
                }
                _initialized = true;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        #region MessageHandler
        private class MessageHandler :
            IHandler<MenuUpdatedEvent>, 
            IHandler<AddMenuCommand>, 
            IHandler<MenuAddedEvent>, 
            IHandler<UpdateMenuCommand>, 
            IHandler<RemoveMenuCommand>, 
            IHandler<MenuRemovedEvent>
        {
            private readonly MenuSet _set;

            internal MessageHandler(MenuSet set)
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
                messageDispatcher.Register((IHandler<AddMenuCommand>)this);
                messageDispatcher.Register((IHandler<MenuAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateMenuCommand>)this);
                messageDispatcher.Register((IHandler<MenuUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveMenuCommand>)this);
                messageDispatcher.Register((IHandler<MenuRemovedEvent>)this);
            }

            public void Handle(AddMenuCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(MenuAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateMenuAddedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, IMenuCreateIo input, bool isCommand)
            {
                var host = _set._host;
                var menuById = _set._menuById;
                var menuRepository = host.RetrieveRequiredService<IRepository<Menu>>();
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                MenuState menu;
                if (host.MenuSet.TryGetMenu(input.Id.Value, out menu))
                {
                    throw new ValidationException("给定标识的实体已经存在" + input.Id);
                }
                if (input.ParentId.HasValue)
                {
                    MenuState parentMenu;
                    if (!host.MenuSet.TryGetMenu(input.ParentId.Value, out parentMenu))
                    {
                        throw new NotExistException("标识为" + input.ParentId.Value + "的父菜单不存在");
                    }
                    if (input.AppSystemId != parentMenu.AppSystemId)
                    {
                        throw new ValidationException("非法的数据，子菜单的应用系统必须和父菜单一致");
                    }
                }

                var entity = Menu.Create(input);

                lock (this)
                {

                    if (host.MenuSet.TryGetMenu(input.Id.Value, out menu))
                    {
                        throw new ValidationException("给定标识的实体已经存在" + input.Id);
                    }
                    if (input.ParentId.HasValue)
                    {
                        MenuState parentMenu;
                        if (!host.MenuSet.TryGetMenu(input.ParentId.Value, out parentMenu))
                        {
                            throw new NotExistException("标识为" + input.ParentId.Value + "的父菜单不存在");
                        }
                    }
                    var menuState = MenuState.Create(host, entity);
                    if (!menuById.ContainsKey(entity.Id))
                    {
                        menuById.Add(entity.Id, menuState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            menuRepository.Add(entity);
                            menuRepository.Context.Commit();
                        }
                        catch
                        {
                            if (menuById.ContainsKey(entity.Id))
                            {
                                menuById.Remove(entity.Id);
                            }
                            menuRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateMenuAddedEvent(acSession, entity, input));
                }
            }

            private class PrivateMenuAddedEvent : MenuAddedEvent
            {
                internal PrivateMenuAddedEvent(IAcSession acSession, MenuBase source, IMenuCreateIo input)
                    : base(acSession, source, input)
                {

                }
            }
            public void Handle(UpdateMenuCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(MenuUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateMenuUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Input, false);
            }

            private void Handle(IAcSession acSession, IMenuUpdateIo input, bool isCommand)
            {
                var host = _set._host;
                var menuById = _set._menuById;
                var menuRepository = host.RetrieveRequiredService<IRepository<Menu>>();
                MenuState bkState;
                if (!host.MenuSet.TryGetMenu(input.Id, out bkState))
                {
                    throw new ValidationException("给定标识的菜单不存在" + input.Id);
                }
                Menu entity;
                var stateChanged = false;
                lock (bkState)
                {
                    MenuState oldState;
                    if (!host.MenuSet.TryGetMenu(input.Id, out oldState))
                    {
                        throw new ValidationException("给定标识的菜单不存在" + input.Id);
                    }
                    entity = menuRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }

                    entity.Update(input);

                    var newState = MenuState.Create(host, entity);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            menuRepository.Update(entity);
                            menuRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            menuRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateMenuUpdatedEvent(acSession, entity, input));
                }
            }

            private class PrivateMenuUpdatedEvent : MenuUpdatedEvent
            {
                internal PrivateMenuUpdatedEvent(IAcSession acSession, MenuBase source, IMenuUpdateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            private void Update(MenuState state)
            {
                var menuById = _set._menuById;
                menuById[state.Id] = state;
            }

            public void Handle(RemoveMenuCommand message)
            {
                this.Handle(message.AcSession, message.EntityId, true);
            }

            public void Handle(MenuRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateMenuRemovedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Source.Id, false);
            }

            private void Handle(IAcSession acSession, Guid menuId, bool isCommand)
            {
                var host = _set._host;
                var menuById = _set._menuById;
                var menuRepository = host.RetrieveRequiredService<IRepository<Menu>>();
                MenuState bkState;
                if (!host.MenuSet.TryGetMenu(menuId, out bkState))
                {
                    return;
                }
                Menu entity;
                lock (bkState)
                {
                    MenuState state;
                    if (!host.MenuSet.TryGetMenu(menuId, out state))
                    {
                        return;
                    }
                    entity = menuRepository.GetByKey(menuId);
                    if (entity == null)
                    {
                        return;
                    }
                    if (menuRepository.AsQueryable().Any(a => a.ParentId.HasValue && a.ParentId.Value == entity.Id))
                    {
                        throw new ValidationException("不能删除父级菜单");
                    }
                    if (menuById.ContainsKey(bkState.Id))
                    {
                        if (isCommand)
                        {
                            host.MessageDispatcher.DispatchMessage(new MenuRemovingEvent(acSession, entity));
                        }
                        menuById.Remove(bkState.Id);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            menuRepository.Remove(entity);
                            menuRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!menuById.ContainsKey(bkState.Id))
                            {
                                menuById.Add(bkState.Id, bkState);
                            }
                            menuRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateMenuRemovedEvent(acSession, entity));
                }
            }

            private class PrivateMenuRemovedEvent : MenuRemovedEvent
            {
                internal PrivateMenuRemovedEvent(IAcSession acSession, MenuBase source)
                    : base(acSession, source)
                {

                }
            }
        }
        #endregion
    }
}