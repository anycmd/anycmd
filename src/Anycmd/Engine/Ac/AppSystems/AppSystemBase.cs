
namespace Anycmd.Engine.Ac.AppSystems
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 权限应用系统基类
    /// </summary>
    public abstract class AppSystemBase : EntityBase, IAppSystem
    {
        private string _code;
        private string _name;
        private Guid _principalId;
        private int _isEnabled;

        protected AppSystemBase()
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
        /// 系统编号
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
        /// 单点登录Http认证接口地址
        /// </summary>
        public string SsoAuthAddress { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public Guid PrincipalId {
            get { return _principalId; }
            set
            {
                if (value == Guid.Empty)
                {
                    throw new ValidationException("负责人是必须的");
                }
                _principalId = value;
            }
        }

        /// <summary>
        /// 系统名称
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
        /// 系统图标Url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }

        public string Description { get; set; }
    }
}
