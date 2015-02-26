
namespace Anycmd.Edi.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using DataContracts;
    using Engine.Edi;
    using Engine.Hecp;
    using Engine.Host.Edi;
    using Engine.Host.Edi.Handlers;
    using Engine.Info;
    using Exceptions;
    using MiniUI;
    using ServiceModel.Operations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels.MessageViewModels;

    /// <summary>
    /// 命令型视图控制器<see cref="MessageEntity"/>
    /// </summary>
    [Guid("D80417C2-B12A-4731-996B-D495C65A047C")]
    public class CommandController : AnycmdController
    {
        #region Views
        /// <summary>
        /// 命令详细信息
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("命令详细信息")]
        [Guid("45CE7626-89C9-4F84-9F81-6AD0C0DF6528")]
        public ViewResultBase Details()
        {
            return ViewResult();
        }

        /// <summary>
        /// 已接收的命令
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("已接收的命令")]
        [Guid("F1800DDA-8DD1-43F8-BCE7-B07DA441618D")]
        public ViewResultBase ReceivedMessage()
        {
            return ViewResult();
        }

        /// <summary>
        /// 接收失败的命令
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("接收失败的命令")]
        [Guid("10FAEF2C-0104-4F79-B038-7DE67A3CB7F8")]
        public ViewResultBase UnacceptedMessage()
        {
            return ViewResult();
        }

        /// <summary>
        /// 执行失败的命令
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("执行失败的命令")]
        [Guid("10FCBDDA-4EA3-498A-B3B9-137F83FEDDF4")]
        public ViewResultBase HandleFailingCommand()
        {
            return ViewResult();
        }

        /// <summary>
        /// 已执行的命令
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("已执行的命令")]
        [Guid("1900CC5F-43BE-4E3D-9836-B6577B7F3207")]
        public ViewResultBase HandledCommand()
        {
            return ViewResult();
        }

        /// <summary>
        /// 待分发的命令
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("待分发的命令")]
        [Guid("15F7345C-6C28-4A38-BF95-29BD3E14F5FF")]
        public ViewResultBase DistributeMessage()
        {
            return ViewResult();
        }

        /// <summary>
        /// 分发失败的命令
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分发失败的命令")]
        [Guid("EFAA2F33-73A6-49E9-B330-D12D914E76C9")]
        public ViewResultBase DistributeFailingMessage()
        {
            return ViewResult();
        }

        /// <summary>
        /// 已分发的命令
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("已分发的命令")]
        [Guid("254D32DA-B0D7-4D90-9708-40E31915EBB9")]
        public ViewResultBase DistributedMessage()
        {
            return ViewResult();
        }

        /// <summary>
        /// 本地事件
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("本地事件")]
        [Guid("8FA60CFF-FC3B-4F62-8388-D100C976AF49")]
        public ViewResultBase LocalEvent()
        {
            return ViewResult();
        }

        /// <summary>
        /// 客户端事件
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("客户端事件")]
        [Guid("6D6ECAE1-06C8-4B4A-9C74-153AE70F8247")]
        public ViewResultBase ClientEvent()
        {
            return ViewResult();
        }

        #endregion

        #region EntityViewResults
        /// <summary>
        /// 已成功执行的命令
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("已成功执行的命令")]
        [Guid("0D3C54C0-C891-4773-A432-43E6AF927989")]
        public ViewResultBase EntityHandledCommands()
        {
            return ViewResult();
        }

        /// <summary>
        /// 执行失败的命令
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("执行失败的命令")]
        [Guid("EE8A513A-EA5B-4CCC-897B-D964F77E385F")]
        public ViewResultBase EntityHandleFailingCommands()
        {
            return ViewResult();
        }

        /// <summary>
        /// 已成功接收的命令
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("已成功接收的命令")]
        [Guid("D1B6EBE8-9739-4564-AD68-55F5E45A199C")]
        public ViewResultBase EntityReceivedMessages()
        {
            return ViewResult();
        }

        /// <summary>
        /// 待分发的命令
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("待分发的命令")]
        [Guid("9CAE9678-91A1-4ED6-B86F-9BCEF1DD2892")]
        public ViewResultBase EntityDistributeMessages()
        {
            return ViewResult();
        }

        /// <summary>
        /// 已分发的命令
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("已分发的命令")]
        [Guid("86E0B1C7-B6E8-41B1-B3B6-7930AA5712F7")]
        public ViewResultBase EntityDistributedMessages()
        {
            return ViewResult();
        }

        /// <summary>
        /// 分发失败的命令
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分发失败的命令")]
        [Guid("9F31F865-7F9C-4AC1-B666-452DA44ED4DD")]
        public ViewResultBase EntityDistributeFailingMessages()
        {
            return ViewResult();
        }

        /// <summary>
        /// 本地事件
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("本地事件")]
        [Guid("E0264C74-EE14-4378-AF0D-9E670D0C5B06")]
        public ViewResultBase EntityLocalEvents()
        {
            return ViewResult();
        }

        /// <summary>
        /// 客户端事件
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("客户端事件")]
        [Guid("C3121A15-B55E-4D3F-898A-74B391AB153E")]
        public ViewResultBase EntityClientEvents()
        {
            return ViewResult();
        }
        #endregion

        #region AuditApproved
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="ontologyCode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("审核通过")]
        [HttpPost]
        [Guid("2F839C42-9A6C-49C0-A9E9-8BB64F98E5F5")]
        public ActionResult AuditApproved(string ontologyCode, string id)
        {
            var response = new ResponseData { id = id, success = true };
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
            {
                throw new ValidationException("非法的本体码");
            }
            string[] ids = id.Split(',');
            var localEventIDs = new Guid[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                Guid tmp;
                if (Guid.TryParse(ids[i], out tmp))
                {
                    localEventIDs[i] = tmp;
                }
                else
                {
                    throw new ValidationException("意外的本地事件标识" + ids[i]);
                }
            }
            foreach (var localEventId in localEventIDs)
            {
                MessageEntity evnt = ontology.MessageProvider.GetCommand(MessageTypeKind.LocalEvent, ontology, localEventId);
                if (evnt != null)
                {
                    if (evnt.Status == (int)Status.ToAudit
                    && evnt.EventSourceType.Equals("Command", StringComparison.OrdinalIgnoreCase))
                    {
                        var node = AcDomain.NodeHost.Nodes.ThisNode;
                        var ticks = DateTime.UtcNow.Ticks;
                        var cmd = new Message()
                        {
                            Version = ApiVersion.V1.ToName(),
                            IsDumb = false,
                            MessageType = MessageType.Event.ToName(),
                            Credential = new CredentialData
                            {
                                ClientType = ClientType.Node.ToName(),
                                UserType = UserType.None.ToName(),
                                CredentialType = CredentialType.Token.ToName(),
                                ClientId = node.Node.PublicKey.ToString(),
                                UserName = evnt.UserName,// UserName
                                Password = TokenObject.Token(node.Node.Id.ToString(), ticks, node.Node.SecretKey),
                                Ticks = ticks
                            },
                            TimeStamp = DateTime.UtcNow.Ticks,
                            MessageId = evnt.Id.ToString(),
                            Verb = evnt.Verb.Code,
                            Ontology = evnt.Ontology,
                            Body = new BodyData(new KeyValue[] { new KeyValue("Id", evnt.LocalEntityId) }, evnt.DataTuple.ValueItems.Items.ToDto())
                            {
                                Event = new EventData
                                {
                                    Status = (int)Status.AuditApproved,
                                    ReasonPhrase = Status.AuditApproved.ToName(),
                                    SourceType = evnt.EventSourceType,
                                    Subject = evnt.EventSubjectCode
                                }
                            }
                        };
                        var result = AnyMessage.Create(HecpRequest.Create(AcDomain, cmd), AcDomain.NodeHost.Nodes.ThisNode).Response();
                        if (result.Body.Event.Status == (int)Status.NotExist)
                        {
                            ontology.MessageProvider.DeleteCommand(MessageTypeKind.LocalEvent, ontology, evnt.Id, evnt.IsDumb);
                        }
                        else
                        {
                            if (result.Body.Event.Status < 200 || result.Body.Event.Status >= 400)
                            {
                                response.success = false;
                                response.msg = result.Body.Event.Description;
                                response.Warning();
                            }
                        }
                    }
                }
                else
                {
                    response.success = false;
                    response.msg = "给定标识的本地事件不存在";
                    response.Warning();
                }
            }

            return this.JsonResult(response);
        }
        #endregion

        #region AuditUnapproved
        /// <summary>
        /// 审核不通过
        /// </summary>
        /// <param name="ontologyCode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("审核不通过")]
        [HttpPost]
        [Guid("B3C0361E-9387-4222-BB9F-4C157CCA8E59")]
        public ActionResult AuditUnapproved(string ontologyCode, string id)
        {
            var response = new ResponseData { id = id, success = true };
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
            {
                throw new ValidationException("非法的本体码");
            }
            string[] ids = id.Split(',');
            var localEventIDs = new Guid[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                Guid tmp;
                if (Guid.TryParse(ids[i], out tmp))
                {
                    localEventIDs[i] = tmp;
                }
                else
                {
                    throw new ValidationException("意外的本地事件标识" + ids[i]);
                }
            }
            foreach (var localEventId in localEventIDs)
            {
                MessageEntity evnt = ontology.MessageProvider.GetCommand(MessageTypeKind.LocalEvent, ontology, localEventId);
                if (evnt != null)
                {
                    if (evnt.Status == (int)Status.ToAudit
                    && evnt.EventSourceType.Equals("Command", StringComparison.OrdinalIgnoreCase))
                    {
                        var node = AcDomain.NodeHost.Nodes.ThisNode;
                        var ticks = DateTime.UtcNow.Ticks;
                        var cmd = new Message()
                        {
                            Version = ApiVersion.V1.ToName(),
                            IsDumb = false,
                            MessageType = MessageType.Event.ToName(),
                            Credential = new CredentialData
                            {
                                ClientType = ClientType.Node.ToName(),
                                UserType = UserType.None.ToName(),
                                CredentialType = CredentialType.Token.ToName(),
                                ClientId = node.Node.Id.ToString(),
                                UserName = evnt.UserName,// UserName
                                Password = TokenObject.Token(node.Node.Id.ToString(), ticks, node.Node.SecretKey),
                                Ticks = ticks
                            },
                            TimeStamp = DateTime.UtcNow.Ticks,
                            MessageId = evnt.Id.ToString(),
                            Verb = evnt.Verb.Code,
                            Ontology = evnt.Ontology,
                            Body = new BodyData(new KeyValue[] { new KeyValue("Id", evnt.LocalEntityId) }, evnt.DataTuple.ValueItems.Items.ToDto())
                            {
                                Event = new EventData
                                {
                                    Status = (int)Status.AuditUnapproved,
                                    ReasonPhrase = Status.AuditUnapproved.ToName(),
                                    SourceType = evnt.EventSourceType,
                                    Subject = evnt.EventSubjectCode
                                }
                            }
                        };
                        var result = AnyMessage.Create(HecpRequest.Create(AcDomain, cmd), AcDomain.NodeHost.Nodes.ThisNode).Response();
                        if (result.Body.Event.Status == (int)Status.NotExist)
                        {
                            ontology.MessageProvider.DeleteCommand(MessageTypeKind.LocalEvent, ontology, evnt.Id, evnt.IsDumb);
                        }
                        else
                        {
                            if ((result.Body.Event.Status < 200 || result.Body.Event.Status >= 400) && result.Body.Event.Status != (int)Status.AuditUnapproved)
                            {
                                response.success = false;
                                response.msg = result.Body.Event.Description;
                                response.Warning();
                            }
                        }
                    }
                }
            }

            return this.JsonResult(response);
        }
        #endregion

        #region GetInfo
        /// <summary>
        /// 根据命令ID获取给定类型的命令的详细信息
        /// </summary>
        /// <param name="ontologyCode"></param>
        /// <param name="id"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据命令ID获取给定类型的命令的详细信息")]
        [Guid("5588AA4B-B45D-47EE-820D-EFDE59A61872")]
        public ActionResult GetInfo(string ontologyCode, Guid? id, string commandType)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("命令标识不能为空");
            }
            if (string.IsNullOrEmpty(commandType))
            {
                throw new ValidationException("命令类型不能为空");
            }
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
            {
                throw new ValidationException("非法的本体码");
            }
            MessageTypeKind commandTypeEnum;
            if (!commandType.TryParse(out commandTypeEnum))
            {
                throw new ValidationException("非法的命令类型" + commandType);
            }
            MessageTr data = null;
            switch (commandTypeEnum)
            {
                case MessageTypeKind.Invalid:
                    throw new ValidationException("非法的命令类型");
                case MessageTypeKind.AnyCommand:
                    throw new ValidationException("AnyCommand不是实体命令类型");
                case MessageTypeKind.Received:
                case MessageTypeKind.Unaccepted:
                case MessageTypeKind.Executed:
                case MessageTypeKind.ExecuteFailing:
                case MessageTypeKind.Distribute:
                case MessageTypeKind.Distributed:
                case MessageTypeKind.DistributeFailing:
                case MessageTypeKind.LocalEvent:
                case MessageTypeKind.ClientEvent:
                    data = ontology.MessageProvider.GetCommandInfo(commandTypeEnum, ontology, id.Value);
                    break;
                default:
                    throw new ValidationException("非法的命令类型" + commandType);
            }
            return this.JsonResult(data);
        }
        #endregion

        #region GetPlist
        /// <summary>
        /// 分页获取命令
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取命令")]
        [Guid("3498B4C6-7896-440D-9941-202E7A485AB3")]
        public ActionResult GetPlist(GetPlistMessages requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(requestModel.OntologyCode, out ontology))
            {
                throw new ValidationException("非法的本体码");
            }
            MessageTypeKind commandTypeEnum;
            if (!requestModel.CommandType.TryParse(out commandTypeEnum))
            {
                throw new ValidationException("非法的命令类型" + requestModel.CommandType);
            }
            long total;
            IList<MessageTr> list = null;
            switch (commandTypeEnum)
            {
                case MessageTypeKind.Invalid:
                    throw new ValidationException("非法的命令类型");
                case MessageTypeKind.AnyCommand:
                    throw new ValidationException("AnyCommand不是实体命令类型");
                case MessageTypeKind.Received:
                case MessageTypeKind.Unaccepted:
                case MessageTypeKind.Executed:
                case MessageTypeKind.ExecuteFailing:
                case MessageTypeKind.Distribute:
                case MessageTypeKind.Distributed:
                case MessageTypeKind.DistributeFailing:
                case MessageTypeKind.LocalEvent:
                case MessageTypeKind.ClientEvent:
                    list = ontology.MessageProvider.GetPlistCommandTrs(
                        commandTypeEnum, ontology, requestModel.CatalogCode,
                        requestModel.ActionCode, requestModel.NodeId, requestModel.EntityId,
                        requestModel.PageIndex, requestModel.PageSize,
                        requestModel.SortField, requestModel.SortOrder, out total);
                    break;
                default:
                    throw new ValidationException("非法的命令类型" + requestModel.CommandType);
            }
            var data = new MiniGrid<MessageTr> { data = list, total = total };

            return this.JsonResult(data);
        }
        #endregion
    }
}
