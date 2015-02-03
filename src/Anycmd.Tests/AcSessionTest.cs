
namespace Anycmd.Tests
{
    using Ac.ViewModels.GroupViewModels;
    using Ac.ViewModels.Identity.AccountViewModels;
    using Ac.ViewModels.Infra.AppSystemViewModels;
    using Ac.ViewModels.Infra.CatalogViewModels;
    using Ac.ViewModels.Infra.FunctionViewModels;
    using Ac.ViewModels.Infra.MenuViewModels;
    using Ac.ViewModels.PrivilegeViewModels;
    using Ac.ViewModels.RoleViewModels;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Messages;
    using Engine.Ac.Messages.Infra;
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
            var host = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
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
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(host.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual("test1", host.GetAcSession().Identity.Name);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            Assert.IsFalse(host.GetAcSession().Identity.IsAuthenticated);
        }
        #endregion

        #region TestUserRoles
        [TestMethod]
        public void TestUserRoles()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(host.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Roles.Count);
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
            Assert.AreEqual(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            RoleState role;
            Assert.IsTrue(host.RoleSet.TryGetRole(roleId, out role));
            host.GetAcSession().AccountPrivilege.AddActiveRole(role);
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.AreEqual(accountId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(roleId, privilegeBigram.ObjectInstanceId);
            Assert.AreEqual(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.AreEqual(AcElementType.Role.ToName(), privilegeBigram.ObjectType);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            host.Handle(new RemovePrivilegeCommand(host.GetAcSession(), entityId));
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            host.GetAcSession().AccountPrivilege.DropActiveRole(role);
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Roles.Count);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Roles.Count);
        }
        #endregion

        #region TestUserFunctions
        [TestMethod]
        public void TestUserFunctions()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(host.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Roles.Count);
            Guid functionId = Guid.NewGuid();
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
            Assert.AreEqual(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Functions.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.AreEqual(accountId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(functionId, privilegeBigram.ObjectInstanceId);
            Assert.AreEqual(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.AreEqual(AcElementType.Function.ToName(), privilegeBigram.ObjectType);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Functions.Count);
            host.Handle(new RemovePrivilegeCommand(host.GetAcSession(), entityId));
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Functions.Count);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Functions.Count);
        }
        #endregion

        #region TestUserCatalogs
        [TestMethod]
        public void TestUserCatalogs()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(host.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Roles.Count);
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
            Guid entityId = Guid.NewGuid();
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
            Assert.AreEqual(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Catalogs.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.AreEqual(accountId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(catalogId, privilegeBigram.ObjectInstanceId);
            Assert.AreEqual(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.AreEqual(AcElementType.Catalog.ToName(), privilegeBigram.ObjectType);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Catalogs.Count);
            host.Handle(new RemovePrivilegeCommand(host.GetAcSession(), entityId));
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Catalogs.Count);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Catalogs.Count);
        }
        #endregion

        #region TestUserGroups
        [TestMethod]
        public void TestUserGroups()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(host.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Roles.Count);
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
            Guid entityId = Guid.NewGuid();
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
            Assert.AreEqual(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Groups.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.AreEqual(accountId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(groupId, privilegeBigram.ObjectInstanceId);
            Assert.AreEqual(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.AreEqual(AcElementType.Group.ToName(), privilegeBigram.ObjectType);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Groups.Count);
            host.Handle(new RemovePrivilegeCommand(host.GetAcSession(), entityId));
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Groups.Count);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Groups.Count);
        }
        #endregion

        #region TestUserMenus
        [TestMethod]
        public void TestUserMenus()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(host.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Roles.Count);
            Guid menuId = Guid.NewGuid();
            host.Handle(new MenuCreateInput
            {
                Id = menuId,
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }.ToCommand(host.GetAcSession()));
            Guid entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = menuId,
                ObjectType = AcElementType.Menu.ToString()
            }));
            Assert.AreEqual(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Menus.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.AreEqual(accountId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(menuId, privilegeBigram.ObjectInstanceId);
            Assert.AreEqual(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.AreEqual(AcElementType.Menu.ToName(), privilegeBigram.ObjectType);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Menus.Count);
            host.Handle(new RemovePrivilegeCommand(host.GetAcSession(), entityId));
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Menus.Count);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Menus.Count);
        }
        #endregion

        #region TestUserAppSystems
        [TestMethod]
        public void TestUserAppSystems()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(host.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Roles.Count);
            Guid appSystemId = Guid.NewGuid();
            host.Handle(new AppSystemCreateInput
            {
                Id = appSystemId,
                Code = "app1",
                Name = "测试1",
                PrincipalId = host.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(host.GetAcSession()));
            Guid entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = appSystemId,
                ObjectType = AcElementType.AppSystem.ToString()
            }));
            Assert.AreEqual(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.AppSystems.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.AreEqual(accountId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(appSystemId, privilegeBigram.ObjectInstanceId);
            Assert.AreEqual(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.AreEqual(AcElementType.AppSystem.ToName(), privilegeBigram.ObjectType);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.AppSystems.Count);
            host.Handle(new RemovePrivilegeCommand(host.GetAcSession(), entityId));
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.AppSystems.Count);
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.AppSystems.Count);
        }
        #endregion

        #region TestUserRolePrivilege
        [TestMethod]
        public void TestUserRolePrivilege()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
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
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(host.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Roles.Count);
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
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.AuthorizedRoles.Count);
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
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            Assert.AreEqual(2, host.GetAcSession().AccountPrivilege.AuthorizedRoles.Count);
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
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            Assert.AreEqual(2, host.GetAcSession().AccountPrivilege.AuthorizedRoles.Count);
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

            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            Assert.AreEqual(3, host.GetAcSession().AccountPrivilege.AuthorizedRoles.Count);// 用户的全部角色来自直接角色、目录角色、工作组角色三者的并集所以是三个角色。
        }
        #endregion

        #region TestUserFunctionPrivilege
        [TestMethod]
        public void TestUserFunctionPrivilege()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
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
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Functions.Count);
            Assert.AreEqual(2, host.GetAcSession().AccountPrivilege.AuthorizedFunctions.Count);
        }
        #endregion

        #region TestUserMenuPrivileges
        [TestMethod]
        public void TestUserMenuPrivileges()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
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
            var menuId = Guid.NewGuid();
            host.Handle(new MenuCreateInput
            {
                Id = menuId,
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }.ToCommand(host.GetAcSession()));
            Guid entityId = Guid.NewGuid();
            // 授予角色菜单
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
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
            menuId = Guid.NewGuid();
            host.Handle(new MenuCreateInput
            {
                Id = menuId,
                Name = "测试2",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }.ToCommand(host.GetAcSession()));
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = menuId,
                ObjectType = AcElementType.Menu.ToString()
            }));
            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.AreEqual(1, host.GetAcSession().AccountPrivilege.Menus.Count);
            Assert.AreEqual(2, host.GetAcSession().AccountPrivilege.AuthorizedMenus.Count);
        }
        #endregion
    }
}
