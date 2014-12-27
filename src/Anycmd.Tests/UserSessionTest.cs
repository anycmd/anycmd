
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
            host.SignOut();
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
            Assert.NotNull(host.GetRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.UserSession.Principal.Identity.IsAuthenticated);
            Assert.Equal("test", host.UserSession.Principal.Identity.Name);
            host.SignOut();
            Assert.False(host.UserSession.Principal.Identity.IsAuthenticated);
        }
        #endregion

        #region TestUserRoles
        [Fact]
        public void TestUserRoles()
        {
            var host = TestHelper.GetAcDomain();
            host.SignOut();
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
            Assert.NotNull(host.GetRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.UserSession.Principal.Identity.IsAuthenticated);
            Assert.Equal(0, host.UserSession.AccountPrivilege.Roles.Count);
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
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = AcSubjectType.Account.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = roleId,
                ObjectType = AcObjectType.Role.ToString()
            }));
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            RoleState role;
            Assert.True(host.RoleSet.TryGetRole(roleId, out role));
            host.UserSession.AccountPrivilege.AddActiveRole(role);
            Assert.Equal(1, host.UserSession.AccountPrivilege.Roles.Count);
            var privilegeBigram = host.GetRequiredService<IRepository<PrivilegeBigram>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(roleId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(AcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcObjectType.Role.ToName(), privilegeBigram.ObjectType);
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.UserSession.AccountPrivilege.Roles.Count);
            host.Handle(new RemovePrivilegeBigramCommand(entityId));
            Assert.Equal(1, host.UserSession.AccountPrivilege.Roles.Count);
            host.UserSession.AccountPrivilege.DropActiveRole(role);
            Assert.Equal(0, host.UserSession.AccountPrivilege.Roles.Count);
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.UserSession.AccountPrivilege.Roles.Count);
        }
        #endregion

        #region TestUserFunctions
        [Fact]
        public void TestUserFunctions()
        {
            var host = TestHelper.GetAcDomain();
            host.SignOut();
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
            Assert.NotNull(host.GetRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.UserSession.Principal.Identity.IsAuthenticated);
            Assert.Equal(0, host.UserSession.AccountPrivilege.Roles.Count);
            Guid functionId = Guid.NewGuid();
            host.Handle(new AddFunctionCommand(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = host.SysUsers.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }));
            Guid entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = AcSubjectType.Account.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = functionId,
                ObjectType = AcObjectType.Function.ToString()
            }));
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.Equal(0, host.UserSession.AccountPrivilege.Functions.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.GetRequiredService<IRepository<PrivilegeBigram>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(functionId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(AcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcObjectType.Function.ToName(), privilegeBigram.ObjectType);
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.UserSession.AccountPrivilege.Functions.Count);
            host.Handle(new RemovePrivilegeBigramCommand(entityId));
            Assert.Equal(1, host.UserSession.AccountPrivilege.Functions.Count);
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.UserSession.AccountPrivilege.Functions.Count);
        }
        #endregion

        #region TestUserOrganizations
        [Fact]
        public void TestUserOrganizations()
        {
            var host = TestHelper.GetAcDomain();
            host.SignOut();
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
            Assert.NotNull(host.GetRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.UserSession.Principal.Identity.IsAuthenticated);
            Assert.Equal(0, host.UserSession.AccountPrivilege.Roles.Count);
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
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = AcSubjectType.Account.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = organizationId,
                ObjectType = AcObjectType.Organization.ToString()
            }));
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.Equal(0, host.UserSession.AccountPrivilege.Organizations.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.GetRequiredService<IRepository<PrivilegeBigram>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(organizationId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(AcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcObjectType.Organization.ToName(), privilegeBigram.ObjectType);
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.UserSession.AccountPrivilege.Organizations.Count);
            host.Handle(new RemovePrivilegeBigramCommand(entityId));
            Assert.Equal(1, host.UserSession.AccountPrivilege.Organizations.Count);
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.UserSession.AccountPrivilege.Organizations.Count);
        }
        #endregion

        #region TestUserGroups
        [Fact]
        public void TestUserGroups()
        {
            var host = TestHelper.GetAcDomain();
            host.SignOut();
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
            Assert.NotNull(host.GetRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.UserSession.Principal.Identity.IsAuthenticated);
            Assert.Equal(0, host.UserSession.AccountPrivilege.Roles.Count);
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
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = AcSubjectType.Account.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = groupId,
                ObjectType = AcObjectType.Group.ToString()
            }));
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.Equal(0, host.UserSession.AccountPrivilege.Groups.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.GetRequiredService<IRepository<PrivilegeBigram>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(groupId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(AcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcObjectType.Group.ToName(), privilegeBigram.ObjectType);
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.UserSession.AccountPrivilege.Groups.Count);
            host.Handle(new RemovePrivilegeBigramCommand(entityId));
            Assert.Equal(1, host.UserSession.AccountPrivilege.Groups.Count);
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.UserSession.AccountPrivilege.Groups.Count);
        }
        #endregion

        #region TestUserMenus
        [Fact]
        public void TestUserMenus()
        {
            var host = TestHelper.GetAcDomain();
            host.SignOut();
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
            Assert.NotNull(host.GetRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.UserSession.Principal.Identity.IsAuthenticated);
            Assert.Equal(0, host.UserSession.AccountPrivilege.Roles.Count);
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
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = AcSubjectType.Account.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = menuId,
                ObjectType = AcObjectType.Menu.ToString()
            }));
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.Equal(0, host.UserSession.AccountPrivilege.Menus.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.GetRequiredService<IRepository<PrivilegeBigram>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(menuId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(AcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcObjectType.Menu.ToName(), privilegeBigram.ObjectType);
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.UserSession.AccountPrivilege.Menus.Count);
            host.Handle(new RemovePrivilegeBigramCommand(entityId));
            Assert.Equal(1, host.UserSession.AccountPrivilege.Menus.Count);
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.UserSession.AccountPrivilege.Menus.Count);
        }
        #endregion

        #region TestUserAppSystems
        [Fact]
        public void TestUserAppSystems()
        {
            var host = TestHelper.GetAcDomain();
            host.SignOut();
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
            Assert.NotNull(host.GetRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.UserSession.Principal.Identity.IsAuthenticated);
            Assert.Equal(0, host.UserSession.AccountPrivilege.Roles.Count);
            Guid appSystemId = Guid.NewGuid();
            host.Handle(new AddAppSystemCommand(new AppSystemCreateInput
            {
                Id = appSystemId,
                Code = "app1",
                Name = "测试1",
                PrincipalId = host.SysUsers.GetDevAccounts().First().Id
            }));
            Guid entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = AcSubjectType.Account.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = appSystemId,
                ObjectType = AcObjectType.AppSystem.ToString()
            }));
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            Assert.Equal(0, host.UserSession.AccountPrivilege.AppSystems.Count);// 需要重新登录才能激活新添加的用户功能授权所以为0
            var privilegeBigram = host.GetRequiredService<IRepository<PrivilegeBigram>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(appSystemId, privilegeBigram.ObjectInstanceId);
            Assert.Equal(AcSubjectType.Account.ToName(), privilegeBigram.SubjectType);
            Assert.Equal(AcObjectType.AppSystem.ToName(), privilegeBigram.ObjectType);
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.UserSession.AccountPrivilege.AppSystems.Count);
            host.Handle(new RemovePrivilegeBigramCommand(entityId));
            Assert.Equal(1, host.UserSession.AccountPrivilege.AppSystems.Count);
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(0, host.UserSession.AccountPrivilege.AppSystems.Count);
        }
        #endregion

        #region TestUserRolePrivilege
        [Fact]
        public void TestUserRolePrivilege()
        {
            var host = TestHelper.GetAcDomain();
            host.SignOut();
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
            Assert.NotNull(host.GetRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.UserSession.Principal.Identity.IsAuthenticated);
            Assert.Equal(0, host.UserSession.AccountPrivilege.Roles.Count);
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
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = AcSubjectType.Account.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = roleId,
                ObjectType = AcObjectType.Role.ToString()
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
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = AcSubjectType.Account.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = organizationId,
                ObjectType = AcObjectType.Organization.ToString()
            }));
            // 授予组织结构角色
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = organizationId,
                SubjectType = AcSubjectType.Organization.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = roleId,
                ObjectType = AcObjectType.Role.ToString()
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
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = AcSubjectType.Account.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = groupId,
                ObjectType = AcObjectType.Group.ToString()
            }));
            // 授予工作组角色
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = AcSubjectType.Role.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = groupId,
                ObjectType = AcObjectType.Group.ToString()
            }));
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.UserSession.AccountPrivilege.Roles.Count);
            Assert.Equal(1, host.UserSession.AccountPrivilege.AuthorizedRoles.Count);
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
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = organizationId,
                SubjectType = AcSubjectType.Organization.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = roleId,
                ObjectType = AcObjectType.Role.ToString()
            }));
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.UserSession.AccountPrivilege.Roles.Count);
            Assert.Equal(2, host.UserSession.AccountPrivilege.AuthorizedRoles.Count);
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = AcSubjectType.Role.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = groupId,
                ObjectType = AcObjectType.Group.ToString()
            }));
            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.UserSession.AccountPrivilege.Roles.Count);
            Assert.Equal(2, host.UserSession.AccountPrivilege.AuthorizedRoles.Count);
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
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = AcSubjectType.Role.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = groupId,
                ObjectType = AcObjectType.Group.ToString()
            }));

            host.SignOut();
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.UserSession.AccountPrivilege.Roles.Count);
            Assert.Equal(3, host.UserSession.AccountPrivilege.AuthorizedRoles.Count);// 用户的全部角色来自直接角色、组织结构角色、工作组角色三者的并集所以是三个角色。
        }
        #endregion

        #region TestUserFunctionPrivilege
        [Fact]
        public void TestUserFunctionPrivilege()
        {
            var host = TestHelper.GetAcDomain();
            host.SignOut();
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
            Assert.NotNull(host.GetRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
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
                DeveloperId = host.SysUsers.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }));
            Guid entityId = Guid.NewGuid();
            // 授予角色功能
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = AcSubjectType.Role.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = functionId,
                ObjectType = AcObjectType.Function.ToString()
            }));
            // 授予账户角色
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = AcSubjectType.Account.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = roleId,
                ObjectType = AcObjectType.Role.ToString()
            }));
            entityId = Guid.NewGuid();
            functionId = Guid.NewGuid();
            host.Handle(new AddFunctionCommand(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun2",
                Description = string.Empty,
                DeveloperId = host.SysUsers.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }));
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = AcSubjectType.Account.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = functionId,
                ObjectType = AcObjectType.Function.ToString()
            }));
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.UserSession.AccountPrivilege.Functions.Count);
            Assert.Equal(2, host.UserSession.AccountPrivilege.AuthorizedFunctions.Count);
        }
        #endregion

        #region TestUserMenuPrivileges
        [Fact]
        public void TestUserMenuPrivileges()
        {
            var host = TestHelper.GetAcDomain();
            host.SignOut();
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
            Assert.NotNull(host.GetRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
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
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = AcSubjectType.Role.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = menuId,
                ObjectType = AcObjectType.Menu.ToString()
            }));
            // 授予账户角色
            entityId = Guid.NewGuid();
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = AcSubjectType.Account.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = roleId,
                ObjectType = AcObjectType.Role.ToString()
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
            host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = AcSubjectType.Account.ToString(),// 主体是账户
                PrivilegeConstraint = null,
                PrivilegeOrientation = 1,
                ObjectInstanceId = menuId,
                ObjectType = AcObjectType.Menu.ToString()
            }));
            host.SignIn(new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.Equal(1, host.UserSession.AccountPrivilege.Menus.Count);
            Assert.Equal(2, host.UserSession.AccountPrivilege.AuthorizedMenus.Count);
        }
        #endregion
    }
}
