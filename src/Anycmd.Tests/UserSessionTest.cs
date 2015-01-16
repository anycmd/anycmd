
namespace Anycmd.Tests
{
    using Ac.ViewModels.GroupViewModels;
    using Ac.ViewModels.Identity.AccountViewModels;
    using Ac.ViewModels.Infra.AppSystemViewModels;
    using Ac.ViewModels.Infra.DicViewModels;
    using Ac.ViewModels.Infra.FunctionViewModels;
    using Ac.ViewModels.Infra.MenuViewModels;
    using Ac.ViewModels.Infra.OrganizationViewModels;
    using Ac.ViewModels.RoleViewModels;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages;
    using Engine.Ac.Messages.Identity;
    using Engine.Ac.Messages.Infra;
    using Engine.Ac.Messages.Rbac;
    using Engine.Host.Ac;
    using Engine.Host.Ac.Identity;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Util;
    using Xunit;

    public class UserSessionTest
    {
        #region TestSignInSignOut
        [Fact]
        public void TestSignInSignOut()
        {
            var host = TestHelper.GetAcDomain();
            UserSessionState.SignOut(host, host.GetUserSession());
            var orgId = Guid.NewGuid();

            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Guid dicId = Guid.NewGuid();
            host.Handle(new AddDicCommand(new DicCreateInput
            {
                Id = dicId,
                Code = "auditStatus",
                Name = "auditStatus1"
            }));
            host.Handle(new AddDicItemCommand(new DicItemCreateInput
            {
                Id = dicId,
                IsEnabled = 1,
                DicId = dicId,
                SortCode = 0,
                Description = string.Empty,
                Code = "auditPass",
                Name = "auditPass"
            }));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AddAccountCommand(new AccountCreateInput
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                LoginName = "test",
                Password = "111111",
                OrganizationCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }));
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetUserSession().Identity.IsAuthenticated);
            Assert.Equal("test", host.GetUserSession().Identity.Name);
            UserSessionState.SignOut(host, host.GetUserSession());
            Assert.False(host.GetUserSession().Identity.IsAuthenticated);
        }
        #endregion

        #region TestUserRoles
        [Fact]
        public void TestUserRoles()
        {
            var host = TestHelper.GetAcDomain();
            UserSessionState.SignOut(host, host.GetUserSession());
            var orgId = Guid.NewGuid();

            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Guid dicId = Guid.NewGuid();
            host.Handle(new AddDicCommand(new DicCreateInput
            {
                Id = dicId,
                Code = "auditStatus",
                Name = "auditStatus1"
            }));
            host.Handle(new AddDicItemCommand(new DicItemCreateInput
            {
                Id = dicId,
                IsEnabled = 1,
                DicId = dicId,
                SortCode = 0,
                Description = string.Empty,
                Code = "auditPass",
                Name = "auditPass"
            }));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AddAccountCommand(new AccountCreateInput
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                LoginName = "test",
                Password = "111111",
                OrganizationCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }));
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetUserSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Roles.Count);
            Guid roleId = Guid.NewGuid();
            host.Handle(new AddRoleCommand(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }));
            Guid entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
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
            host.GetUserSession().AccountPrivilege.AddActiveRole(role);
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Roles.Count);
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(roleId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcElementType.Role.ToName(), privilegeBigram.ObjectType);
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Roles.Count);
            host.Handle(new RemovePrivilegeCommand(entityId));
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Roles.Count);
            host.GetUserSession().AccountPrivilege.DropActiveRole(role);
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Roles.Count);
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Roles.Count);
        }
        #endregion

        #region TestUserFunctions
        [Fact]
        public void TestUserFunctions()
        {
            var host = TestHelper.GetAcDomain();
            UserSessionState.SignOut(host, host.GetUserSession());
            var orgId = Guid.NewGuid();

            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Guid dicId = Guid.NewGuid();
            host.Handle(new AddDicCommand(new DicCreateInput
            {
                Id = dicId,
                Code = "auditStatus",
                Name = "auditStatus1"
            }));
            host.Handle(new AddDicItemCommand(new DicItemCreateInput
            {
                Id = dicId,
                IsEnabled = 1,
                DicId = dicId,
                SortCode = 0,
                Description = string.Empty,
                Code = "auditPass",
                Name = "auditPass"
            }));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AddAccountCommand(new AccountCreateInput
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                LoginName = "test",
                Password = "111111",
                OrganizationCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }));
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetUserSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Roles.Count);
            Guid functionId = Guid.NewGuid();
            host.Handle(new AddFunctionCommand(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }));
            Guid entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
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
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Functions.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(functionId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcElementType.Function.ToName(), privilegeBigram.ObjectType);
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Functions.Count);
            host.Handle(new RemovePrivilegeCommand(entityId));
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Functions.Count);
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Functions.Count);
        }
        #endregion

        #region TestUserOrganizations
        [Fact]
        public void TestUserOrganizations()
        {
            var host = TestHelper.GetAcDomain();
            UserSessionState.SignOut(host, host.GetUserSession());
            var orgId = Guid.NewGuid();

            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Guid dicId = Guid.NewGuid();
            host.Handle(new AddDicCommand(new DicCreateInput
            {
                Id = dicId,
                Code = "auditStatus",
                Name = "auditStatus1"
            }));
            host.Handle(new AddDicItemCommand(new DicItemCreateInput
            {
                Id = dicId,
                IsEnabled = 1,
                DicId = dicId,
                SortCode = 0,
                Description = string.Empty,
                Code = "auditPass",
                Name = "auditPass"
            }));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AddAccountCommand(new AccountCreateInput
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                LoginName = "test",
                Password = "111111",
                OrganizationCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }));
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetUserSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Roles.Count);
            Guid organizationId = Guid.NewGuid();
            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = organizationId,
                Code = "110",
                Name = "测试110",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Guid entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = organizationId,
                ObjectType = AcElementType.Organization.ToString()
            }));
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Organizations.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(organizationId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcElementType.Organization.ToName(), privilegeBigram.ObjectType);
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Organizations.Count);
            host.Handle(new RemovePrivilegeCommand(entityId));
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Organizations.Count);
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Organizations.Count);
        }
        #endregion

        #region TestUserGroups
        [Fact]
        public void TestUserGroups()
        {
            var host = TestHelper.GetAcDomain();
            UserSessionState.SignOut(host, host.GetUserSession());
            var orgId = Guid.NewGuid();

            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Guid dicId = Guid.NewGuid();
            host.Handle(new AddDicCommand(new DicCreateInput
            {
                Id = dicId,
                Code = "auditStatus",
                Name = "auditStatus1"
            }));
            host.Handle(new AddDicItemCommand(new DicItemCreateInput
            {
                Id = dicId,
                IsEnabled = 1,
                DicId = dicId,
                SortCode = 0,
                Description = string.Empty,
                Code = "auditPass",
                Name = "auditPass"
            }));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AddAccountCommand(new AccountCreateInput
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                LoginName = "test",
                Password = "111111",
                OrganizationCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }));
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetUserSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Roles.Count);
            Guid groupId = Guid.NewGuid();
            host.Handle(new AddGroupCommand(new GroupCreateInput
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
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
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
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Groups.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(groupId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcElementType.Group.ToName(), privilegeBigram.ObjectType);
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Groups.Count);
            host.Handle(new RemovePrivilegeCommand(entityId));
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Groups.Count);
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Groups.Count);
        }
        #endregion

        #region TestUserMenus
        [Fact]
        public void TestUserMenus()
        {
            var host = TestHelper.GetAcDomain();
            UserSessionState.SignOut(host, host.GetUserSession());
            var orgId = Guid.NewGuid();

            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Guid dicId = Guid.NewGuid();
            host.Handle(new AddDicCommand(new DicCreateInput
            {
                Id = dicId,
                Code = "auditStatus",
                Name = "auditStatus1"
            }));
            host.Handle(new AddDicItemCommand(new DicItemCreateInput
            {
                Id = dicId,
                IsEnabled = 1,
                DicId = dicId,
                SortCode = 0,
                Description = string.Empty,
                Code = "auditPass",
                Name = "auditPass"
            }));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AddAccountCommand(new AccountCreateInput
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                LoginName = "test",
                Password = "111111",
                OrganizationCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }));
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetUserSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Roles.Count);
            Guid menuId = Guid.NewGuid();
            host.Handle(new AddMenuCommand(new MenuCreateInput
            {
                Id = menuId,
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }));
            Guid entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
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
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Menus.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(menuId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcElementType.Menu.ToName(), privilegeBigram.ObjectType);
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Menus.Count);
            host.Handle(new RemovePrivilegeCommand(entityId));
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Menus.Count);
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Menus.Count);
        }
        #endregion

        #region TestUserAppSystems
        [Fact]
        public void TestUserAppSystems()
        {
            var host = TestHelper.GetAcDomain();
            UserSessionState.SignOut(host, host.GetUserSession());
            var orgId = Guid.NewGuid();

            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Guid dicId = Guid.NewGuid();
            host.Handle(new AddDicCommand(new DicCreateInput
            {
                Id = dicId,
                Code = "auditStatus",
                Name = "auditStatus1"
            }));
            host.Handle(new AddDicItemCommand(new DicItemCreateInput
            {
                Id = dicId,
                IsEnabled = 1,
                DicId = dicId,
                SortCode = 0,
                Description = string.Empty,
                Code = "auditPass",
                Name = "auditPass"
            }));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AddAccountCommand(new AccountCreateInput
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                LoginName = "test",
                Password = "111111",
                OrganizationCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }));
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetUserSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Roles.Count);
            Guid appSystemId = Guid.NewGuid();
            host.Handle(new AddAppSystemCommand(new AppSystemCreateInput
            {
                Id = appSystemId,
                Code = "app1",
                Name = "测试1",
                PrincipalId = host.SysUserSet.GetDevAccounts().First().Id
            }));
            Guid entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
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
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.AppSystems.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(appSystemId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(UserAcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcElementType.AppSystem.ToName(), privilegeBigram.ObjectType);
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.AppSystems.Count);
            host.Handle(new RemovePrivilegeCommand(entityId));
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.AppSystems.Count);
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.AppSystems.Count);
        }
        #endregion

        #region TestUserRolePrivilege
        [Fact]
        public void TestUserRolePrivilege()
        {
            var host = TestHelper.GetAcDomain();
            UserSessionState.SignOut(host, host.GetUserSession());
            var orgId = Guid.NewGuid();

            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Guid dicId = Guid.NewGuid();
            host.Handle(new AddDicCommand(new DicCreateInput
            {
                Id = dicId,
                Code = "auditStatus",
                Name = "auditStatus1"
            }));
            host.Handle(new AddDicItemCommand(new DicItemCreateInput
            {
                Id = dicId,
                IsEnabled = 1,
                DicId = dicId,
                SortCode = 0,
                Description = string.Empty,
                Code = "auditPass",
                Name = "auditPass"
            }));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AddAccountCommand(new AccountCreateInput
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                LoginName = "test",
                Password = "111111",
                OrganizationCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }));
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetUserSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Roles.Count);
            Guid roleId = Guid.NewGuid();
            host.Handle(new AddRoleCommand(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }));
            Guid entityId = Guid.NewGuid();
            // 授予账户角色
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
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
            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = organizationId,
                Code = "110",
                Name = "测试110",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            entityId = Guid.NewGuid();
            // 授予账户组织结构
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
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
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
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
            host.Handle(new AddGroupCommand(new GroupCreateInput
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
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
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
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Roles.Count);
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.AuthorizedRoles.Count);
            roleId = Guid.NewGuid();
            // 添加一个新角色并将该角色授予上面创建的组织结构
            host.Handle(new AddRoleCommand(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = organizationId,
                SubjectType = UserAcSubjectType.Organization.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId,
                ObjectType = AcElementType.Role.ToString()
            }));
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Roles.Count);
            Assert.Equal(2, host.GetUserSession().AccountPrivilege.AuthorizedRoles.Count);
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));
            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Roles.Count);
            Assert.Equal(2, host.GetUserSession().AccountPrivilege.AuthorizedRoles.Count);
            roleId = Guid.NewGuid();
            // 添加一个新角色并将该角色授予上面创建的工作组
            host.Handle(new AddRoleCommand(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试3",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }));
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));

            UserSessionState.SignOut(host, host.GetUserSession());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Roles.Count);
            Assert.Equal(3, host.GetUserSession().AccountPrivilege.AuthorizedRoles.Count);// 用户的全部角色来自直接角色、组织结构角色、工作组角色三者的并集所以是三个角色。
        }
        #endregion

        #region TestUserFunctionPrivilege
        [Fact]
        public void TestUserFunctionPrivilege()
        {
            var host = TestHelper.GetAcDomain();
            UserSessionState.SignOut(host, host.GetUserSession());
            var orgId = Guid.NewGuid();

            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Guid dicId = Guid.NewGuid();
            host.Handle(new AddDicCommand(new DicCreateInput
            {
                Id = dicId,
                Code = "auditStatus",
                Name = "auditStatus1"
            }));
            host.Handle(new AddDicItemCommand(new DicItemCreateInput
            {
                Id = dicId,
                IsEnabled = 1,
                DicId = dicId,
                SortCode = 0,
                Description = string.Empty,
                Code = "auditPass",
                Name = "auditPass"
            }));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AddAccountCommand(new AccountCreateInput
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                LoginName = "test",
                Password = "111111",
                OrganizationCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }));
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            Guid roleId = Guid.NewGuid();
            host.Handle(new AddRoleCommand(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }));
            var functionId = Guid.NewGuid();
            host.Handle(new AddFunctionCommand(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }));
            Guid entityId = Guid.NewGuid();
            // 授予角色功能
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
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
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
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
            host.Handle(new AddFunctionCommand(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun2",
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }));
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = functionId,
                ObjectType = AcElementType.Function.ToString()
            }));
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Functions.Count);
            Assert.Equal(2, host.GetUserSession().AccountPrivilege.AuthorizedFunctions.Count);
        }
        #endregion

        #region TestUserMenuPrivileges
        [Fact]
        public void TestUserMenuPrivileges()
        {
            var host = TestHelper.GetAcDomain();
            UserSessionState.SignOut(host, host.GetUserSession());
            var orgId = Guid.NewGuid();

            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = orgId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Guid dicId = Guid.NewGuid();
            host.Handle(new AddDicCommand(new DicCreateInput
            {
                Id = dicId,
                Code = "auditStatus",
                Name = "auditStatus1"
            }));
            host.Handle(new AddDicItemCommand(new DicItemCreateInput
            {
                Id = dicId,
                IsEnabled = 1,
                DicId = dicId,
                SortCode = 0,
                Description = string.Empty,
                Code = "auditPass",
                Name = "auditPass"
            }));
            Guid accountId = Guid.NewGuid();
            host.Handle(new AddAccountCommand(new AccountCreateInput
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                LoginName = "test",
                Password = "111111",
                OrganizationCode = "100",
                IsEnabled = 1,
                AuditState = "auditPass"
            }));
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            Guid roleId = Guid.NewGuid();
            host.Handle(new AddRoleCommand(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }));
            var menuId = Guid.NewGuid();
            host.Handle(new AddMenuCommand(new MenuCreateInput
            {
                Id = menuId,
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }));
            Guid entityId = Guid.NewGuid();
            // 授予角色菜单
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
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
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
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
            host.Handle(new AddMenuCommand(new MenuCreateInput
            {
                Id = menuId,
                Name = "测试2",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }));
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = menuId,
                ObjectType = AcElementType.Menu.ToString()
            }));
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.GetUserSession().AccountPrivilege.Menus.Count);
            Assert.Equal(2, host.GetUserSession().AccountPrivilege.AuthorizedMenus.Count);
        }
        #endregion
    }
}
