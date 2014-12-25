
namespace Anycmd.Engine.Ac
{
    using Abstractions.Infra;
    using Model;
    using System;
    using Util;

    /// <summary>
    /// 表示系统资源类型业务实体。
    /// </summary>
    public sealed class ResourceTypeState : StateObject<ResourceTypeState>, IResourceType, IStateObject
    {
        public static readonly ResourceTypeState Empty = new ResourceTypeState(Guid.Empty)
        {
            _appSystemId = Guid.Empty,
            _code = string.Empty,
            _createOn = SystemTime.MinDate,
            _icon = string.Empty,
            _name = string.Empty,
            _sortCode = 0
        };

        private Guid _appSystemId;
        private string _name;
        private string _code;
        private string _icon;
        private int _sortCode;
        private DateTime? _createOn;

        private ResourceTypeState(Guid id) : base(id) { }

        public static ResourceTypeState Create(ResourceTypeBase resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            return new ResourceTypeState(resource.Id)
            {
                _appSystemId = resource.AppSystemId,
                _name = resource.Name,
                _code = resource.Code,
                _icon = resource.Icon,
                _sortCode = resource.SortCode,
                _createOn = resource.CreateOn,
            };
        }

        public Guid AppSystemId
        {
            get { return _appSystemId; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Code
        {
            get { return _code; }
        }

        public string Icon
        {
            get { return _icon; }
        }

        public int SortCode
        {
            get { return _sortCode; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        protected override bool DoEquals(ResourceTypeState other)
        {
            return Id == other.Id &&
                AppSystemId == other.AppSystemId &&
                Name == other.Name &&
                Code == other.Code &&
                Icon == other.Icon &&
                SortCode == other.SortCode;
        }
    }
}
