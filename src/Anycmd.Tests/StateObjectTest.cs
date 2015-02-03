
namespace Anycmd.Tests
{
    using Engine.Ac;
    using Engine.Host.Ac.Infra;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    [TestClass]
    public class StateObjectTest
    {
        [TestMethod]
        public void StateObjectEquals()
        {
            var acDomain = TestHelper.GetAcDomain();
            var entity = new AppSystem
            {
                Id = Guid.NewGuid(),
                Name = "app1",
                Code = "app1",
                Description = string.Empty,
                Icon = string.Empty,
                IsEnabled = 1,
                PrincipalId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                SortCode = 10,
                SsoAuthAddress = string.Empty,
                ImageUrl = string.Empty
            };
            var appSystem1 = AppSystemState.Create(acDomain, entity);
            var appSystem2 = appSystem1;

            Assert.AreEqual(appSystem1, appSystem1);
            Assert.IsTrue(appSystem1 == appSystem2);
            Assert.IsTrue(appSystem1.Equals(appSystem2));
            entity = new AppSystem
            {
                Id = appSystem1.Id,
                Name = "app1",
                Code = "app1",
                Description = string.Empty,
                Icon = string.Empty,
                IsEnabled = 1,
                PrincipalId = appSystem1.PrincipalId,
                SortCode = 10,
                SsoAuthAddress = string.Empty,
                ImageUrl = string.Empty
            };
            appSystem2 = AppSystemState.Create(acDomain, entity);
            Assert.AreEqual(appSystem1, appSystem1);
            Assert.IsTrue(appSystem1 == appSystem2);
            Assert.IsTrue(appSystem1.Equals(appSystem2));
            entity.Code = "app";
            appSystem2 = AppSystemState.Create(acDomain, entity);
            Assert.AreNotEqual(appSystem1, appSystem2);
            Assert.IsFalse(appSystem1 == appSystem2);
            Assert.IsFalse(appSystem1.Equals(appSystem2));
        }
    }
}
