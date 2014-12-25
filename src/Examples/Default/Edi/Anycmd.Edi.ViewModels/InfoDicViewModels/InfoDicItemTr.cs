
namespace Anycmd.Edi.ViewModels.InfoDicViewModels
{
    using Engine.Edi.Abstractions;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public partial class InfoDicItemTr
    {
        public InfoDicItemTr() { }

        public static InfoDicItemTr Create(IInfoDicItem infoDicItem)
        {
            return new InfoDicItemTr
            {
                Code = infoDicItem.Code,
                CreateOn = infoDicItem.CreateOn,
                Id = infoDicItem.Id,
                InfoDicId = infoDicItem.InfoDicId,
                IsEnabled = infoDicItem.IsEnabled,
                Level = infoDicItem.Level,
                Name = infoDicItem.Name,
                SortCode = infoDicItem.SortCode
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid InfoDicId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Level { get; set; }
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
