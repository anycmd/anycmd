
namespace Anycmd.Engine.Ac
{
    using Abstractions;
    using System;

    /// <summary>
    /// 表示静态职责分离角色业务实体。
    /// </summary>
    public sealed class SsdRoleState : StateObject<SsdRoleState>, ISsdRole, IStateObject
    {
        private Guid _ssdSetId;
        private Guid _roleId;
        private DateTime? _createOn;

        private SsdRoleState(Guid id) : base(id) { }

        public static SsdRoleState Create(SsdRoleBase ssdRole)
        {
            return new SsdRoleState(ssdRole.Id)
            {
                _roleId = ssdRole.RoleId,
                _ssdSetId = ssdRole.SsdSetId,
                _createOn = ssdRole.CreateOn
            };
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
