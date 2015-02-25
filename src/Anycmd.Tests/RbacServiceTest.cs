
namespace Anycmd.Tests
{
    using Ac.ViewModels.DsdViewModels;
    using Ac.ViewModels.GroupViewModels;
    using Ac.ViewModels.AccountViewModels;
    using Engine.Ac.Dsd;
    using Ac.ViewModels.CatalogViewModels;
    using Ac.ViewModels.FunctionViewModels;
    using Ac.ViewModels.PrivilegeViewModels;
    using Ac.ViewModels.RoleViewModels;
    using Ac.ViewModels.SsdViewModels;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Messages;
    using Engine.Ac.Groups;
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
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            var accountRepository = acDomain.RetrieveRequiredService<IRepository<Account>>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            acDomain.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(acDomain.GetAcSession(), new AccountCreateInput
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
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            var accountRepository = acDomain.RetrieveRequiredService<IRepository<Account>>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            acDomain.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(acDomain.GetAcSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test1",
                Password = "111111",
                CatalogCode = "100"
            });
            rbacService.DeleteUser(acDomain.GetAcSession(), accountId);
            var entity = accountRepository.GetByKey(accountId);
            Assert.IsNull(entity);
        }
        #endregion

        #region TestAddRole
        [TestMethod]
        public void TestAddRole()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            RoleState roleById;
            rbacService.AddRole(acDomain.GetAcSession(), new RoleCreateInput
            {
                Id = entityId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            Assert.AreEqual(1, acDomain.RoleSet.Count());
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(entityId, out roleById));
        }
        #endregion

        #region TestDeleteRole
        [TestMethod]
        public void TestDeleteRole()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            RoleState roleById;
            rbacService.AddRole(acDomain.GetAcSession(), new RoleCreateInput
            {
                Id = entityId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            Assert.AreEqual(1, acDomain.RoleSet.Count());
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(entityId, out roleById));
            rbacService.DeleteRole(acDomain.GetAcSession(), entityId);
            Assert.AreEqual(0, acDomain.RoleSet.Count());
            Assert.IsFalse(acDomain.RoleSet.TryGetRole(entityId, out roleById));
        }
        #endregion

        #region TestAssignUser
        [TestMethod]
        public void TestAssignUser()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = acDomain.RetrieveRequiredService<IRepository<Privilege>>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(acDomain.GetAcSession(), new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            acDomain.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(acDomain.GetAcSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test1",
                Password = "111111",
                CatalogCode = "100"
            });
            rbacService.AssignUser(acDomain.GetAcSession(), accountId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == accountId && a.ObjectInstanceId == roleId);
            Assert.IsNotNull(entity);
        }
        #endregion

        #region TestDeassignUser
        [TestMethod]
        public void TestDeassignUser()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = acDomain.RetrieveRequiredService<IRepository<Privilege>>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(acDomain.GetAcSession(), new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            acDomain.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(acDomain.GetAcSession(), new AccountCreateInput
            {
                Id = accountId,
                LoginName = "test1",
                Code = "test1",
                Name = "test1",
                Password = "111111",
                CatalogCode = "100"
            });
            rbacService.AssignUser(acDomain.GetAcSession(), accountId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == accountId && a.ObjectInstanceId == roleId);
            Assert.IsNotNull(entity);
            rbacService.DeassignUser(acDomain.GetAcSession(), accountId, roleId);
            entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == accountId && a.ObjectInstanceId == roleId);
            Assert.IsNull(entity);
        }
        #endregion

        #region TestGrantPermission
        [TestMethod]
        public void TestGrantPermission()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = acDomain.RetrieveRequiredService<IRepository<Privilege>>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(acDomain.GetAcSession(), new RoleCreateInput
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
            acDomain.Handle(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = TestHelper.TestCatalogNodeId,
                SortCode = 10
            }.ToCommand(acDomain.GetAcSession()));
            rbacService.GrantPermission(acDomain.GetAcSession(), functionId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId);
            Assert.IsNotNull(entity);
            Assert.IsNotNull(acDomain.PrivilegeSet.FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId));
        }
        #endregion

        #region TestRevokePermission
        [TestMethod]
        public void TestRevokePermission()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = acDomain.RetrieveRequiredService<IRepository<Privilege>>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(acDomain.GetAcSession(), new RoleCreateInput
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
            acDomain.Handle(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = TestHelper.TestCatalogNodeId,
                SortCode = 10
            }.ToCommand(acDomain.GetAcSession()));
            rbacService.GrantPermission(acDomain.GetAcSession(), functionId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId);
            Assert.IsNotNull(entity);
            Assert.IsNotNull(acDomain.PrivilegeSet.FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId));
            rbacService.RevokePermission(acDomain.GetAcSession(), functionId, roleId);
            entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId);
            Assert.IsNull(entity);
            Assert.IsNull(acDomain.PrivilegeSet.FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId));
        }
        #endregion

        #region TestCreateSession
        [TestMethod]
        public void TestCreateSession()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            var accountRepository = acDomain.RetrieveRequiredService<IRepository<Account>>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            acDomain.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(acDomain.GetAcSession(), new AccountCreateInput
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
            var acSession = rbacService.CreateSession(acDomain.GetAcSession(), sessionId, AccountState.Create(account));
            Assert.IsNotNull(acSession);
            var sessionEntity = AcSessionState.AcMethod.GetAcSessionEntity(acDomain, sessionId);
            Assert.IsNotNull(sessionEntity);
        }
        #endregion

        #region TestDeleteSession
        [TestMethod]
        public void TestDeleteSession()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            var accountRepository = acDomain.RetrieveRequiredService<IRepository<Account>>();
            var sessionRepository = acDomain.RetrieveRequiredService<IRepository<AcSession>>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            acDomain.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            var accountId = Guid.NewGuid();
            rbacService.AddUser(acDomain.GetAcSession(), new AccountCreateInput
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
            var acSession = rbacService.CreateSession(acDomain.GetAcSession(), sessionId, AccountState.Create(account));
            Assert.IsNotNull(acSession);
            var sessionEntity = sessionRepository.GetByKey(sessionId);
            Assert.IsNotNull(sessionEntity);
            rbacService.DeleteSession(acDomain.GetAcSession(), sessionId);
            sessionEntity = sessionRepository.GetByKey(sessionId);
            Assert.IsNull(sessionEntity);
        }
        #endregion

        #region TestSessionRoles
        [TestMethod]
        public void TestSessionRoles()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            var accountRepository = acDomain.RetrieveRequiredService<IRepository<Account>>();
            var sessionRepository = acDomain.RetrieveRequiredService<IRepository<AcSession>>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var orgId = Guid.NewGuid();

            acDomain.Handle(new CatalogCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            Guid accountId = Guid.NewGuid();
            acDomain.Handle(new AccountCreateInput
            {
                Id = accountId,
                Code = "test1",
                Name = "test1",
                LoginName = "test1",
                Password = "111111",
                CatalogCode = "100",
                IsEnabled = 1,
                AuditState = "anycmd.account.auditStatus.auditPass"
            }.ToCommand(acDomain.GetAcSession()));
            Guid roleId = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            Guid entityId = Guid.NewGuid();
            // 授予账户角色
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new CatalogCreateInput
            {
                Id = catalogId,
                Code = "110",
                Name = "测试110",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            entityId = Guid.NewGuid();
            // 授予账户目录
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new AddGroupCommand(acDomain.GetAcSession(), new GroupCreateInput
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
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            entityId = Guid.NewGuid();
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试3",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            entityId = Guid.NewGuid();
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            var acSession = rbacService.CreateSession(acDomain.GetAcSession(), sessionId, AccountState.Create(account));
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
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            var accountRepository = acDomain.RetrieveRequiredService<IRepository<Account>>();
            var sessionRepository = acDomain.RetrieveRequiredService<IRepository<AcSession>>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var orgId = Guid.NewGuid();

            acDomain.Handle(new CatalogCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            Guid accountId = Guid.NewGuid();
            acDomain.Handle(new AccountCreateInput
            {
                Id = accountId,
                Code = "test1",
                Name = "test1",
                LoginName = "test1",
                Password = "111111",
                CatalogCode = "100",
                IsEnabled = 1,
                AuditState = "anycmd.account.auditStatus.auditPass"
            }.ToCommand(acDomain.GetAcSession()));
            Assert.IsNotNull(acDomain.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            Guid roleId = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            var functionId = Guid.NewGuid();
            acDomain.Handle(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = TestHelper.TestCatalogNodeId,
                SortCode = 10
            }.ToCommand(acDomain.GetAcSession()));
            Guid entityId = Guid.NewGuid();
            // 授予角色功能
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun2",
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = TestHelper.TestCatalogNodeId,
                SortCode = 10
            }.ToCommand(acDomain.GetAcSession()));
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            var acSession = rbacService.CreateSession(acDomain.GetAcSession(), sessionId, AccountState.Create(account));
            Assert.IsNotNull(acSession);
            var sessionEntity = sessionRepository.GetByKey(sessionId);
            Assert.IsNotNull(sessionEntity);
            Assert.AreEqual(1, acSession.AccountPrivilege.Functions.Count);
            Assert.AreEqual(2, acSession.AccountPrivilege.AuthorizedFunctions.Count);
            Assert.AreEqual(2, rbacService.UserPermissions(acDomain.GetAcSession(), acSession).Count);
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
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var orgId = Guid.NewGuid();

            acDomain.Handle(new CatalogCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            Guid accountId = Guid.NewGuid();
            acDomain.Handle(new AccountCreateInput
            {
                Id = accountId,
                Code = "test1",
                Name = "test1",
                LoginName = "test1",
                Password = "111111",
                CatalogCode = "100",
                IsEnabled = 1,
                AuditState = "anycmd.account.auditStatus.auditPass"
            }.ToCommand(acDomain.GetAcSession()));
            Guid roleId = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            Guid entityId = Guid.NewGuid();
            // 授予账户角色
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new CatalogCreateInput
            {
                Id = catalogId,
                Code = "110",
                Name = "测试110",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            entityId = Guid.NewGuid();
            // 授予账户目录
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new AddGroupCommand(acDomain.GetAcSession(), new GroupCreateInput
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
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            entityId = Guid.NewGuid();
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试3",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            entityId = Guid.NewGuid();
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));

            var roles = rbacService.AssignedRoles(acDomain.GetAcSession(), accountId);
            Assert.AreEqual(1, roles.Count);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            roles = rbacService.AuthorizedRoles(acDomain.GetAcSession(), acDomain.GetAcSession());
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
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            var privilegeBigramRepository = acDomain.RetrieveRequiredService<IRepository<Privilege>>();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId = Guid.NewGuid();
            rbacService.AddRole(acDomain.GetAcSession(), new RoleCreateInput
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
            acDomain.Handle(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = TestHelper.TestCatalogNodeId,
                SortCode = 10
            }.ToCommand(acDomain.GetAcSession()));
            rbacService.GrantPermission(acDomain.GetAcSession(), functionId, roleId);
            var entity = privilegeBigramRepository.AsQueryable().FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId);
            Assert.IsNotNull(entity);
            Assert.IsNotNull(acDomain.PrivilegeSet.FirstOrDefault(a => a.SubjectInstanceId == roleId && a.ObjectInstanceId == functionId));
            Assert.AreEqual(1, rbacService.RolePermissions(acDomain.GetAcSession(), roleId).Count);
        }
        #endregion

        #region TestAddInheritance
        [TestMethod]
        public void TestAddInheritance()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, acDomain.RoleSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId1 = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId1,
                Name = "role1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));

            var roleId2 = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId2,
                Name = "role2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            rbacService.AddInheritance(acDomain.GetAcSession(), roleId1, roleId2);
            Assert.AreEqual(2, acDomain.RoleSet.Count());
            RoleState role1;
            RoleState role2;
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(roleId1, out role1));
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(roleId2, out role2));
            Assert.AreEqual(1, acDomain.RoleSet.GetDescendantRoles(role1).Count);
            Assert.AreEqual(0, acDomain.RoleSet.GetDescendantRoles(role2).Count);
            rbacService.DeleteInheritance(acDomain.GetAcSession(), roleId1, roleId2);
            Assert.AreEqual(0, acDomain.RoleSet.GetDescendantRoles(role1).Count);
        }
        #endregion

        #region TestAddAscendant
        [TestMethod]
        public void TestAddAscendant()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, acDomain.RoleSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId1 = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId1,
                Name = "role1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));

            var roleId2 = Guid.NewGuid();
            rbacService.AddAscendant(acDomain.GetAcSession(), roleId1, new RoleCreateInput
            {
                Id = roleId2,
                Name = "role2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            Assert.AreEqual(2, acDomain.RoleSet.Count());
            RoleState role1;
            RoleState role2;
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(roleId1, out role1));
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(roleId2, out role2));
            Assert.AreEqual(1, acDomain.RoleSet.GetDescendantRoles(role1).Count);
            Assert.AreEqual(0, acDomain.RoleSet.GetDescendantRoles(role2).Count);
            rbacService.DeleteInheritance(acDomain.GetAcSession(), roleId1, roleId2);
            Assert.AreEqual(0, acDomain.RoleSet.GetDescendantRoles(role1).Count);
        }
        #endregion

        #region TestAddDescendant
        [TestMethod]
        public void TestAddDescendant()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, acDomain.RoleSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId1 = Guid.NewGuid();
            var roleId2 = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId2,
                Name = "role2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            rbacService.AddDescendant(acDomain.GetAcSession(), roleId2, new RoleCreateInput
            {
                Id = roleId1,
                Name = "role1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            });
            Assert.AreEqual(2, acDomain.RoleSet.Count());
            RoleState role1;
            RoleState role2;
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(roleId1, out role1));
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(roleId2, out role2));
            Assert.AreEqual(1, acDomain.RoleSet.GetDescendantRoles(role1).Count);
            Assert.AreEqual(0, acDomain.RoleSet.GetDescendantRoles(role2).Count);
            rbacService.DeleteInheritance(acDomain.GetAcSession(), roleId1, roleId2);
            Assert.AreEqual(0, acDomain.RoleSet.GetDescendantRoles(role1).Count);
        }
        #endregion

        #region TestCreateSsdSet
        [TestMethod]
        public void TestCreateSsdSet()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, acDomain.SsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            SsdSetState ssdSetById;
            rbacService.CreateSsdSet(acDomain.GetAcSession(), new SsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            });
            Assert.AreEqual(1, acDomain.SsdSetSet.Count());
            Assert.IsTrue(acDomain.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));
        }
        #endregion

        #region TestDeleteSsdSet
        [TestMethod]
        public void TestDeleteSsdSet()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, acDomain.SsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            SsdSetState ssdSetById;
            rbacService.CreateSsdSet(acDomain.GetAcSession(), new SsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            });
            Assert.AreEqual(1, acDomain.SsdSetSet.Count());
            Assert.IsTrue(acDomain.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));
            rbacService.DeleteSsdSet(acDomain.GetAcSession(), entityId);
            Assert.AreEqual(0, acDomain.SsdSetSet.Count());
            Assert.IsFalse(acDomain.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));
        }
        #endregion

        #region TestAddSsdRoleMember
        [TestMethod]
        public void TestAddSsdRoleMember()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, acDomain.SsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var ssdSetId = Guid.NewGuid();

            SsdSetState ssdSetById;
            acDomain.Handle(new AddSsdSetCommand(acDomain.GetAcSession(), new SsdSetCreateIo
            {
                Id = ssdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            }));
            Assert.AreEqual(1, acDomain.SsdSetSet.Count());
            Assert.IsTrue(acDomain.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSetById));

            Assert.AreEqual(0, acDomain.SsdSetSet.GetSsdRoles(ssdSetById).Count);
            RoleState roleById;
            var roleId = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.RoleSet.Count());
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(roleId, out roleById));
            rbacService.AddSsdRoleMember(acDomain.GetAcSession(), ssdSetId, roleId);
            Assert.AreEqual(1, acDomain.SsdSetSet.GetSsdRoles(ssdSetById).Count);
        }
        #endregion

        #region TestDeleteSsdRoleMember
        [TestMethod]
        public void TestDeleteSsdRoleMember()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, acDomain.SsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var ssdSetId = Guid.NewGuid();

            SsdSetState ssdSetById;
            acDomain.Handle(new AddSsdSetCommand(acDomain.GetAcSession(), new SsdSetCreateIo
            {
                Id = ssdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            }));
            Assert.AreEqual(1, acDomain.SsdSetSet.Count());
            Assert.IsTrue(acDomain.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSetById));

            Assert.AreEqual(0, acDomain.SsdSetSet.GetSsdRoles(ssdSetById).Count);
            RoleState roleById;
            var roleId = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.RoleSet.Count());
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(roleId, out roleById));
            rbacService.AddSsdRoleMember(acDomain.GetAcSession(), ssdSetId, roleId);
            Assert.AreEqual(1, acDomain.SsdSetSet.GetSsdRoles(ssdSetById).Count);
            rbacService.DeleteSsdRoleMember(acDomain.GetAcSession(), ssdSetId, roleId);
            Assert.AreEqual(0, acDomain.SsdSetSet.GetSsdRoles(ssdSetById).Count);
        }
        #endregion

        #region TestSetSsdCardinality
        [TestMethod]
        public void TestSetSsdCardinality()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, acDomain.SsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var ssdSetId = Guid.NewGuid();

            SsdSetState ssdSetById;
            acDomain.Handle(new AddSsdSetCommand(acDomain.GetAcSession(), new SsdSetCreateIo
            {
                Id = ssdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            }));
            Assert.AreEqual(1, acDomain.SsdSetSet.Count());
            Assert.IsTrue(acDomain.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSetById));
            Assert.AreEqual(2, ssdSetById.SsdCard);
            rbacService.SetSsdCardinality(acDomain.GetAcSession(), ssdSetId, 3);
            Assert.IsTrue(acDomain.SsdSetSet.TryGetSsdSet(ssdSetId, out ssdSetById));
            Assert.AreEqual(3, ssdSetById.SsdCard);
        }
        #endregion

        #region TestCreateDsdSet
        [TestMethod]
        public void TestCreateDsdSet()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, acDomain.DsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            DsdSetState dsdSetById;
            rbacService.CreateDsdSet(acDomain.GetAcSession(), new DsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            });
            Assert.AreEqual(1, acDomain.DsdSetSet.Count());
            Assert.IsTrue(acDomain.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
        }
        #endregion

        #region TestDeleteDsdSet
        [TestMethod]
        public void TestDeleteDsdSet()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, acDomain.DsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            DsdSetState dsdSetById;
            rbacService.CreateDsdSet(acDomain.GetAcSession(), new DsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            });
            Assert.AreEqual(1, acDomain.DsdSetSet.Count());
            Assert.IsTrue(acDomain.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
            rbacService.DeleteDsdSet(acDomain.GetAcSession(), entityId);
            Assert.AreEqual(0, acDomain.DsdSetSet.Count());
            Assert.IsFalse(acDomain.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
        }
        #endregion

        #region TestAddDsdRoleMember
        [TestMethod]
        public void TestAddDsdRoleMember()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, acDomain.DsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var dsdSetId = Guid.NewGuid();

            DsdSetState dsdSetById;
            acDomain.Handle(new AddDsdSetCommand(acDomain.GetAcSession(), new DsdSetCreateIo
            {
                Id = dsdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.AreEqual(1, acDomain.DsdSetSet.Count());
            Assert.IsTrue(acDomain.DsdSetSet.TryGetDsdSet(dsdSetId, out dsdSetById));

            Assert.AreEqual(0, acDomain.DsdSetSet.GetDsdRoles(dsdSetById).Count);
            RoleState roleById;
            var roleId = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.RoleSet.Count());
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(roleId, out roleById));
            rbacService.AddDsdRoleMember(acDomain.GetAcSession(), dsdSetId, roleId);
            Assert.AreEqual(1, acDomain.DsdSetSet.GetDsdRoles(dsdSetById).Count);
        }
        #endregion

        #region TestDeleteDsdRoleMember
        [TestMethod]
        public void TestDeleteDsdRoleMember()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, acDomain.DsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var dsdSetId = Guid.NewGuid();

            DsdSetState dsdSetById;
            acDomain.Handle(new AddDsdSetCommand(acDomain.GetAcSession(), new DsdSetCreateIo
            {
                Id = dsdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.AreEqual(1, acDomain.DsdSetSet.Count());
            Assert.IsTrue(acDomain.DsdSetSet.TryGetDsdSet(dsdSetId, out dsdSetById));

            Assert.AreEqual(0, acDomain.DsdSetSet.GetDsdRoles(dsdSetById).Count);
            RoleState roleById;
            var roleId = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.RoleSet.Count());
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(roleId, out roleById));
            rbacService.AddDsdRoleMember(acDomain.GetAcSession(), dsdSetId, roleId);
            Assert.AreEqual(1, acDomain.DsdSetSet.GetDsdRoles(dsdSetById).Count);
            rbacService.DeleteDsdRoleMember(acDomain.GetAcSession(), dsdSetId, roleId);
            Assert.AreEqual(0, acDomain.DsdSetSet.GetDsdRoles(dsdSetById).Count);
        }
        #endregion

        #region TestSetDsdCardinality
        [TestMethod]
        public void TestSetDsdCardinality()
        {
            var acDomain = TestHelper.GetAcDomain();
            var rbacService = acDomain.RetrieveRequiredService<IRbacService>();
            Assert.AreEqual(0, acDomain.DsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var ssdSetId = Guid.NewGuid();

            DsdSetState ssdSetById;
            acDomain.Handle(new AddDsdSetCommand(acDomain.GetAcSession(), new DsdSetCreateIo
            {
                Id = ssdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.AreEqual(1, acDomain.DsdSetSet.Count());
            Assert.IsTrue(acDomain.DsdSetSet.TryGetDsdSet(ssdSetId, out ssdSetById));
            Assert.AreEqual(2, ssdSetById.DsdCard);
            rbacService.SetDsdCardinality(acDomain.GetAcSession(), ssdSetId, 3);
            Assert.IsTrue(acDomain.DsdSetSet.TryGetDsdSet(ssdSetId, out ssdSetById));
            Assert.AreEqual(3, ssdSetById.DsdCard);
        }
        #endregion
    }
}
