
namespace Anycmd.Engine.Ac
{
    using Abstractions;
    using Host.Ac;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Util;

    /// <summary>
    /// 表示账户权限类型。账户权限是主体为账户客体为其它Ac元素的权限记录。
    /// </summary>
    public sealed class AccountPrivilege
    {
        private bool _initialized = false;
        private readonly IUserSession _userSession;
        private readonly IAcDomain _acDomain;
        private HashSet<Guid> _authorizedRoleIds;
        private List<RoleState> _authorizedRoles;
        private HashSet<MenuState> _authorizedMenus;
        private HashSet<Guid> _authorizedFunctionIds;
        private List<FunctionState> _authorizedFunctions;
        private List<PrivilegeState> _accountPrivileges;

        private HashSet<OrganizationState> _organizations;
        private HashSet<RoleState> _roles;
        private HashSet<GroupState> _groups;
        private HashSet<FunctionState> _functions;
        private HashSet<MenuState> _menus;
        private HashSet<AppSystemState> _appSystems;

        public AccountPrivilege(IAcDomain acDomain, IUserSession userSession)
        {
            this._acDomain = acDomain;
            this._userSession = userSession;
        }

        public HashSet<OrganizationState> Organizations
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                return _organizations;
            }
        }

        public HashSet<RoleState> Roles
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                return _roles;
            }
        }

        public HashSet<GroupState> Groups
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                return _groups;
            }
        }

        public HashSet<FunctionState> Functions
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                return _functions;
            }
        }

        public HashSet<MenuState> Menus
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                return _menus;
            }
        }

        public HashSet<AppSystemState> AppSystems
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                return _appSystems;
            }
        }

        private IEnumerable<PrivilegeState> AccountPrivileges
        {
            get
            {
                if (_accountPrivileges == null)
                {
                    if (_userSession.Account.Id == Guid.Empty)
                    {
                        return new List<PrivilegeState>();
                    }
                    _accountPrivileges = GetAccountPrivileges();
                }
                return _accountPrivileges;
            }
        }

        private List<PrivilegeState> GetAccountPrivileges()
        {
            var subjectType = UserAcSubjectType.Account.ToName();
            var accountPrivileges = _acDomain.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable()
                .Where(a => a.SubjectType == subjectType && a.SubjectInstanceId == _userSession.Account.Id).ToList().Select(PrivilegeState.Create).ToList();
            return accountPrivileges;
        }

        #region AddActiveRole
        /// <summary>
        /// 添加激活角色。
        /// </summary>
        /// <param name="role"></param>
        public void AddActiveRole(RoleState role)
        {
            if (!_initialized)
            {
                Init();
            }
            if (this.Roles != null)
            {
                this.Roles.Add(role);
            }
            if (this._authorizedRoles == null) return;
            this.AuthorizedRoleIds.Add(role.Id);
            this._authorizedRoles.Add(role);
        }
        #endregion

        #region DropActiveRole
        /// <summary>
        /// 删除激活角色。
        /// </summary>
        /// <param name="role"></param>
        public void DropActiveRole(RoleState role)
        {
            if (!_initialized)
            {
                Init();
            }
            if (this.Roles != null)
            {
                this.Roles.Remove(role);
            }
            if (this._authorizedRoles == null) return;
            this.AuthorizedRoleIds.Remove(role.Id);
            this._authorizedRoles.Remove(role);
        }
        #endregion

        /// <summary>
        /// 账户的角色授权
        /// 这些角色是以下角色集合的并集：
        /// 1，当前账户直接得到的角色；
        /// 2，当前账户所在的工作组的角色；
        /// 3，当前账户所在的组织结构的角色；
        /// 4，当前账户所在的组织结构加入的工作组的角色。
        /// </summary>
        /// <returns></returns>
        public HashSet<Guid> AuthorizedRoleIds
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                if (_authorizedRoleIds != null) return _authorizedRoleIds;
                _authorizedRoleIds = new HashSet<Guid>();
                foreach (var role in this.Roles)
                {
                    _authorizedRoleIds.Add(role.Id);
                }
                foreach (var organization in this.Organizations)
                {
                    var organization1 = organization;
                    foreach (var item in _acDomain.PrivilegeSet.Where(a =>
                        a.SubjectType == AcElementType.Organization && a.SubjectInstanceId == organization1.Id))
                    {
                        switch (item.ObjectType)
                        {
                            case AcElementType.Role:
                                {
                                    RoleState role;
                                    if (_acDomain.RoleSet.TryGetRole(item.ObjectInstanceId, out role))
                                    {
                                        _authorizedRoleIds.Add(role.Id);
                                    }
                                }
                                break;
                            case AcElementType.Group:
                                var item1 = item;
                                foreach (var roleGroup in _acDomain.PrivilegeSet.Where(a =>
                                    a.AcRecordType == AcRecordType.RoleGroup && a.ObjectInstanceId == item1.ObjectInstanceId))
                                {
                                    RoleState role;
                                    if (_acDomain.RoleSet.TryGetRole(roleGroup.SubjectInstanceId, out role))
                                    {
                                        _authorizedRoleIds.Add(role.Id);
                                    }
                                }
                                break;
                        }
                    }
                }
                foreach (var group in this.Groups)
                {
                    var g = group;
                    foreach (var roleGroup in _acDomain.PrivilegeSet.Where(a =>
                        a.AcRecordType == AcRecordType.RoleGroup && a.ObjectInstanceId == g.Id))
                    {
                        RoleState role;
                        if (_acDomain.RoleSet.TryGetRole(roleGroup.SubjectInstanceId, out role))
                        {
                            _authorizedRoleIds.Add(role.Id);
                        }
                    }
                }

                return _authorizedRoleIds;
            }
        }

        public IReadOnlyCollection<RoleState> AuthorizedRoles
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                if (_authorizedRoles != null) return _authorizedRoles;
                _authorizedRoles = new List<RoleState>();
                foreach (var roleId in this.AuthorizedRoleIds)
                {
                    RoleState role;
                    if (this._acDomain.RoleSet.TryGetRole(roleId, out role))
                    {
                        _authorizedRoles.Add(role);
                    }
                }
                return _authorizedRoles;
            }
        }

        public HashSet<MenuState> AuthorizedMenus
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                if (_authorizedMenus != null) return _authorizedMenus;
                _authorizedMenus = new HashSet<MenuState>();
                var menuList = new List<MenuState>();
                if (_userSession.IsDeveloper())
                {
                    menuList.AddRange(_acDomain.MenuSet);
                }
                else
                {
                    var roleIDs = new HashSet<Guid>();
                    foreach (var roleId in this.AuthorizedRoleIds)
                    {
                        roleIDs.Add(roleId);
                    }
                    foreach (var roleMenu in _acDomain.PrivilegeSet.Where(a =>
                        a.AcRecordType == AcRecordType.RoleMenu && roleIDs.Contains(a.SubjectInstanceId)))
                    {
                        MenuState menu;
                        if (_acDomain.MenuSet.TryGetMenu(roleMenu.ObjectInstanceId, out menu))
                        {
                            menuList.Add(menu);
                        }
                    }
                    menuList.AddRange(this.Menus);
                }
                foreach (var menu in menuList.OrderBy(a => a.SortCode))
                {
                    _authorizedMenus.Add(menu);
                }
                return _authorizedMenus;
            }
        }

        public HashSet<Guid> AuthorizedFunctionIDs
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                if (_authorizedFunctionIds == null)
                {
                    _authorizedFunctionIds = new HashSet<Guid>();
                    // TODO:考虑在PrivilegeSet集合中计算好缓存起来，从而可以直接根据角色索引而
                    var roleIDs = this.AuthorizedRoleIds;
                    foreach (var privilegeBigram in _acDomain.PrivilegeSet.Where(a =>
                        a.AcRecordType == AcRecordType.RoleFunction && roleIDs.Contains(a.SubjectInstanceId)))
                    {
                        _authorizedFunctionIds.Add(privilegeBigram.ObjectInstanceId);
                    }
                    // 追加账户所在组织结构的直接功能授权
                    foreach (var organization in this.Organizations)
                    {
                        var organization1 = organization;
                        foreach (var item in _acDomain.PrivilegeSet.Where(a =>
                            a.AcRecordType == AcRecordType.OrganizationFunction && a.SubjectInstanceId == organization1.Id))
                        {
                            var functionId = item.ObjectInstanceId;
                            _authorizedFunctionIds.Add(functionId);
                        }
                    }
                    // 追加账户的直接功能授权
                    foreach (var fun in this.Functions)
                    {
                        _authorizedFunctionIds.Add(fun.Id);
                    }
                }

                return _authorizedFunctionIds;
            }
        }

        public IReadOnlyCollection<FunctionState> AuthorizedFunctions
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                if (_authorizedFunctions != null) return _authorizedFunctions;
                _authorizedFunctions = new List<FunctionState>();
                foreach (var functionId in this.AuthorizedFunctionIDs)
                {
                    FunctionState function;
                    if (_acDomain.FunctionSet.TryGetFunction(functionId, out function))
                    {
                        _authorizedFunctions.Add(function);
                    }
                }
                return _authorizedFunctions;
            }
        }

        private void Init()
        {
            if (_initialized) return;
            var organizations = new HashSet<OrganizationState>();
            var roles = new HashSet<RoleState>();
            var groups = new HashSet<GroupState>();
            var functions = new HashSet<FunctionState>();
            var menus = new HashSet<MenuState>();
            var appSystems = new HashSet<AppSystemState>();
            foreach (var accountPrivilege in this.AccountPrivileges)
            {
                switch (accountPrivilege.ObjectType)
                {
                    case AcElementType.Undefined:
                        break;
                    case AcElementType.Account:
                        break;
                    case AcElementType.Organization:
                        {
                            var organizationId = accountPrivilege.ObjectInstanceId;
                            OrganizationState organization;
                            if (_acDomain.OrganizationSet.TryGetOrganization(organizationId, out organization))
                            {
                                organizations.Add(organization);
                            }
                            break;
                        }
                    case AcElementType.Role:
                        {
                            var roleId = accountPrivilege.ObjectInstanceId;
                            RoleState role;
                            if (_acDomain.RoleSet.TryGetRole(roleId, out role))
                            {
                                roles.Add(role);
                            }
                            break;
                        }
                    case AcElementType.Group:
                        {
                            var groupId = accountPrivilege.ObjectInstanceId;
                            GroupState group;
                            if (_acDomain.GroupSet.TryGetGroup(groupId, out @group))
                            {
                                groups.Add(@group);
                            }
                            break;
                        }
                    case AcElementType.Function:
                        {
                            var functionId = accountPrivilege.ObjectInstanceId;
                            FunctionState function;
                            if (_acDomain.FunctionSet.TryGetFunction(functionId, out function))
                            {
                                functions.Add(function);
                            }
                            break;
                        }
                    case AcElementType.Menu:
                        {
                            var menuId = accountPrivilege.ObjectInstanceId;
                            MenuState menu;
                            if (_acDomain.MenuSet.TryGetMenu(menuId, out menu))
                            {
                                menus.Add(menu);
                            }
                            break;
                        }
                    case AcElementType.AppSystem:
                        {
                            var appSystemId = accountPrivilege.ObjectInstanceId;
                            AppSystemState appSystem;
                            if (_acDomain.AppSystemSet.TryGetAppSystem(appSystemId, out appSystem))
                            {
                                appSystems.Add(appSystem);
                            }
                            break;
                        }
                    case AcElementType.ResourceType:
                        break;
                    case AcElementType.Privilege:
                        break;
                    default:
                        break;
                }
            }
            this._organizations = organizations;
            this._roles = roles;
            this._groups = groups;
            this._functions = functions;
            this._menus = menus;
            this._appSystems = appSystems;
            _initialized = true;
        }
    }
}
