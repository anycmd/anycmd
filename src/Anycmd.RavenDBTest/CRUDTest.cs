
namespace Anycmd.RavenDBTest
{
    using Engine.Host.Ac.Infra;
    using Raven.Abstractions.Commands;
    using System;
    using Xunit;

    public class CrudTest
    {
        [Fact]
        public void Crud()
        {
            var appSystem = new AppSystem
            {
                Id = Guid.NewGuid(),
                Name = "test",
                Code = "test",
                Description = "test",
                Icon = "icon1",
                ImageUrl = string.Empty,
                IsEnabled = 1,
                PrincipalId = Guid.NewGuid(),
                SortCode = 1,
                SsoAuthAddress = string.Empty
            };
            using (var session = CsStore.SingleInstance.OpenSession("anycmd"))
            {
                session.Store(appSystem);
                session.SaveChanges();
            }
            using (var session = CsStore.SingleInstance.OpenSession("anycmd"))
            {
                var entity = session.Load<AppSystem>(appSystem.Id);
                Assert.NotNull(entity);
                Assert.Equal(appSystem.PrincipalId, entity.PrincipalId);
            }
            using (var session = CsStore.SingleInstance.OpenSession("anycmd"))
            {
                var entity = session.Load<AppSystem>(appSystem.Id);
                entity.Name = "new name";
                session.SaveChanges();
            }
            using (var session = CsStore.SingleInstance.OpenSession("anycmd"))
            {
                var entity = session.Load<AppSystem>(appSystem.Id);
                Assert.Equal("new name", entity.Name);
            }
            using (var session = CsStore.SingleInstance.OpenSession("anycmd"))
            {
                session.Advanced.Defer(new DeleteCommandData { Key = "AppSystems/" + appSystem.Id.ToString() });
                session.SaveChanges();
                var entity = session.Load<AppSystem>(appSystem.Id);
                Assert.Null(entity);
            }
        }
    }
}
