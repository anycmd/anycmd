
namespace Anycmd.Engine.Ac.Abstractions.Identity
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 账户实体基类。
    /// </summary>
    public abstract class AccountBase : EntityBase, IAccount
    {
        private int _numberId;
        private string _loginName;
        private string _password;
        private int _isEnabled = 1;

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        public int NumberId
        {
            get { return _numberId; }
            set
            {
                if (value == default(int))
                {
                    throw new AnycmdException("数字标识是必须的");
                }
                else if (_numberId != value && _numberId != default(int))
                {
                    throw new AnycmdException("数字标识不能更改");
                }
                _numberId = value;
            }
        }

        /// <summary>
        /// 审核状态
        /// </summary>
        public string AuditState { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName
        {
            get { return _loginName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ValidationException("登录名不能为空");
                }
                _loginName = value;
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ValidationException("密码不能为空");
                }
                _password = value;
            }
        }

        /// <summary>
        /// 安全等级
        /// </summary>
        public int? SecurityLevel { get; set; }

        /// <summary>
        /// 修改密码日期
        /// </summary>
        public DateTime? LastPasswordChangeOn { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public string Lang { get; set; }

        /// <summary>
        /// 本体
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// 墙纸
        /// </summary>
        public string Wallpaper { get; set; }

        /// <summary>
        /// 背景色
        /// </summary>
        public string BackColor { get; set; }

        /// <summary>
        /// 允许登录开始时间
        /// </summary>
        public DateTime? AllowStartTime { get; set; }

        /// <summary>
        /// 允许登录结束时间
        /// </summary>
        public DateTime? AllowEndTime { get; set; }

        /// <summary>
        /// 锁定登录开始时间
        /// </summary>
        public DateTime? LockStartTime { get; set; }

        /// <summary>
        /// 锁定登录结束时间
        /// </summary>
        public DateTime? LockEndTime { get; set; }

        /// <summary>
        /// 初次登录时间
        /// </summary>
        public DateTime? FirstLoginOn { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? PreviousLoginOn { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int? LoginCount { get; set; }

        /// <summary>
        /// 开放标识
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Mac地址
        /// </summary>
        public string MacAddress { get; set; }

        /// <summary>
        /// 密码问题
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// 密码问题答案
        /// </summary>
        public string AnswerQuestion { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public int DeletionStateCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Qq { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QuickQuery { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QuickQuery1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QuickQuery2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OrganizationCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CommunicationPassword { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SignedPassword { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PublicKey { get; set; }
    }
}
