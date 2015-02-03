
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
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.FunctionSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            FunctionState functionById;
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
            ResourceTypeState resource;
            Assert.IsTrue(host.ResourceTypeSet.TryGetResource(host.ResourceTypeSet.First().Id, out resource));
            Assert.AreEqual(1, host.FunctionSet.Count());
            Assert.IsTrue(host.FunctionSet.TryGetFunction(entityId, out functionById));

            host.Handle(new FunctionUpdateInput
            {
                Id = entityId,
                Description = "test2",
                Code = "fun2",
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = false,
                SortCode = 10
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.FunctionSet.Count());
            Assert.IsTrue(host.FunctionSet.TryGetFunction(entityId, out functionById));
            Assert.AreEqual("test2", functionById.Description);
            Assert.AreEqual("fun2", functionById.Code);

            host.Handle(new RemoveFunctionCommand(host.GetAcSession(), entityId));
            Assert.IsFalse(host.FunctionSet.TryGetFunction(entityId, out functionById));
            Assert.AreEqual(0, host.FunctionSet.Count());
        }
        #endregion

        [TestMethod]
        public void FunctionCodeMustBeUnique()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.FunctionSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            FunctionState functionById;
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
            ResourceTypeState resource;
            Assert.IsTrue(host.ResourceTypeSet.TryGetResource(host.ResourceTypeSet.First().Id, out resource));
            Assert.AreEqual(1, host.FunctionSet.Count());
            Assert.IsTrue(host.FunctionSet.TryGetFunction(entityId, out functionById));
            bool catched = false;
            try
            {
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
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.FunctionSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.RemoveService(typeof(IRepository<Function>));
            var moFunctionRepository = host.GetMoqRepository<Function, IRepository<Function>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            var appsystemId = Guid.NewGuid();
            moFunctionRepository.Setup(a => a.Add(It.Is<Function>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moFunctionRepository.Setup(a => a.Update(It.Is<Function>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moFunctionRepository.Setup(a => a.Remove(It.Is<Function>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moFunctionRepository.Setup<Function>(a => a.GetByKey(entityId1)).Returns(new Function
            {
                Id = entityId1,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id
            });
            moFunctionRepository.Setup<Function>(a => a.GetByKey(entityId2)).Returns(new Function
            {
                Id = entityId2,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id
            });
            host.AddService(typeof(IRepository<Function>), moFunctionRepository.Object);

            host.Handle(new AppSystemCreateInput
            {
                Id = appsystemId,
                Code = "app1",
                Name = "测试1",
                PrincipalId = host.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(host.GetAcSession()));

            bool catched = false;
            try
            {
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
                Assert.AreEqual(0, host.FunctionSet.Count());
            }

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
            Assert.AreEqual(1, host.FunctionSet.Count());

            catched = false;
            try
            {
                host.Handle(new FunctionUpdateInput
                {
                    Id = entityId2,
                    Description = "test2",
                    Code = "fun",
                    DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                    IsEnabled = 1,
                    IsManaged = false,
                    SortCode = 10
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
                Assert.AreEqual(1, host.FunctionSet.Count());
                FunctionState function;
                Assert.IsTrue(host.FunctionSet.TryGetFunction(entityId2, out function));
                Assert.AreEqual("fun2", function.Code);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveFunctionCommand(host.GetAcSession(), entityId2));
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
                Assert.IsTrue(host.FunctionSet.TryGetFunction(entityId2, out function));
                Assert.AreEqual(1, host.FunctionSet.Count());
            }
        }
        #endregion
    }
}
