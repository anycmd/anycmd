
namespace Anycmd.Tests
{
    using Ac.ViewModels.Infra.CatalogViewModels;
    using Engine.Ac;
    using Engine.Ac.Messages.Infra;
    using Engine.Host.Ac.Infra;
    using Moq;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class CatalogTest
    {
        #region CatalogSet
        [Fact]
        public void CatalogSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.CatalogSet.Count());
            Assert.Equal(CatalogState.VirtualRoot, host.CatalogSet.First());
            UserSessionState.SignIn(host, new Dictionary<string, object>
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
            }.ToCommand(host.GetUserSession()));
            Assert.Equal(2, host.CatalogSet.Count());
            Assert.True(host.CatalogSet.TryGetCatalog(entityId, out catalogById));

            host.Handle(new CatalogUpdateInput
            {
                Id = entityId,
                Code = "110",
                Name = "test2",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetUserSession()));
            Assert.Equal(2, host.CatalogSet.Count());
            Assert.True(host.CatalogSet.TryGetCatalog(entityId, out catalogById));
            Assert.Equal("test2", catalogById.Name);

            host.Handle(new RemoveCatalogCommand(host.GetUserSession(), entityId));
            Assert.False(host.CatalogSet.TryGetCatalog(entityId, out catalogById));
            Assert.Equal(1, host.CatalogSet.Count());
        }
        #endregion

        [Fact]
        public void CatalogCodeMustBeUnique()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.CatalogSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
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
            }.ToCommand(host.GetUserSession()));
            Assert.Equal(2, host.CatalogSet.Count());
            Assert.True(host.CatalogSet.TryGetCatalog(entityId, out catalogById));
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
                }.ToCommand(host.GetUserSession()));
                host.Handle(new CatalogCreateInput
                {
                    Id = entityId2,
                    ParentCode = "100",
                    Code = "100",
                    Name = "测试2",
                    Description = "test",
                    SortCode = 10,
                    Icon = null,
                }.ToCommand(host.GetUserSession()));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(2, host.CatalogSet.Count());
            }
        }

        [Fact]
        public void CatalogCanNotRemoveWhenItHasChildCatalogs()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.CatalogSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
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
            }.ToCommand(host.GetUserSession()));
            host.Handle(new CatalogCreateInput
            {
                Id = entityId2,
                ParentCode = "100",
                Code = "100100",
                Name = "测试2",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(host.GetUserSession()));
            Assert.Equal(3, host.CatalogSet.Count());
            Assert.True(host.CatalogSet.TryGetCatalog(entityId, out catalogById));
            bool catched = false;
            try
            {
                host.Handle(new RemoveCatalogCommand(host.GetUserSession(), entityId));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(3, host.CatalogSet.Count());
            }
        }

        #region CatalogSetShouldRollbackedWhenPersistFailed
        [Fact]
        public void CatalogSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.CatalogSet.Count());
            Assert.Equal(CatalogState.VirtualRoot, host.CatalogSet.First());
            UserSessionState.SignIn(host, new Dictionary<string, object>
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
                }.ToCommand(host.GetUserSession()));
            }
            catch (Exception e)
            {
                Assert.Equal(e.GetType(), typeof(DbException));
                catched = true;
                Assert.Equal(entityId1.ToString(), e.Message);
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(1, host.CatalogSet.Count());
            }

            host.Handle(new CatalogCreateInput
            {
                Id = entityId2,
                Code = "100",
                Description = "test",
                SortCode = 10,
                Icon = null,
                Name = name
            }.ToCommand(host.GetUserSession()));
            Assert.Equal(2, host.CatalogSet.Count());

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
                }.ToCommand(host.GetUserSession()));
            }
            catch (Exception e)
            {
                Assert.Equal(e.GetType(), typeof(DbException));
                catched = true;
                Assert.Equal(entityId2.ToString(), e.Message);
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(2, host.CatalogSet.Count());
                CatalogState catalog;
                Assert.True(host.CatalogSet.TryGetCatalog(entityId2, out catalog));
                Assert.Equal(name, catalog.Name);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveCatalogCommand(host.GetUserSession(), entityId2));
            }
            catch (Exception e)
            {
                Assert.Equal(e.GetType(), typeof(DbException));
                catched = true;
                Assert.Equal(entityId2.ToString(), e.Message);
            }
            finally
            {
                Assert.True(catched);
                CatalogState catalog;
                Assert.True(host.CatalogSet.TryGetCatalog(entityId2, out catalog));
                Assert.Equal(2, host.CatalogSet.Count());
            }
        }
        #endregion
    }
}
