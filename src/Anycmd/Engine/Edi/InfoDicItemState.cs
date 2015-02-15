
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Model;
    using System;

    public sealed class InfoDicItemState : StateObject<InfoDicItemState>, IInfoDicItem, IStateObject
    {
        private InfoDicItemState(Guid id) : base(id) { }

        public static InfoDicItemState Create(IInfoDicItem infoDicItem)
        {
            if (infoDicItem == null)
            {
                throw new ArgumentNullException("infoDicItem");
            }
            return new InfoDicItemState(infoDicItem.Id)
            {
                Code = infoDicItem.Code,
                CreateOn = infoDicItem.CreateOn,
                Description = infoDicItem.Description,
                InfoDicId = infoDicItem.InfoDicId,
                IsEnabled = infoDicItem.IsEnabled,
                Level = infoDicItem.Level,
                ModifiedOn = infoDicItem.ModifiedOn,
                Name = infoDicItem.Name,
                SortCode = infoDicItem.SortCode
            };
        }

        public string Level { get; private set; }

        public string Code { get; private set; }

        public int IsEnabled { get; private set; }

        public Guid InfoDicId { get; private set; }

        public string Name { get; private set; }

        public int SortCode { get; private set; }

        public DateTime? CreateOn { get; private set; }

        public DateTime? ModifiedOn { get; private set; }

        public string Description { get; private set; }

        protected override bool DoEquals(InfoDicItemState other)
        {
            return
                Id == other.Id &&
                Level == other.Level &&
                Code == other.Code &&
                IsEnabled == other.IsEnabled &&
                InfoDicId == other.InfoDicId &&
                SortCode == other.SortCode &&
                Name == other.Name;
        }
    }
}
