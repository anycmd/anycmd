
namespace Anycmd.Ac.ViewModels.FunctionViewModels
{
    using Engine.Ac;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class FunctionTr
    {
        private readonly IAcDomain _acDomain;

        private FunctionTr(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public static FunctionTr Create(FunctionState function)
        {
            if (function == null)
            {
                return null;
            }
            return new FunctionTr(function.AcDomain)
            {
                AppSystemId = function.AppSystem.Id,
                AppSystemCode = function.AppSystem.Code,
                AppSystemName = function.AppSystem.Name,
                Code = function.Code,
                CreateOn = function.CreateOn,
                Description = function.Description,
                DeveloperId = function.DeveloperId,
                Id = function.Id,
                IsManaged = function.IsManaged,
                IsEnabled = function.IsEnabled,
                ResourceCode = function.Resource.Code,
                ResourceTypeId = function.Resource.Id,
                ResourceName = function.Resource.Name,
                SortCode = function.SortCode
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsManaged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsUiView
        {
            get
            {
                UiViewState view;
                return _acDomain.UiViewSet.TryGetUiView(this.Id, out view);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        public int IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid ResourceTypeId { get; set; }

        public Guid AppSystemId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AppSystemCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AppSystemName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid DeveloperId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }

        public string DeveloperCode
        {
            get
            {
                AccountState developer;
                if (!_acDomain.SysUserSet.TryGetDevAccount(this.DeveloperId, out developer))
                {
                    return "无效的值";
                }
                return developer.LoginName;
            }
        }
    }
}
