namespace Anycmd.Xacml.Tests.MandatoryToImplementFunctionalityTests
{
    using Context;
    using Policy;
    using Runtime;
    using System;
    using System.IO;
    using Xunit;

    public class CombiningAlgorithms
    {
        public CombiningAlgorithms()
        {
        }

        [Fact]
        public void IID001()
        {
            string[] files = new string[] { "2.IID001Policy.xml", "2.IID001Request.xml", "2.IID001Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID002()
        {
            string[] files = new string[] { "2.IID002Policy.xml", "2.IID002Request.xml", "2.IID002Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID003()
        {
            string[] files = new string[] { "2.IID003Policy.xml", "2.IID003Request.xml", "2.IID003Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID004()
        {
            string[] files = new string[] { "2.IID004Policy.xml", "2.IID004Request.xml", "2.IID004Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID005()
        {
            string[] files = new string[] { "2.IID005Policy.xml", "2.IID005Request.xml", "2.IID005Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID006()
        {
            string[] files = new string[] { "2.IID006Policy.xml", "2.IID006Request.xml", "2.IID006Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID007()
        {
            string[] files = new string[] { "2.IID007Policy.xml", "2.IID007Request.xml", "2.IID007Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID008()
        {
            string[] files = new string[] { "2.IID008Policy.xml", "2.IID008Request.xml", "2.IID008Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID009()
        {
            string[] files = new string[] { "2.IID009Policy.xml", "2.IID009Request.xml", "2.IID009Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID010()
        {
            string[] files = new string[] { "2.IID010Policy.xml", "2.IID010Request.xml", "2.IID010Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID011()
        {
            string[] files = new string[] { "2.IID011Policy.xml", "2.IID011Request.xml", "2.IID011Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID012()
        {
            string[] files = new string[] { "2.IID012Policy.xml", "2.IID012Request.xml", "2.IID012Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID013()
        {
            string[] files = new string[] { "2.IID013Policy.xml", "2.IID013Request.xml", "2.IID013Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID014()
        {
            string[] files = new string[] { "2.IID014Policy.xml", "2.IID014Request.xml", "2.IID014Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID015()
        {
            string[] files = new string[] { "2.IID015Policy.xml", "2.IID015Request.xml", "2.IID015Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID016()
        {
            string[] files = new string[] { "2.IID016Policy.xml", "2.IID016Request.xml", "2.IID016Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID017()
        {
            string[] files = new string[] { "2.IID017Policy.xml", "2.IID017Request.xml", "2.IID017Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID018()
        {
            string[] files = new string[] { "2.IID018Policy.xml", "2.IID018Request.xml", "2.IID018Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID019()
        {
            string[] files = new string[] { "2.IID019Policy.xml", "2.IID019Request.xml", "2.IID019Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID020()
        {
            string[] files = new string[] { "2.IID020Policy.xml", "2.IID020Request.xml", "2.IID020Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID021()
        {
            string[] files = new string[] { "2.IID021Policy.xml", "2.IID021Request.xml", "2.IID021Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID022()
        {
            string[] files = new string[] { "2.IID022Policy.xml", "2.IID022Request.xml", "2.IID022Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID023()
        {
            string[] files = new string[] { "2.IID023Policy.xml", "2.IID023Request.xml", "2.IID023Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID024()
        {
            string[] files = new string[] { "2.IID024Policy.xml", "2.IID024Request.xml", "2.IID024Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID025()
        {
            string[] files = new string[] { "2.IID025Policy.xml", "2.IID025Request.xml", "2.IID025Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID026()
        {
            string[] files = new string[] { "2.IID026Policy.xml", "2.IID026Request.xml", "2.IID026Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID027()
        {
            string[] files = new string[] { "2.IID027Policy.xml", "2.IID027Request.xml", "2.IID027Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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
        public void IID028()
        {
            string[] files = new string[] { "2.IID028Policy.xml", "2.IID028Request.xml", "2.IID028Response.xml" };
            Assert.Equal(files.Length, 3); FileInfo policyFile = new FileInfo(Consts.Path + files[0]);
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

        //IID029Special.txt
        [Fact(Skip = "Configuration must be changed in order to run this test")]
        public void IID029()
        {
            string[] files = new string[] { "2.IID029Request.xml", "2.IID029Response.xml" };
            FileInfo requestFile = new FileInfo(Consts.Path + files[0]);
            FileInfo ResponseElementFile = new FileInfo(Consts.Path + files[1]);
            using (FileStream fs1 = new FileStream(requestFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs2 = new FileStream(ResponseElementFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Request
                ContextDocumentReadWrite requestDocument = ContextLoader.LoadContextDocument(fs1, XacmlVersion.Version20);
                // Load ResponseElement
                ContextDocumentReadWrite ResponseElementDocument = ContextLoader.LoadContextDocument(fs2, XacmlVersion.Version20);
                EvaluationEngine engine = new EvaluationEngine();

                ResponseElement res = engine.Evaluate((ContextDocument)requestDocument);
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }

        //IID030Special.txt
        [Fact(Skip = "Configuration must be changed in order to run this test")]
        public void IID030()
        {
            string[] files = new string[] { "2.IID030Request.xml", "2.IID030Response.xml" };
            FileInfo requestFile = new FileInfo(Consts.Path + files[0]);
            FileInfo ResponseElementFile = new FileInfo(Consts.Path + files[1]);
            using (FileStream fs1 = new FileStream(requestFile.FullName, FileMode.Open, FileAccess.Read))
            using (FileStream fs2 = new FileStream(ResponseElementFile.FullName, FileMode.Open, FileAccess.Read))
            {
                // Load Request
                ContextDocumentReadWrite requestDocument = ContextLoader.LoadContextDocument(fs1, XacmlVersion.Version20);
                // Load ResponseElement
                ContextDocumentReadWrite ResponseElementDocument = ContextLoader.LoadContextDocument(fs2, XacmlVersion.Version20);
                EvaluationEngine engine = new EvaluationEngine();

                ResponseElement res = engine.Evaluate((ContextDocument)requestDocument);
                Assert.Equal(((ResultElement)res.Results[0]).Obligations.Count, ((ResultElement)ResponseElementDocument.Response.Results[0]).Obligations.Count);
                Assert.Equal(ResponseElementDocument.Response.Results.Count, res.Results.Count);
                Assert.True(((ResultElement)res.Results[0]).Decision.ToString() == ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), string.Format("Decission incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Decision.ToString(), ((ResultElement)res.Results[0]).Decision.ToString()));
                Assert.True(((ResultElement)res.Results[0]).Status.StatusCode.Value == ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, String.Format("Status incorrect Expected:{0} Returned:{1}", ((ResultElement)ResponseElementDocument.Response.Results[0]).Status.StatusCode.Value, ((ResultElement)res.Results[0]).Status.StatusCode.Value));

            }
        }
    }
}
