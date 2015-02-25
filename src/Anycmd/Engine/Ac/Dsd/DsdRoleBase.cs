
namespace Anycmd.Engine.Ac.Dsd
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 动态职责分离角色基类。
    /// </summary>
    public abstract class DsdRoleBase : EntityBase, IDsdRole
    {
        private Guid _dsdSetId;
        private Guid _roleId;

        public Guid DsdSetId
        {
            get
            {
                return _dsdSetId;
            }
            set
            {
                if (value == Guid.Empty)
                {
                    throw new ValidationException("必须关联集合");
                }
                _dsdSetId = value;
            }
        }

        public Guid RoleId
        {
            get { return _roleId; }
            set
            {
                if (value == Guid.Empty)
                {
                    throw new ValidationException("必须指定角色");
                }
                _roleId = value;
            }
        }
    }
}
