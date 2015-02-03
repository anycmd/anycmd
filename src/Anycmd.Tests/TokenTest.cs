
namespace Anycmd.Tests
{
    using DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Util;

    [TestClass]
    public class TokenTest
    {
        [TestMethod]
        public void TokenIsValid()
        {
            var ticks = SystemTime.UtcNow().Ticks;
            const string secKey = "DF25BCB5-35E3-41E4-980F-64D916D806FF";
            const string appId = "87E9DAAB-2EA4-4A99-92BA-6C9DDB0F868C";
            TokenObject token = TokenObject.Create(
                TokenObject.Token(appId, ticks, secKey),
                appId,
                ticks);

            Assert.IsTrue(token.IsValid(secKey));
        }

        [TestMethod]
        public void SignatureIsValid()
        {
            var ticks = SystemTime.UtcNow().Ticks;
            const string secKey = "123456";
            const string orignalString = "appID=100&random=778899";
            Signature signature = Signature.Create(orignalString, Signature.Sign(orignalString, secKey));
            Assert.IsTrue(signature.IsValid(secKey));
        }
    }
}
