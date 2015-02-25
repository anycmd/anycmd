
namespace Anycmd.Tests
{
    using Ac.ViewModels.AppSystemViewModels;
    using Ac.ViewModels.MenuViewModels;
    using Engine.Ac;
    using Engine.Ac.AppSystems;
    using Engine.Host.Ac.Infra;
    using Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class AppSystemTest
    {
        #region AppSystemSet
        [TestMethod]
        public void AppSystemSet()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(1, acDomain.AppSystemSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            AppSystemState appSystemById;
            AppSystemState appSystemByCode;
            acDomain.Handle(new AppSystemCreateInput
            {
                Id = entityId,
                Code = "app1",
                Name = "测试1",
                PrincipalId = acDomain.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(2, acDomain.AppSystemSet.Count());
            Assert.IsTrue(acDomain.AppSystemSet.TryGetAppSystem(entityId, out appSystemById));
            Assert.IsTrue(acDomain.AppSystemSet.TryGetAppSystem("app1", out appSystemByCode));
            Assert.AreEqual(appSystemByCode, appSystemById);
            Assert.IsTrue(ReferenceEquals(appSystemById, appSystemByCode));
            acDomain.Handle(new AppSystemUpdateInput
            {
                Id = entityId,
                Name = "test2",
                Code = "app2",
                PrincipalId = acDomain.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(acDomain.GetAcSession()));
            AppSystemState appSystemById1;
            AppSystemState appSystemByCode1;
            Assert.AreEqual(2, acDomain.AppSystemSet.Count());
            Assert.IsTrue(acDomain.AppSystemSet.TryGetAppSystem(entityId, out appSystemById1));
            Assert.IsTrue(acDomain.AppSystemSet.TryGetAppSystem("app2", out appSystemByCode1));
            Assert.AreNotEqual(appSystemByCode, appSystemByCode1);
            Assert.AreNotEqual(appSystemById, appSystemById1);
            Assert.IsFalse(ReferenceEquals(appSystemById, appSystemById1));
            Assert.AreEqual(appSystemByCode1, appSystemById1);
            Assert.IsTrue(ReferenceEquals(appSystemById1, appSystemByCode1));
            Assert.AreEqual("test2", appSystemById1.Name);
            Assert.AreEqual("app2", appSystemById1.Code);

            Assert.IsNotNull(acDomain.RetrieveRequiredService<IRepository<AppSystem>>().GetByKey(entityId));
            acDomain.Handle(new RemoveAppSystemCommand(acDomain.GetAcSession(), entityId));
            Assert.IsFalse(acDomain.AppSystemSet.TryGetAppSystem(entityId, out appSystemById1));
            Assert.IsFalse(acDomain.AppSystemSet.TryGetAppSystem("app2", out appSystemByCode1));
            Assert.AreEqual(1, acDomain.AppSystemSet.Count());
        }
        #endregion

        #region CanNotDeleteAppSystemWhenItHasMenus
        [TestMethod]
        public void CanNotDeleteAppSystemWhenItHasMenus()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(1, acDomain.AppSystemSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            acDomain.Handle(new AppSystemCreateInput
            {
                Id = entityId,
                Code = "app1",
                Name = "测试1",
                PrincipalId = acDomain.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(2, acDomain.AppSystemSet.Count());

            acDomain.Handle(new MenuCreateInput
            {
                Id = Guid.NewGuid(),
                AppSystemId = entityId,
                Name = "menu1",
                SortCode = 10,
                Url = string.Empty,
                Description = string.Empty,
                Icon = string.Empty,
                ParentId = null
            }.ToCommand(acDomain.GetAcSession()));

            bool catched = false;
            try
            {
                acDomain.Handle(new RemoveAppSystemCommand(acDomain.GetAcSession(), entityId));
            }
            catch (ValidationException)
            {
                catched = true;
            }
            finally
            {
                Assert.IsTrue(catched);
                AppSystemState appSystem;
                Assert.IsTrue(acDomain.AppSystemSet.TryGetAppSystem(entityId, out appSystem));
            }
        }
        #endregion

        #region AppSystemSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void AppSystemSetShouldRollbackedWhenPersistFailed()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(1, acDomain.AppSystemSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var moAppSystemRepository = acDomain.GetMoqRepository<AppSystem, IRepository<AppSystem>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string code = "app1";
            const string name = "测试1";
            acDomain.RemoveService(typeof(IRepository<AppSystem>));
            moAppSystemRepository.Setup(a => a.Add(It.Is<AppSystem>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moAppSystemRepository.Setup(a => a.Update(It.Is<AppSystem>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moAppSystemRepository.Setup(a => a.Remove(It.Is<AppSystem>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moAppSystemRepository.Setup<AppSystem>(a => a.GetByKey(entityId1)).Returns(new AppSystem { Id = entityId1, Code = code, Name = name });
            moAppSystemRepository.Setup<AppSystem>(a => a.GetByKey(entityId2)).Returns(new AppSystem { Id = entityId2, Code = code, Name = name });
            acDomain.AddService(typeof(IRepository<AppSystem>), moAppSystemRepository.Object);

            bool catched = false;
            try
            {
                acDomain.Handle(new AppSystemCreateInput
                {
                    Id = entityId1,
                    Code = code,
                    Name = name,
                    PrincipalId = acDomain.SysUserSet.GetDevAccounts().First().Id
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
                Assert.AreEqual(1, acDomain.AppSystemSet.Count());
            }

            acDomain.Handle(new AppSystemCreateInput
            {
                Id = entityId2,
                Code = code,
                Name = name,
                PrincipalId = acDomain.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(2, acDomain.AppSystemSet.Count());

            catched = false;
            try
            {
                acDomain.Handle(new AppSystemUpdateInput
                {
                    Id = entityId2,
                    Name = "test2",
                    Code = "app2",
                    PrincipalId = acDomain.SysUserSet.GetDevAccounts().First().Id
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
                Assert.AreEqual(2, acDomain.AppSystemSet.Count());
                AppSystemState appSystem;
                Assert.IsTrue(acDomain.AppSystemSet.TryGetAppSystem(entityId2, out appSystem));
                Assert.AreEqual(code, appSystem.Code);
            }

            catched = false;
            try
            {
                acDomain.Handle(new RemoveAppSystemCommand(acDomain.GetAcSession(), entityId2));
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
                AppSystemState appSystem;
                Assert.IsTrue(acDomain.AppSystemSet.TryGetAppSystem(entityId2, out appSystem));
                Assert.AreEqual(2, acDomain.AppSystemSet.Count());
            }
        }
        #endregion
    }
}
