
namespace Anycmd.Tests
{
    using Ac.ViewModels.GroupViewModels;
    using Ac.ViewModels.Infra.CatalogViewModels;
    using Engine.Ac;
    using Engine.Ac.Messages.Infra;
    using Engine.Host.Ac.Infra;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class GroupSetTest
    {
        #region GroupSet
        [TestMethod]
        public void GroupSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.GroupSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            GroupState groupById;
            host.Handle(new AddGroupCommand(host.GetAcSession(), new GroupCreateInput
            {
                Id = entityId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                ShortName = "",
                SortCode = 10,
                TypeCode = "Ac"
            }));
            Assert.AreEqual(1, host.GroupSet.Count());
            Assert.IsTrue(host.GroupSet.TryGetGroup(entityId, out groupById));

            host.Handle(new UpdateGroupCommand(host.GetAcSession(), new GroupUpdateInput
            {
                Id = entityId,
                Name = "test2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                ShortName = "",
                SortCode = 10
            }));
            Assert.AreEqual(1, host.GroupSet.Count());
            Assert.IsTrue(host.GroupSet.TryGetGroup(entityId, out groupById));
            Assert.AreEqual("test2", groupById.Name);

            host.Handle(new RemoveGroupCommand(host.GetAcSession(), entityId));
            Assert.IsFalse(host.GroupSet.TryGetGroup(entityId, out groupById));
            Assert.AreEqual(0, host.GroupSet.Count());
        }
        #endregion

        #region GroupSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void GroupSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.GroupSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.RemoveService(typeof(IRepository<Group>));
            var moGroupRepository = host.GetMoqRepository<Group, IRepository<Group>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string name = "测试1";
            moGroupRepository.Setup(a => a.Add(It.Is<Group>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moGroupRepository.Setup(a => a.Update(It.Is<Group>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moGroupRepository.Setup(a => a.Remove(It.Is<Group>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moGroupRepository.Setup<Group>(a => a.GetByKey(entityId1)).Returns(new Group { Id = entityId1, Name = name });
            moGroupRepository.Setup<Group>(a => a.GetByKey(entityId2)).Returns(new Group { Id = entityId2, Name = name });
            host.AddService(typeof(IRepository<Group>), moGroupRepository.Object);

            bool catched = false;
            try
            {
                host.Handle(new AddGroupCommand(host.GetAcSession(), new GroupCreateInput
                {
                    Id = entityId1,
                    Name = name,
                    TypeCode = "Ac"
                }));
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
                Assert.AreEqual(0, host.GroupSet.Count());
            }

            host.Handle(new AddGroupCommand(host.GetAcSession(), new GroupCreateInput
            {
                Id = entityId2,
                Name = name,
                TypeCode = "Ac"
            }));
            Assert.AreEqual(1, host.GroupSet.Count());

            catched = false;
            try
            {
                host.Handle(new UpdateGroupCommand(host.GetAcSession(), new GroupUpdateInput
                {
                    Id = entityId2,
                    Name = "test2"
                }));
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
                Assert.AreEqual(1, host.GroupSet.Count());
                GroupState group;
                Assert.IsTrue(host.GroupSet.TryGetGroup(entityId2, out group));
                Assert.AreEqual(name, group.Name);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveGroupCommand(host.GetAcSession(), entityId2));
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
                GroupState group;
                Assert.IsTrue(host.GroupSet.TryGetGroup(entityId2, out group));
                Assert.AreEqual(1, host.GroupSet.Count());
            }
        }
        #endregion

        [TestMethod]
        public void TestPosition()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.GroupSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            var entityId = Guid.NewGuid();

            GroupState groupById;
            host.Handle(new PositionCreateInput
            {
                Id = entityId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                ShortName = "",
                SortCode = 10,
                CatalogCode = "100"
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.GroupSet.Count());
            Assert.IsTrue(host.GroupSet.TryGetGroup(entityId, out groupById));
            host.Handle(new RemovePositionCommand(host.GetAcSession(), entityId));
            Assert.AreEqual(0, host.GroupSet.Count());
        }
    }
}
