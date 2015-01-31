
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Engine.Ac;
    using Engine.Edi;
    using Exceptions;
    using Hecp;
    using Host.Ac;
    using Host.Ac.Infra;
    using System;
    using System.Linq;
    using Util;

    /// <summary>
    /// 命令审核鉴别器。
    /// </summary>
    public class DefaultAuditDiscriminator : IAuditDiscriminator
    {
        public DefaultAuditDiscriminator()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public DiscriminateResult IsNeedAudit(MessageContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var requestType = context.Command.MessageType;
            // 如果是事件则不需要审核
            if (requestType == MessageType.Event || requestType == MessageType.Undefined)
            {
                return DiscriminateResult.No;
            }
            // 如果未验证通过则不需要审核
            if (context.Ontology == null)
            {
                return DiscriminateResult.No;
            }
            // Level1Action 如果本体动作不需要审核则不审核
            #region Level1Action
            ActionState action;
            if (!context.Ontology.Actions.TryGetValue(context.Command.Verb, out action))
            {
                throw new AnycmdException("非法的动作类型");
            }
            var auditType = action.AuditType;
            switch (auditType)
            {
                case AuditType.Invalid:
                    return new DiscriminateResult(false, "意外的审核类型Invalid");
                case AuditType.ExplicitAudit:
                    return DiscriminateResult.Yes;
                case AuditType.ImplicitAudit:
                    break;
                case AuditType.NotAudit:
                    return DiscriminateResult.No;
                default:
                    return new DiscriminateResult(false, "意外的审核类型" + auditType.ToName());
            }
            #endregion
            // Level2ElementAction 如果所有本体元素都不需要审核则不审核
            #region Level2ElementAction
            if (context.InfoTuplePair == null)
            {
                return DiscriminateResult.No;
            }
            var isAudit = false;
            foreach (var elementActionDic in context.InfoTuplePair.ValueTuple.Select(a => a.Element.Element.ElementActions))
            {
                if (elementActionDic.ContainsKey(context.Command.Verb))
                {
                    continue;
                }
                var elementAction = elementActionDic[context.Command.Verb];
                switch (elementAction.AuditType)
                {
                    case AuditType.Invalid:
                        return new DiscriminateResult(false, "意外的审核类型Invalid");
                    case AuditType.ExplicitAudit:
                        isAudit = true;
                        return DiscriminateResult.Yes;
                    case AuditType.ImplicitAudit:
                        break;
                    case AuditType.NotAudit:
                        break;
                    default:
                        return new DiscriminateResult(false, "意外的审核类型" + auditType.ToName());
                }
            }
            if (isAudit)
            {
                return DiscriminateResult.Yes;
            }
            #endregion
            // Level3ClientAction 如果来源节点的这个本体动作不需要审核则不审核
            #region Level3ClientAction
            if (context.ClientAgent.GetOntologyAudit(context.Ontology, context.Command.Verb) == AuditType.NotAudit)
            {
                return DiscriminateResult.No;
            }
            #endregion
            // Level4ClientElementAction 如果来自来源节点的当前命令的所有涉及元素不需要审核则不审核
            #region Level4ClientElementAction
            var isA = false;
            ElementDescriptor auditElement = null;
            foreach (var valueItem in context.InfoTuplePair.ValueTuple)
            {
                isA = context.ClientAgent.GetElementAudit(valueItem.Element, context.Command.Verb) != AuditType.NotAudit;
                if (isA)
                {
                    auditElement = valueItem.Element;
                    break;
                }
            }
            if (!isA)
            {
                return DiscriminateResult.No;
            }
            #endregion
            if (context.Ontology.Ontology.IsCataloguedEntity)
            {
                // Level5CatalogAction 如果是目录型本体且当前实体所属的目录的这个本体动作不需要审核则不审核
                #region Level5CatalogAction
                CatalogState org;
                if (!context.Host.CatalogSet.TryGetCatalog(context.CatalogCode, out org))
                {
                    throw new AnycmdException("非法的目录码" + context.CatalogCode);
                }
                OntologyCatalogState ontologyOrg;
                if (!context.Ontology.Catalogs.TryGetValue(org, out ontologyOrg))
                {
                    context.Exception = new AnycmdException("非法的目录码。非法的目录码的命令应该未验证通过，不应该走到这一步");
                    throw context.Exception;
                }
                var orgActions = ontologyOrg.CatalogActions;
                if (!orgActions.ContainsKey(context.Command.Verb) || orgActions[context.Command.Verb].AuditType == AuditType.NotAudit)
                {
                    return DiscriminateResult.No;
                }
                else
                {
                    string msg = string.Empty;
                    if (auditElement != null && !auditElement.Element.Code.Equals("ZZJGM", StringComparison.OrdinalIgnoreCase))
                    {
                        msg = "在" + org.Name + "下" + action.Name + context.Ontology.Ontology.Name + auditElement.Element.Name + "需要审核。审核消息已发出，请等待管理员处理";
                    }
                    else
                    {
                        msg = "在" + org.Name + "下" + action.Name + context.Ontology.Ontology.Name + "需要审核。审核消息已发出，请等待管理员处理";
                    }
                    return new DiscriminateResult(true, msg);
                }
                #endregion

                // Level6EntityAction

                // Level7EntityElementAction
            }
            return new DiscriminateResult(true, "需要审核");
        }
    }
}
