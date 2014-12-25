
namespace Anycmd.Ac.ViewModels.Infra.ResourceViewModels
{
    using Engine.Ac;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class ResourceTypeTr
    {
        public ResourceTypeTr() { }

        public static ResourceTypeTr Create(ResourceTypeState resource)
        {
            return new ResourceTypeTr
            {
                Code = resource.Code,
                CreateOn = resource.CreateOn,
                Icon = resource.Icon,
                Id = resource.Id,
                Name = resource.Name,
                SortCode = resource.SortCode
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

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
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
    }
}
