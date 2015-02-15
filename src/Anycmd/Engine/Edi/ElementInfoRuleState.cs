
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Model;
    using System;

    public sealed class ElementInfoRuleState : StateObject<ElementInfoRuleState>, IElementInfoRule, IStateObject
    {
        private readonly IAcDomain _acDomain;

        private ElementInfoRuleState(IAcDomain acDomain, Guid id)
            : base(id)
        {
            this._acDomain = acDomain;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="elementInfoRule"></param>
        /// <returns></returns>
        public static ElementInfoRuleState Create(IAcDomain acDomain, IElementInfoRule elementInfoRule)
        {
            InfoRuleState infoRule;
            if (!acDomain.NodeHost.InfoRules.TryGetInfoRule(elementInfoRule.InfoRuleId, out infoRule))
            {
                throw new InvalidProgramException("请检测InfoRule的存在性");
            }
            return new ElementInfoRuleState(acDomain, elementInfoRule.Id)
            {
                CreateOn = elementInfoRule.CreateOn,
                ElementId = elementInfoRule.ElementId,
                InfoRuleId = elementInfoRule.InfoRuleId,
                IsEnabled = elementInfoRule.IsEnabled,
                SortCode = elementInfoRule.SortCode
            };
        }

        public Guid ElementId { get; private set; }

        public Guid InfoRuleId { get; private set; }

        public int IsEnabled { get; private set; }

        public int SortCode { get; private set; }

        public DateTime CreateOn { get; private set; }

        protected override bool DoEquals(ElementInfoRuleState other)
        {
            return
                Id == other.Id &&
                ElementId == other.ElementId &&
                InfoRuleId == other.InfoRuleId &&
                IsEnabled == other.IsEnabled &&
                SortCode == other.SortCode;
        }
    }
}
