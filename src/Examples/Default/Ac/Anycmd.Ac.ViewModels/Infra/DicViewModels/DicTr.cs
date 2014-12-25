
namespace Anycmd.Ac.ViewModels.Infra.DicViewModels
{
    using Engine.Ac;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class DicTr
    {
        public DicTr() { }

        public static DicTr Create(DicState dic)
        {
            return new DicTr
            {
                Code = dic.Code,
                CreateOn = dic.CreateOn,
                Id = dic.Id,
                IsEnabled = dic.IsEnabled,
                Name = dic.Name,
                SortCode = dic.SortCode
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
