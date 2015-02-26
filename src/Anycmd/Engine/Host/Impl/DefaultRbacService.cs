
namespace Anycmd.Engine.Host.Impl
{
    using Ac;
    using Ac.Rbac;
    using Dapper;
    using Engine.Ac;
    using Engine.Ac.Privileges;
    using Engine.Ac.Accounts;
    using Engine.Ac.InOuts;
    using Engine.Ac.Roles;
    using Engine.Ac.Ssd;
    using Engine.Ac.Dsd;
    using Exceptions;
    using InOuts;
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
        private readonly IAcDomain _acDomain;

        public DefaultRbacService(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public void AddUser(IAcSession subject, IAccountCreateIo input)
        {
            _acDomain.Handle(new AddAccountCommand(subject, input));
        }

        public void DeleteUser(IAcSession subject, Guid accountId)
        {
            _acDomain.Handle(new RemoveAccountCommand(subject, accountId));
        }

        public void AddRole(IAcSession subject, IRoleCreateIo input)
        {
            _acDomain.Handle(new AddRoleCommand(subject, input));
        }

        public void DeleteRole(IAcSession subject, Guid roleId)
        {
            _acDomain.Handle(new RemoveRoleCommand(subject, roleId));
        }

        public void AssignUser(IAcSession subject, Guid accountId, Guid roleId)
        {
            _acDomain.Handle(new AddPrivilegeCommand(subject, new PrivilegeCreateIo
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

        public void DeassignUser(IAcSession subject, Guid accountId, Guid roleId)
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<Privilege>>();
            var subjectType = UserAcSubjectType.Account.ToName();
            var objectType = AcElementType.Role.ToName();
            var entity = repository.AsQueryable().FirstOrDefault(a =>
                a.SubjectType == subjectType && a.SubjectInstanceId == accountId
                && a.ObjectType == objectType && a.ObjectInstanceId == roleId);
            if (entity == null)
            {
                return;
            }
            _acDomain.Handle(new RemovePrivilegeCommand(subject, entity.Id));
        }

        public void GrantPermission(IAcSession subject, Guid functionId, Guid roleId)
        {
            _acDomain.Handle(new AddPrivilegeCommand(subject, new PrivilegeCreateIo
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

        public void RevokePermission(IAcSession subject, Guid functionId, Guid roleId)
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<Privilege>>();
            var subjectType = UserAcSubjectType.Role.ToName();
            var objectType = AcElementType.Function.ToName();
            var entity = repository.AsQueryable().FirstOrDefault(a =>
                a.SubjectType == subjectType && a.SubjectInstanceId == roleId
                && a.ObjectType == objectType && a.ObjectInstanceId == functionId);
            if (entity == null)
            {
                return;
            }
            _acDomain.Handle(new RemovePrivilegeCommand(subject, entity.Id));
        }

        public IAcSession CreateSession(IAcSession subject, Guid sessionId, AccountState account)
        {
            var identity = new AnycmdIdentity(account.LoginName);
            var acSessionEntity = new AcSession
            {
                Id = sessionId,
                AccountId = account.Id,
                AuthenticationType = identity.AuthenticationType,
                Description = null,
                IsAuthenticated = identity.IsAuthenticated,
                IsEnabled = 1,
                LoginName = account.LoginName
            };
            AcSessionState.AcMethod.AddAcSession(_acDomain, acSessionEntity);

            return new AcSessionState(_acDomain, sessionId, account);
        }

        public void DeleteSession(IAcSession subject, Guid sessionId)
        {
            AcSessionState.AcMethod.DeleteAcSession(_acDomain, sessionId);
        }

        public IReadOnlyCollection<RoleState> SessionRoles(IAcSession subject, IAcSession targetSession)
        {
            var result = new List<RoleState>();
            result.AddRange(targetSession.AccountPrivilege.AuthorizedRoles);

            return result;
        }

        public IReadOnlyCollection<FunctionState> SessionPermissions(IAcSession subject, IAcSession targetSession)
        {
            var result = new List<FunctionState>();
            result.AddRange(targetSession.AccountPrivilege.AuthorizedFunctions);

            return result;
        }

        public void AddActiveRole(IAcSession subject, IAcSession targetSession, Guid roleId)
        {
            RoleState role;
            if (!_acDomain.RoleSet.TryGetRole(roleId, out role))
            {
                throw new ValidationException("给定标识的角色不存在" + roleId);
            }
            IAcSession session = targetSession;
            if (session == null)
            {
                throw new ValidationException("给定标识的会话不存在");
            }
            session.AccountPrivilege.AddActiveRole(role);
        }

        public void DropActiveRole(IAcSession subject, IAcSession targetSession, Guid roleId)
        {
            RoleState role;
            if (!_acDomain.RoleSet.TryGetRole(roleId, out role))
            {
                throw new ValidationException("给定标识的角色不存在" + roleId);
            }
            IAcSession session = targetSession;
            if (session == null)
            {
                throw new ValidationException("给定标识的会话不存在");
            }
            session.AccountPrivilege.DropActiveRole(role);
        }

        public bool CheckAccess(IAcSession subject, IAcSession targetSession, Guid functionId, IManagedObject obj)
        {
            var securityService = _acDomain.RetrieveRequiredService<ISecurityService>();
            FunctionState function;
            if (!_acDomain.FunctionSet.TryGetFunction(functionId, out function))
            {
                throw new ValidationException("给定标识的功能不存在" + functionId);
            }
            IAcSession session = targetSession;
            if (session == null)
            {
                throw new ValidationException("给定标识的会话不存在");
            }
            return securityService.Permit(session, function, obj);
        }

        public virtual IReadOnlyCollection<AccountState> AssignedUsers(IAcSession subject, Guid roleId)
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
            if (!_acDomain.EntityTypeSet.TryGetEntityType(new Coder("Ac", "Account"), out entityType))
            {
                throw new AnycmdException("意外的实体类型码Ac.Account");
            }
            Engine.Rdb.RdbDescriptor db;
            if (!_acDomain.Rdbs.TryDb(entityType.DatabaseId, out db))
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

        public IReadOnlyCollection<RoleState> AssignedRoles(IAcSession subject, Guid accountId)
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<Privilege>>();
            var subjectType = UserAcSubjectType.Account.ToName();
            var objectType = AcElementType.Role.ToName();
            var privileges = repository.AsQueryable().Where(a => a.SubjectType == subjectType && a.SubjectInstanceId == accountId && a.ObjectType == objectType);
            var list = new List<RoleState>();
            foreach (var item in privileges)
            {
                RoleState role;
                if (_acDomain.RoleSet.TryGetRole(item.ObjectInstanceId, out role))
                {
                    list.Add(role);
                }
            }
            return list;
        }

        public virtual IReadOnlyCollection<AccountState> AuthorizedUsers(IAcSession subject, IAcSession targetSession, Guid roleId)
        {
            RoleState role;
            if (!_acDomain.RoleSet.TryGetRole(roleId, out role))
            {
                throw new ValidationException("给定标识的角色不存在" + roleId);
            }
            var roles = new HashSet<RoleState> { role };
            foreach (var item in _acDomain.RoleSet.GetAscendantRoles(role))
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
            if (!_acDomain.EntityTypeSet.TryGetEntityType(new Coder("Ac", "Account"), out entityType))
            {
                throw new AnycmdException("意外的实体类型码Ac.Account");
            }
            Engine.Rdb.RdbDescriptor db;
            if (!_acDomain.Rdbs.TryDb(entityType.DatabaseId, out db))
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

        public IReadOnlyCollection<RoleState> AuthorizedRoles(IAcSession subject, IAcSession targetSession)
        {
            AccountPrivilege accountPrivilege = targetSession.AccountPrivilege;
            return accountPrivilege.AuthorizedRoles;
        }

        public IReadOnlyCollection<FunctionState> RolePermissions(IAcSession subject, Guid roleId)
        {
            RoleState role;
            if (!_acDomain.RoleSet.TryGetRole(roleId, out role))
            {
                throw new ValidationException("给定标识的角色不存在" + roleId);
            }
            var functions = new HashSet<FunctionState>();
            foreach (var privilege in _acDomain.PrivilegeSet.Where(a =>
                a.SubjectType == AcElementType.Role && a.SubjectInstanceId == roleId && a.ObjectType == AcElementType.Function))
            {
                FunctionState f;
                if (_acDomain.FunctionSet.TryGetFunction(privilege.ObjectInstanceId, out f))
                {
                    functions.Add(f);
                }
            }
            foreach (var privilege in _acDomain.RoleSet.GetDescendantRoles(role)
                .SelectMany(item =>
                    _acDomain.PrivilegeSet.Where(a =>
                        a.SubjectType == AcElementType.Role
                        && a.SubjectInstanceId == item.Id
                        && a.ObjectType == AcElementType.Function)))
            {
                FunctionState f;
                if (_acDomain.FunctionSet.TryGetFunction(privilege.ObjectInstanceId, out f))
                {
                    functions.Add(f);
                }
            }
            return functions.ToList();
        }

        public IReadOnlyCollection<FunctionState> UserPermissions(IAcSession subject, IAcSession targetSession)
        {
            AccountPrivilege accountPrivilege = targetSession.AccountPrivilege;
            return accountPrivilege.AuthorizedFunctions;
        }

        public IReadOnlyCollection<FunctionState> RoleOperationsOnObject(IAcSession subject, IAcSession targetSession, Guid roleId, IManagedObject obj)
        {
            RoleState role;
            if (!_acDomain.RoleSet.TryGetRole(roleId, out role))
            {
                throw new ValidationException("给定标识的角色不存在" + roleId);
            }
            var functions = new HashSet<FunctionState>();
            foreach (var item in _acDomain.RoleSet.GetDescendantRoles(role))
            {
                foreach (var privilege in _acDomain.PrivilegeSet.Where(a =>
                    a.SubjectType == AcElementType.Role && a.SubjectInstanceId == roleId && a.ObjectType == AcElementType.Function))
                {
                    FunctionState f;
                    if (_acDomain.FunctionSet.TryGetFunction(privilege.ObjectInstanceId, out f))
                    {
                        functions.Add(f);
                    }
                }
            }
            // TODO:执行实体级策略筛选返回的功能列表
            return functions.ToList();
        }

        public IReadOnlyCollection<FunctionState> UserOperationsOnObject(IAcSession subject, IAcSession targetSession, IManagedObject obj)
        {
            AccountPrivilege accountPrivilege = targetSession.AccountPrivilege;
            var functions = new HashSet<FunctionState>();
            foreach (var f in accountPrivilege.AuthorizedFunctions)
            {
                functions.Add(f);
            }
            // TODO:执行实体级策略筛选返回的功能列表
            return functions.ToList();
        }

        public void AddInheritance(IAcSession subject, Guid subjectRoleId, Guid objectRoleId)
        {
            _acDomain.Handle(new AddPrivilegeCommand(subject, new PrivilegeCreateIo
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

        public void DeleteInheritance(IAcSession subject, Guid subjectRoleId, Guid objectRoleId)
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<Privilege>>();
            var subjectType = UserAcSubjectType.Role.ToName();
            var objectType = AcElementType.Role.ToName();
            var entity = repository.AsQueryable().FirstOrDefault(a =>
                a.SubjectType == subjectType && a.SubjectInstanceId == subjectRoleId
                && a.ObjectType == objectType && a.ObjectInstanceId == objectRoleId);
            if (entity == null)
            {
                return;
            }
            _acDomain.Handle(new RemovePrivilegeCommand(subject, entity.Id));
        }

        public void AddAscendant(IAcSession subject, Guid childRoleId, IRoleCreateIo parentRoleCreateInput)
        {
            _acDomain.Handle(new AddRoleCommand(subject, parentRoleCreateInput));
            Debug.Assert(parentRoleCreateInput.Id != null, "parentRoleCreateInput.Id != null");
            _acDomain.Handle(new AddPrivilegeCommand(subject, new PrivilegeCreateIo
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

        public void AddDescendant(IAcSession subject, Guid parentRoleId, IRoleCreateIo childRoleCreateInput)
        {
            _acDomain.Handle(new AddRoleCommand(subject, childRoleCreateInput));
            Debug.Assert(childRoleCreateInput.Id != null, "childRoleCreateInput.Id != null");
            _acDomain.Handle(new AddPrivilegeCommand(subject, new PrivilegeCreateIo
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

        public void CreateSsdSet(IAcSession subject, ISsdSetCreateIo input)
        {
            _acDomain.Handle(new AddSsdSetCommand(subject, input));
        }

        public void DeleteSsdSet(IAcSession subject, Guid ssdSetId)
        {
            _acDomain.Handle(new RemoveSsdSetCommand(subject, ssdSetId));
        }

        public void AddSsdRoleMember(IAcSession subject, Guid ssdSetId, Guid roleId)
        {
            _acDomain.Handle(new AddSsdRoleCommand(subject, new SsdRoleCreateIo
            {
                Id = Guid.NewGuid(),
                RoleId = roleId,
                SsdSetId = ssdSetId
            }));
        }

        public void DeleteSsdRoleMember(IAcSession subject, Guid ssdRoleId)
        {
            _acDomain.Handle(new RemoveSsdRoleCommand(subject, ssdRoleId));
        }

        public void DeleteSsdRoleMember(IAcSession subject, Guid ssdSetId, Guid roleId)
        {
            var ssdRoleRepository = _acDomain.RetrieveRequiredService<IRepository<SsdRole>>();
            var entity = ssdRoleRepository.AsQueryable().FirstOrDefault(a => a.SsdSetId == ssdSetId && a.RoleId == roleId);
            if (entity == null)
            {
                return;
            }
            _acDomain.Handle(new RemoveSsdRoleCommand(subject, entity.Id));
        }

        public void SetSsdCardinality(IAcSession subject, Guid ssdSetId, int cardinality)
        {
            SsdSetState state;
            if (!_acDomain.SsdSetSet.TryGetSsdSet(ssdSetId, out state))
            {
                throw new ValidationException("意外的静态责任分离角色集标识" + ssdSetId);
            }
            _acDomain.Handle(new UpdateSsdSetCommand(subject, new SsdSetUpdateIo
            {
                Id = state.Id,
                Description = state.Description,
                IsEnabled = state.IsEnabled,
                Name = state.Name,
                SsdCard = cardinality
            }));
        }

        public IReadOnlyCollection<SsdRoleState> SsdRoleSets(IAcSession subject)
        {
            return _acDomain.SsdSetSet.GetSsdRoles();
        }

        public IReadOnlyCollection<RoleState> SsdRoleSetRoles(IAcSession subject, Guid ssdSetId)
        {
            SsdSetState ssdSet;
            if (!_acDomain.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSet))
            {
                throw new ValidationException("给定标识的静态责任分离角色集不存在" + ssdSetId);
            }
            var result = new List<RoleState>();
            foreach (var item in _acDomain.SsdSetSet.GetSsdRoles(ssdSet))
            {
                RoleState role;
                if (_acDomain.RoleSet.TryGetRole(item.RoleId, out role))
                {
                    result.Add(role);
                }
            }

            return result;
        }

        public int SsdRoleSetCardinality(IAcSession subject, Guid ssdSetId)
        {
            SsdSetState ssdSet;
            if (!_acDomain.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSet))
            {
                throw new ValidationException("意外的静态责任分离角色集标识" + ssdSetId);
            }
            return ssdSet.SsdCard;
        }

        public void CreateDsdSet(IAcSession subject, IDsdSetCreateIo input)
        {
            _acDomain.Handle(new AddDsdSetCommand(subject, input));
        }

        public void DeleteDsdSet(IAcSession subject, Guid dsdSetId)
        {
            _acDomain.Handle(new RemoveDsdSetCommand(subject, dsdSetId));
        }

        public void AddDsdRoleMember(IAcSession subject, Guid dsdSetId, Guid roleId)
        {
            _acDomain.Handle(new AddDsdRoleCommand(subject, new DsdRoleCreateIo
            {
                Id = Guid.NewGuid(),
                RoleId = roleId,
                DsdSetId = dsdSetId
            }));
        }

        public void DeleteDsdRoleMember(IAcSession subject, Guid dsdRoleId)
        {
            _acDomain.Handle(new RemoveDsdRoleCommand(subject, dsdRoleId));
        }

        public void DeleteDsdRoleMember(IAcSession subject, Guid dsdRoleId, Guid roleId)
        {
            var dsdRoleRepository = _acDomain.RetrieveRequiredService<IRepository<DsdRole>>();
            var entity = dsdRoleRepository.AsQueryable().FirstOrDefault(a => a.DsdSetId == dsdRoleId && a.RoleId == roleId);
            if (entity == null)
            {
                return;
            }
            _acDomain.Handle(new RemoveDsdRoleCommand(subject, entity.Id));
        }

        public void SetDsdCardinality(IAcSession subject, Guid dsdSetId, int cardinality)
        {
            DsdSetState state;
            if (!_acDomain.DsdSetSet.TryGetDsdSet(dsdSetId, out state))
            {
                throw new ValidationException("意外的动态责任分离角色集标识" + dsdSetId);
            }
            _acDomain.Handle(new UpdateDsdSetCommand(subject, new DsdSetUpdateIo
            {
                Id = state.Id,
                Description = state.Description,
                IsEnabled = state.IsEnabled,
                Name = state.Name,
                DsdCard = cardinality
            }));
        }

        public IReadOnlyCollection<DsdRoleState> DsdRoleSets(IAcSession subject)
        {
            return _acDomain.DsdSetSet.GetDsdRoles();
        }

        public IReadOnlyCollection<RoleState> DsdRoleSetRoles(IAcSession subject, Guid dsdSetId)
        {
            DsdSetState dsdSet;
            if (!_acDomain.DsdSetSet.TryGetDsdSet(dsdSetId, out dsdSet))
            {
                throw new ValidationException("给定标识的动态责任分离角色集不存在" + dsdSetId);
            }
            var result = new List<RoleState>();
            foreach (var item in _acDomain.DsdSetSet.GetDsdRoles(dsdSet))
            {
                RoleState role;
                if (_acDomain.RoleSet.TryGetRole(item.RoleId, out role))
                {
                    result.Add(role);
                }
            }

            return result;
        }

        public int DsdRoleSetCardinality(IAcSession subject, Guid dsdSetId)
        {
            DsdSetState dsdSet;
            if (!_acDomain.DsdSetSet.TryGetDsdSet(dsdSetId, out dsdSet))
            {
                throw new ValidationException("意外的动态责任分离角色集标识" + dsdSetId);
            }
            return dsdSet.DsdCard;
        }

        public class PrivilegeCreateIo : EntityCreateInput, IPrivilegeCreateIo
        {
            public string SubjectType { get; set; }

            public Guid SubjectInstanceId { get; set; }

            public string ObjectType { get; set; }

            public Guid ObjectInstanceId { get; set; }

            public string AcContent { get; set; }

            public string AcContentType { get; set; }

            public override IAnycmdCommand ToCommand(IAcSession subject)
            {
                return new AddPrivilegeCommand(subject, this);
            }
        }

        public class PrivilegeUpdateIo : IPrivilegeUpdateIo
        {
            public PrivilegeUpdateIo()
            {
                HecpOntology = "Privilege";
                HecpVerb = "Update";
            }

            public string HecpOntology { get; private set; }

            public string HecpVerb { get; private set; }

            public Guid Id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string AcContent { get; set; }

            public IAnycmdCommand ToCommand(IAcSession subject)
            {
                return new UpdatePrivilegeCommand(subject, this);
            }
        }

        public class DsdRoleCreateIo : EntityCreateInput, IDsdRoleCreateIo
        {
            public Guid DsdSetId { get; set; }

            public Guid RoleId { get; set; }

            public override IAnycmdCommand ToCommand(IAcSession subject)
            {
                return new AddDsdRoleCommand(subject, this);
            }
        }

        public class DsdSetCreateIo : EntityCreateInput, IDsdSetCreateIo
        {
            public string Name { get; set; }

            public int DsdCard { get; set; }

            public int IsEnabled { get; set; }

            public string Description { get; set; }

            public override IAnycmdCommand ToCommand(IAcSession subject)
            {
                return new AddDsdSetCommand(subject, this);
            }
        }

        public class DsdSetUpdateIo : IDsdSetUpdateIo
        {
            public DsdSetUpdateIo()
            {
                HecpOntology = "DsdSet";
                HecpVerb = "Update";
            }

            public string HecpOntology { get; private set; }

            public string HecpVerb { get; private set; }

            public Guid Id { get; set; }

            public string Name { get; set; }

            public int DsdCard { get; set; }

            public int IsEnabled { get; set; }

            public string Description { get; set; }

            public IAnycmdCommand ToCommand(IAcSession subject)
            {
                return new UpdateDsdSetCommand(subject, this);
            }
        }

        public class SsdRoleCreateIo : EntityCreateInput, ISsdRoleCreateIo
        {
            public Guid SsdSetId { get; set; }

            public Guid RoleId { get; set; }

            public override IAnycmdCommand ToCommand(IAcSession subject)
            {
                return new AddSsdRoleCommand(subject, this);
            }
        }

        public class SsdSetCreateIo : EntityCreateInput, ISsdSetCreateIo
        {
            public string Name { get; set; }

            public int SsdCard { get; set; }

            public int IsEnabled { get; set; }

            public string Description { get; set; }

            public override IAnycmdCommand ToCommand(IAcSession subject)
            {
                return new AddSsdSetCommand(subject, this);
            }
        }

        public class SsdSetUpdateIo : ISsdSetUpdateIo
        {
            public SsdSetUpdateIo()
            {
                HecpOntology = "SsdSet";
                HecpVerb = "Update";
            }

            public string HecpOntology { get; private set; }

            public string HecpVerb { get; private set; }

            public string Name { get; set; }

            public int SsdCard { get; set; }

            public int IsEnabled { get; set; }

            public string Description { get; set; }

            public Guid Id { get; set; }

            public IAnycmdCommand ToCommand(IAcSession subject)
            {
                return new UpdateSsdSetCommand(subject, this);
            }
        }
    }
}
