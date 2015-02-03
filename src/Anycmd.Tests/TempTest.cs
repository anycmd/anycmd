
namespace Anycmd.Tests
{
    using Engine.Host.Ac.Infra;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class TempTest
    {
        [TestMethod]
        public void ConfigurationFileTest()
        {
            string fileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            Assert.IsTrue(fileName.EndsWith("dll.config", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(null + string.Empty + " " == " ");
            var entityTypeMaps = new HashSet<EntityTypeMap>();
            var item = EntityTypeMap.Create(this.GetType(), "test", "test");
            entityTypeMaps.Add(item);
            entityTypeMaps.Add(item);
            Assert.AreEqual(1, entityTypeMaps.Count);
        }

        [TestMethod]
        public void ClrTypeTest()
        {
            var privatePublicSubClass = typeof(PrivatePublicSubClass);
            var privateProtectedSubClass = typeof(PrivateProtectedSubClass);
            Assert.IsTrue(typeof(PublicClass).IsNested);
            Assert.IsTrue(!typeof(ProtectedClass).IsPublic);
            Assert.IsTrue(!privatePublicSubClass.IsPublic);
            Assert.IsTrue(!privateProtectedSubClass.IsPublic);
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
