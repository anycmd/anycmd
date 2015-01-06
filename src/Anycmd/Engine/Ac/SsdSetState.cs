
namespace Anycmd.Engine.Ac
{
    using Abstractions;
    using System;

    /// <summary>
    /// 表示静态职责分离角色集业务实体。
    /// </summary>
    public class SsdSetState : StateObject<SsdSetState>, ISsdSet, IStateObject
    {
        private string _name;
        private int _ssdCard;
        private int _isEnabled;
        private string _description;
        private DateTime? _createOn;

        private SsdSetState(Guid id) : base(id) { }

        public static SsdSetState Create(SsdSetBase ssdSet)
        {
            return new SsdSetState(ssdSet.Id)
            {
                _name = ssdSet.Name,
                _isEnabled = ssdSet.IsEnabled,
                _ssdCard = ssdSet.SsdCard,
                _description = ssdSet.Description,
                _createOn = ssdSet.CreateOn
            };
        }

        public string Name
        {
            get { return _name; }
        }

        public int SsdCard
        {
            get { return _ssdCard; }
        }

        public int IsEnabled
        {
            get { return _isEnabled; }
        }

        public string Description
        {
            get { return _description; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        protected override bool DoEquals(SsdSetState other)
        {
            return Id == other.Id &&
                Name == other.Name &&
                SsdCard == other.SsdCard &&
                IsEnabled == other.IsEnabled &&
                Description == other.Description;
        }
    }
}
