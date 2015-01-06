
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using System;

    public sealed class ElementInfoRuleState : StateObject<ElementInfoRuleState>, IElementInfoRule, IStateObject
    {
        private readonly IAcDomain _host;

        private ElementInfoRuleState(IAcDomain host, Guid id)
            : base(id)
        {
            this._host = host;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="elementInfoRule"></param>
        /// <returns></returns>
        public static ElementInfoRuleState Create(IAcDomain host, IElementInfoRule elementInfoRule)
        {
            InfoRuleState infoRule;
            if (!host.NodeHost.InfoRules.TryGetInfoRule(elementInfoRule.InfoRuleId, out infoRule))
            {
                throw new InvalidProgramException("请检测InfoRule的存在性");
            }
            return new ElementInfoRuleState(host, elementInfoRule.Id)
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
