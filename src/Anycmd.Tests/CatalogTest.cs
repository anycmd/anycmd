
namespace Anycmd.Tests
{
    using Ac.ViewModels.CatalogViewModels;
    using Engine.Ac;
    using Engine.Ac.Catalogs;
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
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(6, acDomain.CatalogSet.Count());
            Assert.AreEqual(CatalogState.VirtualRoot, acDomain.CatalogSet.First());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            CatalogState catalogById;
            acDomain.Handle(new CatalogCreateInput
            {
                Id = entityId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(7, acDomain.CatalogSet.Count());
            Assert.IsTrue(acDomain.CatalogSet.TryGetCatalog(entityId, out catalogById));

            acDomain.Handle(new CatalogUpdateInput
            {
                Id = entityId,
                Code = "110",
                Name = "test2",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(7, acDomain.CatalogSet.Count());
            Assert.IsTrue(acDomain.CatalogSet.TryGetCatalog(entityId, out catalogById));
            Assert.AreEqual("test2", catalogById.Name);

            acDomain.Handle(new RemoveCatalogCommand(acDomain.GetAcSession(), entityId));
            Assert.IsFalse(acDomain.CatalogSet.TryGetCatalog(entityId, out catalogById));
            Assert.AreEqual(6, acDomain.CatalogSet.Count());
        }
        #endregion

        [TestMethod]
        public void CatalogCodeMustBeUnique()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(6, acDomain.CatalogSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();

            CatalogState catalogById;
            acDomain.Handle(new CatalogCreateInput
            {
                Id = entityId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(7, acDomain.CatalogSet.Count());
            Assert.IsTrue(acDomain.CatalogSet.TryGetCatalog(entityId, out catalogById));
            bool catched = false;
            try
            {
                acDomain.Handle(new CatalogCreateInput
                {
                    Id = entityId2,
                    ParentCode = null,
                    Code = "100",
                    Name = "测试2",
                    Description = "test",
                    SortCode = 10,
                    Icon = null,
                }.ToCommand(acDomain.GetAcSession()));
                acDomain.Handle(new CatalogCreateInput
                {
                    Id = entityId2,
                    ParentCode = "100",
                    Code = "100",
                    Name = "测试2",
                    Description = "test",
                    SortCode = 10,
                    Icon = null,
                }.ToCommand(acDomain.GetAcSession()));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.IsTrue(catched);
                Assert.AreEqual(7, acDomain.CatalogSet.Count());
            }
        }

        [TestMethod]
        public void CatalogCanNotRemoveWhenItHasChildCatalogs()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(6, acDomain.CatalogSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();

            CatalogState catalogById;
            acDomain.Handle(new CatalogCreateInput
            {
                Id = entityId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            acDomain.Handle(new CatalogCreateInput
            {
                Id = entityId2,
                ParentCode = "100",
                Code = "100100",
                Name = "测试2",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(8, acDomain.CatalogSet.Count());
            Assert.IsTrue(acDomain.CatalogSet.TryGetCatalog(entityId, out catalogById));
            bool catched = false;
            try
            {
                acDomain.Handle(new RemoveCatalogCommand(acDomain.GetAcSession(), entityId));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.IsTrue(catched);
                Assert.AreEqual(8, acDomain.CatalogSet.Count());
            }
        }

        #region CatalogSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void CatalogSetShouldRollbackedWhenPersistFailed()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(6, acDomain.CatalogSet.Count());
            Assert.AreEqual(CatalogState.VirtualRoot, acDomain.CatalogSet.First());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            acDomain.RemoveService(typeof(IRepository<Catalog>));
            var moCatalogRepository = acDomain.GetMoqRepository<Catalog, IRepository<Catalog>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string name = "测试1";
            moCatalogRepository.Setup(a => a.Add(It.Is<Catalog>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moCatalogRepository.Setup(a => a.Update(It.Is<Catalog>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moCatalogRepository.Setup(a => a.Remove(It.Is<Catalog>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moCatalogRepository.Setup<Catalog>(a => a.GetByKey(entityId1)).Returns(new Catalog { Id = entityId1, Name = name });
            moCatalogRepository.Setup<Catalog>(a => a.GetByKey(entityId2)).Returns(new Catalog { Id = entityId2, Name = name });
            acDomain.AddService(typeof(IRepository<Catalog>), moCatalogRepository.Object);

            bool catched = false;
            try
            {
                acDomain.Handle(new CatalogCreateInput
                {
                    Id = entityId1,
                    Code = "100",
                    Description = "test",
                    SortCode = 10,
                    Icon = null,
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
                Assert.AreEqual(6, acDomain.CatalogSet.Count());
            }

            acDomain.Handle(new CatalogCreateInput
            {
                Id = entityId2,
                Code = "100",
                Description = "test",
                SortCode = 10,
                Icon = null,
                Name = name
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(7, acDomain.CatalogSet.Count());

            catched = false;
            try
            {
                acDomain.Handle(new CatalogUpdateInput
                {
                    Id = entityId2,
                    Code = "100",
                    Description = "test",
                    SortCode = 10,
                    Icon = null,
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
                Assert.AreEqual(7, acDomain.CatalogSet.Count());
                CatalogState catalog;
                Assert.IsTrue(acDomain.CatalogSet.TryGetCatalog(entityId2, out catalog));
                Assert.AreEqual(name, catalog.Name);
            }

            catched = false;
            try
            {
                acDomain.Handle(new RemoveCatalogCommand(acDomain.GetAcSession(), entityId2));
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
                Assert.IsTrue(acDomain.CatalogSet.TryGetCatalog(entityId2, out catalog));
                Assert.AreEqual(7, acDomain.CatalogSet.Count());
            }
        }
        #endregion
    }
}
