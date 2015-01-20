
namespace Anycmd.Ac.ViewModels.Identity.AccountViewModels
{
    using Engine;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Identity;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class AccountUpdateInput : IAccountUpdateIo
    {
        public AccountUpdateInput()
        {
            HecpOntology = "Account";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid ContractorId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AllowStartTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AllowEndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LockStartTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LockEndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string AuditState { get; set; }
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
        [Required]
        [StringLength(50)]
        [DisplayName(@"姓名")]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        [DisplayName(@"编码")]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string QuickQuery { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string QuickQuery1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string QuickQuery2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string OrganizationCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string QQ { get; set; }

        public IAnycmdCommand ToCommand(IUserSession userSession)
        {
            return new UpdateAccountCommand(userSession, this);
        }
    }
}
