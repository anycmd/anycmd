
namespace Anycmd.Engine.Ac
{
    using Abstractions;
    using Exceptions;
    using System;
    using Util;

    public sealed class PrivilegeBigramState : StateObject<PrivilegeBigramState>, IAcRecord
    {
        private AcRecordType _acType;
        private AcElementType _subjectType;
        private Guid _subjectInstanceId;
        private AcElementType _objectType;
        private Guid _objectInstanceId;
        private string _privilegeConstraint;
        private int _privilegeOrientation;
        private DateTime? _createOn;
        private string _createBy;
        private Guid? _createUserId;

        private PrivilegeBigramState(Guid id) : base(id) { }

        public static PrivilegeBigramState Create(PrivilegeBigramBase privilegeBigram)
        {
            if (privilegeBigram == null)
            {
                throw new ArgumentNullException("privilegeBigram");
            }
            if (string.IsNullOrEmpty(privilegeBigram.SubjectType))
            {
                throw new AnycmdException("必须指定主授权授权类型");
            }
            if (string.IsNullOrEmpty(privilegeBigram.ObjectType))
            {
                throw new AnycmdException("必须指定授权授权类型");
            }
            AcElementType subjectType;
            AcElementType acObjectType;
            AcRecordType acType;
            if (!privilegeBigram.SubjectType.TryParse(out subjectType))
            {
                throw new AnycmdException("非法的主授权类型" + privilegeBigram.SubjectType);
            }
            if (!privilegeBigram.ObjectType.TryParse(out acObjectType))
            {
                throw new AnycmdException("非法的从授权类型" + privilegeBigram.ObjectType);
            }
            if (!(privilegeBigram.SubjectType + privilegeBigram.ObjectType).TryParse(out acType))
            {
                throw new AnycmdException("非法的授权类型" + privilegeBigram.ObjectType);
            }
            return new PrivilegeBigramState(privilegeBigram.Id)
            {
                _acType = acType,
                _subjectType = subjectType,
                _subjectInstanceId = privilegeBigram.SubjectInstanceId,
                _objectType = acObjectType,
                _objectInstanceId = privilegeBigram.ObjectInstanceId,
                _privilegeConstraint = privilegeBigram.PrivilegeConstraint,
                _createOn = privilegeBigram.CreateOn,
                _createBy = privilegeBigram.CreateBy,
                _createUserId = privilegeBigram.CreateUserId,
                _privilegeOrientation = privilegeBigram.PrivilegeOrientation
            };
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

        public string PrivilegeConstraint
        {
            get { return _privilegeConstraint; }
        }

        public int PrivilegeOrientation
        {
            get { return _privilegeOrientation; }
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

        protected override bool DoEquals(PrivilegeBigramState other)
        {
            return Id == other.Id &&
                SubjectType == other.SubjectType &&
                SubjectInstanceId == other.SubjectInstanceId &&
                ObjectType == other.ObjectType &&
                ObjectInstanceId == other.ObjectInstanceId &&
                PrivilegeConstraint == other.PrivilegeConstraint &&
                PrivilegeOrientation == other.PrivilegeOrientation;
        }
    }
}
