
namespace Anycmd.Engine.Host.Impl
{
    using Ac;
    using Ac.Rbac;
    using Dapper;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages;
    using Engine.Ac.Messages.Identity;
    using Engine.Ac.Messages.Rbac;
    using Exceptions;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Util;

    public class DefaultRbacService : IRbacService
    {
        private readonly IAcDomain _host;

        public DefaultRbacService(IAcDomain host)
        {
            this._host = host;
        }

        public void AddUser(IAccountCreateIo input)
        {
            _host.Handle(new AddAccountCommand(input));
        }

        public void DeleteUser(Guid accountId)
        {
            _host.Handle(new RemoveAccountCommand(accountId));
        }

        public void AddRole(IRoleCreateIo input)
        {
            _host.Handle(new AddRoleCommand(input));
        }

        public void DeleteRole(Guid roleId)
        {
            _host.Handle(new RemoveRoleCommand(roleId));
        }

        public void AssignUser(Guid accountId, Guid roleId)
        {
            _host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = Guid.NewGuid(),
                SubjectType = UserAcSubjectType.Account.ToName(),
                SubjectInstanceId = accountId,
                ObjectType = AcElementType.Role.ToName(),
                ObjectInstanceId = roleId,
                AcContent = null,
                AcContentType = null
            }));
        }

        public void DeassignUser(Guid accountId, Guid roleId)
        {
            var repository = _host.RetrieveRequiredService<IRepository<Privilege>>();
            var subjectType = UserAcSubjectType.Account.ToName();
            var objectType = AcElementType.Role.ToName();
            var entity = repository.AsQueryable().FirstOrDefault(a => 
                a.SubjectType == subjectType && a.SubjectInstanceId == accountId 
                && a.ObjectType == objectType && a.ObjectInstanceId == roleId);
            if (entity == null)
            {
                return;
            }
            _host.Handle(new RemovePrivilegeCommand(entity.Id));
        }

        public void GrantPermission(Guid functionId, Guid roleId)
        {
            _host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = Guid.NewGuid(),
                SubjectType = UserAcSubjectType.Role.ToName(),
                SubjectInstanceId = roleId,
                ObjectType = AcElementType.Function.ToName(),
                ObjectInstanceId = functionId,
                AcContent = null,
                AcContentType = null
            }));
        }

        public void RevokePermission(Guid functionId, Guid roleId)
        {
            var repository = _host.RetrieveRequiredService<IRepository<Privilege>>();
            var subjectType = UserAcSubjectType.Role.ToName();
            var objectType = AcElementType.Function.ToName();
            var entity = repository.AsQueryable().FirstOrDefault(a => 
                a.SubjectType == subjectType && a.SubjectInstanceId == roleId 
                && a.ObjectType == objectType && a.ObjectInstanceId == functionId);
            if (entity == null)
            {
                return;
            }
            _host.Handle(new RemovePrivilegeCommand(entity.Id));
        }

        public IUserSession CreateSession(Guid sessionId, AccountState account)
        {
            var sessionService = _host.RetrieveRequiredService<IUserSessionService>();
            return sessionService.CreateSession(_host, sessionId, account);
        }

        public void DeleteSession(Guid sessionId)
        {
            var sessionService = _host.RetrieveRequiredService<IUserSessionService>();
            sessionService.DeleteSession(_host, sessionId);
        }

        public IReadOnlyCollection<RoleState> SessionRoles(IUserSession userSession)
        {
            var result = new List<RoleState>();
            result.AddRange(userSession.AccountPrivilege.AuthorizedRoles);

            return result;
        }

        public IReadOnlyCollection<FunctionState> SessionPermissions(IUserSession userSession)
        {
            var result = new List<FunctionState>();
            result.AddRange(userSession.AccountPrivilege.AuthorizedFunctions);

            return result;
        }

        public void AddActiveRole(Guid roleId, IUserSession userSession)
        {
            RoleState role;
            if (!_host.RoleSet.TryGetRole(roleId, out role))
            {
                throw new ValidationException("给定标识的角色不存在" + roleId);
            }
            IUserSession session = userSession;
            if (session == null)
            {
                throw new ValidationException("给定标识的会话不存在");
            }
            session.AccountPrivilege.AddActiveRole(role);
        }

        public void DropActiveRole(IUserSession userSession, Guid roleId)
        {
            RoleState role;
            if (!_host.RoleSet.TryGetRole(roleId, out role))
            {
                throw new ValidationException("给定标识的角色不存在" + roleId);
            }
            IUserSession session = userSession;
            if (session == null)
            {
                throw new ValidationException("给定标识的会话不存在");
            }
            session.AccountPrivilege.DropActiveRole(role);
        }

        public bool CheckAccess(IUserSession userSession, Guid functionId, IManagedObject obj)
        {
            var securityService = _host.RetrieveRequiredService<ISecurityService>();
            FunctionState function;
            if (!_host.FunctionSet.TryGetFunction(functionId, out function))
            {
                throw new ValidationException("给定标识的功能不存在" + functionId);
            }
            IUserSession session = userSession;
            if (session == null)
            {
                throw new ValidationException("给定标识的会话不存在");
            }
            return securityService.Permit(session, function, obj);
        }

        public virtual IReadOnlyCollection<AccountState> AssignedUsers(Guid roleId)
        {
            // TODO:移除数据访问代码
            var sql =
@"SELECT  a.Id ,
            a.NumberId,
            a.LoginName ,
            a.CreateOn ,
            a.Name ,
            a.Code ,
            a.Email,
            a.QQ,
            a.Mobile
    FROM    dbo.Account AS a
            JOIN dbo.Privilege AS ar ON a.Id = ar.SubjectInstanceId
                                              AND ar.SubjectType = 'Account'
                                              AND ar.ObjectType = 'Role' AND ar.ObjectInstanceId='" + roleId + @"'
    WHERE   a.DeletionStateCode = 0";
            EntityTypeState entityType;
            if (!_host.EntityTypeSet.TryGetEntityType(new Coder("Ac", "Account"), out entityType))
            {
                throw new AnycmdException("意外的实体类型码Ac.Account");
            }
            Anycmd.Rdb.RdbDescriptor db;
            if (!_host.Rdbs.TryDb(entityType.DatabaseId, out db))
            {
                throw new AnycmdException("意外的账户数据库标识" + entityType.DatabaseId);
            }
            using (var conn = db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<AccountState>(sql).ToList();
            }
        }

        public IReadOnlyCollection<RoleState> AssignedRoles(Guid accountId)
        {
            var repository = _host.RetrieveRequiredService<IRepository<Privilege>>();
            var subjectType = UserAcSubjectType.Account.ToName();
            var objectType = AcElementType.Role.ToName();
            var privileges = repository.AsQueryable().Where(a => a.SubjectType == subjectType && a.SubjectInstanceId == accountId && a.ObjectType == objectType);
            var list = new List<RoleState>();
            foreach (var item in privileges)
            {
                RoleState role;
                if (_host.RoleSet.TryGetRole(item.ObjectInstanceId, out role))
                {
                    list.Add(role);
                }
            }
            return list;
        }

        public virtual IReadOnlyCollection<AccountState> AuthorizedUsers(Guid roleId)
        {
            RoleState role;
            if (!_host.RoleSet.TryGetRole(roleId, out role))
            {
                throw new ValidationException("给定标识的角色不存在" + roleId);
            }
            var roles = new HashSet<RoleState> { role };
            foreach (var item in _host.RoleSet.GetAscendantRoles(role))
            {
                roles.Add(item);
            }
            var sb = new StringBuilder();
            foreach (var item in roles)
            {
                if (sb.Length != 0)
                {
                    sb.Append(",");
                }
                sb.Append("'").Append(item.Id.ToString()).Append("'");
            }
            // TODO:移除数据访问代码
            var sql =
@"SELECT  a.Id ,
            a.NumberId,
            a.LoginName ,
            a.CreateOn ,
            a.Name ,
            a.Code ,
            a.Email,
            a.QQ,
            a.Mobile
    FROM    dbo.Account AS a
            JOIN dbo.Privilege AS ar ON a.Id = ar.SubjectInstanceId
                                              AND ar.SubjectType = 'Account'
                                              AND ar.ObjectType = 'Role' AND ar.ObjectInstanceId IN (" + sb.ToString() + @")
    WHERE   a.DeletionStateCode = 0";
            EntityTypeState entityType;
            if (!_host.EntityTypeSet.TryGetEntityType(new Coder("Ac", "Account"), out entityType))
            {
                throw new AnycmdException("意外的实体类型码Ac.Account");
            }
            Anycmd.Rdb.RdbDescriptor db;
            if (!_host.Rdbs.TryDb(entityType.DatabaseId, out db))
            {
                throw new AnycmdException("意外的账户数据库标识" + entityType.DatabaseId);
            }
            using (var conn = db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<AccountState>(sql).ToList();
            }
        }

        public IReadOnlyCollection<RoleState> AuthorizedRoles(IUserSession userSession)
        {
            AccountPrivilege accountPrivilege = userSession.AccountPrivilege;
            return accountPrivilege.AuthorizedRoles;
        }

        public IReadOnlyCollection<FunctionState> RolePermissions(Guid roleId)
        {
            RoleState role;
            if (!_host.RoleSet.TryGetRole(roleId, out role))
            {
                throw new ValidationException("给定标识的角色不存在" + roleId);
            }
            var functions = new HashSet<FunctionState>();
            foreach (var privilege in _host.PrivilegeSet.Where(a =>
                a.SubjectType == AcElementType.Role && a.SubjectInstanceId == roleId && a.ObjectType == AcElementType.Function))
            {
                FunctionState f;
                if (_host.FunctionSet.TryGetFunction(privilege.ObjectInstanceId, out f))
                {
                    functions.Add(f);
                }
            }
            foreach (var privilege in _host.RoleSet.GetDescendantRoles(role)
                .SelectMany(item => 
                    _host.PrivilegeSet.Where(a =>
                        a.SubjectType == AcElementType.Role 
                        && a.SubjectInstanceId == item.Id 
                        && a.ObjectType == AcElementType.Function)))
            {
                FunctionState f;
                if (_host.FunctionSet.TryGetFunction(privilege.ObjectInstanceId, out f))
                {
                    functions.Add(f);
                }
            }
            return functions.ToList();
        }

        public IReadOnlyCollection<FunctionState> UserPermissions(IUserSession userSession)
        {
            AccountPrivilege accountPrivilege = userSession.AccountPrivilege;
            return accountPrivilege.AuthorizedFunctions;
        }

        public IReadOnlyCollection<FunctionState> RoleOperationsOnObject(Guid roleId, IManagedObject obj)
        {
            RoleState role;
            if (!_host.RoleSet.TryGetRole(roleId, out role))
            {
                throw new ValidationException("给定标识的角色不存在" + roleId);
            }
            var functions = new HashSet<FunctionState>();
            foreach (var item in _host.RoleSet.GetDescendantRoles(role))
            {
                foreach (var privilege in _host.PrivilegeSet.Where(a =>
                    a.SubjectType == AcElementType.Role && a.SubjectInstanceId == roleId && a.ObjectType == AcElementType.Function))
                {
                    FunctionState f;
                    if (_host.FunctionSet.TryGetFunction(privilege.ObjectInstanceId, out f))
                    {
                        functions.Add(f);
                    }
                }
            }
            // TODO:执行实体级策略筛选返回的功能列表
            return functions.ToList();
        }

        public IReadOnlyCollection<FunctionState> UserOperationsOnObject(IUserSession userSession, IManagedObject obj)
        {
            AccountPrivilege accountPrivilege = userSession.AccountPrivilege;
            var functions = new HashSet<FunctionState>();
            foreach (var f in accountPrivilege.AuthorizedFunctions)
            {
                functions.Add(f);
            }
            // TODO:执行实体级策略筛选返回的功能列表
            return functions.ToList();
        }

        public void AddInheritance(Guid subjectRoleId, Guid objectRoleId)
        {
            _host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = Guid.NewGuid(),
                SubjectType = UserAcSubjectType.Role.ToName(),
                SubjectInstanceId = subjectRoleId,
                ObjectType = AcElementType.Role.ToName(),
                ObjectInstanceId = objectRoleId,
                AcContent = null,
                AcContentType = null
            }));
        }

        public void DeleteInheritance(Guid subjectRoleId, Guid objectRoleId)
        {
            var repository = _host.RetrieveRequiredService<IRepository<Privilege>>();
            var subjectType = UserAcSubjectType.Role.ToName();
            var objectType = AcElementType.Role.ToName();
            var entity = repository.AsQueryable().FirstOrDefault(a => 
                a.SubjectType == subjectType && a.SubjectInstanceId == subjectRoleId 
                && a.ObjectType == objectType && a.ObjectInstanceId == objectRoleId);
            if (entity == null)
            {
                return;
            }
            _host.Handle(new RemovePrivilegeCommand(entity.Id));
        }

        public void AddAscendant(Guid childRoleId, IRoleCreateIo parentRoleCreateInput)
        {
            _host.Handle(new AddRoleCommand(parentRoleCreateInput));
            Debug.Assert(parentRoleCreateInput.Id != null, "parentRoleCreateInput.Id != null");
            _host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = Guid.NewGuid(),
                SubjectType = UserAcSubjectType.Role.ToName(),
                SubjectInstanceId = childRoleId,
                ObjectType = AcElementType.Role.ToName(),
                ObjectInstanceId = parentRoleCreateInput.Id.Value,
                AcContent = null,
                AcContentType = null
            }));
        }

        public void AddDescendant(Guid parentRoleId, IRoleCreateIo childRoleCreateInput)
        {
            _host.Handle(new AddRoleCommand(childRoleCreateInput));
            Debug.Assert(childRoleCreateInput.Id != null, "childRoleCreateInput.Id != null");
            _host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = Guid.NewGuid(),
                SubjectType = UserAcSubjectType.Role.ToName(),
                SubjectInstanceId = childRoleCreateInput.Id.Value,
                ObjectType = AcElementType.Role.ToName(),
                ObjectInstanceId = parentRoleId,
                AcContent = null,
                AcContentType = null
            }));
        }

        public void CreateSsdSet(ISsdSetCreateIo input)
        {
            _host.Handle(new AddSsdSetCommand(input));
        }
         
        public void DeleteSsdSet(Guid ssdSetId)
        {
            _host.Handle(new RemoveSsdSetCommand(ssdSetId));
        }

        public void AddSsdRoleMember(Guid ssdSetId, Guid roleId)
        {
            _host.Handle(new AddSsdRoleCommand(new SsdRoleCreateIo
            {
                Id = Guid.NewGuid(),
                RoleId = roleId,
                SsdSetId = ssdSetId
            }));
        }

        public void DeleteSsdRoleMember(Guid ssdRoleId)
        {
            _host.Handle(new RemoveSsdRoleCommand(ssdRoleId));
        }

        public void DeleteSsdRoleMember(Guid ssdSetId, Guid roleId)
        {
            var ssdRoleRepository = _host.RetrieveRequiredService<IRepository<SsdRole>>();
            var entity = ssdRoleRepository.AsQueryable().FirstOrDefault(a => a.SsdSetId == ssdSetId && a.RoleId == roleId);
            if (entity == null)
            {
                return;
            }
            _host.Handle(new RemoveSsdRoleCommand(entity.Id));
        }

        public void SetSsdCardinality(Guid ssdSetId, int cardinality)
        {
            SsdSetState state;
            if (!_host.SsdSetSet.TryGetSsdSet(ssdSetId, out state))
            {
                throw new ValidationException("意外的静态责任分离角色集标识" + ssdSetId);
            }
            _host.Handle(new UpdateSsdSetCommand(new SsdSetUpdateIo
            {
                Id = state.Id,
                Description = state.Description,
                IsEnabled = state.IsEnabled,
                Name = state.Name,
                SsdCard = cardinality
            }));
        }

        public IReadOnlyCollection<SsdRoleState> SsdRoleSets()
        {
            return _host.SsdSetSet.GetSsdRoles();
        }

        public IReadOnlyCollection<RoleState> SsdRoleSetRoles(Guid ssdSetId)
        {
            SsdSetState ssdSet;
            if (!_host.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSet))
            {
                throw new ValidationException("给定标识的静态责任分离角色集不存在" + ssdSetId);
            }
            var result = new List<RoleState>();
            foreach (var item in _host.SsdSetSet.GetSsdRoles(ssdSet))
            {
                RoleState role;
                if (_host.RoleSet.TryGetRole(item.RoleId, out role))
                {
                    result.Add(role);
                }
            }

            return result;
        }

        public int SsdRoleSetCardinality(Guid ssdSetId)
        {
            SsdSetState ssdSet;
            if (!_host.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSet))
            {
                throw new ValidationException("意外的静态责任分离角色集标识" + ssdSetId);
            }
            return ssdSet.SsdCard;
        }

        public void CreateDsdSet(IDsdSetCreateIo input)
        {
            _host.Handle(new AddDsdSetCommand(input));
        }

        public void DeleteDsdSet(Guid dsdSetId)
        {
            _host.Handle(new RemoveDsdSetCommand(dsdSetId));
        }

        public void AddDsdRoleMember(Guid dsdSetId, Guid roleId)
        {
            _host.Handle(new AddDsdRoleCommand(new DsdRoleCreateIo
            {
                Id = Guid.NewGuid(),
                RoleId = roleId,
                DsdSetId = dsdSetId
            }));
        }

        public void DeleteDsdRoleMember(Guid dsdRoleId)
        {
            _host.Handle(new RemoveDsdRoleCommand(dsdRoleId));
        }

        public void DeleteDsdRoleMember(Guid dsdRoleId, Guid roleId)
        {
            var dsdRoleRepository = _host.RetrieveRequiredService<IRepository<DsdRole>>();
            var entity = dsdRoleRepository.AsQueryable().FirstOrDefault(a => a.DsdSetId == dsdRoleId && a.RoleId == roleId);
            if (entity == null)
            {
                return;
            }
            _host.Handle(new RemoveDsdRoleCommand(entity.Id));
        }

        public void SetDsdCardinality(Guid dsdSetId, int cardinality)
        {
            DsdSetState state;
            if (!_host.DsdSetSet.TryGetDsdSet(dsdSetId, out state))
            {
                throw new ValidationException("意外的动态责任分离角色集标识" + dsdSetId);
            }
            _host.Handle(new UpdateDsdSetCommand(new DsdSetUpdateIo
            {
                Id = state.Id,
                Description = state.Description,
                IsEnabled = state.IsEnabled,
                Name = state.Name,
                DsdCard = cardinality
            }));
        }

        public IReadOnlyCollection<DsdRoleState> DsdRoleSets()
        {
            return _host.DsdSetSet.GetDsdRoles();
        }

        public IReadOnlyCollection<RoleState> DsdRoleSetRoles(Guid dsdSetId)
        {
            DsdSetState dsdSet;
            if (!_host.DsdSetSet.TryGetDsdSet(dsdSetId, out dsdSet))
            {
                throw new ValidationException("给定标识的动态责任分离角色集不存在" + dsdSetId);
            }
            var result = new List<RoleState>();
            foreach (var item in _host.DsdSetSet.GetDsdRoles(dsdSet))
            {
                RoleState role;
                if (_host.RoleSet.TryGetRole(item.RoleId, out role))
                {
                    result.Add(role);
                }
            }

            return result;
        }

        public int DsdRoleSetCardinality(Guid dsdSetId)
        {
            DsdSetState dsdSet;
            if (!_host.DsdSetSet.TryGetDsdSet(dsdSetId, out dsdSet))
            {
                throw new ValidationException("意外的动态责任分离角色集标识" + dsdSetId);
            }
            return dsdSet.DsdCard;
        }
    }
}
