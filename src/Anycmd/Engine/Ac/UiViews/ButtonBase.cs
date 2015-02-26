
namespace Anycmd.Engine.Ac.UiViews
{
    using Exceptions;
    using Model;

    /// <summary>
    /// 按钮基类。
    /// </summary>
    public abstract class ButtonBase : EntityBase, IButton
    {
        private string _code;
        private string _name;
        private int _isEnabled;

        protected ButtonBase()
        {
            _isEnabled = 1;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
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
                if (string.IsNullOrEmpty(value))
                {
                    throw new ValidationException("编码是必须的");
                }
                value = value.Trim();
                _code = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name {
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
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
    }
}
