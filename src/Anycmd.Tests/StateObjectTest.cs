
namespace Anycmd.Tests
{
    using Engine.Ac;
    using Engine.Host.Ac.Infra;
    using System;
    using System.Linq;
    using Xunit;

    public class StateObjectTest
    {
        [Fact]
        public void StateObjectEquals()
        {
            var host = TestHelper.GetAcDomain();
            var entity = new AppSystem
            {
                Id = Guid.NewGuid(),
                Name = "app1",
                Code = "app1",
                Description = string.Empty,
                Icon = string.Empty,
                IsEnabled = 1,
                PrincipalId = host.SysUsers.GetDevAccounts().First().Id,
                SortCode = 10,
                SsoAuthAddress = string.Empty,
                ImageUrl = string.Empty
            };
            var appSystem1 = AppSystemState.Create(host, entity);
            var appSystem2 = appSystem1;

            Assert.Equal(appSystem1, appSystem1);
            Assert.True(appSystem1 == appSystem2);
            Assert.True(appSystem1.Equals(appSystem2));
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
            appSystem2 = AppSystemState.Create(host, entity);
            Assert.Equal(appSystem1, appSystem1);
            Assert.True(appSystem1 == appSystem2);
            Assert.True(appSystem1.Equals(appSystem2));
            entity.Code = "app";
            appSystem2 = AppSystemState.Create(host, entity);
            Assert.NotEqual(appSystem1, appSystem2);
            Assert.False(appSystem1 == appSystem2);
            Assert.False(appSystem1.Equals(appSystem2));
        }
    }
}
