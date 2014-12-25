using Anycmd.Xacml.Context;
using Anycmd.Xacml.Policy;
using Anycmd.Xacml.Runtime;
using System;
using System.IO;
using Xunit;

namespace Anycmd.Xacml.Tests.OptionalButNormativeFunctionalityTests
{
    public class AttributeSelectors
    {
        public AttributeSelectors()
        {
        }

        //IIIFSpecial.txt
        [Fact]
        public void IIIF001()
        {
            string[] files = new string[] { "2.IIIF001Policy.xml", "2.IIIF001Request.xml", "2.IIIF001Response.xml" };
            Assert.Equal(files.Length, 3); 
            FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
            FileInfo requestFile = new FileInfo(Consts.Path + files[1]);
            FileInfo responseFile = new FileInfo(Consts.Path + files[2]);
            using (FileStream fs = new FileStream(policyFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs1 = new FileStream(requestFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs2 = new FileStream(responseFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Policy
                PolicyDocument policyDocument = (PolicyDocument)PolicyLoader.LoadPolicyDocument(fs, XacmlVersion.Version20, DocumentAccess.ReadOnly);
                // Load Request
                ContextDocumentReadWrite requestDocument = ContextLoader.LoadContextDocument(fs1, XacmlVersion.Version20);
                // Load ResponseElement
                ContextDocumentReadWrite responseDocument = ContextLoader.LoadContextDocument(fs2, XacmlVersion.Version20);
                EvaluationEngine engine = new EvaluationEngine();

                ResponseElement res = engine.Evaluate(policyDocument, (ContextDocument)requestDocument);
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)responseDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(responseDocument.Response.Results.Count, res.Results.Count);
                var decisionReturned = ((ResultElement)res.Results[0]).Decision.ToString();
                var decisionExpected = ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString();
                Assert.Equal(decisionExpected, decisionReturned);
                Assert.Equal(((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value);
            }
        }


        [Fact]
        public void IIIF002()
        {
            string[] files = new string[] { "2.IIIF002Policy.xml", "2.IIIF002Request.xml", "2.IIIF002Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
            FileInfo requestFile = new FileInfo(Consts.Path + files[1]);
            FileInfo responseFile = new FileInfo(Consts.Path + files[2]);
            using (FileStream fs = new FileStream(policyFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs1 = new FileStream(requestFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs2 = new FileStream(responseFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Policy
                PolicyDocument policyDocument = (PolicyDocument)PolicyLoader.LoadPolicyDocument(fs, XacmlVersion.Version20, DocumentAccess.ReadOnly);
                // Load Request
                ContextDocumentReadWrite requestDocument = ContextLoader.LoadContextDocument(fs1, XacmlVersion.Version20);
                // Load ResponseElement
                ContextDocumentReadWrite responseDocument = ContextLoader.LoadContextDocument(fs2, XacmlVersion.Version20);
                EvaluationEngine engine = new EvaluationEngine();

                ResponseElement res = engine.Evaluate(policyDocument, (ContextDocument)requestDocument);
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)responseDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(responseDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }


        [Fact]
        public void IIIF003()
        {
            string[] files = new string[] { "2.IIIF003Policy.xml", "2.IIIF003Request.xml", "2.IIIF003Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
            FileInfo requestFile = new FileInfo(Consts.Path + files[1]);
            FileInfo responseFile = new FileInfo(Consts.Path + files[2]);
            using (FileStream fs = new FileStream(policyFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs1 = new FileStream(requestFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs2 = new FileStream(responseFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Policy
                PolicyDocument policyDocument = (PolicyDocument)PolicyLoader.LoadPolicyDocument(fs, XacmlVersion.Version20, DocumentAccess.ReadOnly);
                // Load Request
                ContextDocumentReadWrite requestDocument = ContextLoader.LoadContextDocument(fs1, XacmlVersion.Version20);
                // Load ResponseElement
                ContextDocumentReadWrite responseDocument = ContextLoader.LoadContextDocument(fs2, XacmlVersion.Version20);
                EvaluationEngine engine = new EvaluationEngine();

                ResponseElement res = engine.Evaluate(policyDocument, (ContextDocument)requestDocument);
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)responseDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(responseDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }


        [Fact]
        public void IIIF004()
        {
            string[] files = new string[] { "2.IIIF004Policy.xml", "2.IIIF004Request.xml", "2.IIIF004Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
            FileInfo requestFile = new FileInfo(Consts.Path + files[1]);
            FileInfo responseFile = new FileInfo(Consts.Path + files[2]);
            using (FileStream fs = new FileStream(policyFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs1 = new FileStream(requestFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs2 = new FileStream(responseFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Policy
                PolicyDocument policyDocument = (PolicyDocument)PolicyLoader.LoadPolicyDocument(fs, XacmlVersion.Version20, DocumentAccess.ReadOnly);
                // Load Request
                ContextDocumentReadWrite requestDocument = ContextLoader.LoadContextDocument(fs1, XacmlVersion.Version20);
                // Load ResponseElement
                ContextDocumentReadWrite responseDocument = ContextLoader.LoadContextDocument(fs2, XacmlVersion.Version20);
                EvaluationEngine engine = new EvaluationEngine();

                ResponseElement res = engine.Evaluate(policyDocument, (ContextDocument)requestDocument);
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)responseDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(responseDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }


        [Fact]
        public void IIIF005()
        {
            string[] files = new string[] { "2.IIIF005Policy.xml", "2.IIIF005Request.xml", "2.IIIF005Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
            FileInfo requestFile = new FileInfo(Consts.Path + files[1]);
            FileInfo responseFile = new FileInfo(Consts.Path + files[2]);
            using (FileStream fs = new FileStream(policyFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs1 = new FileStream(requestFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs2 = new FileStream(responseFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Policy
                PolicyDocument policyDocument = (PolicyDocument)PolicyLoader.LoadPolicyDocument(fs, XacmlVersion.Version20, DocumentAccess.ReadOnly);
                // Load Request
                ContextDocumentReadWrite requestDocument = ContextLoader.LoadContextDocument(fs1, XacmlVersion.Version20);
                // Load ResponseElement
                ContextDocumentReadWrite responseDocument = ContextLoader.LoadContextDocument(fs2, XacmlVersion.Version20);
                EvaluationEngine engine = new EvaluationEngine();

                ResponseElement res = engine.Evaluate(policyDocument, (ContextDocument)requestDocument);
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)responseDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(responseDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }


        [Fact]
        public void IIIF006()
        {
            string[] files = new string[] { "2.IIIF006Policy.xml", "2.IIIF006Request.xml", "2.IIIF006Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
            FileInfo requestFile = new FileInfo(Consts.Path + files[1]);
            FileInfo responseFile = new FileInfo(Consts.Path + files[2]);
            using (FileStream fs = new FileStream(policyFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs1 = new FileStream(requestFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs2 = new FileStream(responseFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Policy
                PolicyDocument policyDocument = (PolicyDocument)PolicyLoader.LoadPolicyDocument(fs, XacmlVersion.Version20, DocumentAccess.ReadOnly);
                // Load Request
                ContextDocumentReadWrite requestDocument = ContextLoader.LoadContextDocument(fs1, XacmlVersion.Version20);
                // Load ResponseElement
                ContextDocumentReadWrite responseDocument = ContextLoader.LoadContextDocument(fs2, XacmlVersion.Version20);
                EvaluationEngine engine = new EvaluationEngine();

                ResponseElement res = engine.Evaluate(policyDocument, (ContextDocument)requestDocument);
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)responseDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(responseDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }

        [Fact]
        public void IIIF007()
        {
            string[] files = new string[] { "2.IIIF007Policy.xml", "2.IIIF007Request.xml", "2.IIIF007Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
            FileInfo requestFile = new FileInfo(Consts.Path + files[1]);
            FileInfo responseFile = new FileInfo(Consts.Path + files[2]);
            using (FileStream fs = new FileStream(policyFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs1 = new FileStream(requestFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs2 = new FileStream(responseFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Policy
                PolicyDocument policyDocument = (PolicyDocument)PolicyLoader.LoadPolicyDocument(fs, XacmlVersion.Version20, DocumentAccess.ReadOnly);
                // Load Request
                ContextDocumentReadWrite requestDocument = ContextLoader.LoadContextDocument(fs1, XacmlVersion.Version20);
                // Load ResponseElement
                ContextDocumentReadWrite responseDocument = ContextLoader.LoadContextDocument(fs2, XacmlVersion.Version20);
                EvaluationEngine engine = new EvaluationEngine();

                ResponseElement res = engine.Evaluate(policyDocument, (ContextDocument)requestDocument);
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)responseDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(responseDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }

    }
}
