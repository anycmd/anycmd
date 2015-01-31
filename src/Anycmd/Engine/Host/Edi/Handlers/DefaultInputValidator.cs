
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Engine.Ac;
    using Engine.Edi;
    using Hecp;
    using Info;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Util;

    /// <summary>
    /// 命名输入验证器默认实现。
    /// </summary>
    public class DefaultInputValidator : IInputValidator
    {
        public DefaultInputValidator()
        {
        }

        public ProcessResult Validate(MessageContext context)
        {
            if (context == null)
            {
                return new ProcessResult(false, Status.NoneCommand, "空命令");
            }
            MessageType type = context.Command.MessageType;
            #region 非法的表述类型
            if (type == MessageType.Undefined
                || type == default(MessageType))
            {
                return new ProcessResult(false, Status.InvalidMessageType, "非法的表述类型");
            }
            #endregion
            #region 验证事件
            if (type == MessageType.Event)
            {
                #region 非法的事件源类型
                EventSourceType eventSourceType;
                if (!context.Command.EventSourceType.TryParse(out eventSourceType)
                    || eventSourceType == EventSourceType.Invalid
                    || eventSourceType == default(EventSourceType))
                {
                    return new ProcessResult(false, Status.InvalidEventSourceType, "非法的事件源类型");
                }
                #endregion
                #region 非法的事件主题码
                if (!EventSubjectCode.Contains(context.Command.EventSubjectCode))
                {
                    return new ProcessResult(false, Status.InvalidEventSubject, "非法的事件主题码");
                }
                #endregion
                #region 非法的状态码或原因短语
                Status outStateCode;
                if (!context.Command.Status.TryParse(out outStateCode))
                {
                    return new ProcessResult(false, Status.InvalidStatus, "非法的状态码");
                }
                if (!string.IsNullOrEmpty(context.Command.ReasonPhrase)
                    && context.Command.ReasonPhrase.Length > 50)
                {
                    return new ProcessResult(false, Status.OutOfLength, "原因短语超过50个字符长度");
                }
                #endregion
                #region 本节点分发的命令的请求标识是Guid型的

                if (eventSourceType == EventSourceType.Command)
                {
                    Guid requestId;
                    if (!Guid.TryParse(context.Command.MessageId, out requestId))
                    {
                        return new ProcessResult(false, Status.InvalidArgument, @"参数MessageID错误：服务端的请求标识是Guid类型的。当EventSourceType取值Command时该MessageID是服务端分发该条命令时使用的MessageId");
                    }
                }

                #endregion
            }
            #endregion
            #region 发起人验证
            if (!string.IsNullOrEmpty(context.Command.UserName) && context.Command.UserName.Length > 50)
            {
                return new ProcessResult(false, Status.OutOfLength, "发起人超过最大50个字符长度。一条命令至多由一个人发起，这个人就是责任人，不存在联名发起命令一说。");
            }
            #endregion
            #region 时间戳验证
            if (context.Command.TimeStamp < SystemTime.Now().AddYears(-1) // TODO:配置消息的左时间
                || context.Command.TimeStamp > SystemTime.Now().AddSeconds(context.Host.Config.TicksTimeout))
            {
                return new ProcessResult(false, Status.InvalidCommandTicks, "非法的命令时间戳，命令时间戳不能是一年前或将来。");
            }
            #endregion
            #region 客户端类型验证
            if (context.Command.ClientType == ClientType.Undefined)
            {
                return new ProcessResult(false, Status.InvalidClientType, "非法的客户端类型");
            }
            #endregion
            #region 节点验证
            NodeDescriptor requestNode = null;
            if (context.Command.ClientType == ClientType.Node)
            {
                if (!context.Host.NodeHost.Nodes.TryGetNodeById(context.Command.ClientId, out requestNode))
                {
                    return new ProcessResult(false, Status.InvalidClientId, "非法的节点");
                }
            }
            #endregion
            #region 本体码验证
            if (string.IsNullOrEmpty(context.Command.Ontology))
            {
                return new ProcessResult(false, Status.InvalidOntology, "必须通过本体码界定命令的上下文，本体码不能为空。");
            }
            OntologyDescriptor ontology;
            if (!context.Host.NodeHost.Ontologies.TryGetOntology(context.Command.Ontology, out ontology))
            {
                return new ProcessResult(false, Status.InvalidOntology, "非法的本体码。本体列表中不存在编码为" + context.Ontology + "的本体码");
            }
            if (ontology.Ontology.IsEnabled != 1)
            {
                return new ProcessResult(false, Status.InvalidOntology, "该本体已被禁用");
            }
            #endregion
            #region 动作码验证
            if (string.IsNullOrEmpty(context.Command.Verb.Code))
            {
                return new ProcessResult(false, Status.InvalidVerb, "必须通过本体动作码来表明您的命令是要做什么，本体动作码不能为空。");
            }
            if (!ontology.Actions.ContainsKey(context.Command.Verb))
            {
                return new ProcessResult(false, Status.InvalidVerb, "非法的动作码，" + ontology.Ontology.Name + "未定义编码为" + context.Command.Verb + "的动作");
            }
            if (context.Command.ClientType == ClientType.Node)
            {
                var nodeActions = requestNode.Node.NodeActions;
                if (!nodeActions.ContainsKey(ontology) || !nodeActions[ontology].ContainsKey(context.Command.Verb))
                {
                    return new ProcessResult(false, Status.NoPermission, "您的节点没有" + ontology.Actions[context.Command.Verb].Name + ontology.Ontology.Name + "的权限");
                }
            }
            #endregion
            #region 本体元素码验证器
            if (context.Command.DataTuple.IdItems == null || context.Command.DataTuple.IdItems.IsEmpty)
            {
                return new ProcessResult(false, Status.InvalidInfoId, "信息标识不能为空");
            }
            var elementDic = ontology.Elements;
            var failDescription = new List<DataItem>();
            var infoIdItems = new List<InfoItem>();
            if (context.Command.DataTuple.IdItems != null)
            {
                foreach (var item in context.Command.DataTuple.IdItems.Items)
                {
                    if (item.Key == null || !elementDic.ContainsKey(item.Key) || elementDic[item.Key].Element.IsEnabled != 1)
                    {
                        failDescription.Add(item);
                    }
                    else
                    {
                        infoIdItems.Add(InfoItem.Create(elementDic[item.Key], item.Value));
                    }
                }
            }
            var infoValueItems = new List<InfoItem>();
            if (context.Command.DataTuple.ValueItems != null)
            {
                foreach (var item in context.Command.DataTuple.ValueItems.Items)
                {
                    if (item.Key != null && !elementDic.ContainsKey(item.Key))
                    {
                        failDescription.Add(item);
                    }
                    else
                    {
                        infoValueItems.Add(InfoItem.Create(elementDic[item.Key], item.Value));
                    }
                }
            }
            if (failDescription.Count > 0)
            {
                return new ProcessResult(false, Status.InvalidElement, "非法的本体元素码" + failDescription[0].Key);
            }
            #endregion
            #region 信息标识验证
            if (context.Command.DataTuple.IdItems.IsEmpty)
            {
                return new ProcessResult(false, Status.InvalidInfoId, "非法的信息标识");
            }
            else if (ontology.Ontology.IsCataloguedEntity)
            {
                if (Verb.Create.Equals(context.Command.Verb)
                    && type != MessageType.Event)
                {
                    if (requestNode != context.Host.NodeHost.Nodes.CenterNode
                        && context.Command.DataTuple.IdItems.Items.Any(a => string.Equals(a.Key, "Id", StringComparison.OrdinalIgnoreCase)))
                    {
                        return new ProcessResult(false, Status.InvalidInfoId, "非中心节点的create型命令不能提供Id");
                    }
                    else if ((ontology.Ontology.IsCataloguedEntity &&
                        !context.Command.DataTuple.IdItems.Items.Any(a => string.Equals(a.Key, "ZZJGM", StringComparison.OrdinalIgnoreCase)))
                        || !context.Command.DataTuple.IdItems.Items.Any(a => string.Equals(a.Key, "XM", StringComparison.OrdinalIgnoreCase)))
                    {
                        return new ProcessResult(false, Status.InvalidInfoId, "没有提供姓名或目录");
                    }
                }
            }
            #endregion
            #region 信息标识项验证
            ProcessResult result;
            foreach (var infoItem in infoIdItems)
            {
                if (!this.ValidInfoItem(infoItem, out result))
                {
                    return result;
                }
            }
            #endregion
            #region 信息值项验证
            if (Verb.Create.Equals(context.Command.Verb)
                || Verb.Update.Equals(context.Command.Verb))
            {
                if (context.Command.DataTuple.ValueItems.IsEmpty)
                {
                    return new ProcessResult(false, Status.InvalidInfoValue, "Create和Update型动作必须提供InfoValue输入");
                }
                foreach (var infoItem in infoValueItems)
                {
                    if (!this.ValidInfoItem(infoItem, out result))
                    {
                        return result;
                    }
                }
            }
            #endregion

            return new ProcessResult(true, Status.Ok, "命令输入验证通过");
        }

        #region ValidInfoItem
        private bool ValidInfoItem(InfoItem infoItem, out ProcessResult result)
        {
            #region 空值验证
            if (infoItem.Element.Element != null
                && infoItem.Element.Element.IsInput
                && infoItem.Element.DataSchema != null
                && !infoItem.Element.DataSchema.IsNullable)
            {
                if (string.IsNullOrEmpty(infoItem.Value))
                {
                    result = new ProcessResult(false, Status.InvalidInfoValue, "不能为空的值：" + infoItem.Value);
                    return false;
                }
            }
            #endregion
            #region 值长度验证
            if (infoItem.Element.Element.MaxLength.HasValue && infoItem.Element.Element.MaxLength > 0)
            {
                if (infoItem.Value != null && infoItem.Value.Length > infoItem.Element.Element.MaxLength.Value)
                {
                    result = new ProcessResult(false, Status.OutOfLength, "超过长度限制的值：" + infoItem.Value);
                    return false;
                }
            }
            #endregion
            #region 字典验证
            if (infoItem.Element.Element.InfoDicId.HasValue)
            {
                InfoDicState infoDic;
                if (!infoItem.Element.Host.NodeHost.InfoDics.TryGetInfoDic(infoItem.Element.Element.InfoDicId.Value, out infoDic))
                {
                    result = new ProcessResult(false, Status.InternalServerError, infoItem.Element.Element.Name + "本体元素信息字典配置错误，非法的信息字典标识：" + infoItem.Element.Element.InfoDicId.Value);
                    return false;
                }
                InfoDicItemState infoDicItem;
                if (!infoItem.Element.Host.NodeHost.InfoDics.TryGetInfoDicItem(infoDic, infoItem.Value, out infoDicItem) || infoDicItem.IsEnabled != 1)
                {
                    result = new ProcessResult(false, Status.InvalidDicItemValue, "非法的" + infoItem.Element.Element.Name + "字典值：" + infoItem.Value);
                    return false;
                }
            }
            #endregion
            #region 正则验证
            if (!string.IsNullOrEmpty(infoItem.Value) && !string.IsNullOrEmpty(infoItem.Element.Element.Regex))
            {
                if (!Regex.IsMatch(infoItem.Value, infoItem.Element.Element.Regex))
                {
                    result = new ProcessResult(false, Status.InvalidInfoValue, "非法格式的" + infoItem.Element.Element.Name + "值:" + infoItem.Value);
                    return false;
                }
            }
            #endregion
            // 如果是目录
            if (infoItem.Element.Ontology.Ontology.IsCataloguedEntity && infoItem.Key.Equals("ZZJGM", StringComparison.OrdinalIgnoreCase))
            {
                CatalogState org;
                if (!this.ValidCatalogCode(infoItem.Element.Ontology, infoItem.Value, out org, out result))
                {
                    return false;
                }
            }

            // 应用信息验证器
            foreach (var infoRule in infoItem.Element.Element.GetInfoRules())
            {
                var r = infoRule.InfoRule.Valid(infoItem.Value);
                if (r.IsSuccess) continue;
                result = new ProcessResult(false, r.StateCode, r.Description);
                if (r.Exception != null)
                {
                    infoItem.Element.Host.LoggingService.Error(r.Exception);
                }
                return false;
            }
            result = ProcessResult.Ok;
            return true;
        }
        #endregion

        #region ValidCatalogCode
        private bool ValidCatalogCode(OntologyDescriptor ontology, string catalogCode, out CatalogState org, out ProcessResult result)
        {
            if (string.IsNullOrEmpty(catalogCode))
            {
                org = CatalogState.Empty;
                result = ProcessResult.Ok;
                return false;
            }
            if (!ontology.Host.CatalogSet.TryGetCatalog(catalogCode.Trim(), out org))
            {
                result = new ProcessResult(false, Status.InvalidCatalog, string.Format("非法的目录码{0}", catalogCode));
                return false;
            }
            OntologyCatalogState oorg;
            if (!ontology.Catalogs.TryGetValue(org, out oorg))
            {
                result = new ProcessResult(false, Status.InvalidCatalog, string.Format("对于{0}来说{1}是非法的目录码", ontology.Ontology.Name, org.Code));
                return false;
            }
            var orgCode = org.Code;
            if (ontology.Host.CatalogSet.Any(o => orgCode.Equals(o.ParentCode, StringComparison.OrdinalIgnoreCase)))
            {
                result = new ProcessResult(false, Status.InvalidCatalog, string.Format("{0}不是叶节点，不能容纳" + ontology.Ontology.Name, org.Name));
                return false;
            }
            result = ProcessResult.Ok;
            return true;
        }
        #endregion
    }
}
