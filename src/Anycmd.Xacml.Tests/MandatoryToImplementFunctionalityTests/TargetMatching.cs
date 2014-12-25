namespace Anycmd.Xacml.Tests.MandatoryToImplementFunctionalityTests
{
    using Context;
    using Policy;
    using Runtime;
    using System;
    using System.IO;
    using Xunit;

    public class TargetMatching
    {
        public TargetMatching()
        {
        }

        [Fact]
        public void IIB001()
        {
            string[] files = new string[] { "2.IIB001Policy.xml", "2.IIB001Request.xml", "2.IIB001Response.xml" };
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
        public void IIB002()
        {
            string[] files = new string[] { "2.IIB002Policy.xml", "2.IIB002Request.xml", "2.IIB002Response.xml" };
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
        public void IIB003()
        {
            string[] files = new string[] { "2.IIB003Policy.xml", "2.IIB003Request.xml", "2.IIB003Response.xml" };
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
        public void IIB004()
        {
            string[] files = new string[] { "2.IIB004Policy.xml", "2.IIB004Request.xml", "2.IIB004Response.xml" };
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
        public void IIB005()
        {
            string[] files = new string[] { "2.IIB005Policy.xml", "2.IIB005Request.xml", "2.IIB005Response.xml" };
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
        public void IIB006()
        {
            string[] files = new string[] { "2.IIB006Policy.xml", "2.IIB006Request.xml", "2.IIB006Response.xml" };
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
        public void IIB007()
        {
            string[] files = new string[] { "2.IIB007Policy.xml", "2.IIB007Request.xml", "2.IIB007Response.xml" };
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
        public void IIB008()
        {
            string[] files = new string[] { "2.IIB008Policy.xml", "2.IIB008Request.xml", "2.IIB008Response.xml" };
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
        public void IIB009()
        {
            string[] files = new string[] { "2.IIB009Policy.xml", "2.IIB009Request.xml", "2.IIB009Response.xml" };
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
        public void IIB010()
        {
            string[] files = new string[] { "2.IIB010Policy.xml", "2.IIB010Request.xml", "2.IIB010Response.xml" };
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
        public void IIB011()
        {
            string[] files = new string[] { "2.IIB011Policy.xml", "2.IIB011Request.xml", "2.IIB011Response.xml" };
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
        public void IIB012()
        {
            string[] files = new string[] { "2.IIB012Policy.xml", "2.IIB012Request.xml", "2.IIB012Response.xml" };
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
        public void IIB013()
        {
            string[] files = new string[] { "2.IIB013Policy.xml", "2.IIB013Request.xml", "2.IIB013Response.xml" };
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
        public void IIB014()
        {
            string[] files = new string[] { "2.IIB014Policy.xml", "2.IIB014Request.xml", "2.IIB014Response.xml" };
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
        public void IIB015()
        {
            string[] files = new string[] { "2.IIB015Policy.xml", "2.IIB015Request.xml", "2.IIB015Response.xml" };
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
        public void IIB016()
        {
            string[] files = new string[] { "2.IIB016Policy.xml", "2.IIB016Request.xml", "2.IIB016Response.xml" };
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
        public void IIB017()
        {
            string[] files = new string[] { "2.IIB017Policy.xml", "2.IIB017Request.xml", "2.IIB017Response.xml" };
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
        public void IIB018()
        {
            string[] files = new string[] { "2.IIB018Policy.xml", "2.IIB018Request.xml", "2.IIB018Response.xml" };
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
        public void IIB019()
        {
            string[] files = new string[] { "2.IIB019Policy.xml", "2.IIB019Request.xml", "2.IIB019Response.xml" };
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
        public void IIB020()
        {
            string[] files = new string[] { "2.IIB020Policy.xml", "2.IIB020Request.xml", "2.IIB020Response.xml" };
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
        public void IIB021()
        {
            string[] files = new string[] { "2.IIB021Policy.xml", "2.IIB021Request.xml", "2.IIB021Response.xml" };
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
        public void IIB022()
        {
            string[] files = new string[] { "2.IIB022Policy.xml", "2.IIB022Request.xml", "2.IIB022Response.xml" };
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
        public void IIB023()
        {
            string[] files = new string[] { "2.IIB023Policy.xml", "2.IIB023Request.xml", "2.IIB023Response.xml" };
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
        public void IIB024()
        {
            string[] files = new string[] { "2.IIB024Policy.xml", "2.IIB024Request.xml", "2.IIB024Response.xml" };
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
        public void IIB025()
        {
            string[] files = new string[] { "2.IIB025Policy.xml", "2.IIB025Request.xml", "2.IIB025Response.xml" };
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
        public void IIB026()
        {
            string[] files = new string[] { "2.IIB026Policy.xml", "2.IIB026Request.xml", "2.IIB026Response.xml" };
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
        public void IIB027()
        {
            string[] files = new string[] { "2.IIB027Policy.xml", "2.IIB027Request.xml", "2.IIB027Response.xml" };
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
        public void IIB028()
        {
            string[] files = new string[] { "2.IIB028Policy.xml", "2.IIB028Request.xml", "2.IIB028Response.xml" };
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
        public void IIB029()
        {
            string[] files = new string[] { "2.IIB029Policy.xml", "2.IIB029Request.xml", "2.IIB029Response.xml" };
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
        public void IIB030()
        {
            string[] files = new string[] { "2.IIB030Policy.xml", "2.IIB030Request.xml", "2.IIB030Response.xml" };
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
        public void IIB031()
        {
            string[] files = new string[] { "2.IIB031Policy.xml", "2.IIB031Request.xml", "2.IIB031Response.xml" };
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
        public void IIB032()
        {
            string[] files = new string[] { "2.IIB032Policy.xml", "2.IIB032Request.xml", "2.IIB032Response.xml" };
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
        public void IIB033()
        {
            string[] files = new string[] { "2.IIB033Policy.xml", "2.IIB033Request.xml", "2.IIB033Response.xml" };
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
        public void IIB034()
        {
            string[] files = new string[] { "2.IIB034Policy.xml", "2.IIB034Request.xml", "2.IIB034Response.xml" };
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
        public void IIB035()
        {
            string[] files = new string[] { "2.IIB035Policy.xml", "2.IIB035Request.xml", "2.IIB035Response.xml" };
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
        public void IIB036()
        {
            string[] files = new string[] { "2.IIB036Policy.xml", "2.IIB036Request.xml", "2.IIB036Response.xml" };
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
        public void IIB037()
        {
            string[] files = new string[] { "2.IIB037Policy.xml", "2.IIB037Request.xml", "2.IIB037Response.xml" };
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
        public void IIB038()
        {
            string[] files = new string[] { "2.IIB038Policy.xml", "2.IIB038Request.xml", "2.IIB038Response.xml" };
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
        public void IIB039()
        {
            string[] files = new string[] { "2.IIB039Policy.xml", "2.IIB039Request.xml", "2.IIB039Response.xml" };
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
        public void IIB040()
        {
            string[] files = new string[] { "2.IIB040Policy.xml", "2.IIB040Request.xml", "2.IIB040Response.xml" };
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
        public void IIB041()
        {
            string[] files = new string[] { "2.IIB041Policy.xml", "2.IIB041Request.xml", "2.IIB041Response.xml" };
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
        public void IIB042()
        {
            string[] files = new string[] { "2.IIB042Policy.xml", "2.IIB042Request.xml", "2.IIB042Response.xml" };
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
        public void IIB043()
        {
            string[] files = new string[] { "2.IIB043Policy.xml", "2.IIB043Request.xml", "2.IIB043Response.xml" };
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
        public void IIB044()
        {
            string[] files = new string[] { "2.IIB044Policy.xml", "2.IIB044Request.xml", "2.IIB044Response.xml" };
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
        public void IIB045()
        {
            string[] files = new string[] { "2.IIB045Policy.xml", "2.IIB045Request.xml", "2.IIB045Response.xml" };
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
        public void IIB046()
        {
            string[] files = new string[] { "2.IIB046Policy.xml", "2.IIB046Request.xml", "2.IIB046Response.xml" };
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
        public void IIB047()
        {
            string[] files = new string[] { "2.IIB047Policy.xml", "2.IIB047Request.xml", "2.IIB047Response.xml" };
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
        public void IIB048()
        {
            string[] files = new string[] { "2.IIB048Policy.xml", "2.IIB048Request.xml", "2.IIB048Response.xml" };
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
        public void IIB049()
        {
            string[] files = new string[] { "2.IIB049Policy.xml", "2.IIB049Request.xml", "2.IIB049Response.xml" };
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
        public void IIB050()
        {
            string[] files = new string[] { "2.IIB050Policy.xml", "2.IIB050Request.xml", "2.IIB050Response.xml" };
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
        public void IIB051()
        {
            string[] files = new string[] { "2.IIB051Policy.xml", "2.IIB051Request.xml", "2.IIB051Response.xml" };
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
        public void IIB052()
        {
            string[] files = new string[] { "2.IIB052Policy.xml", "2.IIB052Request.xml", "2.IIB052Response.xml" };
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
        public void IIB053()
        {
            string[] files = new string[] { "2.IIB053Policy.xml", "2.IIB053Request.xml", "2.IIB053Response.xml" };
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

    }
}
