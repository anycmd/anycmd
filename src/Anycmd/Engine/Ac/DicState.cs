
namespace Anycmd.Engine.Ac
{
    using Abstractions.Infra;
    using Model;
    using System;
    using Util;

    /// <summary>
    /// 表示系统字典业务实体。
    /// </summary>
    public sealed class DicState : StateObject<DicState>, IDic
    {
        public static readonly DicState Empty = new DicState(Guid.Empty)
        {
            _code = string.Empty,
            _createOn = SystemTime.MinDate,
            _isEnabled = 0,
            _name = string.Empty,
            _sortCode = 0
        };

        private string _code;
        private string _name;
        private int _isEnabled;
        private int _sortCode;
        private DateTime? _createOn;

        private DicState(Guid id) : base(id) { }

        public static DicState Create(DicBase dic)
        {
            if (dic == null)
            {
                throw new ArgumentNullException("dic");
            }
            return new DicState(dic.Id)
            {
                _code = dic.Code,
                _name = dic.Name,
                _isEnabled = dic.IsEnabled,
                _createOn = dic.CreateOn,
                _sortCode = dic.SortCode
            };
        }

        public string Code
        {
            get { return _code; }
        }

        public string Name
        {
            get { return _name; }
        }

        public int IsEnabled
        {
            get { return _isEnabled; }
        }

        public int SortCode
        {
            get { return _sortCode; }
        }

        public DateTime? CreateOn
        {
            get { return _createOn; }
        }

        public override string ToString()
        {
            return string.Format(
@"{{
    Id:'{0}',
    Code:'{1}',
    Name:'{2}',
    IsEnabled:{3},
    SortCode:{4},
    CreateOn:'{5}'
}}", Id, Code, Name, IsEnabled, SortCode, CreateOn);
        }

        protected override bool DoEquals(DicState other)
        {
            return Id == other.Id &&
                Code == other.Code &&
                Name == other.Name &&
                IsEnabled == other.IsEnabled &&
                SortCode == other.SortCode;
        }
    }
}
