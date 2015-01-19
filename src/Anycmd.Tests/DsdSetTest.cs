
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
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            DsdSetState dsdSetById;
            host.Handle(new AddDsdSetCommand(host.GetUserSession(), new DsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.Equal(1, host.DsdSetSet.Count());
            Assert.True(host.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));

            host.Handle(new UpdateDsdSetCommand(host.GetUserSession(), new DsdSetUpdateIo
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

            host.Handle(new RemoveDsdSetCommand(host.GetUserSession(), entityId));
            Assert.False(host.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
            Assert.Equal(0, host.DsdSetSet.Count());
        }

        [Fact]
        public void TestDsdRole()
        {
            var host = TestHelper.GetAcDomain();
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
            var entityId = Guid.NewGuid();
            host.Handle(new AddDsdRoleCommand(host.GetUserSession(), new DsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId,
                DsdSetId = dsdSetId
            }));
            Assert.Equal(1, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
            host.Handle(new RemoveDsdRoleCommand(host.GetUserSession(), entityId));
            Assert.Equal(0, host.DsdSetSet.GetDsdRoles(dsdSetById).Count);
        }

        [Fact]
        public void CheckDsdSetRoles()
        {
            var host = TestHelper.GetAcDomain();
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

            Assert.Equal(0, host.DsdSetSet.GetDsdRoles(ssdSetById).Count);
            var roleId1 = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId1,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            var entityId = Guid.NewGuid();
            host.Handle(new AddDsdRoleCommand(host.GetUserSession(), new DsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId1,
                DsdSetId = ssdSetId
            }));
            var roleId2 = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId2,
                Name = "测试2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            entityId = Guid.NewGuid();
            host.Handle(new AddDsdRoleCommand(host.GetUserSession(), new DsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId2,
                DsdSetId = ssdSetId
            }));
            var roleId3 = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId3,
                Name = "测试3",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            entityId = Guid.NewGuid();
            host.Handle(new AddDsdRoleCommand(host.GetUserSession(), new DsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId3,
                DsdSetId = ssdSetId
            }));
            Assert.Equal(3, host.RoleSet.Count());
            Assert.Equal(3, host.DsdSetSet.GetDsdRoles(ssdSetById).Count);
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
            var accountId = host.GetUserSession().Account.Id;
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.True(host.GetUserSession().Identity.IsAuthenticated);
            Assert.Equal(0, host.GetUserSession().AccountPrivilege.Roles.Count);
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = Guid.NewGuid(),
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId1,
                ObjectType = AcElementType.Role.ToString()
            }));
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = Guid.NewGuid(),
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId2,
                ObjectType = AcElementType.Role.ToString()
            }));
            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
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
