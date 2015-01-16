
namespace Anycmd.Tests
{
    using Ac.ViewModels.DsdViewModels;
    using Ac.ViewModels.Identity.AccountViewModels;
    using Ac.ViewModels.Infra.DicViewModels;
    using Ac.ViewModels.Infra.OrganizationViewModels;
    using Ac.ViewModels.PrivilegeViewModels;
    using Ac.ViewModels.RoleViewModels;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Messages;
    using Engine.Ac.Messages.Identity;
    using Engine.Ac.Messages.Infra;
    using Engine.Ac.Messages.Rbac;
    using Engine.Host.Ac.Identity;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class DsdSetTest
    {
        [Fact]
        public void DsdSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.DsdSetSet.Count());

            var entityId = Guid.NewGuid();

            DsdSetState dsdSetById;
            host.Handle(new AddDsdSetCommand(new DsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.Equal(1, host.DsdSetSet.Count());
            Assert.True(host.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));

            host.Handle(new UpdateDsdSetCommand(new DsdSetUpdateIo
            {
                Id = entityId,
                Name = "test2",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.Equal(1, host.DsdSetSet.Count());
            Assert.True(host.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
            Assert.Equal("test2", dsdSetById.Name);

            host.Handle(new RemoveDsdSetCommand(entityId));
            Assert.False(host.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
            Assert.Equal(0, host.DsdSetSet.Count());
        }

        [Fact]
        public void TestDsdRole()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.DsdSetSet.Count());

            var dsdSetId = Guid.NewGuid();

            DsdSetState dsdSetById;
            host.Handle(new AddDsdSetCommand(new DsdSetCreateIo
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
            Assert.Equal(1, host.RoleSet.Count());
            Assert.True(host.RoleSet.TryGetRole(roleId, out roleById));
            var entityId = Guid.NewGuid();
            host.Handle(new AddDsdRoleCommand(new DsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId,
                DsdSetId = dsdSetId
            }));
            Assert.Equal(1, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
            host.Handle(new RemoveDsdRoleCommand(entityId));
            Assert.Equal(0, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
        }

        [Fact]
        public void CheckDsdSetRoles()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.DsdSetSet.Count());

            var ssdSetId = Guid.NewGuid();

            DsdSetState ssdSetById;
            host.Handle(new AddDsdSetCommand(new DsdSetCreateIo
            {
                Id = ssdSetId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.Equal(1, host.DsdSetSet.Count());
            Assert.True(host.DsdSetSet.TryGetDsdSet(ssdSetId, out ssdSetById));

            Assert.Equal(0, host.DsdSetSet.GetDsdRoles(ssdSetById).Count);
            var roleId1 = Guid.NewGuid();
            host.Handle(new AddRoleCommand(new RoleCreateInput
            {
                Id = roleId1,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }));
            var entityId = Guid.NewGuid();
            host.Handle(new AddDsdRoleCommand(new DsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId1,
                DsdSetId = ssdSetId
            }));
            var roleId2 = Guid.NewGuid();
            host.Handle(new AddRoleCommand(new RoleCreateInput
            {
                Id = roleId2,
                Name = "测试2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }));
            entityId = Guid.NewGuid();
            host.Handle(new AddDsdRoleCommand(new DsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId2,
                DsdSetId = ssdSetId
            }));
            var roleId3 = Guid.NewGuid();
            host.Handle(new AddRoleCommand(new RoleCreateInput
            {
                Id = roleId3,
                Name = "测试3",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }));
            entityId = Guid.NewGuid();
            host.Handle(new AddDsdRoleCommand(new DsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId3,
                DsdSetId = ssdSetId
            }));
            Assert.Equal(3, host.RoleSet.Count());
            Assert.Equal(3, host.DsdSetSet.GetDsdRoles(ssdSetById).Count);
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
            var accountId = Guid.NewGuid();
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
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = Guid.NewGuid(),
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId1,
                ObjectType = AcElementType.Role.ToString()
            }));
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = Guid.NewGuid(),
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId2,
                ObjectType = AcElementType.Role.ToString()
            }));
            host.Handle(new AddPrivilegeCommand(new PrivilegeCreateIo
            {
                Id = Guid.NewGuid(),
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId3,
                ObjectType = AcElementType.Role.ToString()
            }));
            var catched = false;
            UserSessionState.SignOut(host, host.GetUserSession());
            try
            {
                UserSessionState.SignIn(host, new Dictionary<string, object>
                {
                    {"loginName", "test"},
                    {"password", "111111"},
                    {"rememberMe", "rememberMe"}
                });
                var p = host.GetUserSession().AccountPrivilege;
            }
            catch
            {
                catched = true;
            }
            Assert.True(catched);
        }
    }
}
