
namespace Anycmd.Tests
{
    using Ac.ViewModels.Infra.AppSystemViewModels;
    using Ac.ViewModels.Infra.FunctionViewModels;
    using Engine.Ac;
    using Engine.Ac.Messages.Infra;
    using Engine.Host.Ac.Infra;
    using Moq;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class FunctionSetTest
    {
        #region FunctionSet
        [Fact]
        public void FunctionSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.FunctionSet.Count());
            AcSessionState.SignIn(host, new Dictionary<string, object>
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
            Assert.True(host.ResourceTypeSet.TryGetResource(host.ResourceTypeSet.First().Id, out resource));
            Assert.Equal(1, host.FunctionSet.Count());
            Assert.True(host.FunctionSet.TryGetFunction(entityId, out functionById));

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
            Assert.Equal(1, host.FunctionSet.Count());
            Assert.True(host.FunctionSet.TryGetFunction(entityId, out functionById));
            Assert.Equal("test2", functionById.Description);
            Assert.Equal("fun2", functionById.Code);

            host.Handle(new RemoveFunctionCommand(host.GetAcSession(), entityId));
            Assert.False(host.FunctionSet.TryGetFunction(entityId, out functionById));
            Assert.Equal(0, host.FunctionSet.Count());
        }
        #endregion

        [Fact]
        public void FunctionCodeMustBeUnique()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.FunctionSet.Count());
            AcSessionState.SignIn(host, new Dictionary<string, object>
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
            Assert.True(host.ResourceTypeSet.TryGetResource(host.ResourceTypeSet.First().Id, out resource));
            Assert.Equal(1, host.FunctionSet.Count());
            Assert.True(host.FunctionSet.TryGetFunction(entityId, out functionById));
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
                Assert.True(catched);
            }
        }

        #region FunctionSetShouldRollbackedWhenPersistFailed
        [Fact]
        public void FunctionSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.FunctionSet.Count());
            AcSessionState.SignIn(host, new Dictionary<string, object>
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
                Assert.Equal(e.GetType(), typeof(DbException));
                catched = true;
                Assert.Equal(entityId1.ToString(), e.Message);
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(0, host.FunctionSet.Count());
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
            Assert.Equal(1, host.FunctionSet.Count());

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
                Assert.Equal(e.GetType(), typeof(DbException));
                catched = true;
                Assert.Equal(entityId2.ToString(), e.Message);
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(1, host.FunctionSet.Count());
                FunctionState function;
                Assert.True(host.FunctionSet.TryGetFunction(entityId2, out function));
                Assert.Equal("fun2", function.Code);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveFunctionCommand(host.GetAcSession(), entityId2));
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
                FunctionState function;
                Assert.True(host.FunctionSet.TryGetFunction(entityId2, out function));
                Assert.Equal(1, host.FunctionSet.Count());
            }
        }
        #endregion
    }
}
