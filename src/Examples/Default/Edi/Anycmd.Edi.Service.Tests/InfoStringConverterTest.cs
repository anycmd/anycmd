
namespace Anycmd.Edi.Service.Tests
{
    using Engine.Info;
    using InfoStringConverters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    [TestClass]
    public class InfoStringConverterTest
    {
        [TestMethod]
        public void SupportGuidString()
        {
            string guidString = Guid.NewGuid().ToString();
            IInfoStringConverter converter = new JsonInfoStringConverter();
            var infoItems = converter.ToDataItems(guidString);

            Assert.IsTrue(string.CompareOrdinal(guidString, infoItems.First().Value) == 0);

            guidString = Guid.NewGuid().ToString();
            converter = new XmlInfoStringConverter();
            infoItems = converter.ToDataItems(guidString);

            Assert.IsTrue(string.CompareOrdinal(guidString, infoItems.First().Value) == 0);
        }
    }
}
