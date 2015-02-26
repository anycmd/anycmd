
namespace Anycmd.Tests
{
    using Ac.ViewModels.FunctionViewModels;
    using Ac.ViewModels.UIViewViewModels;
    using Engine.Ac;
    using Engine.Ac.UiViews;
    using Engine.Host.Ac.Infra;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class PageTest
    {
        #region PageSet
        [TestMethod]
        public void PageSet()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.UiViewSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            acDomain.Handle(new FunctionCreateInput
            {
                Id = entityId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = TestHelper.TestCatalogNodeId,
                SortCode = 10
            }.ToCommand(acDomain.GetAcSession()));
            FunctionState functionById;
            Assert.AreEqual(1, acDomain.FunctionSet.Count());
            Assert.IsTrue(acDomain.FunctionSet.TryGetFunction(entityId, out functionById));
            UiViewState pageById;
            acDomain.Handle(new UiViewCreateInput
            {
                Id = entityId,
                Icon = null,
                Tooltip = null
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.UiViewSet.Count());
            Assert.IsTrue(acDomain.UiViewSet.TryGetUiView(entityId, out pageById));
            bool catched = false;
            try
            {
                acDomain.Handle(new UiViewCreateInput
                {
                    Id = Guid.NewGuid(),
                    Icon = null,
                    Tooltip = null
                }.ToCommand(acDomain.GetAcSession()));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.IsTrue(catched);
            }
            acDomain.Handle(new UiViewUpdateInput
            {
                Id = entityId,
                Icon = null,
                Tooltip = null
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.UiViewSet.Count());
            Assert.IsTrue(acDomain.UiViewSet.TryGetUiView(entityId, out pageById));

            acDomain.Handle(new RemoveUiViewCommand(acDomain.GetAcSession(), entityId));
            Assert.IsFalse(acDomain.UiViewSet.TryGetUiView(entityId, out pageById));
            Assert.AreEqual(0, acDomain.UiViewSet.Count());
        }
        #endregion

        #region PageSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void PageSetShouldRollbackedWhenPersistFailed()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.UiViewSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            acDomain.RemoveService(typeof(IRepository<UiView>));
            var moPageRepository = acDomain.GetMoqRepository<UiView, IRepository<UiView>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            moPageRepository.Setup(a => a.Add(It.Is<UiView>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moPageRepository.Setup(a => a.Update(It.Is<UiView>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moPageRepository.Setup(a => a.Remove(It.Is<UiView>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moPageRepository.Setup<UiView>(a => a.GetByKey(entityId1)).Returns(new UiView { Id = entityId1 });
            moPageRepository.Setup<UiView>(a => a.GetByKey(entityId2)).Returns(new UiView { Id = entityId2 });
            acDomain.AddService(typeof(IRepository<UiView>), moPageRepository.Object);

            acDomain.Handle(new FunctionCreateInput
            {
                Id = entityId1,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = TestHelper.TestCatalogNodeId,
                SortCode = 10
            }.ToCommand(acDomain.GetAcSession()));
            acDomain.Handle(new FunctionCreateInput
            {
                Id = entityId2,
                Code = "fun2",
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = TestHelper.TestCatalogNodeId,
                SortCode = 10
            }.ToCommand(acDomain.GetAcSession()));
            FunctionState functionById;
            Assert.AreEqual(2, acDomain.FunctionSet.Count());
            Assert.IsTrue(acDomain.FunctionSet.TryGetFunction(entityId1, out functionById));
            Assert.IsTrue(acDomain.FunctionSet.TryGetFunction(entityId2, out functionById));

            bool catched = false;
            try
            {
                acDomain.Handle(new UiViewCreateInput
                {
                    Id = entityId1
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
                Assert.AreEqual(0, acDomain.UiViewSet.Count());
            }

            acDomain.Handle(new UiViewCreateInput
            {
                Id = entityId2
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.UiViewSet.Count());

            catched = false;
            try
            {
                acDomain.Handle(new UiViewUpdateInput
                {
                    Id = entityId2
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
                Assert.AreEqual(1, acDomain.UiViewSet.Count());
                UiViewState page;
                Assert.IsTrue(acDomain.UiViewSet.TryGetUiView(entityId2, out page));
            }

            catched = false;
            try
            {
                acDomain.Handle(new RemoveUiViewCommand(acDomain.GetAcSession(), entityId2));
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
                UiViewState page;
                Assert.IsTrue(acDomain.UiViewSet.TryGetUiView(entityId2, out page));
                Assert.AreEqual(1, acDomain.UiViewSet.Count());
            }
        }
        #endregion
    }
}
