
namespace Anycmd.Tests
{
    using Ac.ViewModels.Infra.MenuViewModels;
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
    public class MenuTest
    {
        #region MenuSet
        [TestMethod]
        public void MenuSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.MenuSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            MenuState menuById;
            host.Handle(new MenuCreateInput
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.MenuSet.Count());
            Assert.IsTrue(host.MenuSet.TryGetMenu(entityId, out menuById));

            host.Handle(new MenuUpdateInput
            {
                Id = entityId,
                Name = "test2",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                Url = string.Empty
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.MenuSet.Count());
            Assert.IsTrue(host.MenuSet.TryGetMenu(entityId, out menuById));
            Assert.AreEqual("test2", menuById.Name);

            host.Handle(new RemoveMenuCommand(host.GetAcSession(), entityId));
            Assert.IsFalse(host.MenuSet.TryGetMenu(entityId, out menuById));
            Assert.AreEqual(0, host.MenuSet.Count());
        }
        #endregion

        [TestMethod]
        public void MenuCanNotRemoveWhenItHasChildMenus()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.MenuSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();

            MenuState menuById;
            host.Handle(new MenuCreateInput
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }.ToCommand(host.GetAcSession()));
            host.Handle(new MenuCreateInput
            {
                Id = entityId2,
                Name = "测试2",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                ParentId = entityId,
                Url = string.Empty
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(2, host.MenuSet.Count());
            Assert.IsNotNull(host.RetrieveRequiredService<IRepository<Menu>>().GetByKey(entityId));
            Assert.IsNotNull(host.RetrieveRequiredService<IRepository<Menu>>().GetByKey(entityId2));
            Assert.AreEqual(entityId, host.RetrieveRequiredService<IRepository<Menu>>().GetByKey(entityId2).ParentId.Value);
            Assert.IsTrue(host.MenuSet.TryGetMenu(entityId, out menuById));
            bool catched = false;
            try
            {
                host.Handle(new RemoveMenuCommand(host.GetAcSession(), entityId));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.IsTrue(catched);
                Assert.AreEqual(2, host.MenuSet.Count());
            }
        }

        #region MenuSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void MenuSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.MenuSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.RemoveService(typeof(IRepository<Menu>));
            var moMenuRepository = host.GetMoqRepository<Menu, IRepository<Menu>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string name = "测试1";
            moMenuRepository.Setup(a => a.Add(It.Is<Menu>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moMenuRepository.Setup(a => a.Update(It.Is<Menu>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moMenuRepository.Setup(a => a.Remove(It.Is<Menu>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moMenuRepository.Setup<Menu>(a => a.GetByKey(entityId1)).Returns(new Menu { Id = entityId1, Name = name });
            moMenuRepository.Setup<Menu>(a => a.GetByKey(entityId2)).Returns(new Menu { Id = entityId2, Name = name });
            host.AddService(typeof(IRepository<Menu>), moMenuRepository.Object);

            bool catched = false;
            try
            {
                host.Handle(new MenuCreateInput
                {
                    Id = entityId1,
                    AppSystemId = host.AppSystemSet.First().Id,
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
                Assert.AreEqual(0, host.MenuSet.Count());
            }

            host.Handle(new MenuCreateInput
            {
                Id = entityId2,
                AppSystemId = host.AppSystemSet.First().Id,
                Name = name
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.MenuSet.Count());

            catched = false;
            try
            {
                host.Handle(new MenuUpdateInput
                {
                    Id = entityId2,
                    AppSystemId = host.AppSystemSet.First().Id,
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
                Assert.AreEqual(1, host.MenuSet.Count());
                MenuState menu;
                Assert.IsTrue(host.MenuSet.TryGetMenu(entityId2, out menu));
                Assert.AreEqual(name, menu.Name);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveMenuCommand(host.GetAcSession(), entityId2));
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
                MenuState menu;
                Assert.IsTrue(host.MenuSet.TryGetMenu(entityId2, out menu));
                Assert.AreEqual(1, host.MenuSet.Count());
            }
        }
        #endregion
    }
}
