
namespace Anycmd.Edi.Service.Tests
{
    using Engine.Host.Info;
    using InfoStringConverters;
    using System;
    using System.Linq;
    using Xunit;

    public class InfoStringConverterTest
    {
        [Fact]
        public void SupportGuidString()
        {
            string guidString = Guid.NewGuid().ToString();
            IInfoStringConverter converter = new JsonInfoStringConverter();
            var infoItems = converter.ToDataItems(guidString);

            Assert.True(string.CompareOrdinal(guidString, infoItems.First().Value) == 0);

            guidString = Guid.NewGuid().ToString();
            converter = new XmlInfoStringConverter();
            infoItems = converter.ToDataItems(guidString);

            Assert.True(string.CompareOrdinal(guidString, infoItems.First().Value) == 0);
        }
    }
}
