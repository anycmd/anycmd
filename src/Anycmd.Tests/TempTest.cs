
namespace Anycmd.Tests
{
    using Engine.Host.Ac.Infra;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class TempTest
    {
        [Fact]
        public void ConfigurationFileTest()
        {
            string fileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            Assert.True(fileName.EndsWith("dll.config", StringComparison.OrdinalIgnoreCase));
            Assert.True(null + string.Empty + " " == " ");
            var entityTypeMaps = new HashSet<EntityTypeMap>();
            var item = EntityTypeMap.Create(this.GetType(), "test", "test");
            entityTypeMaps.Add(item);
            entityTypeMaps.Add(item);
            Assert.Equal(1, entityTypeMaps.Count);
        }

        [Fact]
        public void ClrTypeTest()
        {
            var privatePublicSubClass = typeof(PrivatePublicSubClass);
            var privateProtectedSubClass = typeof(PrivateProtectedSubClass);
            Assert.True(typeof(PublicClass).IsNested);
            Assert.True(!typeof(ProtectedClass).IsPublic);
            Assert.True(!privatePublicSubClass.IsPublic);
            Assert.True(!privateProtectedSubClass.IsPublic);
        }

        protected class ProtectedClass
        {

        }

        public class PublicClass
        {

        }

        public class PublicPublicSubClass : PublicClass
        {

        }

        private class PrivatePublicSubClass : PublicClass
        {

        }

        private class PrivateProtectedSubClass : ProtectedClass
        {

        }
    }
}
