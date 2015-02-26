
namespace Anycmd.Engine.Ac.Ssd
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 静态职责分离角色基类。
    /// </summary>
    public abstract class SsdRoleBase : EntityBase, ISsdRole
    {
        private Guid _ssdSetId;
        private Guid _roleId;

        public Guid SsdSetId
        {
            get
            {
                return _ssdSetId;
            }
            set
            {
                if (value == Guid.Empty)
                {
                    throw new ValidationException("必须关联集合");
                }
                _ssdSetId = value;
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
