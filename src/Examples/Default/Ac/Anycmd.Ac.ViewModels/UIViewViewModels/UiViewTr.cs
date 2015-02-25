
namespace Anycmd.Ac.ViewModels.UIViewViewModels
{
    using Engine.Ac;
    using Exceptions;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewTr
    {
        private readonly IAcDomain _acDomain;

        private UiViewTr(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public static UiViewTr Create(UiViewState view)
        {
            if (view == null)
            {
                return null;
            }
            FunctionState function;
            view.AcDomain.FunctionSet.TryGetFunction(view.Id, out function);
            AppSystemState appSystem;
            view.AcDomain.AppSystemSet.TryGetAppSystem(function.AppSystem.Id, out appSystem);
            return new UiViewTr(view.AcDomain)
            {
                Code = function.Code,
                AppSystemCode = appSystem.Code,
                AppSystemId = appSystem.Id,
                AppSystemName = appSystem.Name,
                ResourceCode = function.Resource.Code,
                CreateOn = view.CreateOn,
                DeveloperId = function.DeveloperId,
                Icon = view.Icon,
                Id = view.Id,
                Description = function.Description
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
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
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid DeveloperId { get; set; }
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
                    throw new ValidationException("意外的开发人员标识" + this.DeveloperId);
                }
                return developer.LoginName;
            }
        }
    }
}
