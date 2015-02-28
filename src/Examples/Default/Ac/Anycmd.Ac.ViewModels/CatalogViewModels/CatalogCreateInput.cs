
namespace Anycmd.Ac.ViewModels.CatalogViewModels
{
    using Engine.Ac.Catalogs;
    using Engine.InOuts;
    using Engine.Messages;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class CatalogCreateInput : EntityCreateInput, ICatalogCreateIo
    {
        public CatalogCreateInput()
        {
            HecpOntology = "Catalog";
            HecpVerb = "Create";
        }

        /// <summary>
        /// 
        /// </summary>
        public string ParentCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        [DisplayName(@"本地编码")]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        [DisplayName(@"简称")]
        public string ShortName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(100)]
        [DisplayName(@"名称")]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string CategoryCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName(@"外线电话")]
        [RegularExpression(@"^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$", ErrorMessage = @"外线电话格式错误")]
        public string OuterPhone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName(@"内线电话")]
        [RegularExpression(@"^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$", ErrorMessage = @"内线电话格式错误")]
        public string InnerPhone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(200)]
        [DisplayName(@"地址")]
        public string Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WebPage { get; set; }
        /// <summary>
        /// 包工头
        /// </summary>
        public Guid? ContractorId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [MaxLength(500)]
        [DisplayName(@"备注")]
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddCatalogCommand(acSession, this);
        }
    }
}
