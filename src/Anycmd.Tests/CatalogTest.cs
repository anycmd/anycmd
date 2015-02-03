
namespace Anycmd.Tests
{
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
    public class CatalogTest
    {
        #region CatalogSet
        [TestMethod]
        public void CatalogSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(1, host.CatalogSet.Count());
            Assert.AreEqual(CatalogState.VirtualRoot, host.CatalogSet.First());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            CatalogState catalogById;
            host.Handle(new CatalogCreateInput
            {
                Id = entityId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(2, host.CatalogSet.Count());
            Assert.IsTrue(host.CatalogSet.TryGetCatalog(entityId, out catalogById));

            host.Handle(new CatalogUpdateInput
            {
                Id = entityId,
                Code = "110",
                Name = "test2",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(2, host.CatalogSet.Count());
            Assert.IsTrue(host.CatalogSet.TryGetCatalog(entityId, out catalogById));
            Assert.AreEqual("test2", catalogById.Name);

            host.Handle(new RemoveCatalogCommand(host.GetAcSession(), entityId));
            Assert.IsFalse(host.CatalogSet.TryGetCatalog(entityId, out catalogById));
            Assert.AreEqual(1, host.CatalogSet.Count());
        }
        #endregion

        [TestMethod]
        public void CatalogCodeMustBeUnique()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(1, host.CatalogSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();

            CatalogState catalogById;
            host.Handle(new CatalogCreateInput
            {
                Id = entityId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(2, host.CatalogSet.Count());
            Assert.IsTrue(host.CatalogSet.TryGetCatalog(entityId, out catalogById));
            bool catched = false;
            try
            {
                host.Handle(new CatalogCreateInput
                {
                    Id = entityId2,
                    ParentCode = null,
                    Code = "100",
                    Name = "测试2",
                    Description = "test",
                    SortCode = 10,
                    Icon = null,
                }.ToCommand(host.GetAcSession()));
                host.Handle(new CatalogCreateInput
                {
                    Id = entityId2,
                    ParentCode = "100",
                    Code = "100",
                    Name = "测试2",
                    Description = "test",
                    SortCode = 10,
                    Icon = null,
                }.ToCommand(host.GetAcSession()));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.IsTrue(catched);
                Assert.AreEqual(2, host.CatalogSet.Count());
            }
        }

        [TestMethod]
        public void CatalogCanNotRemoveWhenItHasChildCatalogs()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(1, host.CatalogSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();

            CatalogState catalogById;
            host.Handle(new CatalogCreateInput
            {
                Id = entityId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            host.Handle(new CatalogCreateInput
            {
                Id = entityId2,
                ParentCode = "100",
                Code = "100100",
                Name = "测试2",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(3, host.CatalogSet.Count());
            Assert.IsTrue(host.CatalogSet.TryGetCatalog(entityId, out catalogById));
            bool catched = false;
            try
            {
                host.Handle(new RemoveCatalogCommand(host.GetAcSession(), entityId));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.IsTrue(catched);
                Assert.AreEqual(3, host.CatalogSet.Count());
            }
        }

        #region CatalogSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void CatalogSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(1, host.CatalogSet.Count());
            Assert.AreEqual(CatalogState.VirtualRoot, host.CatalogSet.First());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.RemoveService(typeof(IRepository<Catalog>));
            var moCatalogRepository = host.GetMoqRepository<Catalog, IRepository<Catalog>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string name = "测试1";
            moCatalogRepository.Setup(a => a.Add(It.Is<Catalog>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moCatalogRepository.Setup(a => a.Update(It.Is<Catalog>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moCatalogRepository.Setup(a => a.Remove(It.Is<Catalog>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moCatalogRepository.Setup<Catalog>(a => a.GetByKey(entityId1)).Returns(new Catalog { Id = entityId1, Name = name });
            moCatalogRepository.Setup<Catalog>(a => a.GetByKey(entityId2)).Returns(new Catalog { Id = entityId2, Name = name });
            host.AddService(typeof(IRepository<Catalog>), moCatalogRepository.Object);

            bool catched = false;
            try
            {
                host.Handle(new CatalogCreateInput
                {
                    Id = entityId1,
                    Code = "100",
                    Description = "test",
                    SortCode = 10,
                    Icon = null,
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
                Assert.AreEqual(1, host.CatalogSet.Count());
            }

            host.Handle(new CatalogCreateInput
            {
                Id = entityId2,
                Code = "100",
                Description = "test",
                SortCode = 10,
                Icon = null,
                Name = name
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(2, host.CatalogSet.Count());

            catched = false;
            try
            {
                host.Handle(new CatalogUpdateInput
                {
                    Id = entityId2,
                    Code = "100",
                    Description = "test",
                    SortCode = 10,
                    Icon = null,
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
                Assert.AreEqual(2, host.CatalogSet.Count());
                CatalogState catalog;
                Assert.IsTrue(host.CatalogSet.TryGetCatalog(entityId2, out catalog));
                Assert.AreEqual(name, catalog.Name);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveCatalogCommand(host.GetAcSession(), entityId2));
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
                CatalogState catalog;
                Assert.IsTrue(host.CatalogSet.TryGetCatalog(entityId2, out catalog));
                Assert.AreEqual(2, host.CatalogSet.Count());
            }
        }
        #endregion
    }
}
