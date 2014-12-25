
namespace Anycmd.Edi.Service.Tests
{
    using Engine.Host.Edi;
    using Xunit;

    public class EnumTest
    {
        [Fact]
        public void EnumToStringTest()
        {
            Assert.Equal(Status.InvalidClientId.ToString(), "InvalidClientId");
            Assert.Equal(Status.InvalidClientId.ToName(), "InvalidClientId");
            Status stateCode;
            Assert.False(0.TryParse(out stateCode));
            var sb = new System.Text.StringBuilder();
        }
    }
}
