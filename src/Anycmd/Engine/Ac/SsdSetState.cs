
namespace Anycmd.Engine.Ac
{
    using Model;
    using Ssd;
    using System;

    /// <summary>
    /// 表示静态职责分离角色集业务实体。
    /// </summary>
    public class SsdSetState : StateObject<SsdSetState>, ISsdSet
    {
        private string _name;
        private int _ssdCard;
        private int _isEnabled;
        private string _description;
        private DateTime? _createOn;

        private SsdSetState(Guid id) : base(id) { }

        public static SsdSetState Create(SsdSetBase ssdSet)
        {
            if (ssdSet == null)
            {
                throw new ArgumentNullException("ssdSet");
            }
            return new SsdSetState(ssdSet.Id)
            {
                _createOn = ssdSet.CreateOn
            }.InternalModify(ssdSet);
        }

        internal SsdSetState InternalModify(SsdSetBase ssdSet)
        {
            if (ssdSet == null)
            {
                throw new ArgumentNullException("ssdSet");
            }
            _name = ssdSet.Name;
            _isEnabled = ssdSet.IsEnabled;
            _ssdCard = ssdSet.SsdCard;
            _description = ssdSet.Description;

            return this;
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

        public override string ToString()
        {
            return string.Format(
@"{{
    Id:'{0}',
    Name:'{1}',
    SsdCard:{2},
    IsEnabled:{3},
    Description:'{4}',
    CreateOn:'{5}'
}}", Id, Name, SsdCard, IsEnabled, Description, CreateOn);
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
