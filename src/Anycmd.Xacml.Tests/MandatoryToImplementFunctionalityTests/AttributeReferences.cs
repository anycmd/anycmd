
namespace Anycmd.Xacml.Tests.MandatoryToImplementFunctionalityTests
{
    using Context;
    using Policy;
    using Runtime;
    using System;
    using System.IO;
    using Xunit;

    public class AttributeReferences
    {
        public AttributeReferences()
        {
        }

        [Fact]
        public void IIA001()
        {
            string[] files = new string[] { "2.IIA001Policy.xml", "2.IIA001Request.xml", "2.IIA001Response.xml" };
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
        public void IIA002()
        {
            string[] files = new string[] { "2.IIA002Policy.xml", "2.IIA002Request.xml", "2.IIA002Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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


        //IIA002Special.txt
        [Fact]
        public void IIA003()
        {
            string[] files = new string[] { "2.IIA003Policy.xml", "2.IIA003Request.xml", "2.IIA003Response.xml" };
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
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }


        [Fact]
        public void IIA004()
        {
            string[] files = new string[] { "2.IIA004Policy.xml", "2.IIA004Request.xml", "2.IIA004Response.xml" };
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
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }


        //IIA004Special.txt
        [Fact]
        public void IIA005()
        {
            string[] files = new string[] { "2.IIA005Policy.xml", "2.IIA005Request.xml", "2.IIA005Response.xml" };
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
        public void IIA006()
        {
            string[] files = new string[] { "2.IIA006Policy.xml", "2.IIA006Request.xml", "2.IIA006Response.xml" };
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
        public void IIA007()
        {
            string[] files = new string[] { "2.IIA007Policy.xml", "2.IIA007Request.xml", "2.IIA007Response.xml" };
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
        public void IIA008()
        {
            string[] files = new string[] { "2.IIA008Policy.xml", "2.IIA008Request.xml", "2.IIA008Response.xml" };
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
        public void IIA009()
        {
            string[] files = new string[] { "2.IIA009Policy.xml", "2.IIA009Request.xml", "2.IIA009Response.xml" };
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
        public void IIA010()
        {
            string[] files = new string[] { "2.IIA010Policy.xml", "2.IIA010Request.xml", "2.IIA010Response.xml" };
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
        public void IIA011()
        {
            string[] files = new string[] { "2.IIA011Policy.xml", "2.IIA011Request.xml", "2.IIA011Response.xml" };
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
        public void IIA012()
        {
            string[] files = new string[] { "2.IIA012Policy.xml", "2.IIA012Request.xml", "2.IIA012Response.xml" };
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
        public void IIA013()
        {
            string[] files = new string[] { "2.IIA013Policy.xml", "2.IIA013Request.xml", "2.IIA013Response.xml" };
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
        public void IIA014()
        {
            string[] files = new string[] { "2.IIA014Policy.xml", "2.IIA014Request.xml", "2.IIA014Response.xml" };
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
        public void IIA015()
        {
            string[] files = new string[] { "2.IIA015Policy.xml", "2.IIA015Request.xml", "2.IIA015Response.xml" };
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
        public void IIA016()
        {
            string[] files = new string[] { "2.IIA016Policy.xml", "2.IIA016Request.xml", "2.IIA016Response.xml" };
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
        public void IIA017()
        {
            string[] files = new string[] { "2.IIA017Policy.xml", "2.IIA017Request.xml", "2.IIA017Response.xml" };
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
                EvaluationEngine engine = new EvaluationEngine(true);

                ResponseElement res = engine.Evaluate(policyDocument, (ContextDocument)requestDocument);
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)responseDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(responseDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)responseDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }


        [Fact]
        public void IIA018()
        {
            string[] files = new string[] { "2.IIA018Policy.xml", "2.IIA018Request.xml", "2.IIA018Response.xml" };
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
        public void IIA019()
        {
            string[] files = new string[] { "2.IIA019Policy.xml", "2.IIA019Request.xml", "2.IIA019Response.xml" };
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
        public void IIA020()
        {
            string[] files = new string[] { "2.IIA020Policy.xml", "2.IIA020Request.xml", "2.IIA020Response.xml" };
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
        public void IIA021()
        {
            string[] files = new string[] { "2.IIA021Policy.xml", "2.IIA021Request.xml", "2.IIA021Response.xml" };
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
        public void IIA022()
        {
            string[] files = new string[] { "2.IIA022Policy.xml", "2.IIA022Request.xml", "2.IIA022Response.xml" };
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
        public void IIA023()
        {
            string[] files = new string[] { "2.IIA023Policy.xml", "2.IIA023Request.xml", "2.IIA023Response.xml" };
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
        public void IIA024()
        {
            string[] files = new string[] { "2.IIA024Policy.xml", "2.IIA024Request.xml", "2.IIA024Response.xml" };
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
