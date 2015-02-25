
namespace Anycmd.Tests
{
    using Ac.ViewModels.GroupViewModels;
    using Ac.ViewModels.AccountViewModels;
    using Ac.ViewModels.AppSystemViewModels;
    using Ac.ViewModels.CatalogViewModels;
    using Ac.ViewModels.FunctionViewModels;
    using Ac.ViewModels.MenuViewModels;
    using Ac.ViewModels.PrivilegeViewModels;
    using Ac.ViewModels.RoleViewModels;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Messages;
    using Engine.Ac.Groups;
    using Engine.Host.Ac;
    using Engine.Host.Ac.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Util;

    [TestClass]
    public class AcSessionTest
    {
        #region TestSignInSignOut
        [TestMethod]
        public void TestSignInSignOut()
        {
            var acDomain = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
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
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(acDomain.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual("test1", acDomain.GetAcSession().Identity.Name);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            Assert.IsFalse(acDomain.GetAcSession().Identity.IsAuthenticated);
        }
        #endregion

        #region TestUserRoles
        [TestMethod]
        public void TestUserRoles()
        {
            var acDomain = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(acDomain.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
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
            Assert.AreEqual(0, acDomain.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            RoleState role;
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(roleId, out role));
            acDomain.GetAcSession().AccountPrivilege.AddActiveRole(role);
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
            var privilegeBigram = acDomain.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.AreEqual(accountId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(roleId, privilegeBigram.ObjectInstanceId);
            Assert.AreEqual(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.AreEqual(AcElementType.Role.ToName(), privilegeBigram.ObjectType);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
            acDomain.Handle(new RemovePrivilegeCommand(acDomain.GetAcSession(), entityId));
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
            acDomain.GetAcSession().AccountPrivilege.DropActiveRole(role);
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
        }
        #endregion

        #region TestUserFunctions
        [TestMethod]
        public void TestUserFunctions()
        {
            var acDomain = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(acDomain.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
            Guid functionId = Guid.NewGuid();
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
            Assert.AreEqual(0, acDomain.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Functions.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = acDomain.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.AreEqual(accountId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(functionId, privilegeBigram.ObjectInstanceId);
            Assert.AreEqual(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.AreEqual(AcElementType.Function.ToName(), privilegeBigram.ObjectType);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Functions.Count);
            acDomain.Handle(new RemovePrivilegeCommand(acDomain.GetAcSession(), entityId));
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Functions.Count);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Functions.Count);
        }
        #endregion

        #region TestUserCatalogs
        [TestMethod]
        public void TestUserCatalogs()
        {
            var acDomain = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(acDomain.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
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
            Guid entityId = Guid.NewGuid();
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
            Assert.AreEqual(0, acDomain.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Catalogs.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = acDomain.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.AreEqual(accountId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(catalogId, privilegeBigram.ObjectInstanceId);
            Assert.AreEqual(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.AreEqual(AcElementType.Catalog.ToName(), privilegeBigram.ObjectType);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Catalogs.Count);
            acDomain.Handle(new RemovePrivilegeCommand(acDomain.GetAcSession(), entityId));
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Catalogs.Count);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Catalogs.Count);
        }
        #endregion

        #region TestUserGroups
        [TestMethod]
        public void TestUserGroups()
        {
            var acDomain = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(acDomain.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
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
            Guid entityId = Guid.NewGuid();
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
            Assert.AreEqual(0, acDomain.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Groups.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = acDomain.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.AreEqual(accountId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(groupId, privilegeBigram.ObjectInstanceId);
            Assert.AreEqual(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.AreEqual(AcElementType.Group.ToName(), privilegeBigram.ObjectType);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Groups.Count);
            acDomain.Handle(new RemovePrivilegeCommand(acDomain.GetAcSession(), entityId));
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Groups.Count);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Groups.Count);
        }
        #endregion

        #region TestUserMenus
        [TestMethod]
        public void TestUserMenus()
        {
            var acDomain = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(acDomain.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
            Guid menuId = Guid.NewGuid();
            acDomain.Handle(new MenuCreateInput
            {
                Id = menuId,
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                AppSystemId = acDomain.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }.ToCommand(acDomain.GetAcSession()));
            Guid entityId = Guid.NewGuid();
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = menuId,
                ObjectType = AcElementType.Menu.ToString()
            }));
            Assert.AreEqual(0, acDomain.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Menus.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = acDomain.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.AreEqual(accountId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(menuId, privilegeBigram.ObjectInstanceId);
            Assert.AreEqual(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.AreEqual(AcElementType.Menu.ToName(), privilegeBigram.ObjectType);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Menus.Count);
            acDomain.Handle(new RemovePrivilegeCommand(acDomain.GetAcSession(), entityId));
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Menus.Count);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Menus.Count);
        }
        #endregion

        #region TestUserAppSystems
        [TestMethod]
        public void TestUserAppSystems()
        {
            var acDomain = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(acDomain.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
            Guid appSystemId = Guid.NewGuid();
            acDomain.Handle(new AppSystemCreateInput
            {
                Id = appSystemId,
                Code = "app1",
                Name = "测试1",
                PrincipalId = acDomain.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(acDomain.GetAcSession()));
            Guid entityId = Guid.NewGuid();
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = appSystemId,
                ObjectType = AcElementType.AppSystem.ToString()
            }));
            Assert.AreEqual(0, acDomain.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.AppSystems.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = acDomain.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.AreEqual(accountId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(appSystemId, privilegeBigram.ObjectInstanceId);
            Assert.AreEqual(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.AreEqual(AcElementType.AppSystem.ToName(), privilegeBigram.ObjectType);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.AppSystems.Count);
            acDomain.Handle(new RemovePrivilegeCommand(acDomain.GetAcSession(), entityId));
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.AppSystems.Count);
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.AppSystems.Count);
        }
        #endregion

        #region TestUserRolePrivilege
        [TestMethod]
        public void TestUserRolePrivilege()
        {
            var acDomain = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(acDomain.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
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
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.AuthorizedRoles.Count);
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
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
            Assert.AreEqual(2, acDomain.GetAcSession().AccountPrivilege.AuthorizedRoles.Count);
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
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
            Assert.AreEqual(2, acDomain.GetAcSession().AccountPrivilege.AuthorizedRoles.Count);
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

            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
            Assert.AreEqual(3, acDomain.GetAcSession().AccountPrivilege.AuthorizedRoles.Count);// 用户的全部角色来自直接角色、目录角色、工作组角色三者的并集所以是三个角色。
        }
        #endregion

        #region TestUserFunctionPrivilege
        [TestMethod]
        public void TestUserFunctionPrivilege()
        {
            var acDomain = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
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
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Functions.Count);
            Assert.AreEqual(2, acDomain.GetAcSession().AccountPrivilege.AuthorizedFunctions.Count);
        }
        #endregion

        #region TestUserMenuPrivileges
        [TestMethod]
        public void TestUserMenuPrivileges()
        {
            var acDomain = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
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
            var menuId = Guid.NewGuid();
            acDomain.Handle(new MenuCreateInput
            {
                Id = menuId,
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                AppSystemId = acDomain.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }.ToCommand(acDomain.GetAcSession()));
            Guid entityId = Guid.NewGuid();
            // 授予角色菜单
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = menuId,
                ObjectType = AcElementType.Menu.ToString()
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
            menuId = Guid.NewGuid();
            acDomain.Handle(new MenuCreateInput
            {
                Id = menuId,
                Name = "测试2",
                Description = "test",
                SortCode = 10,
                AppSystemId = acDomain.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }.ToCommand(acDomain.GetAcSession()));
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = menuId,
                ObjectType = AcElementType.Menu.ToString()
            }));
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, acDomain.GetAcSession().AccountPrivilege.Menus.Count);
            Assert.AreEqual(2, acDomain.GetAcSession().AccountPrivilege.AuthorizedMenus.Count);
        }
        #endregion
    }
}
