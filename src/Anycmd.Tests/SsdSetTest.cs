
namespace Anycmd.Tests
{
    using Ac.ViewModels.Identity.AccountViewModels;
    using Ac.ViewModels.Infra.CatalogViewModels;
    using Ac.ViewModels.PrivilegeViewModels;
    using Ac.ViewModels.RoleViewModels;
    using Ac.ViewModels.SsdViewModels;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Messages;
    using Engine.Ac.Messages.Rbac;
    using Engine.Host.Ac.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class SsdSetTest
    {
        [TestMethod]
        public void SsdSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.SsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            SsdSetState ssdSetById;
            host.Handle(new AddSsdSetCommand(host.GetAcSession(), new SsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            }));
            Assert.AreEqual(1, host.SsdSetSet.Count());
            Assert.IsTrue(host.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));

            host.Handle(new UpdateSsdSetCommand(host.GetAcSession(), new SsdSetUpdateIo
            {
                Id = entityId,
                Name = "test2",
                Description = "test",
                IsEnabled = 1,
                SsdCard = 2
            }));
            Assert.AreEqual(1, host.SsdSetSet.Count());
            Assert.IsTrue(host.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));
            Assert.AreEqual("test2", ssdSetById.Name);

            host.Handle(new RemoveSsdSetCommand(host.GetAcSession(), entityId));
            Assert.IsFalse(host.SsdSetSet.TryGetSsdSet(entityId, out ssdSetById));
            Assert.AreEqual(0, host.SsdSetSet.Count());
        }

        [TestMethod]
        public void TestSsdRole()
        {
            var host = TestHelper.GetAcDomain();
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
            var entityId = Guid.NewGuid();
            host.Handle(new AddSsdRoleCommand(host.GetAcSession(), new SsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId,
                SsdSetId = ssdSetId
            }));
            Assert.AreEqual(1, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
            host.Handle(new RemoveSsdRoleCommand(host.GetAcSession(), entityId));
            Assert.AreEqual(0, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
        }

        [TestMethod]
        public void CheckSsdSetRoles()
        {
            var host = TestHelper.GetAcDomain();
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
            }.ToCommand(host.GetAcSession()));
            Guid entityId = Guid.NewGuid();
            host.Handle(new AddSsdRoleCommand(host.GetAcSession(), new SsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId1,
                SsdSetId = ssdSetId
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
            }.ToCommand(host.GetAcSession()));
            entityId = Guid.NewGuid();
            host.Handle(new AddSsdRoleCommand(host.GetAcSession(), new SsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId2,
                SsdSetId = ssdSetId
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
            }.ToCommand(host.GetAcSession()));
            entityId = Guid.NewGuid();
            host.Handle(new AddSsdRoleCommand(host.GetAcSession(), new SsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId3,
                SsdSetId = ssdSetId
            }));
            Assert.AreEqual(3, host.RoleSet.Count());
            Assert.AreEqual(3, host.SsdSetSet.GetSsdRoles(ssdSetById).Count);
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
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(host.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, host.GetAcSession().AccountPrivilege.Roles.Count);
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = Guid.NewGuid(),
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId1,
                ObjectType = AcElementType.Role.ToString()
            }));
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
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
                host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
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
            Assert.IsTrue(catched);
        }
    }
}
