
namespace Anycmd.Engine.Edi
{
    using Info;
    using Model;
    using System;

    public sealed class InfoRuleState : StateObject<InfoRuleState>, IStateObject
    {
        private InfoRuleState(Guid id) : base(id) { }

        public static InfoRuleState Create(InfoRuleEntityBase entity, IInfoRule infoRule)
        {
            return new InfoRuleState(entity.Id)
            {
                CreateOn = entity.CreateOn,
                IsEnabled = entity.IsEnabled,
                InfoRule = infoRule
            };
        }

        public int IsEnabled { get; private set; }

        public DateTime? CreateOn { get; private set; }

        public IInfoRule InfoRule { get; private set; }

        protected override bool DoEquals(InfoRuleState other)
        {
            return Id == other.Id &&
                IsEnabled == other.IsEnabled;
        }
    }
}
