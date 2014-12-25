
namespace Anycmd.Edi.ViewModels.InfoDicViewModels
{
    using Engine.Edi.Abstractions;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public partial class InfoDicTr
    {
        public InfoDicTr() { }

        public static InfoDicTr Create(IInfoDic infoDic)
        {
            return new InfoDicTr
            {
                Code = infoDic.Code,
                CreateOn = infoDic.CreateOn,
                Id = infoDic.Id,
                IsEnabled = infoDic.IsEnabled,
                Name = infoDic.Name,
                SortCode = infoDic.SortCode
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
