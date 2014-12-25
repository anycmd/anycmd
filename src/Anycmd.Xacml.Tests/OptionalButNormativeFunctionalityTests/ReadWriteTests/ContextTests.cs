
namespace Anycmd.Xacml.Tests.OptionalButNormativeFunctionalityTests.ReadWriteTests
{
    using Context;
    using System.IO;
    using System.Xml;
    using Xunit;

    public class ContextTests
    {
        public ContextTests()
        {
        }

        [Fact]
        public void IIA001()
        {
            string[] files = new string[] { "2.IIA001Request.xml" };
            string tempFile = Path.GetTempFileName();
            XmlTextWriter tw = new XmlTextWriter(tempFile, System.Text.Encoding.ASCII);
            tw.Namespaces = true;
            tw.Formatting = Formatting.Indented;

            FileInfo requestFile = new FileInfo(Consts.Path + files[0]);
            using (FileStream fs = new FileStream(requestFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Request
                ContextDocumentReadWrite contextDocument = ContextLoader.LoadContextDocument(fs, XacmlVersion.Version20, DocumentAccess.ReadWrite);

                contextDocument.Request.Action.Attributes[0].AttributeId = "UnitTest!!!";

                contextDocument.WriteRequestDocument(tw);
                tw.Close();

                using (FileStream fs1 = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                {
                    ContextDocument newCon = (ContextDocument)ContextLoader.LoadContextDocument(fs1, XacmlVersion.Version20);

                    Assert.Equal(newCon.Request.Action.Attributes[0].AttributeId, contextDocument.Request.Action.Attributes[0].AttributeId);
                }
            }
        }
    }
}
