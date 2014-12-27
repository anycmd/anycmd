
namespace Anycmd.Engine.Ac.Abstractions
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 权限二元组基类。
    /// </summary>
    public abstract class PrivilegeBigramBase : EntityBase, IPrivilegeBigram
    {
        private string _subjectType;
        private Guid _subjectInstanceId;
        private string _objectType;
        private Guid _objectInstanceId;

        public string SubjectType {
            get { return _subjectType; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new AnycmdException("必须指定主授权类型");
                }
                if (value != _subjectType && _subjectType != null)
                {
                    throw new AnycmdException("主授权类型不能更改");
                }
                _subjectType = value;
            }
        }

        public Guid SubjectInstanceId
        {
            get { return _subjectInstanceId; }
            set
            {
                if (value == Guid.Empty)
                {
                    throw new AnycmdException("必须指定主授权类型");
                }
                if (value != _subjectInstanceId && _subjectInstanceId != Guid.Empty)
                {
                    throw new AnycmdException("主授权类型不能更改");
                }
                _subjectInstanceId = value;
            }
        }

        public string ObjectType
        {
            get { return _objectType; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new AnycmdException("必须指定从授权类型");
                }
                if (value != _objectType && _objectType != null)
                {
                    throw new AnycmdException("从授权类型不能更改");
                }
                _objectType = value;
            }
        }

        public Guid ObjectInstanceId
        {
            get { return _objectInstanceId; }
            set
            {
                if (value == Guid.Empty)
                {
                    throw new AnycmdException("必须指定从授权类型");
                }
                if (value != _objectInstanceId && _objectInstanceId != Guid.Empty)
                {
                    throw new AnycmdException("从授权类型不能更改");
                }
                _objectInstanceId = value;
            }
        }

        public int PrivilegeOrientation { get; set; }

        public string PrivilegeConstraint { get; set; }
    }
}
