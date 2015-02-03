
namespace Anycmd.Tests
{
    using Ac.ViewModels.Infra.FunctionViewModels;
    using Ac.ViewModels.Infra.UIViewViewModels;
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
    public class PageTest
    {
        #region PageSet
        [TestMethod]
        public void PageSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.UiViewSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            host.Handle(new FunctionCreateInput
            {
                Id = entityId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }.ToCommand(host.GetAcSession()));
            FunctionState functionById;
            Assert.AreEqual(1, host.FunctionSet.Count());
            Assert.IsTrue(host.FunctionSet.TryGetFunction(entityId, out functionById));
            UiViewState pageById;
            host.Handle(new UiViewCreateInput
            {
                Id = entityId,
                Icon = null,
                Tooltip = null
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.UiViewSet.Count());
            Assert.IsTrue(host.UiViewSet.TryGetUiView(entityId, out pageById));
            bool catched = false;
            try
            {
                host.Handle(new UiViewCreateInput
                {
                    Id = Guid.NewGuid(),
                    Icon = null,
                    Tooltip = null
                }.ToCommand(host.GetAcSession()));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.IsTrue(catched);
            }
            host.Handle(new UiViewUpdateInput
            {
                Id = entityId,
                Icon = null,
                Tooltip = null
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.UiViewSet.Count());
            Assert.IsTrue(host.UiViewSet.TryGetUiView(entityId, out pageById));

            host.Handle(new RemoveUiViewCommand(host.GetAcSession(), entityId));
            Assert.IsFalse(host.UiViewSet.TryGetUiView(entityId, out pageById));
            Assert.AreEqual(0, host.UiViewSet.Count());
        }
        #endregion

        #region PageSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void PageSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.UiViewSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.RemoveService(typeof(IRepository<UiView>));
            var moPageRepository = host.GetMoqRepository<UiView, IRepository<UiView>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            moPageRepository.Setup(a => a.Add(It.Is<UiView>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moPageRepository.Setup(a => a.Update(It.Is<UiView>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moPageRepository.Setup(a => a.Remove(It.Is<UiView>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moPageRepository.Setup<UiView>(a => a.GetByKey(entityId1)).Returns(new UiView { Id = entityId1 });
            moPageRepository.Setup<UiView>(a => a.GetByKey(entityId2)).Returns(new UiView { Id = entityId2 });
            host.AddService(typeof(IRepository<UiView>), moPageRepository.Object);

            host.Handle(new FunctionCreateInput
            {
                Id = entityId1,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }.ToCommand(host.GetAcSession()));
            host.Handle(new FunctionCreateInput
            {
                Id = entityId2,
                Code = "fun2",
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }.ToCommand(host.GetAcSession()));
            FunctionState functionById;
            Assert.AreEqual(2, host.FunctionSet.Count());
            Assert.IsTrue(host.FunctionSet.TryGetFunction(entityId1, out functionById));
            Assert.IsTrue(host.FunctionSet.TryGetFunction(entityId2, out functionById));

            bool catched = false;
            try
            {
                host.Handle(new UiViewCreateInput
                {
                    Id = entityId1
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
                Assert.AreEqual(0, host.UiViewSet.Count());
            }

            host.Handle(new UiViewCreateInput
            {
                Id = entityId2
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.UiViewSet.Count());

            catched = false;
            try
            {
                host.Handle(new UiViewUpdateInput
                {
                    Id = entityId2
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
                Assert.AreEqual(1, host.UiViewSet.Count());
                UiViewState page;
                Assert.IsTrue(host.UiViewSet.TryGetUiView(entityId2, out page));
            }

            catched = false;
            try
            {
                host.Handle(new RemoveUiViewCommand(host.GetAcSession(), entityId2));
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
                Assert.IsTrue(host.UiViewSet.TryGetUiView(entityId2, out page));
                Assert.AreEqual(1, host.UiViewSet.Count());
            }
        }
        #endregion
    }
}
