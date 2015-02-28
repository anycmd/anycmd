
namespace Anycmd.Edi.ViewModels.InfoDicViewModels
{
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.Messages;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicUpdateInput : IInfoDicUpdateIo
    {
        public InfoDicUpdateInput()
        {
            HecpOntology = "InfoDic";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid Id { get; set; }
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

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateInfoDicCommand(acSession, this);
        }
    }
}
