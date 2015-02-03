
namespace Anycmd.Tests
{
    using Ac.ViewModels.Infra.EntityTypeViewModels;
    using Engine.Ac;
    using Engine.Ac.Messages.Infra;
    using Engine.Host.Ac.Infra;
    using Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Util;

    [TestClass]
    public class EntityTypeSetTest
    {
        #region EntityTypeSet
        [TestMethod]
        public void EntityTypeSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.EntityTypeSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            const string codespace = "Test";
            var entityTypeId = Guid.NewGuid();
            var propertyId = Guid.NewGuid();

            EntityTypeState entityTypeById;
            EntityTypeState entityTypeByCode;
            host.Handle(new EntityTypeCreateInput
            {
                Id = entityTypeId,
                Code = "EntityType1",
                Name = "测试1",
                Codespace = codespace,
                DatabaseId = host.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                EditHeight = 100,
                EditWidth = 100,
                IsCatalogued = false,
                SchemaName = string.Empty,
                SortCode = 10,
                TableName = string.Empty
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.EntityTypeSet.Count());
            Assert.IsTrue(host.EntityTypeSet.TryGetEntityType(entityTypeId, out entityTypeById));
            Assert.IsTrue(host.EntityTypeSet.TryGetEntityType(new Coder(codespace, "EntityType1"), out entityTypeByCode));
            Assert.AreEqual(entityTypeByCode, entityTypeById);
            Assert.IsTrue(ReferenceEquals(entityTypeById, entityTypeByCode));

            host.Handle(new EntityTypeUpdateInput
            {
                Id = entityTypeId,
                Name = "test2",
                Code = "EntityType2",
                Codespace = "test",
                DatabaseId = host.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                EditWidth = 100,
                EditHeight = 100,
                IsCatalogued = false,
                SchemaName = string.Empty,
                SortCode = 100,
                TableName = string.Empty
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.EntityTypeSet.Count());
            Assert.IsTrue(host.EntityTypeSet.TryGetEntityType(entityTypeId, out entityTypeById));
            Assert.IsTrue(host.EntityTypeSet.TryGetEntityType(new Coder(codespace, "EntityType2"), out entityTypeByCode));
            Assert.AreEqual(entityTypeByCode, entityTypeById);
            Assert.IsTrue(ReferenceEquals(entityTypeById, entityTypeByCode));
            Assert.AreEqual("test2", entityTypeById.Name);
            Assert.AreEqual("EntityType2", entityTypeById.Code);

            host.Handle(new RemoveEntityTypeCommand(host.GetAcSession(), entityTypeId));
            Assert.IsFalse(host.EntityTypeSet.TryGetEntityType(entityTypeId, out entityTypeById));
            Assert.IsFalse(host.EntityTypeSet.TryGetEntityType(new Coder(codespace, "EntityType2"), out entityTypeByCode));
            Assert.AreEqual(0, host.EntityTypeSet.Count());

            // 开始测试Property
            host.Handle(new EntityTypeCreateInput
            {
                Id = entityTypeId,
                Code = "EntityType1",
                Name = "测试1",
                Codespace = codespace,
                DatabaseId = host.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                EditHeight = 100,
                EditWidth = 100,
                IsCatalogued = false,
                SchemaName = string.Empty,
                SortCode = 10,
                TableName = string.Empty
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.EntityTypeSet.Count());
            Assert.IsTrue(host.EntityTypeSet.TryGetEntityType(entityTypeId, out entityTypeById));
            PropertyState propertyById;
            PropertyState propertyByCode;
            host.Handle(new PropertyCreateInput
            {
                Id = propertyId,
                DicId = null,
                ForeignPropertyId = null,
                GuideWords = string.Empty,
                Icon = string.Empty,
                InputType = string.Empty,
                IsDetailsShow = false,
                IsDeveloperOnly = false,
                IsInput = false,
                IsTotalLine = false,
                MaxLength = null,
                EntityTypeId = entityTypeId,
                SortCode = 0,
                Description = string.Empty,
                Code = "Property1",
                Name = "测试1"
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.EntityTypeSet.GetProperties(entityTypeById).Count());
            Assert.IsTrue(host.EntityTypeSet.TryGetProperty(propertyId, out propertyById));
            Assert.IsTrue(host.EntityTypeSet.TryGetProperty(entityTypeById, "Property1", out propertyByCode));
            Assert.AreEqual(propertyByCode, propertyById);
            Assert.IsTrue(ReferenceEquals(propertyById, propertyByCode));

            host.Handle(new PropertyUpdateInput
            {
                Id = propertyId,
                Name = "test2",
                Code = "Property2"
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.EntityTypeSet.GetProperties(entityTypeById).Count);
            Assert.IsTrue(host.EntityTypeSet.TryGetProperty(propertyId, out propertyById));
            Assert.IsTrue(host.EntityTypeSet.TryGetProperty(entityTypeById, "Property2", out propertyByCode));
            Assert.AreEqual(propertyByCode, propertyById);
            Assert.IsTrue(ReferenceEquals(propertyById, propertyByCode));
            Assert.AreEqual("test2", propertyById.Name);
            Assert.AreEqual("Property2", propertyById.Code);

            host.Handle(new RemovePropertyCommand(host.GetAcSession(), propertyId));
            Assert.IsFalse(host.EntityTypeSet.TryGetProperty(propertyId, out propertyById));
            Assert.IsFalse(host.EntityTypeSet.TryGetProperty(entityTypeById, "Property2", out propertyByCode));
            Assert.AreEqual(0, host.EntityTypeSet.GetProperties(entityTypeById).Count);
        }
        #endregion

        #region CanNotDeleteEntityTypeWhenItHasProperties
        [TestMethod]
        public void CanNotDeleteEntityTypeWhenItHasProperties()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.EntityTypeSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityTypeId = Guid.NewGuid();

            host.Handle(new EntityTypeCreateInput
            {
                Id = entityTypeId,
                Code = "EntityType1",
                Name = "测试1",
                Codespace = "Test",
                DatabaseId = host.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                EditHeight = 100,
                EditWidth = 100,
                IsCatalogued = false,
                SchemaName = string.Empty,
                SortCode = 10,
                TableName = string.Empty
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.EntityTypeSet.Count());

            host.Handle(new PropertyCreateInput
            {
                Id = Guid.NewGuid(),
                DicId = null,
                ForeignPropertyId = null,
                GuideWords = string.Empty,
                Icon = string.Empty,
                InputType = string.Empty,
                IsDetailsShow = false,
                IsDeveloperOnly = false,
                IsInput = false,
                IsTotalLine = false,
                MaxLength = null,
                EntityTypeId = entityTypeId,
                SortCode = 0,
                Description = string.Empty,
                Code = "Property1",
                Name = "测试1"
            }.ToCommand(host.GetAcSession()));

            bool catched = false;
            try
            {
                host.Handle(new RemoveEntityTypeCommand(host.GetAcSession(), entityTypeId));
            }
            catch (ValidationException)
            {
                catched = true;
            }
            finally
            {
                Assert.IsTrue(catched);
                EntityTypeState entityType;
                Assert.IsTrue(host.EntityTypeSet.TryGetEntityType(entityTypeId, out entityType));
            }
        }
        #endregion

        #region EntityTypeSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void EntityTypeSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.EntityTypeSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.RemoveService(typeof(IRepository<EntityType>));
            var moEntityTypeRepository = host.GetMoqRepository<EntityType, IRepository<EntityType>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            moEntityTypeRepository.Setup(a => a.Add(It.Is<EntityType>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moEntityTypeRepository.Setup(a => a.Update(It.Is<EntityType>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moEntityTypeRepository.Setup(a => a.Remove(It.Is<EntityType>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moEntityTypeRepository.Setup<EntityType>(a => a.GetByKey(entityId1)).Returns(new EntityType
            {
                Id = entityId1,
                Name = "test1",
                Code = "EntityType1",
                Codespace = "test",
                DatabaseId = host.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                EditWidth = 100,
                EditHeight = 100,
                IsCatalogued = false,
                SchemaName = string.Empty,
                SortCode = 100,
                TableName = string.Empty
            });
            moEntityTypeRepository.Setup<EntityType>(a => a.GetByKey(entityId2)).Returns(new EntityType
            {
                Id = entityId2,
                Name = "test2",
                Code = "EntityType2",
                Codespace = "test",
                DatabaseId = host.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                EditWidth = 100,
                EditHeight = 100,
                IsCatalogued = false,
                SchemaName = string.Empty,
                SortCode = 100,
                TableName = string.Empty
            });
            host.AddService(typeof(IRepository<EntityType>), moEntityTypeRepository.Object);


            bool catched = false;
            try
            {
                host.Handle(new EntityTypeCreateInput
                {
                    Id = entityId1,
                    Code = "EntityType1",
                    Name = "测试1",
                    Codespace = "Test",
                    DatabaseId = host.Rdbs.First().Database.Id,
                    Description = string.Empty,
                    DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                    EditHeight = 100,
                    EditWidth = 100,
                    IsCatalogued = false,
                    SchemaName = string.Empty,
                    SortCode = 10,
                    TableName = string.Empty
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
                Assert.AreEqual(0, host.EntityTypeSet.Count());
            }

            host.Handle(new EntityTypeCreateInput
            {
                Id = entityId2,
                Code = "EntityType2",
                Name = "测试2",
                Codespace = "Test",
                DatabaseId = host.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                EditHeight = 100,
                EditWidth = 100,
                IsCatalogued = false,
                SchemaName = string.Empty,
                SortCode = 10,
                TableName = string.Empty
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.EntityTypeSet.Count());

            catched = false;
            try
            {
                host.Handle(new EntityTypeUpdateInput
                {
                    Id = entityId2,
                    Name = "test2",
                    Code = "EntityType2",
                    Codespace = "test",
                    DatabaseId = host.Rdbs.First().Database.Id,
                    Description = string.Empty,
                    DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                    EditWidth = 100,
                    EditHeight = 100,
                    IsCatalogued = false,
                    SchemaName = string.Empty,
                    SortCode = 100,
                    TableName = string.Empty
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
                Assert.AreEqual(1, host.EntityTypeSet.Count());
                EntityTypeState entityType;
                Assert.IsTrue(host.EntityTypeSet.TryGetEntityType(entityId2, out entityType));
                Assert.AreEqual("EntityType2", entityType.Code);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveEntityTypeCommand(host.GetAcSession(), entityId2));
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
                EntityTypeState entityType;
                Assert.IsTrue(host.EntityTypeSet.TryGetEntityType(entityId2, out entityType));
                Assert.AreEqual(1, host.EntityTypeSet.Count());
            }
        }
        #endregion
    }
}
