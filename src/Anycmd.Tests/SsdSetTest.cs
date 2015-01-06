
namespace Anycmd.Tests
{
    using Ac.ViewModels.Identity.AccountViewModels;
    using Ac.ViewModels.Infra.DicViewModels;
    using Ac.ViewModels.Infra.OrganizationViewModels;
    using Ac.ViewModels.RoleViewModels;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages;
    using Engine.Ac.Messages.Identity;
    using Engine.Ac.Messages.Infra;
    using Engine.Host.Ac.Identity;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class SsdSetTest
    {
        [Fact]
        public void SsdSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.SsdSetSet.Count());

            var entityId = Guid.NewGuid();

            SsdSetState ssdSetById;
            host.Handle(new AddSsdSetCommand(new SsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            }));
            Assert.Equal(1, host.SsdSetSet.Count());
            Assert.True(host.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));

            host.Handle(new UpdateSsdSetCommand(new SsdSetUpdateIo
            {
                Id = entityId,
                Name = "test2",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            }));
            Assert.Equal(1, host.SsdSetSet.Count());
            Assert.True(host.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));
            Assert.Equal("test2", ssdSetById.Name);

            host.Handle(new RemoveSsdSetCommand(entityId));
            Assert.False(host.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));
            Assert.Equal(0, host.SsdSetSet.Count());
        }

        [Fact]
        public void TestSsdRole()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.SsdSetSet.Count());

            var ssdSetId = Guid.NewGuid();

            SsdSetState ssdSetById;
            host.Handle(new AddSsdSetCommand(new SsdSetCreateIo
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
            host.Handle(new AddSsdRoleCommand(new SsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId,
                SsdSetId = ssdSetId
            }));
            Assert.Equal(1, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
            host.Handle(new RemoveSsdRoleCommand(entityId));
            Assert.Equal(0, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
        }

        [Fact]
        public void CheckSsdSetRoles()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.SsdSetSet.Count());

            var ssdSetId = Guid.NewGuid();

            SsdSetState ssdSetById;
            host.Handle(new AddSsdSetCommand(new SsdSetCreateIo
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
            Guid entityId = Guid.NewGuid();
            host.Handle(new AddSsdRoleCommand(new SsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId1,
                SsdSetId = ssdSetId
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
            host.Handle(new AddSsdRoleCommand(new SsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId2,
                SsdSetId = ssdSetId
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
            host.Handle(new AddSsdRoleCommand(new SsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId3,
                SsdSetId = ssdSetId
            }));
            Assert.Equal(3, host.RoleSet.Count());
            Assert.Equal(3, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
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
            var catched = false;
            try
            {
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
            }
            catch
            {
                catched = true;
            }
            Assert.True(catched);
        }
    }
}
