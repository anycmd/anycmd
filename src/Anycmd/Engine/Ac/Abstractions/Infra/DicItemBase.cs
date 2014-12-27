
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 系统字典项基类
    /// </summary>
    public abstract class DicItemBase : EntityBase, IDicItem
    {
        private string _code;
        private string _name;
        private Guid _dicId;
        private int _isEnabled;

        protected DicItemBase()
        {
            _isEnabled = 1;
        }

        public int IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid DicId
        {
            get { return _dicId; }
            set
            {
                if (value == _dicId) return;
                if (_dicId != Guid.Empty)
                {
                    throw new AnycmdException("不能更改字典项的所属字典");
                }
                _dicId = value;
            }
        }
    }
}
