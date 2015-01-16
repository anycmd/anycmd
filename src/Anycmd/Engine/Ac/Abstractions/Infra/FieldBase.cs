
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using Exceptions;
    using Model;
    using System;

    public abstract class FieldBase : EntityBase, IField
    {
        private Guid _resourceTypeId;
        private string _code;
        private string _name;

        protected FieldBase() { }

        public Guid ResourceTypeId
        {
            get { return _resourceTypeId; }
            set
            {
                if (_resourceTypeId != value && _resourceTypeId != Guid.Empty)
                {
                    throw new AnycmdException("不能更改字段的所属资源类型");
                }
                _resourceTypeId = value;
            }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                }
                _code = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ValidationException("名称是必须的");
                }
                _name = value;
            }
        }

        public int SortCode { get; set; }

        public string Icon { get; set; }

        public string Description { get; set; }
    }
}
