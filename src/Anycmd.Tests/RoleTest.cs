
namespace Anycmd.Tests
{
    using Ac.ViewModels.PrivilegeViewModels;
    using Ac.ViewModels.RoleViewModels;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Messages;
    using Engine.Ac.Messages.Rbac;
    using Engine.Host.Ac;
    using Engine.Host.Ac.Rbac;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class RoleTest
    {
        #region RoleSet
        [TestMethod]
        public void RoleSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.RoleSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            RoleState roleById;
            host.Handle(new RoleCreateInput
            {
                Id = entityId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.RoleSet.Count());
            Assert.IsTrue(host.RoleSet.TryGetRole(entityId, out roleById));

            host.Handle(new RoleUpdateInput
            {
                Id = entityId,
                Name = "test2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.RoleSet.Count());
            Assert.IsTrue(host.RoleSet.TryGetRole(entityId, out roleById));
            Assert.AreEqual("test2", roleById.Name);

            host.Handle(new RemoveRoleCommand(host.GetAcSession(), entityId));
            Assert.IsFalse(host.RoleSet.TryGetRole(entityId, out roleById));
            Assert.AreEqual(0, host.RoleSet.Count());
        }
        #endregion

        #region RoleSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void RoleSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.RoleSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.RemoveService(typeof(IRepository<Role>));
            var moRoleRepository = host.GetMoqRepository<Role, IRepository<Role>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string name = "测试1";
            moRoleRepository.Setup(a => a.Add(It.Is<Role>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moRoleRepository.Setup(a => a.Update(It.Is<Role>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moRoleRepository.Setup(a => a.Remove(It.Is<Role>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moRoleRepository.Setup<Role>(a => a.GetByKey(entityId1)).Returns(new Role { Id = entityId1, Name = name });
            moRoleRepository.Setup<Role>(a => a.GetByKey(entityId2)).Returns(new Role { Id = entityId2, Name = name });
            host.AddService(typeof(IRepository<Role>), moRoleRepository.Object);

            bool catched = false;
            try
            {
                host.Handle(new RoleCreateInput
                {
                    Id = entityId1,
                    Name = name
                }.ToCommand(host.GetAcSession()));
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(DbException));
                catched = true;
                Assert.AreEqual(entityId1.ToString(), e.Message);
            }
            finally
            {
                Assert.IsTrue(catched);
                Assert.AreEqual(0, host.RoleSet.Count());
            }

            host.Handle(new RoleCreateInput
            {
                Id = entityId2,
                Name = name
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.RoleSet.Count());

            catched = false;
            try
            {
                host.Handle(new RoleUpdateInput
                {
                    Id = entityId2,
                    Name = "test2"
                }.ToCommand(host.GetAcSession()));
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(DbException));
                catched = true;
                Assert.AreEqual(entityId2.ToString(), e.Message);
            }
            finally
            {
                Assert.IsTrue(catched);
                Assert.AreEqual(1, host.RoleSet.Count());
                RoleState role;
                Assert.IsTrue(host.RoleSet.TryGetRole(entityId2, out role));
                Assert.AreEqual(name, role.Name);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveRoleCommand(host.GetAcSession(), entityId2));
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(DbException));
                catched = true;
                Assert.AreEqual(entityId2.ToString(), e.Message);
            }
            finally
            {
                Assert.IsTrue(catched);
                RoleState role;
                Assert.IsTrue(host.RoleSet.TryGetRole(entityId2, out role));
                Assert.AreEqual(1, host.RoleSet.Count());
            }
        }
        #endregion

        [TestMethod]
        public void TestRoleHierarchy()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.RoleSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var roleId1 = Guid.NewGuid();
            // 创建一个角色
            host.Handle(new RoleCreateInput
            {
                Id = roleId1,
                Name = "role1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetAcSession()));

            var roleId2 = Guid.NewGuid();
            // 再创建一个角色
            host.Handle(new RoleCreateInput
            {
                Id = roleId2,
                Name = "role2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetAcSession()));

            var privilegeId = Guid.NewGuid();
            // 使role1继承role2
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = privilegeId,
                SubjectInstanceId = roleId1,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是角色
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId2,
                ObjectType = AcElementType.Role.ToString()// 客体也是角色
            }));
            PrivilegeState privilegeBigram = host.PrivilegeSet.First(a => a.Id == privilegeId);
            Assert.IsNotNull(privilegeBigram);
            Assert.IsNotNull(host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == privilegeId));

            var roleId3 = Guid.NewGuid();
            // 创建role3
            host.Handle(new RoleCreateInput
            {
                Id = roleId3,
                Name = "role3",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetAcSession()));
            privilegeId = Guid.NewGuid();
            // 使role2继承role3
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = privilegeId,
                SubjectInstanceId = roleId2,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是角色
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId3,
                ObjectType = AcElementType.Role.ToString()// 客体也是角色
            }));
            RoleState role3;
            Assert.IsTrue(host.RoleSet.TryGetRole(roleId3, out role3));
            var roleId4 = Guid.NewGuid();
            host.Handle(new RoleCreateInput
            {
                Id = roleId4,
                Name = "role4",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetAcSession()));
            privilegeId = Guid.NewGuid();
            host.Handle(new AddPrivilegeCommand(host.GetAcSession(), new PrivilegeCreateIo
            {
                Id = privilegeId,
                SubjectInstanceId = roleId3,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是角色
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = roleId4,
                ObjectType = AcElementType.Role.ToString()// 客体也是角色
            }));
            Assert.AreEqual(1, host.RoleSet.GetDescendantRoles(role3).Count);
        }
    }
}
