
namespace Anycmd.Engine.Ac
{
    using Model;
    using Ssd;
    using System;

    /// <summary>
    /// 表示静态职责分离角色业务实体。
    /// </summary>
    public sealed class SsdRoleState : StateObject<SsdRoleState>, ISsdRole
    {
        private Guid _ssdSetId;
        private Guid _roleId;
        private DateTime? _createOn;

        private SsdRoleState(Guid id) : base(id) { }

        public static SsdRoleState Create(SsdRoleBase ssdRole)
        {
            if (ssdRole == null)
            {
                throw new ArgumentNullException("ssdRole");
            }
            return new SsdRoleState(ssdRole.Id)
            {
                _createOn = ssdRole.CreateOn
            }.InternalModify(ssdRole);
        }

        internal SsdRoleState InternalModify(SsdRoleBase ssdRole)
        {
            if (ssdRole == null)
            {
                throw new ArgumentNullException("ssdRole");
            }
            _roleId = ssdRole.RoleId;
            _ssdSetId = ssdRole.SsdSetId;

            return this;
        }

        public Guid SsdSetId
        {
            get { return _ssdSetId; }
        }

        public Guid RoleId
        {
            get { return _roleId; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        protected override bool DoEquals(SsdRoleState other)
        {
            return Id == other.Id &&
                SsdSetId == other.SsdSetId &&
                RoleId == other.RoleId;
        }
    }
}
