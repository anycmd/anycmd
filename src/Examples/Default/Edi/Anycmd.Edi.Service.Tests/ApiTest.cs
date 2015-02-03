
namespace Anycmd.Edi.Service.Tests
{
    using Application;
    using Client;
    using DataContracts;
    using Ef;
    using Engine.Hecp;
    using Engine.Host;
    using Engine.Host.Edi;
    using Engine.Host.Edi.Handlers;
    using Engine.Host.Impl;
    using Engine.Info;
    using Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ServiceModel.Operations;
    using ServiceStack;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Util;

    /// <summary>
    /// 在Web服务器上对外提供有一个AnyMessage接口，该接口采用两种方式提供：一种基于soap/webservice，一种基于原生的http协议。
    /// 这只是同一个接口的两种终结点，两者的内涵完全一样，两者没有任何不同。
    /// <remarks>
    /// 本类中的测试用例最终到底是调用的WebApi接口还是WebService接口是由被测试的服务提供节点配置的“命令转移方式”决定的。
    /// 下面测试的都是中心节点的服务，命令转移方式在Anycmd.dbo.Node表的TransferID列中配置。也可从Mis系统的“节点管理”界面配置。
    /// 注意：因暂不支持配置同步，所以改变配置需重启服务应用程序。
    /// </remarks>
    /// </summary>
    [TestClass]
    public class ApiTest
    {
        private static readonly DefaultAcDomain AcDomain = new DefaultAcDomain();

        static ApiTest()
        {
            EfContext.InitStorage(new SimpleEfContextStorage());
            AcDomain.AddService(typeof(ILoggingService), new Log4NetLoggingService(AcDomain));
            AcDomain.AddService(typeof(IAcSessionStorage), new SimpleAcSessionStorage());
            AcDomain.Init();
            AcDomain.RegisterRepository(new List<string>
            {
                "EdiEntities",
                "AcEntities",
                "InfraEntities",
                "IdentityEntities"
            }, typeof(AcDomain).Assembly);
            AcDomain.RegisterEdiCore();
        }

        #region IsAlive
        [TestMethod]
        public async Task IsAlive()
        {
            var client = new JsonServiceClient(AcDomain.NodeHost.Nodes.ThisNode.Node.AnycmdApiAddress);
            var isAlive = new IsAlive
            {
                Version = "v1"
            };
            var response = await client.GetAsync(isAlive);
            Assert.IsTrue(response.IsAlive);
            isAlive.Version = "version2";
            response = await client.GetAsync(isAlive);
            Assert.IsFalse(response.IsAlive);
            Assert.IsTrue(Status.InvalidApiVersion.ToName() == response.ReasonPhrase, response.Description);
        }
        #endregion

        #region Should_Get_InvalidVersion
        [TestMethod]
        public void Should_Get_InvalidVersion()
        {
            var infoValue = new KeyValueBuilder().Append("XM", "测试").Append("ZZJGM", "11011421005").ToArray();
            var request = new Message
            {
                Version = "InvalidVersion",
                MessageType = "action",
                IsDumb = true,
                Verb = Verb.Update.Code,
                Ontology = "JS",
                MessageId = Guid.NewGuid().ToString(),
                Body = new BodyData(new KeyValueBuilder().Append("Id", "010C1D7A-9BA5-4AEA-9D4B-290476A79D12").ToArray(), infoValue),
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.InvalidApiVersion == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.InvalidApiVersion == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Should_Get_InvalidMessageType
        [TestMethod]
        public void Should_Get_InvalidMessageType()
        {
            var infoValue = new KeyValueBuilder().Append("XM", "测试").Append("ZZJGM", "11011421005").ToArray();
            var request = new Message
            {
                Version = "v1",
                IsDumb = true,
                Verb = Verb.Update.Code,
                Ontology = "JS",
                MessageType = "InvalidMessageType",
                MessageId = new string('A', 100),
                Body = new BodyData(new KeyValueBuilder().Append("Id", "010C1D7A-9BA5-4AEA-9D4B-290476A79D12").ToArray(), infoValue)
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.InvalidMessageType == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.InvalidMessageType == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Action_Create_Dumb
        [TestMethod]
        public void Action_Create_Dumb()
        {
            var xm = System.Guid.NewGuid().ToString();
            // 注意:基于serviceStack.Text的json反序列化貌似不认单引号只认双引号.
            var json = "{\"XM\":\"" + xm + "\",\"ZZJGM\":\"11011421004\"}";
            IInfoStringConverter converter;
            AcDomain.NodeHost.InfoStringConverters.TryGetInfoStringConverter("json", out converter);
            var infoValue = converter.ToDataItems(json);
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                Verb = Verb.Create.Code,
                MessageType = "action",
                IsDumb = true,
                Ontology = "JS",
                Body = new BodyData(infoValue.ToDto(), infoValue.ToDto())
                {
                    QueryList = new string[] { "Id" }
                },
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxSignature();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            request.Verb = "Get";
            request.JspxSignature();// 重新签名
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.NotExist == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region NotAuthorized
        [TestMethod]
        public void NotAuthorized()
        {
            var xm = NewXM();
            var keys = new string[]{
                "XM","ZZJGM"
            };
            var values = new string[]{
                xm,"11011421004"
            };
            var infoValue = new KeyValueBuilder(keys, values).ToArray();
            var client = new JsonServiceClient(AcDomain.NodeHost.Nodes.ThisNode.Node.AnycmdApiAddress);
            var ticks = DateTime.UtcNow.Ticks;
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                MessageType = "action",
                IsDumb = true,
                Verb = Verb.Create.Code,
                Ontology = "JS",
                Body = new BodyData(infoValue, infoValue),
                Credential = new CredentialData
                {
                    ClientType = ClientType.Node.ToName(),
                    CredentialType = CredentialType.Token.ToName(),
                    ClientId = "41e711c6-f215-4606-a0bf-9af11cce1d54",
                    Ticks = ticks,
                    Password = TokenObject.Token("41e711c6-f215-4606-a0bf-9af11cce1d54", ticks, "invalidSecretKey")
                },
                TimeStamp = DateTime.UtcNow.Ticks
            };
            var response = client.Get(request);
            Assert.IsTrue(Status.NotAuthorized.ToName() == response.Body.Event.ReasonPhrase, response.Body.Event.Description);
            request.Verb = "Get";
            request.JspxSignature();// 签名
            var result = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.NotExist == result.Body.Event.Status, result.Body.Event.Description);
        }
        #endregion

        #region Action_Create
        [TestMethod]
        public void Action_Create()
        {
            var xm = NewXM();
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM",xm},
                {"ZZJGM","11011421004"}
            }).ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                MessageType = "action",
                IsDumb = true,
                Verb = Verb.Create.Code,
                Ontology = "JS",
                Body = new BodyData(infoValue, infoValue)
                {
                    QueryList = new string[] { "Id" }
                },
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            //var response = request.RequestNode(acDomain.NodeHost.Nodes.CenterNode);
            var response = AnyMessage.Create(HecpRequest.Create(AcDomain, request), AcDomain.NodeHost.Nodes.ThisNode).Response();
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            request.JspxSignature();// 签名
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            request.JspxSignature();// 命令对象有更改则需重新签名
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            request.Verb = "Delete";
            request.JspxSignature();// 命令对象有更改则需重新签名
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Action_Delete
        [TestMethod]
        public void Action_Delete()
        {
            var xm = NewXM();
            var infoValue = new List<KeyValue> {
                new KeyValue("XM",xm),
                new KeyValue("ZZJGM", "11011421004")
            }.ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                MessageType = "action",
                IsDumb = true,
                Verb = "Create",
                Ontology = "JS",
                Body = new BodyData(infoValue, infoValue)
                {
                    QueryList = new string[] { "Id" }
                },
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            request.Verb = "delete";
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);

        }
        #endregion

        #region Command_Create
        [TestMethod]
        public void Command_Create()
        {
            var xm = System.Guid.NewGuid().ToString();
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM",xm},
                {"ZZJGM","11011421004"},
                {"DZXX", "23934360@qq.com"}
            }).ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                MessageType = "command",
                IsDumb = true,
                Verb = "Create",
                Ontology = "JS",
                Body = new BodyData(infoValue, infoValue),
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ReceiveOk == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ReceiveOk == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Command_MessageID_CanNotBeNullOrEmpty
        [TestMethod]
        public void Command_MessageID_CanNotBeNullOrEmpty()
        {
            var xm = System.Guid.NewGuid().ToString();
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM",xm},
                {"ZZJGM","11011421005"}
            }).ToArray();
            var request = new Message
            {
                Version = "v1",
                MessageType = "command",
                IsDumb = true,
                Verb = "Create",
                Ontology = "JS",
                MessageId = (DateTime.Now.Ticks % 2) == 0 ? null : string.Empty,
                Body = new BodyData(infoValue, infoValue),
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.InvalidArgument == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.InvalidArgument == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Event_MessageIDNotExist
        [TestMethod]
        public void Event_MessageIDNotExist()
        {
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM","test"},
                {"ZZJGM","11010424"}
            }).ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                MessageType = "event",
                Verb = "Create",
                Ontology = "JS",
                IsDumb = true,
                TimeStamp = DateTime.UtcNow.Ticks,
                Version = "v1",
                Body = new BodyData(infoValue, infoValue)
                {
                    Event = new EventData
                    {
                        Status = (int)Status.AuditApproved,
                        ReasonPhrase = Status.AuditApproved.ToName(),
                        Subject = "StateCodeChanged.Audit",
                        SourceType = EventSourceType.Command.ToName()
                    }
                },
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.NotExist == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.NotExist == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Command_Event_NotExist
        [TestMethod]
        public void Command_Event_NotExist()
        {
            var xm = System.Guid.NewGuid().ToString();
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM",xm},
                {"ZZJGM","11011421005"}
            }).ToArray();
            var dto = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                MessageType = "event",
                Verb = "Create",
                Ontology = "JS",
                IsDumb = true,
                TimeStamp = DateTime.UtcNow.Ticks,
                Version = "v1",
                Body = new BodyData(infoValue, infoValue)
                {
                    Event = new EventData
                    {
                        Status = (int)Status.AuditApproved,
                        Subject = "StateCodeChanged",
                        ReasonPhrase = Status.AuditApproved.ToName(),
                        SourceType = EventSourceType.Command.ToName()
                    }
                },
            }.JspxToken();
            // var response = dto.RequestNode(acDomain.NodeHost.Nodes.CenterNode);
            var response = AnyMessage.Create(HecpRequest.Create(AcDomain, dto), AcDomain.NodeHost.Nodes.ThisNode).Response();
            Assert.IsTrue((int)Status.NotExist == response.Body.Event.Status, response.Body.Event.Description);
            dto.IsDumb = false;
            response = dto.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.NotExist == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Command_Event_NotExist2
        [TestMethod]
        public void Command_Event_NotExist2()
        {
            var xm = System.Guid.NewGuid().ToString();
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM",xm},
                {"ZZJGM","11011421005"}
            }).ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Verb = "Create",
                Ontology = "JS",
                MessageType = "event",
                IsDumb = true,
                TimeStamp = DateTime.UtcNow.Ticks,
                Version = "v1",
                Body = new BodyData(infoValue, infoValue)
                {
                    Event = new EventData
                    {
                        Subject = "StateCodeChanged",
                        SourceType = EventSourceType.Command.ToName(),
                        Status = (int)Status.AuditApproved
                    }
                }
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.NotExist == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.NotExist == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Entity_Event_ReceiveOk
        [TestMethod]
        public void Entity_Event_ReceiveOk()
        {
            var xm = System.Guid.NewGuid().ToString();
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM",xm},
                {"ZZJGM","11011421005"}
            }).ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                MessageType = "event",
                IsDumb = true,
                Verb = "Create",
                Ontology = "JS",
                TimeStamp = DateTime.UtcNow.Ticks,
                Version = "v1",
                Body = new BodyData(infoValue, infoValue)
                {
                    Event = new EventData
                    {
                        Status = (int)Status.Ok,
                        Subject = "StateCodeChanged",
                        ReasonPhrase = Status.Ok.ToName(),
                        SourceType = "entity"
                    }
                }
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.Nonsupport == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.Nonsupport == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Entity_Event_InvalidStateCode
        [TestMethod]
        public void Entity_Event_InvalidStateCode()
        {
            var xm = System.Guid.NewGuid().ToString();
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM",xm},
                {"ZZJGM","11011421005"}
            }).ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                MessageType = "event",
                IsDumb = true,
                Verb = "Create",
                Ontology = "JS",
                TimeStamp = DateTime.UtcNow.Ticks,
                Version = "v1",
                Body = new BodyData(infoValue, infoValue)
                {
                    Event = new EventData
                    {
                        Subject = "StateCodeChanged",
                        SourceType = "entity"
                    }
                },
            }.JspxToken();
            //var response = request.RequestNode(acDomain.NodeHost.Nodes.CenterNode);
            var response = AnyMessage.Create(HecpRequest.Create(AcDomain, request), AcDomain.NodeHost.Nodes.CenterNode).Response();
            Assert.IsTrue((int)Status.InvalidStatus == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.InvalidStatus == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Ticks_TimeOut
        [TestMethod]
        public void Ticks_TimeOut()
        {
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM","测试"},
                {"ZZJGM","11011421005"}
            }).ToArray();
            var client = new JsonServiceClient(AcDomain.NodeHost.Nodes.ThisNode.Node.AnycmdApiAddress);
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                MessageType = "action",
                IsDumb = true,
                Verb = "Delete",
                Ontology = "JS",
                Body = new BodyData(new KeyValueBuilder().Append("Id", "69e58ec0-5eb2-4633-9117-b433fc205b8f").ToArray(), infoValue),
                TimeStamp = DateTime.UtcNow.Ticks
            }.TimeOutToken();
            var response = client.Get(request);
            Assert.IsTrue(Status.NotAuthorized.ToName() == response.Body.Event.ReasonPhrase, response.Body.Event.Description);
            request.IsDumb = false;
            response = client.Get(request);
            Assert.IsTrue(Status.NotAuthorized.ToName() == response.Body.Event.ReasonPhrase, response.Body.Event.Description);
        }
        #endregion

        #region Action_Create_Must_Gave_XM_And_ZZJGM
        [TestMethod]
        public void Action_Create_Must_Gave_XM_And_ZZJGM()
        {
            var infoValue = new KeyValueBuilder().Append("XM", "test").ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                MessageType = "action",
                IsDumb = true,
                Verb = "Create",
                Ontology = "JS",
                Body = new BodyData(infoValue, infoValue)
                {
                    QueryList = new string[] { "Id" }
                },
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.InvalidInfoId == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.InvalidInfoId == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Command_Create_Must_Gave_XM_And_ZZJGM
        [TestMethod]
        public void Command_Create_Must_Gave_XM_And_ZZJGM()
        {
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM","test"}
            }).ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                MessageType = "command",
                IsDumb = true,
                Verb = "Create",
                Ontology = "JS",
                Body = new BodyData(infoValue, infoValue),
                TimeStamp = DateTime.UtcNow.Ticks,
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.InvalidInfoId == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.InvalidInfoId == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Command_Action_Update
        [TestMethod]
        public void Command_Action_Update()
        {
            // 首先使用一个Create型命令准备测试数据。然后测试Command和Action，最后打扫现场删除测试数据。
            var xm = NewXM();
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM",xm},
                {"ZZJGM","11011421004"}
            }).ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                MessageType = "action",
                IsDumb = false,
                Verb = "Create",
                Ontology = "JS",
                Body = new BodyData(infoValue, infoValue),
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            // 从配置文件中读取或者从数据库表的列名读取
            KeyValue[] infoId = new KeyValueBuilder(new Dictionary<string, string> {
                {"XBM",(DateTime.Now.Ticks % 3).ToString()}
            }).ToArray();
            request.MessageId = Guid.NewGuid().ToString();
            request.MessageType = "Command";
            request.Body = new BodyData(infoValue, new KeyValueBuilder(request.Body.InfoValue).Append("XBM", "1").ToArray());
            request.TimeStamp = DateTime.UtcNow.Ticks;
            request.Verb = "Update";
            request.IsDumb = true;
            //response = request.RequestNode(acDomain.NodeHost.Nodes.CenterNode);
            response = AnyMessage.Create(HecpRequest.Create(AcDomain, request), AcDomain.NodeHost.Nodes.CenterNode).Response();
            Assert.IsTrue((int)Status.ReceiveOk == response.Body.Event.Status, response.Body.Event.Description);
            request.MessageType = "action";
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);

            request.MessageType = "command";
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ReceiveOk == response.Body.Event.Status, response.Body.Event.Description);
            request.MessageType = "action";
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            request.Verb = "delete";
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Action_Update
        [TestMethod]
        public void Action_Update()
        {
            var xm = NewXM();
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM",xm},
                {"ZZJGM","11011421004"}
            }).ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                MessageType = "action",
                IsDumb = false,
                Verb = "Create",
                Ontology = "JS",
                Body = new BodyData(infoValue, infoValue),
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            request.MessageId = Guid.NewGuid().ToString();
            request.Verb = "Update";
            request.TimeStamp = DateTime.UtcNow.Ticks;
            request.Body.InfoValue = new KeyValueBuilder().Append("XM", xm).Append("ZZJGM", "11011421004").ToArray();
            xm = NewXM();
            request.Body.InfoValue[0].Value = xm;
            request.Body.InfoValue[1].Value = "11011421005";
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            request.Body.InfoId = new KeyValueBuilder().Append("XM", xm).Append("ZZJGM", "11011421005").ToArray();
            request.Verb = "delete";
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Catalog_Must_IsLeaf
        // 目录必须是叶子节点
        [TestMethod]
        public void Catalog_Must_IsLeaf()
        {
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM","测试"},
                {"ZZJGM","110114"}
            }).ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                MessageType = "action",
                IsDumb = true,
                Verb = "Update",
                Ontology = "JS",
                Body = new BodyData(new KeyValueBuilder().Append("Id", "0008E9A4-CC11-48FB-9B1C-C72D4795AEDF").ToArray(), infoValue),
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.InvalidCatalog == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.InvalidCatalog == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Action_Update_OutOfLength
        [TestMethod]
        public void Action_Update_OutOfLength()
        {
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM","测试"},
                {"ZZJGM","11011421005"}
            }).ToArray();
            var request = new Message
            {
                Version = "v1",
                MessageType = "action",
                IsDumb = true,
                Verb = "Update",
                Ontology = "JS",
                MessageId = new string('A', 100),
                Body = new BodyData(new KeyValueBuilder().Append("Id", "010C1D7A-9BA5-4AEA-9D4B-290476A79D12").ToArray(), infoValue),
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.OutOfLength == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.OutOfLength == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Action_Get
        [TestMethod]
        public void Action_Get()
        {
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                Verb = "Get",
                MessageType = "action",
                IsDumb = true,
                Ontology = "JS",
                Body = new BodyData(new KeyValueBuilder().Append("Id", "0000A33A-F0A1-48CD-A9F2-FEB19F8E2BD0").ToArray(), null)
                {
                    QueryList = new string[] { "Id" }
                },
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            Assert.IsFalse(string.IsNullOrEmpty(response.Body.InfoValue[0].Key));
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            Assert.IsFalse(string.IsNullOrEmpty(response.Body.InfoValue[0].Key));
        }
        #endregion

        #region Action_Get_Performance
        [TestMethod]
        public async Task Action_Get_PerformanceAsync()
        {
            var client = new JsonServiceClient(AcDomain.NodeHost.Nodes.ThisNode.Node.AnycmdApiAddress);
            for (int i = 0; i < 1000; i++)
            {
                var request = new Message
                {
                    MessageId = System.Guid.NewGuid().ToString(),
                    Version = "v1",
                    Verb = "Get",
                    MessageType = "action",
                    IsDumb = false,
                    Ontology = "JS",
                    Body = new BodyData(new KeyValueBuilder().Append("Id", Guid.NewGuid().ToString()).ToArray(), null),
                    TimeStamp = DateTime.UtcNow.Ticks
                }.JspxToken();
                var response = await client.GetAsync(request);
            }
        }

        [TestMethod]
        public void Action_Get_Performance()
        {
            var client = new JsonServiceClient(AcDomain.NodeHost.Nodes.ThisNode.Node.AnycmdApiAddress);
            for (int i = 0; i < 1000; i++)
            {
                var request = new Message
                {
                    MessageId = System.Guid.NewGuid().ToString(),
                    Version = "v1",
                    Verb = "Get",
                    MessageType = "action",
                    IsDumb = false,
                    Ontology = "JS",
                    Body = new BodyData(new KeyValueBuilder().Append("Id", Guid.NewGuid().ToString()).ToArray(), null),
                    TimeStamp = DateTime.UtcNow.Ticks
                }.JspxToken();
                var response = client.Get(request);
            }
        }
        #endregion

        #region Action_Get_IncrementId
        [TestMethod]
        public void Action_Get_IncrementId()
        {
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                Verb = "Get",
                MessageType = "action",
                IsDumb = true,
                Ontology = "JS",
                Body = new BodyData(new KeyValueBuilder().Append("Id", "0000A33A-F0A1-48CD-A9F2-FEB19F8E2BD0").ToArray(), null),
                TimeStamp = DateTime.UtcNow.Ticks
            }.UiaToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            Assert.IsFalse(string.IsNullOrEmpty(response.Body.InfoValue[0].Key));
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            Assert.IsTrue(response.Body.InfoValue.Any(a => a.Key.Equals("IncrementId", StringComparison.OrdinalIgnoreCase)));
        }
        #endregion

        #region Action_Get2
        [TestMethod]
        public void Action_Get2()
        {
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                Verb = "Get",
                MessageType = "action",
                IsDumb = true,
                Ontology = "JS",
                Body = new BodyData(new KeyValueBuilder().Append("Id", "0000A33A-F0A1-48CD-A9F2-FEB19F8E2BD0").ToArray(), null),
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            var xm = response.Body.InfoValue.FirstOrDefault(a => a.Key.Equals("XM", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(xm != null);
            Assert.IsTrue(response.Body.InfoValue.Any(a => a.Key.Equals("ZZJGM", StringComparison.OrdinalIgnoreCase)));
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            xm = response.Body.InfoValue.FirstOrDefault(a => a.Key.Equals("XM", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(xm != null);
            Assert.IsTrue(response.Body.InfoValue.Any(a => a.Key.Equals("ZZJGM", StringComparison.OrdinalIgnoreCase)));
        }
        #endregion

        #region Command_Get
        [TestMethod]
        public void Command_Get()
        {
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                Verb = "Get",
                MessageType = "command",
                IsDumb = true,
                Ontology = "JS",
                Body = new BodyData(new KeyValueBuilder().Append("Id", "0000A33A-F0A1-48CD-A9F2-FEB19F8E2BD0").ToArray(), null),
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ReceiveOk == response.Body.Event.Status, response.Body.Event.Description);
            Assert.IsFalse(string.IsNullOrEmpty(response.Body.InfoValue[0].Key));
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ReceiveOk == response.Body.Event.Status, response.Body.Event.Description);
            Assert.IsFalse(string.IsNullOrEmpty(response.Body.InfoValue[0].Key));
        }
        #endregion

        #region Action_Head
        [TestMethod]
        public void Action_Head()
        {
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM","教师100"}
            }).ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                Verb = "Head",
                MessageType = "action",
                IsDumb = true,
                Ontology = "JS",
                Body = new BodyData(infoValue, null),
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Command_Head
        [TestMethod]
        public void Command_Head()
        {
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM","教师100"}
            }).ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                IsDumb = true,
                Verb = "Head",
                MessageType = "command",
                Ontology = "JS",
                Body = new BodyData(infoValue, null),
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ReceiveOk == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ReceiveOk == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        #region Permission
        [TestMethod]
        public void Permission()
        {
            var infoId = new KeyValueBuilder(new Dictionary<string, string> {
                {"SFZJH","320113198108242027"},
                {"SFZJLXM","1"},
                {"GHHM","85012345"}
            }).ToArray();
            var cmdDto = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                Verb = "Head",
                MessageType = "action",
                IsDumb = true,
                Ontology = "JS",
                Body = new BodyData(infoId, null),
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = cmdDto.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            //var response = AnyMessage.Create(HecpRequest.Create(request, Credential.Create(request))).Response();
            Assert.IsTrue((int)Status.NoPermission == response.Body.Event.Status, response.Body.Event.Description);
            cmdDto.IsDumb = false;
            //response = request.RequestNode(acDomain.NodeHost.Nodes.CenterNode);
            // 使用下面这行可以绕过网络传输从而易于调试，而上面那行需要网络传输
            response = AnyMessage.Create(HecpRequest.Create(AcDomain, cmdDto), AcDomain.NodeHost.Nodes.ThisNode).Response();
            Assert.IsTrue((int)Status.NoPermission == response.Body.Event.Status, response.Body.Event.Description);

            cmdDto.MessageType = "Command";
            response = cmdDto.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            //var response = AnyMessage.Create(HecpRequest.Create(request, Credential.Create(request))).Response();
            Assert.IsTrue((int)Status.NoPermission == response.Body.Event.Status, response.Body.Event.Description);
            cmdDto.IsDumb = false;
            //response = request.RequestNode(acDomain.NodeHost.Nodes.CenterNode);
            // 使用下面这行可以绕过网络传输从而易于调试，而上面那行需要网络传输
            response = AnyMessage.Create(HecpRequest.Create(AcDomain, cmdDto), AcDomain.NodeHost.Nodes.ThisNode).Response();
            Assert.IsTrue((int)Status.NoPermission == response.Body.Event.Status, response.Body.Event.Description);
        }

        [TestMethod]
        public void Permission_Level1Action()
        {

        }

        [TestMethod]
        public void Permission_Level2ElementAction()
        {

        }

        [TestMethod]
        public void Permission_Level3ClientAction()
        {

        }

        [TestMethod]
        public void Permission_Level4ClientElementAction()
        {

        }

        [TestMethod]
        public void Permission_Level5CatalogAction()
        {

        }

        [TestMethod]
        public void Permission_Level6EntityAction()
        {

        }

        [TestMethod]
        public void Permission_Level7EntityElementAction()
        {

        }
        #endregion

        #region Audit
        [TestMethod]
        public void Audit_Level1Action()
        {

        }

        [TestMethod]
        public void Audit_Level2ElementAction()
        {

        }

        [TestMethod]
        public void Audit_Level3ClientAction()
        {

        }

        [TestMethod]
        public void Audit_Level4ClientElementAction()
        {

        }

        #region Audit_Level5CatalogAction
        [TestMethod]
        public void Audit_Level5CatalogAction()
        {
            var xm = NewXM();
            var infoValue = new KeyValueBuilder(new Dictionary<string, string> {
                {"XM",xm},
                {"ZZJGM","11010000001"}
            }).ToArray();
            var request = new Message
            {
                MessageId = System.Guid.NewGuid().ToString(),
                Version = "v1",
                MessageType = "action",
                IsDumb = true,
                Verb = "Create",
                Ontology = "JS",
                Body = new BodyData(infoValue, infoValue),
                TimeStamp = DateTime.UtcNow.Ticks
            }.JspxToken();
            var response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ToAudit == response.Body.Event.Status, response.Body.Event.Description);
            request.IsDumb = false;
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.ToAudit == response.Body.Event.Status, response.Body.Event.Description);
            request.Verb = "update";
            response = request.RequestNode(AcDomain.NodeHost.Nodes.CenterNode);
            Assert.IsTrue((int)Status.NotExist == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        [TestMethod]
        public void Audit_Level6EntityAction()
        {

        }

        [TestMethod]
        public void Audit_Level7EntityElementAction()
        {

        }
        #endregion

        #region UpdateLoginName
        [TestMethod]
        public void UpdateLoginName()
        {
            var localEntityId = Guid.NewGuid().ToString();
            var xm = NewXM();
            var infoId = new KeyValue[] { 
                new KeyValue("ZZJGM", "11010621022"), 
                new KeyValue("XM", xm) 
            };
            var infoValue = infoId;
            var request = new Message()
            {
                Version = ApiVersion.V1.ToName(),
                IsDumb = false,
                MessageType = MessageType.Action.ToName(),
                MessageId = Guid.NewGuid().ToString(),
                Verb = "create",
                Ontology = "JS",
                TimeStamp = DateTime.UtcNow.Ticks,
                Body = new BodyData(infoValue, infoValue),
            }.UiaSignature();
            var response = AnyMessage.Create(HecpRequest.Create(AcDomain, request), AcDomain.NodeHost.Nodes.ThisNode).Response();
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            request.Body.InfoId = response.Body.InfoValue;
            request.Verb = "update";
            request.Body.InfoValue = new KeyValue[] { new KeyValue("LoginName", DateTime.Now.Ticks.ToString()) };
            response = AnyMessage.Create(HecpRequest.Create(AcDomain, request), AcDomain.NodeHost.Nodes.ThisNode).Response();
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
            request.Verb = "delete";
            response = AnyMessage.Create(HecpRequest.Create(AcDomain, request), AcDomain.NodeHost.Nodes.ThisNode).Response();
            Assert.IsTrue((int)Status.ExecuteOk == response.Body.Event.Status, response.Body.Event.Description);
        }
        #endregion

        private string NewXM()
        {
            var s = DateTime.Now.ToString("yyMMddHHmmssfff");
            s = s.Substring(2, 12);
            return s;
        }
    }
}
