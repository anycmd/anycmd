
namespace Anycmd.Engine.Ac
{
    using Exceptions;
    using Model;
    using Privileges;
    using System;
    using Util;

    public sealed class PrivilegeState : StateObject<PrivilegeState>, IAcRecord
    {
        private AcRecordType _acType;
        private AcElementType _subjectType;
        private Guid _subjectInstanceId;
        private AcElementType _objectType;
        private Guid _objectInstanceId;
        private string _acContent;
        private string _acContentType;
        private DateTime? _createOn;
        private string _createBy;
        private Guid? _createUserId;

        private PrivilegeState(Guid id) : base(id) { }

        public static PrivilegeState Create(PrivilegeBase privilege)
        {
            if (privilege == null)
            {
                throw new ArgumentNullException("privilege");
            }
            return new PrivilegeState(privilege.Id)
            {
                _createOn = privilege.CreateOn
            }.InternalModify(privilege);
        }

        internal PrivilegeState InternalModify(PrivilegeBase privilege)
        {
            if (privilege == null)
            {
                throw new ArgumentNullException("privilege");
            }
            if (string.IsNullOrEmpty(privilege.SubjectType))
            {
                throw new AnycmdException("必须指定主授权授权类型");
            }
            if (string.IsNullOrEmpty(privilege.ObjectType))
            {
                throw new AnycmdException("必须指定授权授权类型");
            }
            AcElementType subjectType;
            AcElementType acObjectType;
            AcRecordType acType;
            if (!privilege.SubjectType.TryParse(out subjectType))
            {
                throw new AnycmdException("非法的主授权类型" + privilege.SubjectType);
            }
            if (!privilege.ObjectType.TryParse(out acObjectType))
            {
                throw new AnycmdException("非法的从授权类型" + privilege.ObjectType);
            }
            if (!(privilege.SubjectType + privilege.ObjectType).TryParse(out acType))
            {
                throw new AnycmdException("非法的授权类型" + privilege.ObjectType);
            }

            _acType = acType;
            _subjectType = subjectType;
            _subjectInstanceId = privilege.SubjectInstanceId;
            _objectType = acObjectType;
            _objectInstanceId = privilege.ObjectInstanceId;
            _acContent = privilege.AcContent;
            _createBy = privilege.CreateBy;
            _createUserId = privilege.CreateUserId;
            _acContentType = privilege.AcContentType;

            return this;
        }

        public AcRecordType AcRecordType
        {
            get { return _acType; }
        }

        public AcElementType SubjectType
        {
            get { return _subjectType; }
        }

        public Guid SubjectInstanceId
        {
            get { return _subjectInstanceId; }
        }

        public AcElementType ObjectType
        {
            get { return _objectType; }
        }

        public Guid ObjectInstanceId
        {
            get { return _objectInstanceId; }
        }

        public string AcContent
        {
            get { return _acContent; }
        }

        public string AcContentType
        {
            get { return _acContentType; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        public string CreateBy
        {
            get { return _createBy; }
        }

        public Guid? CreateUserId
        {
            get { return _createUserId; }
        }

        protected override bool DoEquals(PrivilegeState other)
        {
            return Id == other.Id &&
                SubjectType == other.SubjectType &&
                SubjectInstanceId == other.SubjectInstanceId &&
                ObjectType == other.ObjectType &&
                ObjectInstanceId == other.ObjectInstanceId &&
                AcContent == other.AcContent &&
                AcContentType == other.AcContentType;
        }
    }
}
