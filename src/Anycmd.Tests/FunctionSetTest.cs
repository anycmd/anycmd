
namespace Anycmd.Tests
{
    using Ac.ViewModels.Infra.AppSystemViewModels;
    using Ac.ViewModels.Infra.FunctionViewModels;
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
    public class FunctionSetTest
    {
        #region FunctionSet
        [TestMethod]
        public void FunctionSet()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.FunctionSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            FunctionState functionById;
            acDomain.Handle(new FunctionCreateInput
            {
                Id = entityId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = acDomain.ResourceTypeSet.First().Id,
                SortCode = 10
            }.ToCommand(acDomain.GetAcSession()));
            ResourceTypeState resource;
            Assert.IsTrue(acDomain.ResourceTypeSet.TryGetResource(acDomain.ResourceTypeSet.First().Id, out resource));
            Assert.AreEqual(1, acDomain.FunctionSet.Count());
            Assert.IsTrue(acDomain.FunctionSet.TryGetFunction(entityId, out functionById));

            acDomain.Handle(new FunctionUpdateInput
            {
                Id = entityId,
                Description = "test2",
                Code = "fun2",
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = false,
                SortCode = 10
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.FunctionSet.Count());
            Assert.IsTrue(acDomain.FunctionSet.TryGetFunction(entityId, out functionById));
            Assert.AreEqual("test2", functionById.Description);
            Assert.AreEqual("fun2", functionById.Code);

            acDomain.Handle(new RemoveFunctionCommand(acDomain.GetAcSession(), entityId));
            Assert.IsFalse(acDomain.FunctionSet.TryGetFunction(entityId, out functionById));
            Assert.AreEqual(0, acDomain.FunctionSet.Count());
        }
        #endregion

        [TestMethod]
        public void FunctionCodeMustBeUnique()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.FunctionSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            FunctionState functionById;
            acDomain.Handle(new FunctionCreateInput
            {
                Id = entityId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = acDomain.ResourceTypeSet.First().Id,
                SortCode = 10
            }.ToCommand(acDomain.GetAcSession()));
            ResourceTypeState resource;
            Assert.IsTrue(acDomain.ResourceTypeSet.TryGetResource(acDomain.ResourceTypeSet.First().Id, out resource));
            Assert.AreEqual(1, acDomain.FunctionSet.Count());
            Assert.IsTrue(acDomain.FunctionSet.TryGetFunction(entityId, out functionById));
            bool catched = false;
            try
            {
                acDomain.Handle(new FunctionCreateInput
                {
                    Id = entityId,
                    Code = "fun1",
                    Description = string.Empty,
                    DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                    IsEnabled = 1,
                    IsManaged = true,
                    ResourceTypeId = acDomain.ResourceTypeSet.First().Id,
                    SortCode = 10
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
        }

        #region FunctionSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void FunctionSetShouldRollbackedWhenPersistFailed()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.FunctionSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            acDomain.RemoveService(typeof(IRepository<Function>));
            var moFunctionRepository = acDomain.GetMoqRepository<Function, IRepository<Function>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            var appsystemId = Guid.NewGuid();
            moFunctionRepository.Setup(a => a.Add(It.Is<Function>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moFunctionRepository.Setup(a => a.Update(It.Is<Function>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moFunctionRepository.Setup(a => a.Remove(It.Is<Function>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moFunctionRepository.Setup<Function>(a => a.GetByKey(entityId1)).Returns(new Function
            {
                Id = entityId1,
                ResourceTypeId = acDomain.ResourceTypeSet.First().Id,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id
            });
            moFunctionRepository.Setup<Function>(a => a.GetByKey(entityId2)).Returns(new Function
            {
                Id = entityId2,
                ResourceTypeId = acDomain.ResourceTypeSet.First().Id,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id
            });
            acDomain.AddService(typeof(IRepository<Function>), moFunctionRepository.Object);

            acDomain.Handle(new AppSystemCreateInput
            {
                Id = appsystemId,
                Code = "app1",
                Name = "测试1",
                PrincipalId = acDomain.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(acDomain.GetAcSession()));

            bool catched = false;
            try
            {
                acDomain.Handle(new FunctionCreateInput
                {
                    Id = entityId1,
                    Code = "fun1",
                    Description = string.Empty,
                    DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                    IsEnabled = 1,
                    IsManaged = true,
                    ResourceTypeId = acDomain.ResourceTypeSet.First().Id,
                    SortCode = 10
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
                Assert.AreEqual(0, acDomain.FunctionSet.Count());
            }

            acDomain.Handle(new FunctionCreateInput
            {
                Id = entityId2,
                Code = "fun2",
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = acDomain.ResourceTypeSet.First().Id,
                SortCode = 10
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.FunctionSet.Count());

            catched = false;
            try
            {
                acDomain.Handle(new FunctionUpdateInput
                {
                    Id = entityId2,
                    Description = "test2",
                    Code = "fun",
                    DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                    IsEnabled = 1,
                    IsManaged = false,
                    SortCode = 10
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
                Assert.AreEqual(1, acDomain.FunctionSet.Count());
                FunctionState function;
                Assert.IsTrue(acDomain.FunctionSet.TryGetFunction(entityId2, out function));
                Assert.AreEqual("fun2", function.Code);
            }

            catched = false;
            try
            {
                acDomain.Handle(new RemoveFunctionCommand(acDomain.GetAcSession(), entityId2));
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
                FunctionState function;
                Assert.IsTrue(acDomain.FunctionSet.TryGetFunction(entityId2, out function));
                Assert.AreEqual(1, acDomain.FunctionSet.Count());
            }
        }
        #endregion
    }
}
