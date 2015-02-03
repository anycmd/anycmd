
namespace Anycmd.Tests
{
    using Ac.ViewModels.DsdViewModels;
    using Ac.ViewModels.GroupViewModels;
    using Ac.ViewModels.Identity.AccountViewModels;
    using Ac.ViewModels.Infra.CatalogViewModels;
    using Ac.ViewModels.Infra.FunctionViewModels;
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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class RbAcServiceTest
    {
        #region TestAddUser
        [TestMethod]
        public void TestAddUser()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var accountRepository = host.RetrieveRequiredService<IRepository<Account>>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(host.GetAcSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test",
                Password = "111111",
                CatalogCode = "100"
            });
            var entity = accountRepository.GetByKey(accountId);
            Assert.IsNotNull(entity);
        }
        #endregion

        #region TestDeleteUser
        [TestMethod]
        public void TestDeleteUser()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var accountRepository = host.RetrieveRequiredService<IRepository<Account>>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(host.GetAcSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test1",
                Password = "111111",
                CatalogCode = "100"
            });
            rbacService.DeleteUser(host.GetAcSession(), accountId);
            var entity = accountRepository.GetByKey(accountId);
            Assert.IsNull(entity);
        }
        #endregion

        #region TestAddRole
        [TestMethod]
        public void TestAddRole()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            RoleState roleById;
            rbacService.AddRole(host.GetAcSession(), new RoleCreateInput
            {
                Id = entityId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            Assert.AreEqual(1, host.RoleSet.Count());
            Assert.IsTrue(host.RoleSet.TryGetRole(entityId, out roleById));
        }
        #endregion

        #region TestDeleteRole
        [TestMethod]
        public void TestDeleteRole()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            RoleState roleById;
            rbacService.AddRole(host.GetAcSession(), new RoleCreateInput
            {
                Id = entityId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            Assert.AreEqual(1, host.RoleSet.Count());
            Assert.IsTrue(host.RoleSet.TryGetRole(entityId, out roleById));
            rbacService.DeleteRole(host.GetAcSession(), entityId);
            Assert.AreEqual(0, host.RoleSet.Count());
            Assert.IsFalse(host.RoleSet.TryGetRole(entityId, out roleById));
        }
        #endregion

        #region TestAssignUser
        [TestMethod]
        public void TestAssignUser()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = host.RetrieveRequiredService<IRepository<Privilege>>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(host.GetAcSession(), new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            host.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(host.GetAcSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test1",
                Password = "111111",
                CatalogCode = "100"
            });
            rbacService.AssignUser(host.GetAcSession(), accountId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == accountId && a.ObjectInstanceId == roleId);
            Assert.IsNotNull(entity);
        }
        #endregion

        #region TestDeassignUser
        [TestMethod]
        public void TestDeassignUser()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = host.RetrieveRequiredService<IRepository<Privilege>>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(host.GetAcSession(), new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            host.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(host.GetAcSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test1",
                Password = "111111",
                CatalogCode = "100"
            });
            rbacService.AssignUser(host.GetAcSession(), accountId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == accountId && a.ObjectInstanceId == roleId);
            Assert.IsNotNull(entity);
            rbacService.DeassignUser(host.GetAcSession(), accountId, roleId);
            entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == accountId && a.ObjectInstanceId == roleId);
            Assert.IsNull(entity);
        }
        #endregion

        #region TestGrantPermission
        [TestMethod]
        public void TestGrantPermission()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = host.RetrieveRequiredService<IRepository<Privilege>>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(host.GetAcSession(), new RoleCreateInput
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
            }.ToCommand(host.GetAcSession()));
            rbacService.GrantPermission(host.GetAcSession(), functionId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId);
            Assert.IsNotNull(entity);
            Assert.IsNotNull(host.PrivilegeSet.FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId));
        }
        #endregion

        #region TestRevokePermission
        [TestMethod]
        public void TestRevokePermission()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = host.RetrieveRequiredService<IRepository<Privilege>>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(host.GetAcSession(), new RoleCreateInput
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
            }.ToCommand(host.GetAcSession()));
            rbacService.GrantPermission(host.GetAcSession(), functionId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId);
            Assert.IsNotNull(entity);
            Assert.IsNotNull(host.PrivilegeSet.FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId));
            rbacService.RevokePermission(host.GetAcSession(), functionId, roleId);
            entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId);
            Assert.IsNull(entity);
            Assert.IsNull(host.PrivilegeSet.FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId));
        }
        #endregion

        #region TestCreateSession
        [TestMethod]
        public void TestCreateSession()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var accountRepository = host.RetrieveRequiredService<IRepository<Account>>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(host.GetAcSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test1",
                Password = "111111",
                CatalogCode = "100"
            });
            var account = accountRepository.GetByKey(accountId);
            var sessionId = Guid.NewGuid();
            var acSession = rbacService.CreateSession(host.GetAcSession(), sessionId, AccountState.Create(account));
            Assert.IsNotNull(acSession);
            var sessionEntity = AcSessionState.AcMethod.GetAcSessionEntity(host, sessionId);
            Assert.IsNotNull(sessionEntity);
        }
        #endregion

        #region TestDeleteSession
        [TestMethod]
        public void TestDeleteSession()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var accountRepository = host.RetrieveRequiredService<IRepository<Account>>();
            var sessionRepository = host.RetrieveRequiredService<IRepository<AcSession>>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(host.GetAcSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test1",
                Password = "111111",
                CatalogCode = "100"
            });
            var account = accountRepository.GetByKey(accountId);
            var sessionId = Guid.NewGuid();
            var acSession = rbacService.CreateSession(host.GetAcSession(), sessionId, AccountState.Create(account));
            Assert.IsNotNull(acSession);
            var sessionEntity = sessionRepository.GetByKey(sessionId);
            Assert.IsNotNull(sessionEntity);
            rbacService.DeleteSession(host.GetAcSession(), sessionId);
            sessionEntity = sessionRepository.GetByKey(sessionId);
            Assert.IsNull(sessionEntity);
        }
        #endregion

        #region TestSessionRoles
        [TestMethod]
        public void TestSessionRoles()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var accountRepository = host.RetrieveRequiredService<IRepository<Account>>();
            var sessionRepository = host.RetrieveRequiredService<IRepository<AcSession>>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var orgId = Guid.NewGuid();

            host.Handle(new CatalogCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AccountCreateInput
            {
                Id = accountId,
                Code = "test1",
                Name = "test1",
                LoginName = "test1",
                Password = "111111",
                CatalogCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }.ToCommand(host.GetAcSession()));
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
            }.ToCommand(host.GetAcSession()));
            Guid entityId = Guid.NewGuid();
            // 授予账户角色
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            Guid catalogId = Guid.NewGuid();
            host.Handle(new CatalogCreateInput
            {
                Id = catalogId,
                Code = "110",
                Name = "测试110",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            entityId = Guid.NewGuid();
            // 授予账户目录
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = catalogId,
                ObjectType = AcElementType.Catalog.ToString()
            }));
            // 授予目录角色
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = catalogId,
                SubjectType = UserAcSubjectType.Catalog.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            Guid groupId = Guid.NewGuid();
            host.Handle(new AddGroupCommand(host.GetAcSession(), new GroupCreateInput
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
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
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
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
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
            // 添加一个新角色并将该角色授予上面创建的目录
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetAcSession()));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = catalogId,
                SubjectType = UserAcSubjectType.Catalog.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
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
            }.ToCommand(host.GetAcSession()));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
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
            var acSession = rbacService.CreateSession(host.GetAcSession(), sessionId, AccountState.Create(account));
            Assert.IsNotNull(acSession);
            var sessionEntity = sessionRepository.GetByKey(sessionId);
            Assert.IsNotNull(sessionEntity);
            Assert.AreEqual(1, acSession.AccountPrivilege.Roles.Count);
            Assert.AreEqual(3, acSession.AccountPrivilege.AuthorizedRoles.Count);// 用户的全部角色来自直接角色、目录角色、工作组角色三者的并集所以是三个角色。
        }
        #endregion

        #region TestSessionPermissions
        [TestMethod]
        public void TestSessionPermissions()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var accountRepository = host.RetrieveRequiredService<IRepository<Account>>();
            var sessionRepository = host.RetrieveRequiredService<IRepository<AcSession>>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var orgId = Guid.NewGuid();

            host.Handle(new CatalogCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AccountCreateInput
            {
                Id = accountId,
                Code = "test1",
                Name = "test1",
                LoginName = "test1",
                Password = "111111",
                CatalogCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }.ToCommand(host.GetAcSession()));
            Assert.IsNotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
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
            }.ToCommand(host.GetAcSession()));
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
            }.ToCommand(host.GetAcSession()));
            Guid entityId = Guid.NewGuid();
            // 授予角色功能
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
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
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
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
            }.ToCommand(host.GetAcSession()));
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
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
            var acSession = rbacService.CreateSession(host.GetAcSession(), sessionId, AccountState.Create(account));
            Assert.IsNotNull(acSession);
            var sessionEntity = sessionRepository.GetByKey(sessionId);
            Assert.IsNotNull(sessionEntity);
            Assert.AreEqual(1, acSession.AccountPrivilege.Functions.Count);
            Assert.AreEqual(2, acSession.AccountPrivilege.AuthorizedFunctions.Count);
            Assert.AreEqual(2, rbacService.UserPermissions(host.GetAcSession(), acSession).Count);
        }
        #endregion

        #region TestAddActiveRole
        [TestMethod]
        public void TestAddActiveRole()
        {
            // TODO:实现单元测试
        }
        #endregion

        #region TestDropActiveRole
        [TestMethod]
        public void TestDropActiveRole()
        {
            // TODO:实现单元测试
        }
        #endregion

        #region TestAssignedUsers
        [TestMethod]
        public void TestAssignedUsers()
        {
            // TODO:实现单元测试
        }
        #endregion

        #region TestAssignedRoles
        [TestMethod]
        public void TestAssignedRoles()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var orgId = Guid.NewGuid();

            host.Handle(new CatalogCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AccountCreateInput
            {
                Id = accountId,
                Code = "test1",
                Name = "test1",
                LoginName = "test1",
                Password = "111111",
                CatalogCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }.ToCommand(host.GetAcSession()));
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
            }.ToCommand(host.GetAcSession()));
            Guid entityId = Guid.NewGuid();
            // 授予账户角色
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            Guid catalogId = Guid.NewGuid();
            host.Handle(new CatalogCreateInput
            {
                Id = catalogId,
                Code = "110",
                Name = "测试110",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            entityId = Guid.NewGuid();
            // 授予账户目录
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = catalogId,
                ObjectType = AcElementType.Catalog.ToString()
            }));
            // 授予目录角色
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = catalogId,
                SubjectType = UserAcSubjectType.Catalog.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            Guid groupId = Guid.NewGuid();
            host.Handle(new AddGroupCommand(host.GetAcSession(), new GroupCreateInput
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
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
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
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
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
            // 添加一个新角色并将该角色授予上面创建的目录
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetAcSession()));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = catalogId,
                SubjectType = UserAcSubjectType.Catalog.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
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
            }.ToCommand(host.GetAcSession()));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));

            var roles = rbacService.AssignedRoles(host.GetAcSession(), accountId);
            Assert.AreEqual(1, roles.Count);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            roles = rbacService.AuthorizedRoles(host.GetAcSession(), host.GetAcSession());
            Assert.AreEqual(3, roles.Count);// 用户的全部角色来自直接角色、目录角色、工作组角色三者的并集所以是三个角色。
        }
        #endregion

        #region TestAuthorizedUsers
        [TestMethod]
        public void TestAuthorizedUsers()
        {
            // TODO:实现单元测试
        }
        #endregion

        #region TestRolePermissions
        [TestMethod]
        public void TestRolePermissions()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = host.RetrieveRequiredService<IRepository<Privilege>>();
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(host.GetAcSession(), new RoleCreateInput
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
            }.ToCommand(host.GetAcSession()));
            rbacService.GrantPermission(host.GetAcSession(), functionId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId);
            Assert.IsNotNull(entity);
            Assert.IsNotNull(host.PrivilegeSet.FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId));
            Assert.AreEqual(1, rbacService.RolePermissions(host.GetAcSession(), roleId).Count);
        }
        #endregion

        #region TestAddInheritance
        [TestMethod]
        public void TestAddInheritance()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, host.RoleSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
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
            }.ToCommand(host.GetAcSession()));

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
            }.ToCommand(host.GetAcSession()));
            rbacService.AddInheritance(host.GetAcSession(), roleId1, roleId2);
            Assert.AreEqual(2, host.RoleSet.Count());
            RoleState role1;
            RoleState role2;
            Assert.IsTrue(host.RoleSet.TryGetRole(roleId1, out role1));
            Assert.IsTrue(host.RoleSet.TryGetRole(roleId2, out role2));
            Assert.AreEqual(1, host.RoleSet.GetDescendantRoles(role1).Count);
            Assert.AreEqual(0, host.RoleSet.GetDescendantRoles(role2).Count);
            rbacService.DeleteInheritance(host.GetAcSession(), roleId1, roleId2);
            Assert.AreEqual(0, host.RoleSet.GetDescendantRoles(role1).Count);
        }
        #endregion

        #region TestAddAscendant
        [TestMethod]
        public void TestAddAscendant()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, host.RoleSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
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
            }.ToCommand(host.GetAcSession()));

            var roleId2 = Guid.NewGuid();
            rbacService.AddAscendant(host.GetAcSession(), roleId1, new RoleCreateInput
            {
                Id = roleId2,
                Name = "role2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            Assert.AreEqual(2, host.RoleSet.Count());
            RoleState role1;
            RoleState role2;
            Assert.IsTrue(host.RoleSet.TryGetRole(roleId1, out role1));
            Assert.IsTrue(host.RoleSet.TryGetRole(roleId2, out role2));
            Assert.AreEqual(1, host.RoleSet.GetDescendantRoles(role1).Count);
            Assert.AreEqual(0, host.RoleSet.GetDescendantRoles(role2).Count);
            rbacService.DeleteInheritance(host.GetAcSession(), roleId1, roleId2);
            Assert.AreEqual(0, host.RoleSet.GetDescendantRoles(role1).Count);
        }
        #endregion

        #region TestAddDescendant
        [TestMethod]
        public void TestAddDescendant()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, host.RoleSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
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
            }.ToCommand(host.GetAcSession()));
            rbacService.AddDescendant(host.GetAcSession(), roleId2, new RoleCreateInput
            {
                Id = roleId1,
                Name = "role1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            Assert.AreEqual(2, host.RoleSet.Count());
            RoleState role1;
            RoleState role2;
            Assert.IsTrue(host.RoleSet.TryGetRole(roleId1, out role1));
            Assert.IsTrue(host.RoleSet.TryGetRole(roleId2, out role2));
            Assert.AreEqual(1, host.RoleSet.GetDescendantRoles(role1).Count);
            Assert.AreEqual(0, host.RoleSet.GetDescendantRoles(role2).Count);
            rbacService.DeleteInheritance(host.GetAcSession(), roleId1, roleId2);
            Assert.AreEqual(0, host.RoleSet.GetDescendantRoles(role1).Count);
        }
        #endregion

        #region TestCreateSsdSet
        [TestMethod]
        public void TestCreateSsdSet()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, host.SsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            SsdSetState ssdSetById;
            rbacService.CreateSsdSet(host.GetAcSession(), new SsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            });
            Assert.AreEqual(1, host.SsdSetSet.Count());
            Assert.IsTrue(host.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));
        }
        #endregion

        #region TestDeleteSsdSet
        [TestMethod]
        public void TestDeleteSsdSet()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, host.SsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            SsdSetState ssdSetById;
            rbacService.CreateSsdSet(host.GetAcSession(), new SsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            });
            Assert.AreEqual(1, host.SsdSetSet.Count());
            Assert.IsTrue(host.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));
            rbacService.DeleteSsdSet(host.GetAcSession(), entityId);
            Assert.AreEqual(0, host.SsdSetSet.Count());
            Assert.IsFalse(host.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));
        }
        #endregion

        #region TestAddSsdRoleMember
        [TestMethod]
        public void TestAddSsdRoleMember()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, host.SsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var ssdSetId = Guid.NewGuid();

            SsdSetState ssdSetById;
            host.Handle(new AddSsdSetCommand(host.GetAcSession(), new SsdSetCreateIo
            {
                Id = ssdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            }));
            Assert.AreEqual(1, host.SsdSetSet.Count());
            Assert.IsTrue(host.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSetById));

            Assert.AreEqual(0, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
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
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.RoleSet.Count());
            Assert.IsTrue(host.RoleSet.TryGetRole(roleId, out roleById));
            rbacService.AddSsdRoleMember(host.GetAcSession(), ssdSetId, roleId);
            Assert.AreEqual(1, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
        }
        #endregion

        #region TestDeleteSsdRoleMember
        [TestMethod]
        public void TestDeleteSsdRoleMember()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, host.SsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var ssdSetId = Guid.NewGuid();

            SsdSetState ssdSetById;
            host.Handle(new AddSsdSetCommand(host.GetAcSession(), new SsdSetCreateIo
            {
                Id = ssdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            }));
            Assert.AreEqual(1, host.SsdSetSet.Count());
            Assert.IsTrue(host.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSetById));

            Assert.AreEqual(0, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
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
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.RoleSet.Count());
            Assert.IsTrue(host.RoleSet.TryGetRole(roleId, out roleById));
            rbacService.AddSsdRoleMember(host.GetAcSession(), ssdSetId, roleId);
            Assert.AreEqual(1, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
            rbacService.DeleteSsdRoleMember(host.GetAcSession(), ssdSetId, roleId);
            Assert.AreEqual(0, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
        }
        #endregion

        #region TestSetSsdCardinality
        [TestMethod]
        public void TestSetSsdCardinality()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, host.SsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var ssdSetId = Guid.NewGuid();

            SsdSetState ssdSetById;
            host.Handle(new AddSsdSetCommand(host.GetAcSession(), new SsdSetCreateIo
            {
                Id = ssdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            }));
            Assert.AreEqual(1, host.SsdSetSet.Count());
            Assert.IsTrue(host.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSetById));
            Assert.AreEqual(2, ssdSetById.SsdCard);
            rbacService.SetSsdCardinality(host.GetAcSession(), ssdSetId, 3);
            Assert.IsTrue(host.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSetById));
            Assert.AreEqual(3, ssdSetById.SsdCard);
        }
        #endregion

        #region TestCreateDsdSet
        [TestMethod]
        public void TestCreateDsdSet()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, host.DsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            DsdSetState dsdSetById;
            rbacService.CreateDsdSet(host.GetAcSession(), new DsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            });
            Assert.AreEqual(1, host.DsdSetSet.Count());
            Assert.IsTrue(host.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
        }
        #endregion

        #region TestDeleteDsdSet
        [TestMethod]
        public void TestDeleteDsdSet()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, host.DsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            DsdSetState dsdSetById;
            rbacService.CreateDsdSet(host.GetAcSession(), new DsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            });
            Assert.AreEqual(1, host.DsdSetSet.Count());
            Assert.IsTrue(host.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
            rbacService.DeleteDsdSet(host.GetAcSession(), entityId);
            Assert.AreEqual(0, host.DsdSetSet.Count());
            Assert.IsFalse(host.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
        }
        #endregion

        #region TestAddDsdRoleMember
        [TestMethod]
        public void TestAddDsdRoleMember()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, host.DsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var dsdSetId = Guid.NewGuid();

            DsdSetState dsdSetById;
            host.Handle(new AddDsdSetCommand(host.GetAcSession(), new DsdSetCreateIo
            {
                Id = dsdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.AreEqual(1, host.DsdSetSet.Count());
            Assert.IsTrue(host.DsdSetSet.TryGetDsdSet(dsdSetId, out dsdSetById));

            Assert.AreEqual(0, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
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
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.RoleSet.Count());
            Assert.IsTrue(host.RoleSet.TryGetRole(roleId, out roleById));
            rbacService.AddDsdRoleMember(host.GetAcSession(), dsdSetId, roleId);
            Assert.AreEqual(1, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
        }
        #endregion

        #region TestDeleteDsdRoleMember
        [TestMethod]
        public void TestDeleteDsdRoleMember()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, host.DsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var dsdSetId = Guid.NewGuid();

            DsdSetState dsdSetById;
            host.Handle(new AddDsdSetCommand(host.GetAcSession(), new DsdSetCreateIo
            {
                Id = dsdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.AreEqual(1, host.DsdSetSet.Count());
            Assert.IsTrue(host.DsdSetSet.TryGetDsdSet(dsdSetId, out dsdSetById));

            Assert.AreEqual(0, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
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
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.RoleSet.Count());
            Assert.IsTrue(host.RoleSet.TryGetRole(roleId, out roleById));
            rbacService.AddDsdRoleMember(host.GetAcSession(), dsdSetId, roleId);
            Assert.AreEqual(1, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
            rbacService.DeleteDsdRoleMember(host.GetAcSession(), dsdSetId, roleId);
            Assert.AreEqual(0, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
        }
        #endregion

        #region TestSetDsdCardinality
        [TestMethod]
        public void TestSetDsdCardinality()
        {
            var host = TestHelper.GetAcDomain();
            var rbacService = host.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, host.DsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var ssdSetId = Guid.NewGuid();

            DsdSetState ssdSetById;
            host.Handle(new AddDsdSetCommand(host.GetAcSession(), new DsdSetCreateIo
            {
                Id = ssdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.AreEqual(1, host.DsdSetSet.Count());
            Assert.IsTrue(host.DsdSetSet.TryGetDsdSet(ssdSetId, out ssdSetById));
            Assert.AreEqual(2, ssdSetById.DsdCard);
            rbacService.SetDsdCardinality(host.GetAcSession(), ssdSetId, 3);
            Assert.IsTrue(host.DsdSetSet.TryGetDsdSet(ssdSetId, out ssdSetById));
            Assert.AreEqual(3, ssdSetById.DsdCard);
        }
        #endregion
    }
}
