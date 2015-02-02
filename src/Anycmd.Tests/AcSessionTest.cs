
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
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Util;
    using Xunit;

    public class AcSessionTest
    {
        #region TestSignInSignOut
        [Fact]
        public void TestSignInSignOut()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
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
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetAcSession().Identity.IsAuthenticated);
            Assert.Equal("test1", host.GetAcSession().Identity.Name);
            AcSessionState.SignOut(host, host.GetAcSession());
            Assert.False(host.GetAcSession().Identity.IsAuthenticated);
        }
        #endregion

        #region TestUserRoles
        [Fact]
        public void TestUserRoles()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
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
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetAcSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Roles.Count);
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
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            RoleState role;
            Assert.True(host.RoleSet.TryGetRole(roleId, out role));
            host.GetAcSession().AccountPrivilege.AddActiveRole(role);
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(roleId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcElementType.Role.ToName(), privilegeBigram.ObjectType);
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            host.Handle(new RemovePrivilegeCommand(host.GetAcSession(), entityId));
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            host.GetAcSession().AccountPrivilege.DropActiveRole(role);
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Roles.Count);
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Roles.Count);
        }
        #endregion

        #region TestUserFunctions
        [Fact]
        public void TestUserFunctions()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
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
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetAcSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Roles.Count);
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
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Functions.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(functionId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcElementType.Function.ToName(), privilegeBigram.ObjectType);
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Functions.Count);
            host.Handle(new RemovePrivilegeCommand(host.GetAcSession(), entityId));
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Functions.Count);
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Functions.Count);
        }
        #endregion

        #region TestUserCatalogs
        [Fact]
        public void TestUserCatalogs()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
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
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetAcSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Roles.Count);
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
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Catalogs.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(catalogId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcElementType.Catalog.ToName(), privilegeBigram.ObjectType);
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Catalogs.Count);
            host.Handle(new RemovePrivilegeCommand(host.GetAcSession(), entityId));
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Catalogs.Count);
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Catalogs.Count);
        }
        #endregion

        #region TestUserGroups
        [Fact]
        public void TestUserGroups()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
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
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetAcSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Roles.Count);
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
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Groups.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(groupId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcElementType.Group.ToName(), privilegeBigram.ObjectType);
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Groups.Count);
            host.Handle(new RemovePrivilegeCommand(host.GetAcSession(), entityId));
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Groups.Count);
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Groups.Count);
        }
        #endregion

        #region TestUserMenus
        [Fact]
        public void TestUserMenus()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
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
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetAcSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Roles.Count);
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
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Menus.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(menuId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcElementType.Menu.ToName(), privilegeBigram.ObjectType);
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Menus.Count);
            host.Handle(new RemovePrivilegeCommand(host.GetAcSession(), entityId));
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Menus.Count);
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Menus.Count);
        }
        #endregion

        #region TestUserAppSystems
        [Fact]
        public void TestUserAppSystems()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
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
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetAcSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Roles.Count);
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
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.AppSystems.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(appSystemId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcElementType.AppSystem.ToName(), privilegeBigram.ObjectType);
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.AppSystems.Count);
            host.Handle(new RemovePrivilegeCommand(host.GetAcSession(), entityId));
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.AppSystems.Count);
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.AppSystems.Count);
        }
        #endregion

        #region TestUserRolePrivilege
        [Fact]
        public void TestUserRolePrivilege()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
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
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetAcSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetAcSession().AccountPrivilege.Roles.Count);
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
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.AuthorizedRoles.Count);
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
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            Assert.Equal(2, host.GetAcSession().AccountPrivilege.AuthorizedRoles.Count);
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
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            Assert.Equal(2, host.GetAcSession().AccountPrivilege.AuthorizedRoles.Count);
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

            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Roles.Count);
            Assert.Equal(3, host.GetAcSession().AccountPrivilege.AuthorizedRoles.Count);// 用户的全部角色来自直接角色、目录角色、工作组角色三者的并集所以是三个角色。
        }
        #endregion

        #region TestUserFunctionPrivilege
        [Fact]
        public void TestUserFunctionPrivilege()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
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
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Functions.Count);
            Assert.Equal(2, host.GetAcSession().AccountPrivilege.AuthorizedFunctions.Count);
        }
        #endregion

        #region TestUserMenuPrivileges
        [Fact]
        public void TestUserMenuPrivileges()
        {
            var host = TestHelper.GetAcDomain();
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
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
            AcSessionState.SignOut(host, host.GetAcSession());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test1"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetAcSession().AccountPrivilege.Menus.Count);
            Assert.Equal(2, host.GetAcSession().AccountPrivilege.AuthorizedMenus.Count);
        }
        #endregion
    }
}
