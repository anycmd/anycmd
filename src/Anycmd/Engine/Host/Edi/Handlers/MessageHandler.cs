
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Ac;
    using Exceptions;
    using Hecp;
    using Info;
    using Logging;
    using Model;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Util;

    /// <summary>
    /// 命令描述对象，它完整的描述了一条命令的信息。该模型是只读的且无法在核心程序集外部构造。
    /// 之所以需要是只读的内部的是因为该对象会被传递给命令验证器、信息项验证器等执行验证逻辑
    /// （这些验证器不保证全部由Anycmd团队编写），从而需要将本对象置为只读的和内部的防止被修改。
    /// <remarks>命令的信息ID字典、信息值字典，节点、本体、有效状态等都在该对象上读取。</remarks>
    /// </summary>
    public sealed class MessageHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public static MessageHandler Instance
        {
            get
            {
                return new MessageHandler();
            }
        }

        #region Ctor
        private MessageHandler()
        {
        }
        #endregion

        #region Response
        /// <summary>
        /// 命令响应。进入此方法标示者进入
        /// </summary>
        /// <returns></returns>
        public void Response(MessageContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            try
            {
                if (!context.Result.IsClosed)
                {
                    var requestType = context.Command.MessageType;
                    if (!context.IsValid)
                    {
                        if (!context.Result.ResultDataItems.Any(a => a.Key.Equals("Id", StringComparison.OrdinalIgnoreCase)))
                        {
                            context.Result.ResultDataItems.Add(new DataItem("Id", context.LocalEntityId));
                        }
                        if (context.Command is IEntity)
                        {
                            using (var cmdProviderAct = new WfAct(context.Host, context, context.Ontology.MessageProvider, "保存执行失败的命令"))
                            {
                                if (requestType == MessageType.Action || context.Command.CommandType == MessageTypeKind.Received)
                                {
                                    context.Ontology.MessageProvider.SaveCommand(context.Ontology, context.Command.ToExecuteFailing(context.Result));
                                }
                                else if (requestType == MessageType.Command)
                                {
                                    context.Ontology.MessageProvider.SaveCommand(context.Ontology, context.Command.ToUnaccepted(context.Result));
                                }
                            }
                            context.Ontology.MessageProvider.DeleteCommand(context.Command.CommandType, context.Ontology, context.Command.Id, context.Command.IsDumb);
                        }
                    }
                    else
                    {
                        if (!context.Result.ResultDataItems.Any(a => a.Key.Equals("Id", StringComparison.OrdinalIgnoreCase)))
                        {
                            context.Result.ResultDataItems.Add(new DataItem("Id", context.LocalEntityId));
                        }
                        if (Verb.Get.Equals(context.Command.Verb))
                        {
                            using (var act = new WfAct(context.Host, context, context.Ontology.EntityProvider, "填充get型命令的InfoValue字段"))
                            {
                                var selectElements = new OrderedElementSet();
                                foreach (var element in context.Ontology.Elements.Values.Where(a => a.Element.IsEnabled == 1))
                                {
                                    if (context.ClientAgent.GetElementPermission(element, Verb.Get) != AllowType.NotAllow)
                                    {
                                        selectElements.Add(element);
                                    }
                                }
                                IList<InfoItem> infoValues = context.Ontology.EntityProvider.Get(
                                    context.Ontology, context.LocalEntityId, selectElements);
                                context.Result.ResultDataItems = infoValues.Select(a => new DataItem(a.Key, a.Value)).ToList();
                            }
                        }
                        var nodeHost = context.Host.NodeHost;
                        // ApplyProcessingFilters 应用处理前过滤器
                        ProcessResult result = nodeHost.ApplyEdiMessageHandingFilters(context);
                        if (!result.IsSuccess)
                        {
                            context.Result.UpdateStatus(result.StateCode, result.Description);
                        }
                        if (!context.Result.IsClosed)
                        {
                            switch (requestType)
                            {
                                case MessageType.Action:
                                    HandleAction(context);
                                    break;
                                case MessageType.Command:
                                    HandleCommand(context);
                                    break;
                                case MessageType.Event:
                                    HandleEvent(context);
                                    break;
                                default:
                                    throw new AnycmdException("意外的请求类型" + context.Command.MessageType);
                            }

                            // ApplyProcessedFilters 应用处理后过滤器
                            result = nodeHost.ApplyEdiMessageHandledFilters(context);
                            if (!result.IsSuccess)
                            {
                                context.Result.UpdateStatus(result.StateCode, result.Description);
                            }
                        }
                    }
                }
                int stateCode = context.Result.Status;
                if (stateCode < 200)
                {
                    IInfoStringConverter converter;
                    if (!context.Host.NodeHost.InfoStringConverters.TryGetInfoStringConverter(context.Command.DataTuple.InfoFormat, out converter))
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
                        CatalogCode = context.Command.CatalogCode,
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
            }
            catch (Exception ex)
            {
                context.Result.UpdateStatus(Status.InternalServerError, "服务器内部逻辑异常");
                context.Result.IsClosed = true;
                context.Exception = ex;
                throw;
            }
        }
        #endregion

        #region private Methods
        #region HandleAction
        /// <summary>
        /// 执行。Action类型的命令是立即执行的。
        /// <remarks>
        /// 该执行不会从成功接收的命令表中删除已执行的记录。
        /// </remarks>
        /// </summary>
        /// <exception cref="AnycmdException">
        /// 当当前命令不是成功接收的命令时或者当前命令的动作码是get或head时引发
        /// </exception>
        /// <returns></returns>
        private static void HandleAction(MessageContext context)
        {
            if (context.NeedAudit)
            {
                PublishAuditEvent(context);
                return;
            }
            context.Result.UpdateStatus(Status.ExecuteOk, "执行成功");
            if (Verb.Get.Equals(context.Command.Verb)
                || Verb.Head.Equals(context.Command.Verb))
            {
                return;
            }
            #region 执行
            using (var act = new WfAct(context.Host, context, context.Ontology.EntityProvider, context.Ontology.EntityProvider.Description))
            {
                using (var sqlAct = new WfAct(context.Host, context, context.Ontology.EntityProvider, "执行命令"))
                {
                    Verb actionCode = context.Command.Verb;
                    ProcessResult r = context.Ontology.EntityProvider.ExecuteCommand(context.ToDbCommand());
                    context.Result.UpdateStatus(r.StateCode, r.Description);
                    // 执行成功
                    if (r.IsSuccess)
                    {
                        using (var cmdProviderAct = new WfAct(context.Host, context, context.Ontology.MessageProvider, "保存成功执行的命令和由于命令执行而生产的待分发命令"))
                        {
                            #region 如果是正在接收的命令
                            if (!(context.Command is IEntity))
                            {
                                context.Ontology.MessageProvider.SaveCommand(context.Ontology, context.Command.ToExecuted(context.Result));
                            }
                            #endregion
                            #region 如果是以前接收的命令
                            else if (context.Command is IEntity)
                            {
                                context.Ontology.MessageProvider.SaveCommand(context.Ontology, context.Command.ToExecuted(context.Result));
                                context.Ontology.MessageProvider.DeleteCommand(MessageTypeKind.Received, context.Ontology, context.Command.Id, context.Command.IsDumb);
                            }
                            #endregion
                            #region 建造待分发命令
                            var commandFactory = context.Host.NodeHost.MessageProducer;
                            using (var factoryAct = new WfAct(context.Host, context, commandFactory, "命令工厂建造待分发命令"))
                            {
                                var products = commandFactory.Produce(new MessageTuple(context, GetTuple(context)));
                                // 保存生产的待分发命令
                                if (products != null && products.Count != 0)
                                {
                                    context.Ontology.MessageProvider.SaveCommands(context.Ontology, products.ToArray());
                                }
                            }
                            #endregion
                        }
                    }
                    // 执行失败
                    else
                    {
                        using (var cmdProviderAct = new WfAct(context.Host, context, context.Ontology.MessageProvider, "保存执行失败的命令"))
                        {
                            context.Ontology.MessageProvider.SaveCommand(context.Ontology, context.Command.ToExecuteFailing(context.Result));
                            if (context.Command is IEntity)
                            {
                                context.Ontology.MessageProvider.DeleteCommand(MessageTypeKind.Received, context.Ontology, context.Command.Id, context.Command.IsDumb);
                            }
                        }
                    }
                }
            }
            #endregion
        }
        #endregion

        #region HandleCommand
        /// <summary>
        /// 接收MessageType为Command的命令。Command类型的命令是推迟执行的。
        /// </summary>
        /// <returns></returns>
        private static void HandleCommand(MessageContext context)
        {
            if (context.Command.CommandType == MessageTypeKind.Received)
            {
                HandleAction(context);
                return;
            }
            context.Result.UpdateStatus(Status.ReceiveOk, "接收成功");
            if (context.NeedAudit)
            {
                PublishAuditEvent(context);
                return;
            }
            else
            {
                // 如果当前命令不是DTO则不需要持久化
                if (context.Command is IEntity)
                {
                    return;
                }
                // 如果当前本体动作配置为不需要持久化则不持久化
                if (context.Ontology.Actions[context.Command.Verb].IsPersist)
                {
                    using (var act = new WfAct(context.Host, context, context.Ontology.MessageProvider, "持久化成功接收的命令"))
                    {
                        var r = context.Ontology.MessageProvider.SaveCommand(context.Ontology, context.Command.ToReceived(context.Result));
                        if (!r.IsSuccess)
                        {
                            context.Result.UpdateStatus(r.StateCode, r.Description);
                        }
                    }
                }
            }
        }
        #endregion

        #region PublishAuditEvent
        private static void PublishAuditEvent(MessageContext context)
        {
            #region 审核事件
            using (var auditAct = new WfAct(context.Host, context, context.Ontology.MessageProvider, "持久化本地事件，转化为待分发事件打入分发队列"))
            {
                var evnt = new MessageEntity(MessageTypeKind.LocalEvent, context.Command.Id, context.Command.DataTuple)
                {
                    Verb = context.Command.Verb,
                    ClientType = context.Command.ClientType,
                    ReceivedOn = context.Command.ReceivedOn,
                    TimeStamp = context.Command.TimeStamp,
                    CreateOn = DateTime.Now,
                    LocalEntityId = context.LocalEntityId,
                    CatalogCode = context.CatalogCode,
                    ClientId = context.Command.ClientId,
                    Ontology = context.Command.Ontology,
                    Status = (int)context.Result.Status,
                    ReasonPhrase = context.Result.ReasonPhrase,
                    Description = context.Result.Description,
                    MessageId = context.Command.MessageId,
                    EventSourceType = EventSourceType.Command.ToName(),
                    EventSubjectCode = EventSubjectCode.StateCodeChanged_Audit,
                    MessageType = context.Command.MessageType,
                    UserName = context.Command.UserName,
                    IsDumb = context.Command.IsDumb,
                    Version = context.Command.Version
                };
                // 持久化本地事件
                context.Ontology.MessageProvider.SaveCommand(context.Ontology, evnt);
                // 如果是之前已经接收的命令，但现在需要审核了。这往往是因为成功接收后系统配置有变化
                if (context.Command is IEntity)
                {
                    context.Ontology.MessageProvider.DeleteCommand(MessageTypeKind.Received, context.Ontology, context.Command.Id, context.Command.IsDumb);
                }
            }
            #endregion
        }
        #endregion

        #region HandleEvent
        /// <summary>
        /// 处理事件。Event类型的命令是面向主题和状态码编程的。
        /// </summary>
        /// <returns></returns>
        private static void HandleEvent(MessageContext context)
        {
            EventSourceType eventSourceType;
            context.Command.EventSourceType.TryParse(out eventSourceType);
            switch (eventSourceType)
            {
                case EventSourceType.Command:
                    {
                        #region 命令事件
                        // 如果是审计节点的审计事件
                        if (context.Command.EventSubjectCode.Equals(EventSubjectCode.StateCodeChanged_Audit)
                                && (context.ClientAgent == context.Host.NodeHost.Nodes.ThisNode))
                        {
                            #region 审计节点事件
                            var localEvent = context.Ontology.MessageProvider.GetCommand(MessageTypeKind.LocalEvent,
                                context.Ontology, new Guid(context.Command.MessageId));
                            if (localEvent == null)
                            {
                                context.Result.UpdateStatus(Status.NotExist, "给定的MessageID标识的服务端事件不存在。");
                                return;
                            }
                            if (!Verb.Create.Equals(localEvent.Verb))
                            {
                                long total;
                                var localEvents = context.Ontology.MessageProvider.GetPlistCommands(MessageTypeKind.LocalEvent, context.Ontology, null, null, null, localEvent.LocalEntityId, 0, 1, "CreateOn", "asc", out total);
                                if (localEvents[0].Id != localEvent.Id)
                                {
                                    context.Result.UpdateStatus(Status.InvalidCommandTicks, "请按照时间顺序审核命令");
                                    return;
                                }
                            }
                            using (var act = new WfAct(context.Host, context, context.Ontology.MessageProvider, "审核完成"))
                            {
                                var ctx = new MessageContext(context.Host, localEvent);
                                if (context.Command.Status == (int)Status.AuditApproved)
                                {
                                    localEvent.IsDumb = context.Command.IsDumb;
                                    #region 执行
                                    using (var sqlAct = new WfAct(context.Host, ctx, ctx.Ontology.EntityProvider, "执行命令"))
                                    {
                                        // 注意：执行的是本地事件，而不是请求事件
                                        var actionCode = ctx.Command.Verb;
                                        var r = ctx.Ontology.EntityProvider.ExecuteCommand(ctx.ToDbCommand());
                                        context.Result.UpdateStatus(r.StateCode, r.Description);
                                        ctx.Result.UpdateStatus(r.StateCode, r.Description);

                                        if (r.IsSuccess)
                                        {
                                            ctx.Ontology.MessageProvider.SaveCommand(ctx.Ontology, ctx.Command.ToExecuted(context.Result));

                                            var commandFactory = context.Host.NodeHost.MessageProducer;
                                            using (var factoryAct = new WfAct(context.Host, ctx, commandFactory, "生产命令"))
                                            {
                                                var products = commandFactory.Produce(new MessageTuple(ctx, GetTuple(ctx)));
                                                // 保存生产的待分发命令
                                                if (products != null && products.Count != 0)
                                                {
                                                    ctx.Ontology.MessageProvider.SaveCommands(ctx.Ontology, products.ToArray());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ctx.Ontology.MessageProvider.SaveCommand(ctx.Ontology, ctx.Command.ToExecuteFailing(context.Result));
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    using (var cmdProviderAct = new WfAct(context.Host, ctx, ctx.Ontology.MessageProvider, "保存执行失败的命令"))
                                    {
                                        ctx.Ontology.MessageProvider.SaveCommand(ctx.Ontology, ctx.Command.ToExecuteFailing(context.Result));
                                    }
                                    context.Result.UpdateStatus(Status.AuditUnapproved, "审计未通过");
                                }
                                ctx.Ontology.MessageProvider.DeleteCommand(MessageTypeKind.LocalEvent, ctx.Ontology, localEvent.Id, context.Command.IsDumb);
                            }
                            #endregion
                        }
                        else
                        {
                            var distrubutedCommand = context.Ontology.MessageProvider.GetCommand(MessageTypeKind.Distributed,
                                context.Ontology, new Guid(context.Command.MessageId));
                            if (distrubutedCommand == null)
                            {
                                context.Result.UpdateStatus(Status.NotExist, "给定的MessageID标识的服务端命令不存在。服务端认为没有向您的节点分发过MessageID为" + context.Command.MessageId + "的命令");
                            }
                            else
                            {
                                using (var act = new WfAct(context.Host, context, context.Ontology.MessageProvider, context.Result.Description))
                                {
                                    context.Ontology.MessageProvider.SaveCommand(context.Ontology, context.Command.ToClientEvent(context.Result));
                                }
                                context.Result.UpdateStatus(Status.ReceiveOk, "接收成功");
                            }
                        }
                        break;
                        #endregion
                    }
                case EventSourceType.Entity:
                    {
                        #region 实体事件
                        context.Result.UpdateStatus(Status.Nonsupport, "暂不支持客户端实体事件");
                        break;
                        #endregion
                    }
                default:
                    context.Exception = new AnycmdException("意外的事件源类型");
                    throw context.Exception;
            }
        }
        #endregion

        #region GetTuple
        private static InfoItem[] GetTuple(MessageContext context)
        {
            if (context.Command.Verb == Verb.Create)
            {
                var items = new List<InfoItem>();
                var commandFactory = context.Host.NodeHost.MessageProducer;
                foreach (var node in context.Host.NodeHost.Nodes)
                {
                    if (commandFactory.IsProduce(context, node))
                    {
                        bool isCare = false;
                        foreach (var item in context.InfoTuplePair.ValueTuple)
                        {
                            if (node.IsCareforElement(item.Element))
                            {
                                isCare = true;
                                items.Add(item);
                            }
                        }
                        if (isCare)
                        {
                            foreach (var element in node.GetInfoIdElements())
                            {
                                items.Add(InfoItem.Create(element, string.Empty));
                            }
                        }
                    }
                }
                return items.ToArray();
            }
            return context.TowInfoTuple.SingleInfoTuple;
        }
        #endregion
        #endregion
    }
}
