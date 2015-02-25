
namespace Anycmd.Tests
{
    using Ac.ViewModels.GroupViewModels;
    using Engine.Ac;
    using Engine.Ac.Groups;
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
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.GroupSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            GroupState groupById;
            acDomain.Handle(new AddGroupCommand(acDomain.GetAcSession(), new GroupCreateInput
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
            Assert.AreEqual(1, acDomain.GroupSet.Count());
            Assert.IsTrue(acDomain.GroupSet.TryGetGroup(entityId, out groupById));

            acDomain.Handle(new UpdateGroupCommand(acDomain.GetAcSession(), new GroupUpdateInput
            {
                Id = entityId,
                Name = "test2",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                ShortName = "",
                SortCode = 10
            }));
            Assert.AreEqual(1, acDomain.GroupSet.Count());
            Assert.IsTrue(acDomain.GroupSet.TryGetGroup(entityId, out groupById));
            Assert.AreEqual("test2", groupById.Name);

            acDomain.Handle(new RemoveGroupCommand(acDomain.GetAcSession(), entityId));
            Assert.IsFalse(acDomain.GroupSet.TryGetGroup(entityId, out groupById));
            Assert.AreEqual(0, acDomain.GroupSet.Count());
        }
        #endregion

        #region GroupSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void GroupSetShouldRollbackedWhenPersistFailed()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.GroupSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            acDomain.RemoveService(typeof(IRepository<Group>));
            var moGroupRepository = acDomain.GetMoqRepository<Group, IRepository<Group>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string name = "测试1";
            moGroupRepository.Setup(a => a.Add(It.Is<Group>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moGroupRepository.Setup(a => a.Update(It.Is<Group>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moGroupRepository.Setup(a => a.Remove(It.Is<Group>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moGroupRepository.Setup<Group>(a => a.GetByKey(entityId1)).Returns(new Group { Id = entityId1, Name = name });
            moGroupRepository.Setup<Group>(a => a.GetByKey(entityId2)).Returns(new Group { Id = entityId2, Name = name });
            acDomain.AddService(typeof(IRepository<Group>), moGroupRepository.Object);

            bool catched = false;
            try
            {
                acDomain.Handle(new AddGroupCommand(acDomain.GetAcSession(), new GroupCreateInput
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
                Assert.AreEqual(0, acDomain.GroupSet.Count());
            }

            acDomain.Handle(new AddGroupCommand(acDomain.GetAcSession(), new GroupCreateInput
            {
                Id = entityId2,
                Name = name,
                TypeCode = "Ac"
            }));
            Assert.AreEqual(1, acDomain.GroupSet.Count());

            catched = false;
            try
            {
                acDomain.Handle(new UpdateGroupCommand(acDomain.GetAcSession(), new GroupUpdateInput
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
                Assert.AreEqual(1, acDomain.GroupSet.Count());
                GroupState group;
                Assert.IsTrue(acDomain.GroupSet.TryGetGroup(entityId2, out group));
                Assert.AreEqual(name, group.Name);
            }

            catched = false;
            try
            {
                acDomain.Handle(new RemoveGroupCommand(acDomain.GetAcSession(), entityId2));
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
                Assert.IsTrue(acDomain.GroupSet.TryGetGroup(entityId2, out group));
                Assert.AreEqual(1, acDomain.GroupSet.Count());
            }
        }
        #endregion
    }
}
