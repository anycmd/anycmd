
namespace Anycmd.Edi.ViewModels.InfoDicViewModels
{
    using Engine;
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicCreateInput : EntityCreateInput, IInfoDicCreateIo
    {
        public InfoDicCreateInput()
        {
            HecpOntology = "InfoDic";
            HecpVerb = "Create";
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1)]
        public int IsEnabled { get; set; }

        public AddInfoDicCommand ToCommand(IUserSession userSession)
        {
            return new AddInfoDicCommand(userSession, this);
        }
    }
}
