
namespace Anycmd.Engine.Ac
{
    using Abstractions.Infra;
    using Exceptions;
    using System;
    using Util;

    /// <summary>
    /// 表示系统字典项业务实体。
    /// </summary>
    public sealed class DicItemState : StateObject<DicItemState>, IDicItem
    {
        public static readonly DicItemState Empty = new DicItemState(Guid.Empty)
        {
            _createOn = SystemTime.MinDate,
            _isEnabled = 0,
            _sortCode = 0,
            _code = string.Empty,
            _dicId = Guid.Empty,
            _name = string.Empty
        };

        private string _code;
        private string _name;
        private Guid _dicId;
        private int _sortCode;
        private int _isEnabled;
        private DateTime? _createOn;

        private DicItemState(Guid id) : base(id) { }

        public static DicItemState Create(IAcDomain acDomain, DicItemBase dicItem)
        {
            if (dicItem == null)
            {
                throw new ArgumentNullException("dicItem");
            }
            if (!acDomain.DicSet.ContainsDic(dicItem.DicId))
            {
                throw new AnycmdException("意外的字典" + dicItem.DicId);
            }
            return new DicItemState(dicItem.Id)
            {
                _code = dicItem.Code,
                _name = dicItem.Name,
                _dicId = dicItem.DicId,
                _sortCode = dicItem.SortCode,
                _isEnabled = dicItem.IsEnabled,
                _createOn = dicItem.CreateOn
            };
        }

        public string Code
        {
            get { return _code; }
        }

        public string Name
        {
            get { return _name; }
        }

        public Guid DicId
        {
            get { return _dicId; }
        }

        public int SortCode
        {
            get { return _sortCode; }
        }

        public int IsEnabled
        {
            get { return _isEnabled; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        public override string ToString()
        {
            return string.Format(
@"{{
    Id:'{0}',
    Code:'{1}',
    Name:'{2}',
    DicId:'{3}',
    SortCode:{4},
    IsEnabled:{5},
    CreateOn:'{6}'
}}", Id, Code, Name, DicId, SortCode, IsEnabled, CreateOn);
        }

        protected override bool DoEquals(DicItemState other)
        {
            return Id == other.Id &&
                Code == other.Code &&
                Name == other.Name &&
                DicId == other.DicId &&
                SortCode == other.SortCode &&
                IsEnabled == other.IsEnabled;
        }
    }
}
