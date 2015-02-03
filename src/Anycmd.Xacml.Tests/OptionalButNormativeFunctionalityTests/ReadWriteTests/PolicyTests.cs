
namespace Anycmd.Xacml.Tests.OptionalButNormativeFunctionalityTests.ReadWriteTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Policy;
    using System.IO;
    using System.Xml;

    [TestClass]
    public class PolicyTests
    {
        public PolicyTests()
        {
        }

        [TestMethod]
        public void IIA001()
        {
            string[] files = new string[] { "2.IIA001Policy.xml" };
            string tempFile = Path.GetTempFileName();
            XmlTextWriter tw = new XmlTextWriter(tempFile, System.Text.Encoding.ASCII);
            tw.Namespaces = true;
            tw.Formatting = Formatting.Indented;

            FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
            using (FileStream fs = new FileStream(policyFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Policy
                PolicyDocumentReadWrite policyDocument = PolicyLoader.LoadPolicyDocument(fs, XacmlVersion.Version20, DocumentAccess.ReadWrite);

                if (policyDocument.Policy == null)
                {
                    policyDocument.PolicySet.Description = "UnitTest!!";
                }
                else
                {
                    policyDocument.Policy.Description = "UnitTest!!";
                }
                policyDocument.WriteDocument(tw);
                tw.Close();

                using (FileStream fs1 = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                {
                    PolicyDocument newPd = (PolicyDocument)PolicyLoader.LoadPolicyDocument(fs1, XacmlVersion.Version20, DocumentAccess.ReadOnly);

                    if (newPd.Policy == null)
                    {
                        Assert.AreEqual(newPd.PolicySet.Description, policyDocument.PolicySet.Description);
                    }
                    else
                    {
                        Assert.AreEqual(newPd.Policy.Description, policyDocument.Policy.Description);
                    }
                }
            }
        }
    }
}
