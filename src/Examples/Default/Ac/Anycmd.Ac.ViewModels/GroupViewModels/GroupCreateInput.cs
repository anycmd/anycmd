
namespace Anycmd.Ac.ViewModels.GroupViewModels
{
    using Engine.Ac.Groups;
    using Engine.InOuts;
    using Engine.Messages;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class GroupCreateInput : EntityCreateInput, IGroupCreateIo
    {
        public GroupCreateInput()
        {
            HecpOntology = "Group";
            HecpVerb = "Create";
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string TypeCode { get; set; }
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
        public int SortCode { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddGroupCommand(acSession, this);
        }
    }
}
