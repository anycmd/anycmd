
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Ac;
    using Engine.Edi;
    using Engine.Edi.Abstractions;
    using Exceptions;
    using Hecp;
    using Info;
    using Util;

    /// <summary>
    /// 默认实现的命令权限验证器。
    /// </summary>
    public class DefaultPermissionValidator : IPermissionValidator
    {
        /// <summary>
        /// 验证Level1、Level2、Level3、Level4级权限。
        /// <remarks>
        /// 其它级权限不要在此验证。
        /// </remarks>
        /// </summary>
        /// <param name="context">命令上下文</param>
        /// <returns></returns>
        public ProcessResult Validate(MessageContext context)
        {
            // Level1Action
            #region 验证动作级权限
            ActionState action;
            if (!context.Ontology.Actions.TryGetValue(context.Command.Verb, out action))
            {
                throw new AnycmdException("非法的动作类型");
            }
            switch (action.AllowType)
            {
                case AllowType.Invalid:
                    return new ProcessResult(false, Status.NoPermission, "意外的AllowType:Invalid");
                case AllowType.ExplicitAllow:
                    return ProcessResult.Ok;
                case AllowType.ImplicitAllow:
                    break;
                case AllowType.NotAllow:
                    return new ProcessResult(false, Status.NoPermission, "已禁止" + context.Ontology.Actions[context.Command.Verb].Name + context.Ontology.Ontology.Name);
                default:
                    return new ProcessResult(false, Status.NoPermission, "意外的AllowType:" + action.AllowType.ToName());
            }
            #endregion
            // Level2ElementAction
            #region 验证本体元素级权限
            foreach (var infoIdItem in context.InfoTuplePair.IdTuple)
            {
                var elementActions = infoIdItem.Element.Element.ElementActions;
                IElement element = infoIdItem.Element.Element;
                bool getConfiged = elementActions.ContainsKey(Verb.Get);
                bool headConfiged = elementActions.ContainsKey(Verb.Head);
                if (!getConfiged && !headConfiged)
                {
                    return new ProcessResult(false, Status.NoPermission, element.Name + "无法进入InfoID项，因为它既不可get又不可head");
                }
                if ((getConfiged && elementActions[Verb.Get].AllowType == AllowType.NotAllow)
                    && (headConfiged && elementActions[Verb.Head].AllowType == AllowType.NotAllow))
                {
                    return new ProcessResult(false, Status.NoPermission, element.Name + "无法进入InfoID项，因为已禁止get和head" + element.Name);
                }
            }
            bool isUpdate = Verb.Update.Equals(context.Command.Verb);
            foreach (InfoItem infoValueItem in context.InfoTuplePair.ValueTuple)
            {
                var elementActions = infoValueItem.Element.Element.ElementActions;
                IElement element = infoValueItem.Element.Element;
                if (!elementActions.ContainsKey(context.Command.Verb))
                {
                    return new ProcessResult(false, Status.NoPermission, "已禁止" + context.Ontology.Actions[context.Command.Verb].Name + element.Name);
                }
                switch (elementActions[context.Command.Verb].AllowType)
                {
                    case AllowType.Invalid:
                        return new ProcessResult(false, Status.NoPermission, "意外的AllowType:Invalid");
                    case AllowType.ExplicitAllow:
                        return ProcessResult.Ok;
                    case AllowType.ImplicitAllow:
                        break;
                    case AllowType.NotAllow:
                        return new ProcessResult(false, Status.NoPermission, "已禁止" + context.Ontology.Actions[context.Command.Verb].Name + element.Name);
                    default:
                        return new ProcessResult(false, Status.NoPermission, "意外的AllowType:Invalid");
                }
                if (isUpdate && string.IsNullOrWhiteSpace(infoValueItem.Value))
                {
                    if (!elementActions.ContainsKey(Verb.Delete))
                    {
                        return new ProcessResult(false, Status.NoPermission, "已禁止" + context.Ontology.Actions[Verb.Delete].Name + element.Name);
                    }
                    switch (elementActions[Verb.Delete].AllowType)
                    {
                        case AllowType.Invalid:
                            return new ProcessResult(false, Status.NoPermission, "意外的AllowType:Invalid");
                        case AllowType.ExplicitAllow:
                            return ProcessResult.Ok;
                        case AllowType.ImplicitAllow:
                            break;
                        case AllowType.NotAllow:
                            return new ProcessResult(false, Status.NoPermission, "已禁止" + context.Ontology.Actions[Verb.Delete].Name + element.Name);
                        default:
                            return new ProcessResult(false, Status.NoPermission, "意外的AllowType:Invalid");
                    }
                }
            }
            #endregion
            // Level3ClientAction
            #region 验证节点动作级级权限
            var allowType = context.ClientAgent.GetOntologyPermission(context.Ontology, context.Command.Verb);
            switch (allowType)
            {
                case AllowType.Invalid:
                    return new ProcessResult(false, Status.NoPermission, "意外的AllowType:Invalid");
                case AllowType.ExplicitAllow:
                    return ProcessResult.Ok;
                case AllowType.ImplicitAllow:
                    break;
                case AllowType.NotAllow:
                    return new ProcessResult(false, Status.NoPermission, "已禁止" + context.ClientAgent.Name + context.Ontology.Actions[Verb.Delete].Name + context.Ontology.Ontology.Name);
                default:
                    return new ProcessResult(false, Status.NoPermission, "意外的AllowType:Invalid");
            }
            #endregion
            // Level4ClientElementAction
            #region 节点动作权限验证
            foreach (var infoIdItem in context.InfoTuplePair.IdTuple)
            {
                if (context.ClientAgent.GetElementPermission(infoIdItem.Element, Verb.Get) == AllowType.NotAllow
                    && context.ClientAgent.GetElementPermission(infoIdItem.Element, Verb.Head) == AllowType.NotAllow)
                {
                    return new ProcessResult(false, Status.NoPermission, string.Format("{0}不能进入InfoID项，因为来源节点既没有get{0}的权限又没有head{0}的权限", infoIdItem.Element.Element.Name));
                }
            }
            foreach (InfoItem infoValueItem in context.InfoTuplePair.ValueTuple)
            {
                IElement element = infoValueItem.Element.Element;
                if (context.ClientAgent.GetElementPermission(infoValueItem.Element, context.Command.Verb) == AllowType.NotAllow)
                {
                    return new ProcessResult(false, Status.NoPermission, string.Format("客户端'" + context.ClientAgent.Name + "'没有{0}{1}的权限", context.Ontology.Actions[context.Command.Verb].Name, element.Name));
                }
                if (isUpdate && string.IsNullOrWhiteSpace(infoValueItem.Value))
                {
                    if (context.ClientAgent.GetElementPermission(infoValueItem.Element, Verb.Delete) == AllowType.NotAllow)
                    {
                        return new ProcessResult(false, Status.NoPermission, string.Format("客户端'" + context.ClientAgent.Name + "'没有{0}{1}的权限", context.Ontology.Actions[Verb.Delete].Name, element.Name));
                    }
                }
            }
            #endregion

            return ProcessResult.Ok;
        }
    }
}
