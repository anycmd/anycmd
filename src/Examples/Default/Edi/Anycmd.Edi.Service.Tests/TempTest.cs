
namespace Anycmd.Edi.Service.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ServiceStack;
    using System;

    [TestClass]
    public class TempTest
    {
        [TestMethod]
        public void StructTest()
        {
            Guid id = Guid.NewGuid();
            StructValue s = StructValue.Create(id, "Name1");
            StructValue s1 = new StructValue().PopulateWith(s);
            Assert.AreEqual(Guid.Empty, s1.Id);
            Assert.AreEqual(null, s1.Name);
            var nameProperty = s1.GetType().GetProperty("Name");
            nameProperty.SetValue(s1, "Name1");
            Assert.AreEqual(null, s1.Name);
            s1.Name = "Name1";
            Assert.AreEqual("Name1", s1.Name);
        }

        public struct StructValue
        {
            public static StructValue Create(Guid id, string name)
            {
                return new StructValue
                {
                    Id = id,
                    Name = name
                };
            }

            public Guid Id { get; private set; }
            public string Name { get; set; }
        }
    }
}
