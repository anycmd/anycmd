
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 目录基类<see cref="IOrganization"/>
    /// </summary>
    public abstract class OrganizationBase : EntityBase, IOrganization
    {
        private string _code;
        private string _name;

        #region Ctor
        protected OrganizationBase() { }
        #endregion

        /// <summary>
        /// 是否启用
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ParentCode { get; set; }

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
        public string ShortName { get; set; }

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
        /// 包工头标识
        /// </summary>
        public Guid? ContractorId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public int DeletionStateCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OuterPhone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InnerPhone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Postalcode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WebPage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
    }
}
