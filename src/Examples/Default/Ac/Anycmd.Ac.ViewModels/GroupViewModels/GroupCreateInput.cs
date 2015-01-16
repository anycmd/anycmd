
namespace Anycmd.Ac.ViewModels.GroupViewModels
{
    using Engine;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Infra;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class GroupCreateInput : EntityCreateInput, IGroupCreateIo
    {
        public GroupCreateInput()
        {
            OntologyCode = "Group";
            Verb = "Create";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

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

        public AddGroupCommand ToCommand()
        {
            return new AddGroupCommand(this);
        }
    }
}
