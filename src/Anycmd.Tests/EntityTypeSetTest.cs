
namespace Anycmd.Tests
{
    using Ac.ViewModels.EntityTypeViewModels;
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
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.EntityTypeSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
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
            acDomain.Handle(new EntityTypeCreateInput
            {
                Id = entityTypeId,
                Code = "EntityType1",
                Name = "测试1",
                Codespace = codespace,
                DatabaseId = acDomain.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                EditHeight = 100,
                EditWidth = 100,
                IsCatalogued = false,
                SchemaName = string.Empty,
                SortCode = 10,
                TableName = string.Empty
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.EntityTypeSet.Count());
            Assert.IsTrue(acDomain.EntityTypeSet.TryGetEntityType(entityTypeId, out entityTypeById));
            Assert.IsTrue(acDomain.EntityTypeSet.TryGetEntityType(new Coder(codespace, "EntityType1"), out entityTypeByCode));
            Assert.AreEqual(entityTypeByCode, entityTypeById);
            Assert.IsTrue(ReferenceEquals(entityTypeById, entityTypeByCode));

            acDomain.Handle(new EntityTypeUpdateInput
            {
                Id = entityTypeId,
                Name = "test2",
                Code = "EntityType2",
                Codespace = "test",
                DatabaseId = acDomain.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                EditWidth = 100,
                EditHeight = 100,
                IsCatalogued = false,
                SchemaName = string.Empty,
                SortCode = 100,
                TableName = string.Empty
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.EntityTypeSet.Count());
            Assert.IsTrue(acDomain.EntityTypeSet.TryGetEntityType(entityTypeId, out entityTypeById));
            Assert.IsTrue(acDomain.EntityTypeSet.TryGetEntityType(new Coder(codespace, "EntityType2"), out entityTypeByCode));
            Assert.AreEqual(entityTypeByCode, entityTypeById);
            Assert.IsTrue(ReferenceEquals(entityTypeById, entityTypeByCode));
            Assert.AreEqual("test2", entityTypeById.Name);
            Assert.AreEqual("EntityType2", entityTypeById.Code);

            acDomain.Handle(new RemoveEntityTypeCommand(acDomain.GetAcSession(), entityTypeId));
            Assert.IsFalse(acDomain.EntityTypeSet.TryGetEntityType(entityTypeId, out entityTypeById));
            Assert.IsFalse(acDomain.EntityTypeSet.TryGetEntityType(new Coder(codespace, "EntityType2"), out entityTypeByCode));
            Assert.AreEqual(0, acDomain.EntityTypeSet.Count());

            // 开始测试Property
            acDomain.Handle(new EntityTypeCreateInput
            {
                Id = entityTypeId,
                Code = "EntityType1",
                Name = "测试1",
                Codespace = codespace,
                DatabaseId = acDomain.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                EditHeight = 100,
                EditWidth = 100,
                IsCatalogued = false,
                SchemaName = string.Empty,
                SortCode = 10,
                TableName = string.Empty
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.EntityTypeSet.Count());
            Assert.IsTrue(acDomain.EntityTypeSet.TryGetEntityType(entityTypeId, out entityTypeById));
            PropertyState propertyById;
            PropertyState propertyByCode;
            acDomain.Handle(new PropertyCreateInput
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
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.EntityTypeSet.GetProperties(entityTypeById).Count());
            Assert.IsTrue(acDomain.EntityTypeSet.TryGetProperty(propertyId, out propertyById));
            Assert.IsTrue(acDomain.EntityTypeSet.TryGetProperty(entityTypeById, "Property1", out propertyByCode));
            Assert.AreEqual(propertyByCode, propertyById);
            Assert.IsTrue(ReferenceEquals(propertyById, propertyByCode));

            acDomain.Handle(new PropertyUpdateInput
            {
                Id = propertyId,
                Name = "test2",
                Code = "Property2"
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.EntityTypeSet.GetProperties(entityTypeById).Count);
            Assert.IsTrue(acDomain.EntityTypeSet.TryGetProperty(propertyId, out propertyById));
            Assert.IsTrue(acDomain.EntityTypeSet.TryGetProperty(entityTypeById, "Property2", out propertyByCode));
            Assert.AreEqual(propertyByCode, propertyById);
            Assert.IsTrue(ReferenceEquals(propertyById, propertyByCode));
            Assert.AreEqual("test2", propertyById.Name);
            Assert.AreEqual("Property2", propertyById.Code);

            acDomain.Handle(new RemovePropertyCommand(acDomain.GetAcSession(), propertyId));
            Assert.IsFalse(acDomain.EntityTypeSet.TryGetProperty(propertyId, out propertyById));
            Assert.IsFalse(acDomain.EntityTypeSet.TryGetProperty(entityTypeById, "Property2", out propertyByCode));
            Assert.AreEqual(0, acDomain.EntityTypeSet.GetProperties(entityTypeById).Count);
        }
        #endregion

        #region CanNotDeleteEntityTypeWhenItHasProperties
        [TestMethod]
        public void CanNotDeleteEntityTypeWhenItHasProperties()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.EntityTypeSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityTypeId = Guid.NewGuid();

            acDomain.Handle(new EntityTypeCreateInput
            {
                Id = entityTypeId,
                Code = "EntityType1",
                Name = "测试1",
                Codespace = "Test",
                DatabaseId = acDomain.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                EditHeight = 100,
                EditWidth = 100,
                IsCatalogued = false,
                SchemaName = string.Empty,
                SortCode = 10,
                TableName = string.Empty
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.EntityTypeSet.Count());

            acDomain.Handle(new PropertyCreateInput
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
            }.ToCommand(acDomain.GetAcSession()));

            bool catched = false;
            try
            {
                acDomain.Handle(new RemoveEntityTypeCommand(acDomain.GetAcSession(), entityTypeId));
            }
            catch (ValidationException)
            {
                catched = true;
            }
            finally
            {
                Assert.IsTrue(catched);
                EntityTypeState entityType;
                Assert.IsTrue(acDomain.EntityTypeSet.TryGetEntityType(entityTypeId, out entityType));
            }
        }
        #endregion

        #region EntityTypeSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void EntityTypeSetShouldRollbackedWhenPersistFailed()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.EntityTypeSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            acDomain.RemoveService(typeof(IRepository<EntityType>));
            var moEntityTypeRepository = acDomain.GetMoqRepository<EntityType, IRepository<EntityType>>();
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
                DatabaseId = acDomain.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
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
                DatabaseId = acDomain.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                EditWidth = 100,
                EditHeight = 100,
                IsCatalogued = false,
                SchemaName = string.Empty,
                SortCode = 100,
                TableName = string.Empty
            });
            acDomain.AddService(typeof(IRepository<EntityType>), moEntityTypeRepository.Object);


            bool catched = false;
            try
            {
                acDomain.Handle(new EntityTypeCreateInput
                {
                    Id = entityId1,
                    Code = "EntityType1",
                    Name = "测试1",
                    Codespace = "Test",
                    DatabaseId = acDomain.Rdbs.First().Database.Id,
                    Description = string.Empty,
                    DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                    EditHeight = 100,
                    EditWidth = 100,
                    IsCatalogued = false,
                    SchemaName = string.Empty,
                    SortCode = 10,
                    TableName = string.Empty
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
                Assert.AreEqual(0, acDomain.EntityTypeSet.Count());
            }

            acDomain.Handle(new EntityTypeCreateInput
            {
                Id = entityId2,
                Code = "EntityType2",
                Name = "测试2",
                Codespace = "Test",
                DatabaseId = acDomain.Rdbs.First().Database.Id,
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                EditHeight = 100,
                EditWidth = 100,
                IsCatalogued = false,
                SchemaName = string.Empty,
                SortCode = 10,
                TableName = string.Empty
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.EntityTypeSet.Count());

            catched = false;
            try
            {
                acDomain.Handle(new EntityTypeUpdateInput
                {
                    Id = entityId2,
                    Name = "test2",
                    Code = "EntityType2",
                    Codespace = "test",
                    DatabaseId = acDomain.Rdbs.First().Database.Id,
                    Description = string.Empty,
                    DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                    EditWidth = 100,
                    EditHeight = 100,
                    IsCatalogued = false,
                    SchemaName = string.Empty,
                    SortCode = 100,
                    TableName = string.Empty
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
                Assert.AreEqual(1, acDomain.EntityTypeSet.Count());
                EntityTypeState entityType;
                Assert.IsTrue(acDomain.EntityTypeSet.TryGetEntityType(entityId2, out entityType));
                Assert.AreEqual("EntityType2", entityType.Code);
            }

            catched = false;
            try
            {
                acDomain.Handle(new RemoveEntityTypeCommand(acDomain.GetAcSession(), entityId2));
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
                Assert.IsTrue(acDomain.EntityTypeSet.TryGetEntityType(entityId2, out entityType));
                Assert.AreEqual(1, acDomain.EntityTypeSet.Count());
            }
        }
        #endregion
    }
}
