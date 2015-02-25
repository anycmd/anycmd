
namespace Anycmd.Tests
{
    using Ac.ViewModels.Identity.AccountViewModels;
    using Ac.ViewModels.Infra.CatalogViewModels;
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
            var acDomain = TestHelper.GetAcDomain();
            Assert.IsTrue(acDomain.SysUserSet.GetDevAccounts().Count == 1);
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Guid accountId = Guid.NewGuid();
            acDomain.RetrieveRequiredService<IRepository<Account>>().Add(new Account
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                LoginName = "anycmd"
            });
            acDomain.RetrieveRequiredService<IRepository<Account>>().Context.Commit();
            Assert.IsTrue(acDomain.SysUserSet.GetDevAccounts().Count == 1);
            acDomain.Handle(new AddDeveloperCommand(acDomain.GetAcSession(), accountId));
            AccountState developer;
            Assert.IsTrue(acDomain.SysUserSet.GetDevAccounts().Count == 2);
            Assert.IsTrue(acDomain.SysUserSet.TryGetDevAccount(accountId, out developer));
            Assert.IsTrue(acDomain.SysUserSet.TryGetDevAccount("anycmd", out developer));

            acDomain.Handle(new RemoveDeveloperCommand(acDomain.GetAcSession(), accountId));
            Assert.IsTrue(acDomain.SysUserSet.GetDevAccounts().Count == 1);
            Assert.IsFalse(acDomain.SysUserSet.TryGetDevAccount(accountId, out developer));
            Assert.IsFalse(acDomain.SysUserSet.TryGetDevAccount("anycmd", out developer));

            bool catched = false;
            try
            {
                acDomain.Handle(new AddDeveloperCommand(acDomain.GetAcSession(), Guid.NewGuid()));// 将不存在的账户设为开发人员时应引发异常
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
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(1, acDomain.SysUserSet.GetDevAccounts().Count);
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            acDomain.RemoveService(typeof(IRepository<DeveloperId>));
            var moDeveloperRepository = acDomain.GetMoqRepository<DeveloperId, IRepository<DeveloperId>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string name = "测试1";
            const string loginName1 = "anycmd1";
            const string loginName2 = "anycmd2";
            moDeveloperRepository.Setup(a => a.Add(It.Is<DeveloperId>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moDeveloperRepository.Setup(a => a.Remove(It.Is<DeveloperId>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moDeveloperRepository.Setup<DeveloperId>(a => a.GetByKey(entityId1)).Returns(new DeveloperId { Id = entityId1 });
            moDeveloperRepository.Setup<DeveloperId>(a => a.GetByKey(entityId2)).Returns(new DeveloperId { Id = entityId2 });
            acDomain.AddService(typeof(IRepository<DeveloperId>), moDeveloperRepository.Object);
            acDomain.Handle(new CatalogCreateInput
            {
                Id = Guid.NewGuid(),
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }.ToCommand(acDomain.GetAcSession()));
            acDomain.Handle(new AddAccountCommand(acDomain.GetAcSession(), new AccountCreateInput()
            {
                Id = entityId1,
                CatalogCode = "100",
                Code = "test",
                Name = "test",
                AuditState = "anycmd.account.auditStatus.auditPass",
                LoginName = loginName1,
                Password = "111111",
                IsEnabled = 1
            }));
            acDomain.Handle(new AddAccountCommand(acDomain.GetAcSession(), new AccountCreateInput()
            {
                Id = entityId2,
                CatalogCode = "100",
                Code = "tes2t",
                Name = "test2",
                AuditState = "anycmd.account.auditStatus.auditPass",
                LoginName = loginName2,
                Password = "111111",
                IsEnabled = 1
            }));
            acDomain.RetrieveRequiredService<IRepository<Account>>().Context.Commit();
            Assert.IsNotNull(acDomain.RetrieveRequiredService<IRepository<Account>>().GetByKey(entityId2));
            Assert.IsNotNull(AcSessionState.AcMethod.GetAccountById(acDomain, entityId2));
            Assert.IsNotNull(AcSessionState.AcMethod.GetAccountByLoginName(acDomain, loginName2));
            Assert.IsTrue(acDomain.SysUserSet.GetDevAccounts().Count == 1);
            bool catched = false;
            try
            {
                acDomain.Handle(new AddDeveloperCommand(acDomain.GetAcSession(), entityId1));
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
                Assert.AreEqual(1, acDomain.SysUserSet.GetDevAccounts().Count);
            }

            acDomain.Handle(new AddDeveloperCommand(acDomain.GetAcSession(), entityId2));
            Assert.AreEqual(2, acDomain.SysUserSet.GetDevAccounts().Count);

            AcSessionState.AcMethod.SignOut(acDomain, acDomain.GetAcSession());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", loginName2},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            acDomain.Handle(new AccountUpdateInput
            {
                CatalogCode = "100",
                Id = entityId2,
                Name = "test2"
            }.ToCommand(acDomain.GetAcSession()));
            Assert.IsTrue(catched);
            Assert.AreEqual(2, acDomain.SysUserSet.GetDevAccounts().Count);

            catched = false;
            try
            {
                acDomain.Handle(new RemoveDeveloperCommand(acDomain.GetAcSession(), entityId2));
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
                Assert.AreEqual(2, acDomain.SysUserSet.GetDevAccounts().Count);
            }
        }
        #endregion
    }
}
