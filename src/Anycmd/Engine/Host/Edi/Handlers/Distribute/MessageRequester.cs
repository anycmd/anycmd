
namespace Anycmd.Engine.Host.Edi.Handlers.Distribute
{
    using Exceptions;
    using Info;
    using Logging;
    using Model;
    using System;
    using Util;

    /// <summary>
    /// 命令请求者
    /// </summary>
    public class MessageRequester
    {
        /// <summary>
        /// 
        /// </summary>
        public static MessageRequester Instance
        {
            get
            {
                return new MessageRequester();
            }
        }

        #region Ctor
        private MessageRequester()
        {
        }
        #endregion

        #region Request
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public virtual void Request(DistributeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (context.Result.IsClosed)
            {
                return;
            }
            QueryResult result = context.Result;
            if (!CanRequest(context))
            {
                #region 请求条件不成立，不引发分发请求
                context.Ontology.MessageProvider.SaveCommand(context.Ontology, context.Command.ToDistributeFailing(result));
                if (context.Command is IEntity)
                {
                    context.Ontology.MessageProvider.DeleteCommand(MessageTypeKind.Distribute, context.Ontology, context.Command.Id, context.Command.IsDumb);
                }
                return;
                #endregion
            }
            else
            {
                #region 请求
                context.ClientAgent.Transfer.Transmit(context);

                if (context.Exception != null)
                {
                    context.ClientAgent.IsNetException = true;
                    context.Result.UpdateStatus(Status.InternalServerError, context.Exception.Message);
                    context.Ontology.Host.LoggingService.Error(context.Exception);
                }
                else
                {
                    var stateCode = result.Status;
                    if (stateCode < 200)
                    {
                        if (context.Responder == context.Ontology.Host.NodeHost.Nodes.ThisNode) return;
                        IInfoStringConverter converter;
                        if (!context.Ontology.Host.NodeHost.InfoStringConverters.TryGetInfoStringConverter(context.Command.DataTuple.InfoFormat, out converter))
                        {
                            throw new AnycmdException("意外的信息格式" + context.Command.DataTuple.InfoFormat);
                        }
                        var anyLog = new AnyLog(Guid.NewGuid())
                        {
                            Req_Ontology = context.Ontology.Ontology.Code,
                            Req_Verb = context.Command.Verb.Code,
                            Req_ClientId = context.Command.ClientId,
                            Req_ClientType = context.Command.ClientType.ToName(),
                            CreateOn = context.Command.CreateOn,
                            Req_Description = context.Command.Description,
                            Req_EventSourceType = context.Command.EventSourceType,
                            Req_EventSubjectCode = context.Command.EventSubjectCode,
                            InfoFormat = context.Command.DataTuple.InfoFormat,
                            Req_InfoId = context.Command.DataTuple.IdItems.InfoString,
                            Req_InfoValue = context.Command.DataTuple.ValueItems.InfoString,
                            Req_UserName = context.Command.UserName,
                            Req_IsDumb = context.Command.IsDumb,
                            LocalEntityId = context.Command.LocalEntityId,
                            OrganizationCode = context.Command.OrganizationCode,
                            Req_ReasonPhrase = context.Command.ReasonPhrase,
                            ReceivedOn = context.Command.ReceivedOn,
                            Req_MessageId = context.Command.MessageId,
                            Req_MessageType = context.Command.MessageType.ToName(),
                            Req_QueryList = context.Command.DataTuple.QueryListString,
                            Req_Status = context.Command.Status,
                            Req_TimeStamp = context.Command.TimeStamp,
                            Req_Version = context.Command.Version,
                            Res_InfoValue = converter.ToInfoString(context.Result.ResultDataItems),
                            Res_Description = context.Result.Description,
                            Res_ReasonPhrase = context.Result.ReasonPhrase,
                            Res_StateCode = (int)context.Result.Status
                        };
                        context.Ontology.Host.LoggingService.Log(anyLog);
                    }
                    else if (stateCode >= 500)
                    {
                        context.ClientAgent.ServiceIsNotAlive = true;
                    }
                    else
                    {
                        if (stateCode >= 200 && stateCode < 300)
                        {
                            if (context.Command.CommandType == MessageTypeKind.Distribute)
                            {
                                context.Ontology.MessageProvider.SaveCommand(context.Ontology, context.Command.ToDistributed(result));
                            }
                        }
                        else
                        {
                            context.Ontology.MessageProvider.SaveCommand(context.Ontology, context.Command.ToDistributeFailing(result));
                        }
                        if (context.Command is IEntity)
                        {
                            context.Ontology.MessageProvider.DeleteCommand(MessageTypeKind.Distribute, context.Ontology, context.Command.Id, context.Command.IsDumb);
                        }
                    }
                }
                #endregion
            }
        }
        #endregion

        #region IsCanRequest
        /// <summary>
        /// 查看当前命令是否满足发起分发请求的条件
        /// </summary>
        /// <returns></returns>
        private static bool CanRequest(DistributeContext context)
        {
            if (context.Ontology == null)
            {
                context.Result.UpdateStatus(Status.InvalidOntology, "非法的本体");
                return false;
            }
            if (context.Responder == null)
            {
                context.Result.UpdateStatus(Status.InvalidClientId, "非法的节点");
                return false;
            }
            if (!context.Responder.IsEnabled)
            {
                context.Result.UpdateStatus(Status.NodeIsDisabled, context.Responder.Name + "节点已被禁用");
                return false;
            }
            if (!context.Responder.IsDistributeEnabled)
            {
                context.Result.UpdateStatus(Status.SendIsDisabled, "已禁止向" + context.Responder.Name + "节点分发命令");
                return false;
            }
            if (context.Responder.IsNetException)
            {
                context.Result.UpdateStatus(Status.NodeException, "与" + context.Responder.Name + "节点的连接网络异常。请" + context.Responder.BeatPeriod.ToString() + "分钟后重试");
                return false;
            }
            if (context.Responder.ServiceIsNotAlive)
            {
                context.Result.UpdateStatus(Status.ServiceIsNotAlive, context.Responder.Name + "节点的服务当前不可用");
                return false;
            }

            return true;
        }
        #endregion
    }
}
