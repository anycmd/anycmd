
namespace Anycmd.Ac.ViewModels.Infra.DicViewModels
{
    using Engine.Ac;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class DicItemTr
    {
        public DicItemTr() { }

        public static DicItemTr Create(DicItemState dicItem)
        {
            return new DicItemTr
            {
                Code = dicItem.Code,
                CreateOn = dicItem.CreateOn,
                DicId = dicItem.DicId,
                Id = dicItem.Id,
                IsEnabled = dicItem.IsEnabled,
                Name = dicItem.Name,
                SortCode = dicItem.SortCode
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
        public Guid DicId { get; set; }

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
