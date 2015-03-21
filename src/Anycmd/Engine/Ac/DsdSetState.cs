
namespace Anycmd.Engine.Ac
{
    using Dsd;
    using Model;
    using System;

    /// <summary>
    /// 表示动态职责分离角色集业务实体。
    /// </summary>
    public sealed class DsdSetState : StateObject<DsdSetState>, IDsdSet
    {
        private string _name;
        private int _isEnabled;
        private int _dsdCard;
        private string _description;
        private DateTime? _createOn;

        private DsdSetState(Guid id) : base(id) { }

        public static DsdSetState Create(DsdSetBase dsdSet)
        {
            if (dsdSet == null)
            {
                throw new ArgumentNullException("dsdSet");
            }
            return new DsdSetState(dsdSet.Id)
            {
                _createOn = dsdSet.CreateOn
            }.InternalModify(dsdSet);
        }

        internal DsdSetState InternalModify(DsdSetBase dsdSet)
        {
            if (dsdSet == null)
            {
                throw new ArgumentNullException("dsdSet");
            }
            _name = dsdSet.Name;
            _isEnabled = dsdSet.IsEnabled;
            _dsdCard = dsdSet.DsdCard;
            _description = dsdSet.Description;

            return this;
        }

        public string Name
        {
            get { return _name; }
        }

        public int IsEnabled
        {
            get { return _isEnabled; }
        }

        public int DsdCard
        {
            get { return _dsdCard; }
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
    IsEnabled:{2},
    DsdCard:{3},
    Description:'{4}',
    CreateOn:'{5}'
}}", Id, Name, IsEnabled, DsdCard, Description, CreateOn);
        }

        protected override bool DoEquals(DsdSetState other)
        {
            return Id == other.Id &&
                Name == other.Name &&
                DsdCard == other.DsdCard &&
                IsEnabled == other.IsEnabled &&
                Description == other.Description;
        }
    }
}
