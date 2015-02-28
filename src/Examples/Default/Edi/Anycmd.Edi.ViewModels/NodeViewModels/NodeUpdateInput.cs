
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.Messages;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class NodeUpdateInput : INodeUpdateIo
    {
        public NodeUpdateInput()
        {
            HecpOntology = "Node";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 业务说明
        /// </summary>
        [Required]
        [DisplayName(@"业务说明")]
        public string Abstract { get; set; }
        /// <summary>
        /// 接入单位名称
        /// </summary>
        [Required]
        [StringLength(100)]
        [DisplayName(@"接入单位名称")]
        public string Catalog { get; set; }
        /// <summary>
        /// 专员
        /// </summary>
        [Required]
        [StringLength(50)]
        [DisplayName(@"专员")]
        public string Steward { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        [Required]
        public string Telephone { get; set; }
        /// <summary>
        /// 电子信箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string Qq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1)]
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid TransferId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string AnycmdApiAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AnycmdWsAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? BeatPeriod { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string PublicKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string SecretKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateNodeCommand(acSession, this);
        }
    }
}
