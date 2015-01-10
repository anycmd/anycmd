
namespace Anycmd.Tests
{
    using Ac.ViewModels.Identity.AccountViewModels;
    using Engine.Ac;
    using Engine.Ac.Messages.Identity;
    using Engine.Host.Ac.Identity;
    using Moq;
    using Repositories;
    using System;
    using Xunit;

    public class SysUserTest
    {
        [Fact]
        public void SysUserSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.True(host.SysUserSet.GetDevAccounts().Count == 1);
            Guid accountId = Guid.NewGuid();
            host.RetrieveRequiredService<IRepository<Account>>().Add(new Account
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                LoginName = "anycmd"
            });
            host.RetrieveRequiredService<IRepository<Account>>().Context.Commit();
            Assert.True(host.SysUserSet.GetDevAccounts().Count == 1);
            host.Handle(new AddDeveloperCommand(accountId));
            AccountState developer;
            Assert.True(host.SysUserSet.GetDevAccounts().Count == 2);
            Assert.True(host.SysUserSet.TryGetDevAccount(accountId, out developer));
            Assert.True(host.SysUserSet.TryGetDevAccount("anycmd", out developer));

            host.Handle(new RemoveDeveloperCommand(accountId));
            Assert.True(host.SysUserSet.GetDevAccounts().Count == 1);
            Assert.False(host.SysUserSet.TryGetDevAccount(accountId, out developer));
            Assert.False(host.SysUserSet.TryGetDevAccount("anycmd", out developer));

            bool catched = false;
            try
            {
                host.Handle(new AddDeveloperCommand(Guid.NewGuid()));// 将不存在的账户设为开发人员时应引发异常
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

        #region SysUserSetShouldRollbackedWhenPersistFailed
        [Fact]
        public void SysUserSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.SysUserSet.GetDevAccounts().Count);

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
                LoginName = loginName2
            });
            host.RetrieveRequiredService<IRepository<Account>>().Context.Commit();
            Assert.True(host.SysUserSet.GetDevAccounts().Count == 1);
            bool catched = false;
            try
            {
                host.Handle(new AddDeveloperCommand(entityId1));
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
                Assert.Equal(1, host.SysUserSet.GetDevAccounts().Count);
            }

            host.Handle(new AddDeveloperCommand(entityId2));
            Assert.Equal(2, host.SysUserSet.GetDevAccounts().Count);

            host.Handle(new UpdateAccountCommand(new AccountUpdateInput
            {
                Id = entityId2,
                Name = "test2"
            }));
            Assert.True(catched);
            Assert.Equal(2, host.SysUserSet.GetDevAccounts().Count);

            catched = false;
            try
            {
                host.Handle(new RemoveDeveloperCommand(entityId2));
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
                Assert.Equal(2, host.SysUserSet.GetDevAccounts().Count);
            }
        }
        #endregion
    }
}
