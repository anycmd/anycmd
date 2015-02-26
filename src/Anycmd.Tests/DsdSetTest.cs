
namespace Anycmd.Tests
{
    using Ac.ViewModels.CatalogViewModels;
    using Ac.ViewModels.DsdViewModels;
    using Ac.ViewModels.PrivilegeViewModels;
    using Ac.ViewModels.RoleViewModels;
    using Engine.Ac;
    using Engine.Ac.Dsd;
    using Engine.Ac.Privileges;
    using Engine.Host.Ac.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class DsdSetTest
    {
        [TestMethod]
        public void DsdSet()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.DsdSetSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            DsdSetState dsdSetById;
            acDomain.Handle(new AddDsdSetCommand(acDomain.GetAcSession(), new DsdSetCreateIo
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.AreEqual(1, acDomain.DsdSetSet.Count());
            Assert.IsTrue(acDomain.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));

            acDomain.Handle(new UpdateDsdSetCommand(acDomain.GetAcSession(), new DsdSetUpdateIo
            {
                Id = entityId,
                Name = "test2",
                Description = "test",
                IsEnabled = 1,
                DsdCard = 2
            }));
            Assert.AreEqual(1, acDomain.DsdSetSet.Count());
            Assert.IsTrue(acDomain.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
            Assert.AreEqual("test2", dsdSetById.Name);

            acDomain.Handle(new RemoveDsdSetCommand(acDomain.GetAcSession(), entityId));
            Assert.IsFalse(acDomain.DsdSetSet.TryGetDsdSet(entityId, out dsdSetById));
            Assert.AreEqual(0, acDomain.DsdSetSet.Count());
        }

        [TestMethod]
        public void TestDsdRole()
        {
            var acDomain = TestHelper.GetAcDomain();
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
            var entityId = Guid.NewGuid();
            acDomain.Handle(new AddDsdRoleCommand(acDomain.GetAcSession(), new DsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId,
                DsdSetId = dsdSetId
            }));
            Assert.AreEqual(1, acDomain.DsdSetSet.GetDsdRoles(dsdSetById).Count);
            acDomain.Handle(new RemoveDsdRoleCommand(acDomain.GetAcSession(), entityId));
            Assert.AreEqual(0, acDomain.DsdSetSet.GetDsdRoles(dsdSetById).Count);
        }

        [TestMethod]
        public void CheckDsdSetRoles()
        {
            var acDomain = TestHelper.GetAcDomain();
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

            Assert.AreEqual(0, acDomain.DsdSetSet.GetDsdRoles(ssdSetById).Count);
            var roleId1 = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId1,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            var entityId = Guid.NewGuid();
            acDomain.Handle(new AddDsdRoleCommand(acDomain.GetAcSession(), new DsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId1,
                DsdSetId = ssdSetId
            }));
            var roleId2 = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId2,
                Name = "测试2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            entityId = Guid.NewGuid();
            acDomain.Handle(new AddDsdRoleCommand(acDomain.GetAcSession(), new DsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId2,
                DsdSetId = ssdSetId
            }));
            var roleId3 = Guid.NewGuid();
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId3,
                Name = "测试3",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            entityId = Guid.NewGuid();
            acDomain.Handle(new AddDsdRoleCommand(acDomain.GetAcSession(), new DsdRoleCreateIo
            {
                Id = entityId,
                RoleId = roleId3,
                DsdSetId = ssdSetId
            }));
            Assert.AreEqual(3, acDomain.RoleSet.Count());
            Assert.AreEqual(3, acDomain.DsdSetSet.GetDsdRoles(ssdSetById).Count);
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
            var accountId = acDomain.GetAcSession().Account.Id;
            Assert.IsNotNull(acDomain.RetrieveRequiredService<IRepository<Account>>().AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, "test", StringComparison.OrdinalIgnoreCase)));
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Assert.IsTrue(acDomain.GetAcSession().Identity.IsAuthenticated);
            Assert.AreEqual(0, acDomain.GetAcSession().AccountPrivilege.Roles.Count);
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
            {
                Id = Guid.NewGuid(),
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId1,
                ObjectType = AcElementType.Role.ToString()
            }));
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
            {
                Id = Guid.NewGuid(),
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId2,
                ObjectType = AcElementType.Role.ToString()
            }));
            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            try
            {
                AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
                {
                    {"loginName", "test"},
                    {"password", "111111"},
                    {"rememberMe", "rememberMe"}
                });
                var p = acDomain.GetAcSession().AccountPrivilege;
            }
            catch
            {
                catched = true;
            }
            Assert.IsTrue(catched);
        }
    }
}
