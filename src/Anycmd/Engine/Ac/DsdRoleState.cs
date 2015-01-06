
namespace Anycmd.Engine.Ac
{
    using Abstractions;
    using System;

    /// <summary>
    /// 表示东塔职责分离角色业务实体。
    /// </summary>
    public sealed class DsdRoleState : StateObject<DsdRoleState>, IDsdRole
    {
        private Guid _dsdSetId;
        private Guid _roleId;
        private DateTime? _createOn;

        private DsdRoleState(Guid id) : base(id) { }

        public static DsdRoleState Create(DsdRoleBase dsdRole)
        {
            return new DsdRoleState(dsdRole.Id)
            {
                _roleId = dsdRole.RoleId,
                _dsdSetId = dsdRole.DsdSetId,
                _createOn = dsdRole.CreateOn
            };
        }

        public Guid DsdSetId
        {
            get { return _dsdSetId; }
        }

        public Guid RoleId
        {
            get { return _roleId; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        protected override bool DoEquals(DsdRoleState other)
        {
            return Id == other.Id &&
                DsdSetId == other.DsdSetId &&
                RoleId == other.RoleId;
        }
    }
}
