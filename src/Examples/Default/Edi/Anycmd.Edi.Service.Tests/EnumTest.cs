
namespace Anycmd.Edi.Service.Tests
{
    using Engine.Host.Edi;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnumTest
    {
        [TestMethod]
        public void EnumToStringTest()
        {
            Assert.AreEqual(Status.InvalidClientId.ToString(), "InvalidClientId");
            Assert.AreEqual(Status.InvalidClientId.ToName(), "InvalidClientId");
            Status stateCode;
            Assert.IsFalse(0.TryParse(out stateCode));
            var sb = new System.Text.StringBuilder();
        }
    }
}
