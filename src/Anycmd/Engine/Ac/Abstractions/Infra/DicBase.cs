
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using Exceptions;
    using Model;

    /// <summary>
    /// 系统字典基类
    /// </summary>
    public abstract class DicBase : EntityBase, IDic
    {
        private string _code;
        private string _name;
        private int _isEnabled;

        protected DicBase()
        {
            _isEnabled = 1;
        }

        /// <summary>
        /// 字典有效状态
        /// </summary>
        public int IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        /// <summary>
        /// 字典码
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
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
    }
}
