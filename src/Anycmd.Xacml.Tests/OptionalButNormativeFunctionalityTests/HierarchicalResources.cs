using Anycmd.Xacml.Context;
using Anycmd.Xacml.Policy;
using Anycmd.Xacml.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;


namespace Anycmd.Xacml.Tests.OptionalButNormativeFunctionalityTests
{
    [TestClass]
    public class HierarchicalResources
    {
        public HierarchicalResources()
        {
        }

        //IIICSpecial.txt
        [TestMethod]
        public void IIIC001()
        {
            string[] files = new string[] { "2.IIIC001Policy.xml", "2.IIIC001Request.xml", "2.IIIC001Response.xml" };
            Assert.AreEqual(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
            FileInfo requestFile = new FileInfo(Consts.Path + files[1]);
            FileInfo ResponseElementFile = new FileInfo(Consts.Path + files[2]);
            using (FileStream fs = new FileStream(policyFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs1 = new FileStream(requestFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs2 = new FileStream(ResponseElementFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Policy
                PolicyDocument policyDocument = (PolicyDocument)PolicyLoader.LoadPolicyDocument(fs, XacmlVersion.Version20, DocumentAccess.ReadOnly);
                // Load Request
                ContextDocumentReadWrite requestDocument = ContextLoader.LoadContextDocument(fs1, XacmlVersion.Version20);
                // Load ResponseElement
                ContextDocumentReadWrite ResponseElementDocument = ContextLoader.LoadContextDocument(fs2, XacmlVersion.Version20);
                EvaluationEngine engine = new EvaluationEngine();

                ResponseElement res = engine.Evaluate(policyDocument, (ContextDocument)requestDocument);
                Assert.AreEqual(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.AreEqual(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                for (int i = 0; i < ResponseElementDocument.Response.Results.Count; i++)
                {
                    Assert.AreEqual(((ResultElement)res.Results[i]).Decision, ((ResultElement)ResponseElementDocument.Response.Results[i]).Decision);
                }
                Assert.IsTrue(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.IsTrue(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }

        [TestMethod]
        public void IIIC002()
        {
            string[] files = new string[] { "2.IIIC002Policy.xml", "2.IIIC002Request.xml", "2.IIIC002Response.xml" };
            Assert.AreEqual(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
            FileInfo requestFile = new FileInfo(Consts.Path + files[1]);
            FileInfo ResponseElementFile = new FileInfo(Consts.Path + files[2]);
            using (FileStream fs = new FileStream(policyFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs1 = new FileStream(requestFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs2 = new FileStream(ResponseElementFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Policy
                PolicyDocument policyDocument = (PolicyDocument)PolicyLoader.LoadPolicyDocument(fs, XacmlVersion.Version20, DocumentAccess.ReadOnly);
                // Load Request
                ContextDocumentReadWrite requestDocument = ContextLoader.LoadContextDocument(fs1, XacmlVersion.Version20);
                // Load ResponseElement
                ContextDocumentReadWrite ResponseElementDocument = ContextLoader.LoadContextDocument(fs2, XacmlVersion.Version20);
                EvaluationEngine engine = new EvaluationEngine();

                ResponseElement res = engine.Evaluate(policyDocument, (ContextDocument)requestDocument);
                Assert.AreEqual(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.AreEqual(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                for (int i = 0; i < ResponseElementDocument.Response.Results.Count; i++)
                {
                    Assert.AreEqual(((ResultElement)res.Results[i]).Decision, ((ResultElement)ResponseElementDocument.Response.Results[i]).Decision);
                }
                Assert.IsTrue(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.IsTrue(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));
            }
        }

        [TestMethod]
        public void IIIC003()
        {
            string[] files = new string[] { "2.IIIC003Policy.xml", "2.IIIC003Request.xml", "2.IIIC003Response.xml" };
            Assert.AreEqual(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
            FileInfo requestFile = new FileInfo(Consts.Path + files[1]);
            FileInfo ResponseElementFile = new FileInfo(Consts.Path + files[2]);
            using (FileStream fs = new FileStream(policyFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs1 = new FileStream(requestFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs2 = new FileStream(ResponseElementFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Policy
                PolicyDocument policyDocument = (PolicyDocument)PolicyLoader.LoadPolicyDocument(fs, XacmlVersion.Version20, DocumentAccess.ReadOnly);
                // Load Request
                ContextDocumentReadWrite requestDocument = ContextLoader.LoadContextDocument(fs1, XacmlVersion.Version20);
                // Load ResponseElement
                ContextDocumentReadWrite ResponseElementDocument = ContextLoader.LoadContextDocument(fs2, XacmlVersion.Version20);
                EvaluationEngine engine = new EvaluationEngine();

                ResponseElement res = engine.Evaluate(policyDocument, (ContextDocument)requestDocument);
                Assert.AreEqual(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.AreEqual(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                for (int i = 0; i < ResponseElementDocument.Response.Results.Count; i++)
                {
                    Assert.AreEqual(((ResultElement)res.Results[i]).Decision, ((ResultElement)ResponseElementDocument.Response.Results[i]).Decision);
                }
                Assert.IsTrue(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.IsTrue(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }
    }
}
