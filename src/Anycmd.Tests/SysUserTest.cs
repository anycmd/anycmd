
namespace Anycmd.Tests
{
    using Ac.ViewModels.Identity.AccountViewModels;
    using Engine.Ac;
    using Engine.Ac.Messages.Identity;
    using Engine.Host.Ac.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Repositories;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class SysUserTest
    {
        [TestMethod]
        public void SysUserSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.IsTrue(host.SysUserSet.GetDevAccounts().Count == 1);
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Guid accountId = Guid.NewGuid();
            host.RetrieveRequiredService<IRepository<Account>>().Add(new Account
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                LoginName = "anycmd"
            });
            host.RetrieveRequiredService<IRepository<Account>>().Context.Commit();
            Assert.IsTrue(host.SysUserSet.GetDevAccounts().Count == 1);
            host.Handle(new AddDeveloperCommand(host.GetAcSession(), accountId));
            AccountState developer;
            Assert.IsTrue(host.SysUserSet.GetDevAccounts().Count == 2);
            Assert.IsTrue(host.SysUserSet.TryGetDevAccount(accountId, out developer));
            Assert.IsTrue(host.SysUserSet.TryGetDevAccount("anycmd", out developer));

            host.Handle(new RemoveDeveloperCommand(host.GetAcSession(), accountId));
            Assert.IsTrue(host.SysUserSet.GetDevAccounts().Count == 1);
            Assert.IsFalse(host.SysUserSet.TryGetDevAccount(accountId, out developer));
            Assert.IsFalse(host.SysUserSet.TryGetDevAccount("anycmd", out developer));

            bool catched = false;
            try
            {
                host.Handle(new AddDeveloperCommand(host.GetAcSession(), Guid.NewGuid()));// 将不存在的账户设为开发人员时应引发异常
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

        #region SysUserSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void SysUserSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(1, host.SysUserSet.GetDevAccounts().Count);
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.RemoveService(typeof(IRepository<Account>));
            host.RemoveService(typeof(IRepository<DeveloperId>));
            var moAccountRepository = host.GetMoqRepository<Account, IRepository<Account>>();
            var moDeveloperRepository = host.GetMoqRepository<DeveloperId, IRepository<DeveloperId>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string name = "测试1";
            const string loginName1 = "anycmd1";
            const string loginName2 = "anycmd2";
            moDeveloperRepository.Setup(a => a.Add(It.Is<DeveloperId>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moDeveloperRepository.Setup(a => a.Remove(It.Is<DeveloperId>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moAccountRepository.Setup<Account>(a => a.GetByKey(entityId1)).Returns(new Account { Id = entityId1, Name = name, LoginName = loginName1 });
            moAccountRepository.Setup<Account>(a => a.GetByKey(entityId2)).Returns(new Account { Id = entityId2, Name = name, LoginName = loginName2 });
            moDeveloperRepository.Setup<DeveloperId>(a => a.GetByKey(entityId1)).Returns(new DeveloperId { Id = entityId1 });
            moDeveloperRepository.Setup<DeveloperId>(a => a.GetByKey(entityId2)).Returns(new DeveloperId { Id = entityId2 });
            host.AddService(typeof(IRepository<Account>), moAccountRepository.Object);
            host.AddService(typeof(IRepository<DeveloperId>), moDeveloperRepository.Object);

            host.RetrieveRequiredService<IRepository<Account>>().Add(new Account
            {
                Id = entityId1,
                Code = "test",
                Name = "test",
                LoginName = loginName1
            });
            host.RetrieveRequiredService<IRepository<Account>>().Add(new Account
            {
                Id = entityId2,
                Code = "tes2t",
                Name = "test2",
                LoginName = loginName2,
                Password = "111111"
            });
            host.RetrieveRequiredService<IRepository<Account>>().Context.Commit();
            Assert.IsNotNull(host.RetrieveRequiredService<IRepository<Account>>().GetByKey(entityId2));
            Assert.IsNotNull(AcSessionState.AcMethod.GetAccountById(host, entityId2));
            Assert.IsNotNull(AcSessionState.AcMethod.GetAccountByLoginName(host, loginName2));
            Assert.IsTrue(host.SysUserSet.GetDevAccounts().Count == 1);
            bool catched = false;
            try
            {
                host.Handle(new AddDeveloperCommand(host.GetAcSession(), entityId1));
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
                Assert.AreEqual(1, host.SysUserSet.GetDevAccounts().Count);
            }

            host.Handle(new AddDeveloperCommand(host.GetAcSession(), entityId2));
            Assert.AreEqual(2, host.SysUserSet.GetDevAccounts().Count);

            AcSessionState.AcMethod.SignOut(host, host.GetAcSession());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", loginName2},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            host.Handle(new AccountUpdateInput
            {
                Id = entityId2,
                Name = "test2"
            }.ToCommand(host.GetAcSession()));
            Assert.IsTrue(catched);
            Assert.AreEqual(2, host.SysUserSet.GetDevAccounts().Count);

            catched = false;
            try
            {
                host.Handle(new RemoveDeveloperCommand(host.GetAcSession(), entityId2));
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
                Assert.AreEqual(2, host.SysUserSet.GetDevAccounts().Count);
            }
        }
        #endregion
    }
}
