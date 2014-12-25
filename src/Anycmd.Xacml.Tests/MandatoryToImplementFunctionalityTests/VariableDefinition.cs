namespace Anycmd.Xacml.Tests.MandatoryToImplementFunctionalityTests
{
    using Context;
    using Policy;
    using Runtime;
    using System;
    using System.IO;
    using Xunit;

    public class VariableDefinition
    {
        public VariableDefinition()
        {
        }

        [Fact]
        public void IIF001()
        {
            string[] files = new string[] { "2.IIF001Policy.xml", "2.IIF001Request.xml", "2.IIF001Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }

        [Fact]
        public void IIF002()
        {
            string[] files = new string[] { "2.IIF002Policy.xml", "2.IIF002Request.xml", "2.IIF002Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }

        [Fact]
        public void IIF003()
        {
            string[] files = new string[] { "2.IIF003Policy.xml", "2.IIF003Request.xml", "2.IIF003Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }

        [Fact]
        public void IIF004()
        {
            string[] files = new string[] { "2.IIF004Policy.xml", "2.IIF004Request.xml", "2.IIF004Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }


        [Fact]
        public void IIF005()
        {
            string[] files = new string[] { "2.IIF005Policy.xml", "2.IIF005Request.xml", "2.IIF005Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }

        [Fact]
        public void IIF006()
        {
            string[] files = new string[] { "2.IIF006Policy.xml", "2.IIF006Request.xml", "2.IIF006Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }

        [Fact]
        public void IIF007()
        {
            string[] files = new string[] { "2.IIF007Policy.xml", "2.IIF007Request.xml", "2.IIF007Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }

        [Fact]
        public void IIF008()
        {
            string[] files = new string[] { "2.IIF008Policy.xml", "2.IIF008Request.xml", "2.IIF008Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }

        [Fact]
        public void IIF009()
        {
            string[] files = new string[] { "2.IIF009Policy.xml", "2.IIF009Request.xml", "2.IIF009Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));
            }
        }

        [Fact]
        public void IIF010()
        {
            string[] files = new string[] { "2.IIF010Policy.xml", "2.IIF010Request.xml", "2.IIF010Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));
            }
        }

        [Fact]
        public void IIF011()
        {
            string[] files = new string[] { "2.IIF011Policy.xml", "2.IIF011Request.xml", "2.IIF011Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));
            }
        }

        [Fact]
        public void IIF012()
        {
            string[] files = new string[] { "2.IIF012Policy.xml", "2.IIF012Request.xml", "2.IIF012Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));
            }
        }

        [Fact]
        public void IIF013()
        {
            string[] files = new string[] { "2.IIF013Policy.xml", "2.IIF013Request.xml", "2.IIF013Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));
            }
        }


        [Fact]
        public void IIF014()
        {
            string[] files = new string[] { "2.IIF014Policy.xml", "2.IIF014Request.xml", "2.IIF014Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));
            }
        }

        [Fact]
        public void IIF015()
        {
            string[] files = new string[] { "2.IIF015Policy.xml", "2.IIF015Request.xml", "2.IIF015Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));
            }
        }

        [Fact]
        public void IIF016()
        {
            string[] files = new string[] { "2.IIF016Policy.xml", "2.IIF016Request.xml", "2.IIF016Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));
            }
        }


        [Fact]
        public void IIF017()
        {
            string[] files = new string[] { "2.IIF017Policy.xml", "2.IIF017Request.xml", "2.IIF017Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));
            }
        }


        [Fact]
        public void IIF018()
        {
            string[] files = new string[] { "2.IIF018Policy.xml", "2.IIF018Request.xml", "2.IIF018Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));
            }
        }

        [Fact]
        public void IIF019()
        {
            string[] files = new string[] { "2.IIF019Policy.xml", "2.IIF019Request.xml", "2.IIF019Response.xml" };
            Assert.Equal(files.Length, 3); Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));
            }
        }
    }
}
