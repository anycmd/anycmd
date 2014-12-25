
namespace Anycmd.Edi.Service.Tests
{
    using ServiceStack;
    using System;
    using Xunit;

    public class TempTest
    {
        [Fact]
        public void StructTest()
        {
            Guid id = Guid.NewGuid();
            StructValue s = StructValue.Create(id, "Name1");
            StructValue s1 = new StructValue().PopulateWith(s);
            Assert.Equal(Guid.Empty, s1.Id);
            Assert.Equal(null, s1.Name);
            var nameProperty = s1.GetType().GetProperty("Name");
            nameProperty.SetValue(s1, "Name1");
            Assert.Equal(null, s1.Name);
            s1.Name = "Name1";
            Assert.Equal("Name1", s1.Name);
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
