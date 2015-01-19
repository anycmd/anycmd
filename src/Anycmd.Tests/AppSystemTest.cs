
namespace Anycmd.Tests
{
    using Ac.ViewModels.Infra.AppSystemViewModels;
    using Ac.ViewModels.Infra.MenuViewModels;
    using Engine.Ac;
    using Engine.Ac.Messages.Infra;
    using Engine.Host.Ac.Infra;
    using Exceptions;
    using Moq;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class AppSystemTest
    {
        #region AppSystemSet
        [Fact]
        public void AppSystemSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.AppSystemSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            AppSystemState appSystemById;
            AppSystemState appSystemByCode;
            host.Handle(new AppSystemCreateInput
            {
                Id = entityId,
                Code = "app1",
                Name = "测试1",
                PrincipalId = host.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(host.GetUserSession()));
            Assert.Equal(2, host.AppSystemSet.Count());
            Assert.True(host.AppSystemSet.TryGetAppSystem(entityId, out appSystemById));
            Assert.True(host.AppSystemSet.TryGetAppSystem("app1", out appSystemByCode));
            Assert.Equal(appSystemByCode, appSystemById);
            Assert.True(ReferenceEquals(appSystemById, appSystemByCode));
            host.Handle(new AppSystemUpdateInput
            {
                Id = entityId,
                Name = "test2",
                Code = "app2",
                PrincipalId = host.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(host.GetUserSession()));
            AppSystemState appSystemById1;
            AppSystemState appSystemByCode1;
            Assert.Equal(2, host.AppSystemSet.Count());
            Assert.True(host.AppSystemSet.TryGetAppSystem(entityId, out appSystemById1));
            Assert.True(host.AppSystemSet.TryGetAppSystem("app2", out appSystemByCode1));
            Assert.NotEqual(appSystemByCode, appSystemByCode1);
            Assert.NotEqual(appSystemById, appSystemById1);
            Assert.False(ReferenceEquals(appSystemById, appSystemById1));
            Assert.Equal(appSystemByCode1, appSystemById1);
            Assert.True(ReferenceEquals(appSystemById1, appSystemByCode1));
            Assert.Equal("test2", appSystemById1.Name);
            Assert.Equal("app2", appSystemById1.Code);

            Assert.NotNull(host.RetrieveRequiredService<IRepository<AppSystem>>().GetByKey(entityId));
            host.Handle(new RemoveAppSystemCommand(host.GetUserSession(), entityId));
            Assert.False(host.AppSystemSet.TryGetAppSystem(entityId, out appSystemById1));
            Assert.False(host.AppSystemSet.TryGetAppSystem("app2", out appSystemByCode1));
            Assert.Equal(1, host.AppSystemSet.Count());
        }
        #endregion

        #region CanNotDeleteAppSystemWhenItHasMenus
        [Fact]
        public void CanNotDeleteAppSystemWhenItHasMenus()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.AppSystemSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            host.Handle(new AppSystemCreateInput
            {
                Id = entityId,
                Code = "app1",
                Name = "测试1",
                PrincipalId = host.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(host.GetUserSession()));
            Assert.Equal(2, host.AppSystemSet.Count());

            host.Handle(new MenuCreateInput
            {
                Id = Guid.NewGuid(),
                AppSystemId = entityId,
                Name = "menu1",
                SortCode = 10,
                Url = string.Empty,
                Description = string.Empty,
                Icon = string.Empty,
                ParentId = null
            }.ToCommand(host.GetUserSession()));

            bool catched = false;
            try
            {
                host.Handle(new RemoveAppSystemCommand(host.GetUserSession(), entityId));
            }
            catch (ValidationException)
            {
                catched = true;
            }
            finally
            {
                Assert.True(catched);
                AppSystemState appSystem;
                Assert.True(host.AppSystemSet.TryGetAppSystem(entityId, out appSystem));
            }
        }
        #endregion

        #region AppSystemSetShouldRollbackedWhenPersistFailed
        [Fact]
        public void AppSystemSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.AppSystemSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var moAppSystemRepository = host.GetMoqRepository<AppSystem, IRepository<AppSystem>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string code = "app1";
            const string name = "测试1";
            host.RemoveService(typeof(IRepository<AppSystem>));
            moAppSystemRepository.Setup(a => a.Add(It.Is<AppSystem>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moAppSystemRepository.Setup(a => a.Update(It.Is<AppSystem>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moAppSystemRepository.Setup(a => a.Remove(It.Is<AppSystem>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moAppSystemRepository.Setup<AppSystem>(a => a.GetByKey(entityId1)).Returns(new AppSystem { Id = entityId1, Code = code, Name = name });
            moAppSystemRepository.Setup<AppSystem>(a => a.GetByKey(entityId2)).Returns(new AppSystem { Id = entityId2, Code = code, Name = name });
            host.AddService(typeof(IRepository<AppSystem>), moAppSystemRepository.Object);

            bool catched = false;
            try
            {
                host.Handle(new AppSystemCreateInput
                {
                    Id = entityId1,
                    Code = code,
                    Name = name,
                    PrincipalId = host.SysUserSet.GetDevAccounts().First().Id
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
                Assert.Equal(1, host.AppSystemSet.Count());
            }

            host.Handle(new AppSystemCreateInput
            {
                Id = entityId2,
                Code = code,
                Name = name,
                PrincipalId = host.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(host.GetUserSession()));
            Assert.Equal(2, host.AppSystemSet.Count());

            catched = false;
            try
            {
                host.Handle(new AppSystemUpdateInput
                {
                    Id = entityId2,
                    Name = "test2",
                    Code = "app2",
                    PrincipalId = host.SysUserSet.GetDevAccounts().First().Id
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
                Assert.Equal(2, host.AppSystemSet.Count());
                AppSystemState appSystem;
                Assert.True(host.AppSystemSet.TryGetAppSystem(entityId2, out appSystem));
                Assert.Equal(code, appSystem.Code);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveAppSystemCommand(host.GetUserSession(), entityId2));
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
                AppSystemState appSystem;
                Assert.True(host.AppSystemSet.TryGetAppSystem(entityId2, out appSystem));
                Assert.Equal(2, host.AppSystemSet.Count());
            }
        }
        #endregion
    }
}
