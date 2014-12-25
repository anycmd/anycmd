
namespace Anycmd.Ac.ViewModels.Infra.OrganizationViewModels
{
    using Engine.Ac;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class OrganizationTr
    {
        public OrganizationTr() { }

        public static OrganizationTr Create(OrganizationState organization)
        {
            return new OrganizationTr
            {
                CategoryCode = organization.CategoryCode,
                Code = organization.Code,
                CreateOn = organization.CreateOn,
                Id = organization.Id,
                IsEnabled = organization.IsEnabled,
                Name = organization.Name,
                ParentCode = organization.ParentCode,
                ParentName = organization.Parent.Name,
                SortCode = organization.SortCode
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
    }
}
