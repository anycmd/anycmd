
namespace Anycmd.Tests
{
    using Ac.ViewModels.MenuViewModels;
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
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.MenuSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            MenuState menuById;
            acDomain.Handle(new MenuCreateInput
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                AppSystemId = acDomain.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.MenuSet.Count());
            Assert.IsTrue(acDomain.MenuSet.TryGetMenu(entityId, out menuById));

            acDomain.Handle(new MenuUpdateInput
            {
                Id = entityId,
                Name = "test2",
                Description = "test",
                SortCode = 10,
                AppSystemId = acDomain.AppSystemSet.First().Id,
                Icon = null,
                Url = string.Empty
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.MenuSet.Count());
            Assert.IsTrue(acDomain.MenuSet.TryGetMenu(entityId, out menuById));
            Assert.AreEqual("test2", menuById.Name);

            acDomain.Handle(new RemoveMenuCommand(acDomain.GetAcSession(), entityId));
            Assert.IsFalse(acDomain.MenuSet.TryGetMenu(entityId, out menuById));
            Assert.AreEqual(0, acDomain.MenuSet.Count());
        }
        #endregion

        [TestMethod]
        public void MenuCanNotRemoveWhenItHasChildMenus()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.MenuSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();

            MenuState menuById;
            acDomain.Handle(new MenuCreateInput
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                AppSystemId = acDomain.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }.ToCommand(acDomain.GetAcSession()));
            acDomain.Handle(new MenuCreateInput
            {
                Id = entityId2,
                Name = "测试2",
                Description = "test",
                SortCode = 10,
                AppSystemId = acDomain.AppSystemSet.First().Id,
                Icon = null,
                ParentId = entityId,
                Url = string.Empty
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(2, acDomain.MenuSet.Count());
            Assert.IsNotNull(acDomain.RetrieveRequiredService<IRepository<Menu>>().GetByKey(entityId));
            Assert.IsNotNull(acDomain.RetrieveRequiredService<IRepository<Menu>>().GetByKey(entityId2));
            Assert.AreEqual(entityId, acDomain.RetrieveRequiredService<IRepository<Menu>>().GetByKey(entityId2).ParentId.Value);
            Assert.IsTrue(acDomain.MenuSet.TryGetMenu(entityId, out menuById));
            bool catched = false;
            try
            {
                acDomain.Handle(new RemoveMenuCommand(acDomain.GetAcSession(), entityId));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.IsTrue(catched);
                Assert.AreEqual(2, acDomain.MenuSet.Count());
            }
        }

        #region MenuSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void MenuSetShouldRollbackedWhenPersistFailed()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.MenuSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            acDomain.RemoveService(typeof(IRepository<Menu>));
            var moMenuRepository = acDomain.GetMoqRepository<Menu, IRepository<Menu>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string name = "测试1";
            moMenuRepository.Setup(a => a.Add(It.Is<Menu>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moMenuRepository.Setup(a => a.Update(It.Is<Menu>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moMenuRepository.Setup(a => a.Remove(It.Is<Menu>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moMenuRepository.Setup<Menu>(a => a.GetByKey(entityId1)).Returns(new Menu { Id = entityId1, Name = name });
            moMenuRepository.Setup<Menu>(a => a.GetByKey(entityId2)).Returns(new Menu { Id = entityId2, Name = name });
            acDomain.AddService(typeof(IRepository<Menu>), moMenuRepository.Object);

            bool catched = false;
            try
            {
                acDomain.Handle(new MenuCreateInput
                {
                    Id = entityId1,
                    AppSystemId = acDomain.AppSystemSet.First().Id,
                    Name = name
                }.ToCommand(acDomain.GetAcSession()));
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
                Assert.AreEqual(0, acDomain.MenuSet.Count());
            }

            acDomain.Handle(new MenuCreateInput
            {
                Id = entityId2,
                AppSystemId = acDomain.AppSystemSet.First().Id,
                Name = name
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.MenuSet.Count());

            catched = false;
            try
            {
                acDomain.Handle(new MenuUpdateInput
                {
                    Id = entityId2,
                    AppSystemId = acDomain.AppSystemSet.First().Id,
                    Name = "test2"
                }.ToCommand(acDomain.GetAcSession()));
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
                Assert.AreEqual(1, acDomain.MenuSet.Count());
                MenuState menu;
                Assert.IsTrue(acDomain.MenuSet.TryGetMenu(entityId2, out menu));
                Assert.AreEqual(name, menu.Name);
            }

            catched = false;
            try
            {
                acDomain.Handle(new RemoveMenuCommand(acDomain.GetAcSession(), entityId2));
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
                Assert.IsTrue(acDomain.MenuSet.TryGetMenu(entityId2, out menu));
                Assert.AreEqual(1, acDomain.MenuSet.Count());
            }
        }
        #endregion
    }
}
