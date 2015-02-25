
namespace Anycmd.Ac.ViewModels.CatalogViewModels
{
    using Engine.Ac;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class CatalogTr
    {
        public CatalogTr() { }

        public static CatalogTr Create(CatalogState catalog)
        {
            return new CatalogTr
            {
                CategoryCode = catalog.CategoryCode,
                Code = catalog.Code,
                CreateOn = catalog.CreateOn,
                Id = catalog.Id,
                IsEnabled = catalog.IsEnabled,
                Name = catalog.Name,
                ParentCode = catalog.ParentCode,
                ParentName = catalog.Parent.Name,
                SortCode = catalog.SortCode
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
