
namespace Anycmd.Tests
{
    using Ac.ViewModels.DsdViewModels;
    using Ac.ViewModels.GroupViewModels;
    using Ac.ViewModels.Identity.AccountViewModels;
    using Ac.ViewModels.Infra.DicViewModels;
    using Ac.ViewModels.Infra.FunctionViewModels;
    using Ac.ViewModels.Infra.OrganizationViewModels;
    using Ac.ViewModels.PrivilegeViewModels;
    using Ac.ViewModels.RoleViewModels;
    using Ac.ViewModels.SsdViewModels;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Messages;
    using Engine.Ac.Messages.Infra;
    using Engine.Ac.Messages.Rbac;
    using Engine.Host.Ac;
    using Engine.Host.Ac.Identity;
    using Engine.Host.Ac.Rbac;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class RbAcServiceTest
    {
        #region TestAddUser
        [Fact]
        public void TestAddUser()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var accountRepository = host.RetrieveRequiredService<IRepository<Account>>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.Handle(new OrganizationCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetUserSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(host.GetUserSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test",
                Password = "111111",
                OrganizationCode = "100"
            });
            var entity = accountRepository.GetByKey(accountId);
            Assert.NotNull(entity);
        }
        #endregion

        #region TestDeleteUser
        [Fact]
        public void TestDeleteUser()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var accountRepository = host.RetrieveRequiredService<IRepository<Account>>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.Handle(new OrganizationCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetUserSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(host.GetUserSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test1",
                Password = "111111",
                OrganizationCode = "100"
            });
            rbacService.DeleteUser(host.GetUserSession(), accountId);
            var entity = accountRepository.GetByKey(accountId);
            Assert.Null(entity);
        }
        #endregion

        #region TestAddRole
        [Fact]
        public void TestAddRole()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            RoleState roleById;
            rbacService.AddRole(host.GetUserSession(), new RoleCreateInput
            {
                Id = entityId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            Assert.Equal(1, host.RoleSet.Count());
            Assert.True(host.RoleSet.TryGetRole(entityId, out roleById));
        }
        #endregion

        #region TestDeleteRole
        [Fact]
        public void TestDeleteRole()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            RoleState roleById;
            rbacService.AddRole(host.GetUserSession(), new RoleCreateInput
            {
                Id = entityId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            Assert.Equal(1, host.RoleSet.Count());
            Assert.True(host.RoleSet.TryGetRole(entityId, out roleById));
            rbacService.DeleteRole(host.GetUserSession(), entityId);
            Assert.Equal(0, host.RoleSet.Count());
            Assert.False(host.RoleSet.TryGetRole(entityId, out roleById));
        }
        #endregion

        #region TestAssignUser
        [Fact]
        public void TestAssignUser()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = host.RetrieveRequiredService<IRepository<Privilege>>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(host.GetUserSession(), new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            host.Handle(new OrganizationCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetUserSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(host.GetUserSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test1",
                Password = "111111",
                OrganizationCode = "100"
            });
            rbacService.AssignUser(host.GetUserSession(), accountId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == accountId && a.ObjectInstanceId == roleId);
            Assert.NotNull(entity);
        }
        #endregion

        #region TestDeassignUser
        [Fact]
        public void TestDeassignUser()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = host.RetrieveRequiredService<IRepository<Privilege>>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(host.GetUserSession(), new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            host.Handle(new OrganizationCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetUserSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(host.GetUserSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test1",
                Password = "111111",
                OrganizationCode = "100"
            });
            rbacService.AssignUser(host.GetUserSession(), accountId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == accountId && a.ObjectInstanceId == roleId);
            Assert.NotNull(entity);
            rbacService.DeassignUser(host.GetUserSession(), accountId, roleId);
            entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == accountId && a.ObjectInstanceId == roleId);
            Assert.Null(entity);
        }
        #endregion

        #region TestGrantPermission
        [Fact]
        public void TestGrantPermission()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = host.RetrieveRequiredService<IRepository<Privilege>>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(host.GetUserSession(), new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            var functionId = Guid.NewGuid();
            host.Handle(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }.ToCommand(host.GetUserSession()));
            rbacService.GrantPermission(host.GetUserSession(), functionId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId);
            Assert.NotNull(entity);
            Assert.NotNull(host.PrivilegeSet.FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId));
        }
        #endregion

        #region TestRevokePermission
        [Fact]
        public void TestRevokePermission()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = host.RetrieveRequiredService<IRepository<Privilege>>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(host.GetUserSession(), new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            var functionId = Guid.NewGuid();
            host.Handle(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }.ToCommand(host.GetUserSession()));
            rbacService.GrantPermission(host.GetUserSession(), functionId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId);
            Assert.NotNull(entity);
            Assert.NotNull(host.PrivilegeSet.FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId));
            rbacService.RevokePermission(host.GetUserSession(), functionId, roleId);
            entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId);
            Assert.Null(entity);
            Assert.Null(host.PrivilegeSet.FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId));
        }
        #endregion

        #region TestCreateSession
        [Fact]
        public void TestCreateSession()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var accountRepository = host.RetrieveRequiredService<IRepository<Account>>();
            var sessionRepository = host.RetrieveRequiredService<IRepository<UserSession>>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.Handle(new OrganizationCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetUserSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(host.GetUserSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test1",
                Password = "111111",
                OrganizationCode = "100"
            });
            var account = accountRepository.GetByKey(accountId);
            var sessionId = Guid.NewGuid();
            var userSession = rbacService.CreateSession(host.GetUserSession(), sessionId, AccountState.Create(account));
            Assert.NotNull(userSession);
            var sessionEntity = sessionRepository.GetByKey(sessionId);
            Assert.NotNull(sessionEntity);
        }
        #endregion

        #region TestDeleteSession
        [Fact]
        public void TestDeleteSession()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var accountRepository = host.RetrieveRequiredService<IRepository<Account>>();
            var sessionRepository = host.RetrieveRequiredService<IRepository<UserSession>>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.Handle(new OrganizationCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetUserSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(host.GetUserSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test1",
                Password = "111111",
                OrganizationCode = "100"
            });
            var account = accountRepository.GetByKey(accountId);
            var sessionId = Guid.NewGuid();
            var userSession = rbacService.CreateSession(host.GetUserSession(), sessionId, AccountState.Create(account));
            Assert.NotNull(userSession);
            var sessionEntity = sessionRepository.GetByKey(sessionId);
            Assert.NotNull(sessionEntity);
            rbacService.DeleteSession(host.GetUserSession(), sessionId);
            sessionEntity = sessionRepository.GetByKey(sessionId);
            Assert.Null(sessionEntity);
        }
        #endregion

        #region TestSessionRoles
        [Fact]
        public void TestSessionRoles()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var accountRepository = host.RetrieveRequiredService<IRepository<Account>>();
            var sessionRepository = host.RetrieveRequiredService<IRepository<UserSession>>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var orgId = Guid.NewGuid();

            host.Handle(new OrganizationCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetUserSession()));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AccountCreateInput
            {
                Id = accountId,
                Code = "test1",
                Name = "test1",
                LoginName = "test1",
                Password = "111111",
                OrganizationCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }.ToCommand(host.GetUserSession()));
            Guid roleId = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            Guid entityId = Guid.NewGuid();
            // 授予账户角色
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            Guid organizationId = Guid.NewGuid();
            host.Handle(new OrganizationCreateInput
            {
                Id = organizationId,
                Code = "110",
                Name = "测试110",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetUserSession()));
            entityId = Guid.NewGuid();
            // 授予账户组织结构
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = organizationId,
                ObjectType = AcElementType.Organization.ToString()
            }));
            // 授予组织结构角色
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = organizationId,
                SubjectType = UserAcSubjectType.Organization.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            Guid groupId = Guid.NewGuid();
            host.Handle(new AddGroupCommand(host.GetUserSession(), new GroupCreateInput
            {
                Id = groupId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                ShortName = "",
                SortCode = 10,
                TypeCode = "Ac"
            }));
            entityId = Guid.NewGuid();
            // 授予账户工作组
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));
            // 授予工作组角色
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));
            roleId = Guid.NewGuid();
            // 添加一个新角色并将该角色授予上面创建的组织结构
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = organizationId,
                SubjectType = UserAcSubjectType.Organization.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));
            roleId = Guid.NewGuid();
            // 添加一个新角色并将该角色授予上面创建的工作组
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试3",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));
            var account = accountRepository.GetByKey(accountId);
            var sessionId = Guid.NewGuid();
            var userSession = rbacService.CreateSession(host.GetUserSession(), sessionId, AccountState.Create(account));
            Assert.NotNull(userSession);
            var sessionEntity = sessionRepository.GetByKey(sessionId);
            Assert.NotNull(sessionEntity);
            Assert.Equal(1, userSession.AccountPrivilege.Roles.Count);
            Assert.Equal(3, userSession.AccountPrivilege.AuthorizedRoles.Count);// 用户的全部角色来自直接角色、组织结构角色、工作组角色三者的并集所以是三个角色。
        }
        #endregion

        #region TestSessionPermissions
        [Fact]
        public void TestSessionPermissions()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var accountRepository = host.RetrieveRequiredService<IRepository<Account>>();
            var sessionRepository = host.RetrieveRequiredService<IRepository<UserSession>>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var orgId = Guid.NewGuid();

            host.Handle(new OrganizationCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetUserSession()));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AccountCreateInput
            {
                Id = accountId,
                Code = "test1",
                Name = "test1",
                LoginName = "test1",
                Password = "111111",
                OrganizationCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }.ToCommand(host.GetUserSession()));
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            Guid roleId = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            var functionId = Guid.NewGuid();
            host.Handle(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }.ToCommand(host.GetUserSession()));
            Guid entityId = Guid.NewGuid();
            // 授予角色功能
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = functionId,
                ObjectType = AcElementType.Function.ToString()
            }));
            // 授予账户角色
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            entityId = Guid.NewGuid();
            functionId = Guid.NewGuid();
            host.Handle(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun2",
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }.ToCommand(host.GetUserSession()));
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = functionId,
                ObjectType = AcElementType.Function.ToString()
            }));
            var account = accountRepository.GetByKey(accountId);
            var sessionId = Guid.NewGuid();
            var userSession = rbacService.CreateSession(host.GetUserSession(), sessionId, AccountState.Create(account));
            Assert.NotNull(userSession);
            var sessionEntity = sessionRepository.GetByKey(sessionId);
            Assert.NotNull(sessionEntity);
            Assert.Equal(1, userSession.AccountPrivilege.Functions.Count);
            Assert.Equal(2, userSession.AccountPrivilege.AuthorizedFunctions.Count);
            Assert.Equal(2, rbacService.UserPermissions(host.GetUserSession(), userSession).Count);
        }
        #endregion

        #region TestAddActiveRole
        [Fact]
        public void TestAddActiveRole()
        {
            // TODO:
        }
        #endregion

        #region TestDropActiveRole
        [Fact]
        public void TestDropActiveRole()
        {
            // TODO:
        }
        #endregion

        #region TestAssignedUsers
        [Fact]
        public void TestAssignedUsers()
        {
            // TODO:
        }
        #endregion

        #region TestAssignedRoles
        [Fact]
        public void TestAssignedRoles()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var orgId = Guid.NewGuid();

            host.Handle(new OrganizationCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetUserSession()));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AccountCreateInput
            {
                Id = accountId,
                Code = "test1",
                Name = "test1",
                LoginName = "test1",
                Password = "111111",
                OrganizationCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }.ToCommand(host.GetUserSession()));
            Guid roleId = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            Guid entityId = Guid.NewGuid();
            // 授予账户角色
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            Guid organizationId = Guid.NewGuid();
            host.Handle(new OrganizationCreateInput
            {
                Id = organizationId,
                Code = "110",
                Name = "测试110",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetUserSession()));
            entityId = Guid.NewGuid();
            // 授予账户组织结构
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = organizationId,
                ObjectType = AcElementType.Organization.ToString()
            }));
            // 授予组织结构角色
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = organizationId,
                SubjectType = UserAcSubjectType.Organization.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            Guid groupId = Guid.NewGuid();
            host.Handle(new AddGroupCommand(host.GetUserSession(), new GroupCreateInput
            {
                Id = groupId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                ShortName = "",
                SortCode = 10,
                TypeCode = "Ac"
            }));
            entityId = Guid.NewGuid();
            // 授予账户工作组
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));
            // 授予工作组角色
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));
            roleId = Guid.NewGuid();
            // 添加一个新角色并将该角色授予上面创建的组织结构
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = organizationId,
                SubjectType = UserAcSubjectType.Organization.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));
            roleId = Guid.NewGuid();
            // 添加一个新角色并将该角色授予上面创建的工作组
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试3",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));

            var roles = rbacService.AssignedRoles(host.GetUserSession(), accountId);
            Assert.Equal(1, roles.Count);
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            roles = rbacService.AuthorizedRoles(host.GetUserSession(), host.GetUserSession());
            Assert.Equal(3, roles.Count);// 用户的全部角色来自直接角色、组织结构角色、工作组角色三者的并集所以是三个角色。
        }
        #endregion

        #region TestAuthorizedUsers
        [Fact]
        public void TestAuthorizedUsers()
        {
            // TODO:
        }
        #endregion

        #region TestRolePermissions
        [Fact]
        public void TestRolePermissions()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = host.RetrieveRequiredService<IRepository<Privilege>>();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(host.GetUserSession(), new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            var functionId = Guid.NewGuid();
            host.Handle(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }.ToCommand(host.GetUserSession()));
            rbacService.GrantPermission(host.GetUserSession(), functionId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId);
            Assert.NotNull(entity);
            Assert.NotNull(host.PrivilegeSet.FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId));
            Assert.Equal(1, rbacService.RolePermissions(host.GetUserSession(), roleId).Count);
        }
        #endregion

        #region TestAddInheritance
        [Fact]
        public void TestAddInheritance()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.Equal(0, host.RoleSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId1 = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId1,
                Name = "role1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));

            var roleId2 = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId2,
                Name = "role2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            rbacService.AddInheritance(host.GetUserSession(), roleId1, roleId2);
            Assert.Equal(2, host.RoleSet.Count());
            RoleState role1;
            RoleState role2;
            Assert.True(host.RoleSet.TryGetRole(roleId1, out role1));
            Assert.True(host.RoleSet.TryGetRole(roleId2, out role2));
            Assert.Equal(1, host.RoleSet.GetDescendantRoles(role1).Count);
            Assert.Equal(0, host.RoleSet.GetDescendantRoles(role2).Count);
            rbacService.DeleteInheritance(host.GetUserSession(), roleId1, roleId2);
            Assert.Equal(0, host.RoleSet.GetDescendantRoles(role1).Count);
        }
        #endregion

        #region TestAddAscendant
        [Fact]
        public void TestAddAscendant()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.Equal(0, host.RoleSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId1 = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId1,
                Name = "role1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));

            var roleId2 = Guid.NewGuid();
            rbacService.AddAscendant(host.GetUserSession(), roleId1, new RoleCreateInput
            {
                Id = roleId2,
                Name = "role2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            Assert.Equal(2, host.RoleSet.Count());
            RoleState role1;
            RoleState role2;
            Assert.True(host.RoleSet.TryGetRole(roleId1, out role1));
            Assert.True(host.RoleSet.TryGetRole(roleId2, out role2));
            Assert.Equal(1, host.RoleSet.GetDescendantRoles(role1).Count);
            Assert.Equal(0, host.RoleSet.GetDescendantRoles(role2).Count);
            rbacService.DeleteInheritance(host.GetUserSession(), roleId1, roleId2);
            Assert.Equal(0, host.RoleSet.GetDescendantRoles(role1).Count);
        }
        #endregion

        #region TestAddDescendant
        [Fact]
        public void TestAddDescendant()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.Equal(0, host.RoleSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId1 = Guid.NewGuid();
            var roleId2 = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId2,
                Name = "role2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            rbacService.AddDescendant(host.GetUserSession(), roleId2, new RoleCreateInput
            {
                Id = roleId1,
                Name = "role1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            Assert.Equal(2, host.RoleSet.Count());
            RoleState role1;
            RoleState role2;
            Assert.True(host.RoleSet.TryGetRole(roleId1, out role1));
            Assert.True(host.RoleSet.TryGetRole(roleId2, out role2));
            Assert.Equal(1, host.RoleSet.GetDescendantRoles(role1).Count);
            Assert.Equal(0, host.RoleSet.GetDescendantRoles(role2).Count);
            rbacService.DeleteInheritance(host.GetUserSession(), roleId1, roleId2);
            Assert.Equal(0, host.RoleSet.GetDescendantRoles(role1).Count);
        }
        #endregion

        #region TestCreateSsdSet
        [Fact]
        public void TestCreateSsdSet()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.Equal(0, host.SsdSetSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            SsdSetState ssdSetById;
            rbacService.CreateSsdSet(host.GetUserSession(), new SsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            });
            Assert.Equal(1, host.SsdSetSet.Count());
            Assert.True(host.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));
        }
        #endregion

        #region TestDeleteSsdSet
        [Fact]
        public void TestDeleteSsdSet()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.Equal(0, host.SsdSetSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            SsdSetState ssdSetById;
            rbacService.CreateSsdSet(host.GetUserSession(), new SsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            });
            Assert.Equal(1, host.SsdSetSet.Count());
            Assert.True(host.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));
            rbacService.DeleteSsdSet(host.GetUserSession(), entityId);
            Assert.Equal(0, host.SsdSetSet.Count());
            Assert.False(host.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));
        }
        #endregion

        #region TestAddSsdRoleMember
        [Fact]
        public void TestAddSsdRoleMember()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.Equal(0, host.SsdSetSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var ssdSetId = Guid.NewGuid();

            SsdSetState ssdSetById;
            host.Handle(new AddSsdSetCommand(host.GetUserSession(), new SsdSetCreateIo
            {
                Id = ssdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            }));
            Assert.Equal(1, host.SsdSetSet.Count());
            Assert.True(host.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSetById));

            Assert.Equal(0, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
            RoleState roleById;
            var roleId = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            Assert.Equal(1, host.RoleSet.Count());
            Assert.True(host.RoleSet.TryGetRole(roleId, out roleById));
            rbacService.AddSsdRoleMember(host.GetUserSession(), ssdSetId, roleId);
            Assert.Equal(1, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
        }
        #endregion

        #region TestDeleteSsdRoleMember
        [Fact]
        public void TestDeleteSsdRoleMember()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.Equal(0, host.SsdSetSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var ssdSetId = Guid.NewGuid();

            SsdSetState ssdSetById;
            host.Handle(new AddSsdSetCommand(host.GetUserSession(), new SsdSetCreateIo
            {
                Id = ssdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            }));
            Assert.Equal(1, host.SsdSetSet.Count());
            Assert.True(host.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSetById));

            Assert.Equal(0, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
            RoleState roleById;
            var roleId = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            Assert.Equal(1, host.RoleSet.Count());
            Assert.True(host.RoleSet.TryGetRole(roleId, out roleById));
            rbacService.AddSsdRoleMember(host.GetUserSession(), ssdSetId, roleId);
            Assert.Equal(1, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
            rbacService.DeleteSsdRoleMember(host.GetUserSession(), ssdSetId, roleId);
            Assert.Equal(0, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
        }
        #endregion

        #region TestSetSsdCardinality
        [Fact]
        public void TestSetSsdCardinality()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.Equal(0, host.SsdSetSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var ssdSetId = Guid.NewGuid();

            SsdSetState ssdSetById;
            host.Handle(new AddSsdSetCommand(host.GetUserSession(), new SsdSetCreateIo
            {
                Id = ssdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            }));
            Assert.Equal(1, host.SsdSetSet.Count());
            Assert.True(host.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSetById));
            Assert.Equal(2, ssdSetById.SsdCard);
            rbacService.SetSsdCardinality(host.GetUserSession(), ssdSetId, 3);
            Assert.True(host.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSetById));
            Assert.Equal(3, ssdSetById.SsdCard);
        }
        #endregion

        #region TestCreateDsdSet
        [Fact]
        public void TestCreateDsdSet()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.Equal(0, host.DsdSetSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            DsdSetState dsdSetById;
            rbacService.CreateDsdSet(host.GetUserSession(), new DsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            });
            Assert.Equal(1, host.DsdSetSet.Count());
            Assert.True(host.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
        }
        #endregion

        #region TestDeleteDsdSet
        [Fact]
        public void TestDeleteDsdSet()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.Equal(0, host.DsdSetSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            DsdSetState dsdSetById;
            rbacService.CreateDsdSet(host.GetUserSession(), new DsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            });
            Assert.Equal(1, host.DsdSetSet.Count());
            Assert.True(host.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
            rbacService.DeleteDsdSet(host.GetUserSession(), entityId);
            Assert.Equal(0, host.DsdSetSet.Count());
            Assert.False(host.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
        }
        #endregion

        #region TestAddDsdRoleMember
        [Fact]
        public void TestAddDsdRoleMember()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.Equal(0, host.DsdSetSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var dsdSetId = Guid.NewGuid();

            DsdSetState dsdSetById;
            host.Handle(new AddDsdSetCommand(host.GetUserSession(), new DsdSetCreateIo
            {
                Id = dsdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.Equal(1, host.DsdSetSet.Count());
            Assert.True(host.DsdSetSet.TryGetDsdSet(dsdSetId, out dsdSetById));

            Assert.Equal(0, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
            RoleState roleById;
            var roleId = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            Assert.Equal(1, host.RoleSet.Count());
            Assert.True(host.RoleSet.TryGetRole(roleId, out roleById));
            rbacService.AddDsdRoleMember(host.GetUserSession(), dsdSetId, roleId);
            Assert.Equal(1, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
        }
        #endregion

        #region TestDeleteDsdRoleMember
        [Fact]
        public void TestDeleteDsdRoleMember()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.Equal(0, host.DsdSetSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var dsdSetId = Guid.NewGuid();

            DsdSetState dsdSetById;
            host.Handle(new AddDsdSetCommand(host.GetUserSession(), new DsdSetCreateIo
            {
                Id = dsdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.Equal(1, host.DsdSetSet.Count());
            Assert.True(host.DsdSetSet.TryGetDsdSet(dsdSetId, out dsdSetById));

            Assert.Equal(0, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
            RoleState roleById;
            var roleId = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            Assert.Equal(1, host.RoleSet.Count());
            Assert.True(host.RoleSet.TryGetRole(roleId, out roleById));
            rbacService.AddDsdRoleMember(host.GetUserSession(), dsdSetId, roleId);
            Assert.Equal(1, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
            rbacService.DeleteDsdRoleMember(host.GetUserSession(), dsdSetId, roleId);
            Assert.Equal(0, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
        }
        #endregion

        #region TestSetDsdCardinality
        [Fact]
        public void TestSetDsdCardinality()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.Equal(0, host.DsdSetSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var ssdSetId = Guid.NewGuid();

            DsdSetState ssdSetById;
            host.Handle(new AddDsdSetCommand(host.GetUserSession(), new DsdSetCreateIo
            {
                Id = ssdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.Equal(1, host.DsdSetSet.Count());
            Assert.True(host.DsdSetSet.TryGetDsdSet(ssdSetId, out ssdSetById));
            Assert.Equal(2, ssdSetById.DsdCard);
            rbacService.SetDsdCardinality(host.GetUserSession(), ssdSetId, 3);
            Assert.True(host.DsdSetSet.TryGetDsdSet(ssdSetId, out ssdSetById));
            Assert.Equal(3, ssdSetById.DsdCard);
        }
        #endregion
    }
}
