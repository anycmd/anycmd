
namespace Anycmd.Ac.ViewModels.FunctionViewModels
{
    using Engine.Ac.Functions;
    using Engine.InOuts;
    using Engine.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class FunctionCreateInput : EntityCreateInput, IFunctionCreateIo
    {
        public FunctionCreateInput()
        {
            HecpOntology = "Function";
            HecpVerb = "Create";
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid ResourceTypeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public bool IsManaged { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid DeveloperId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddFunctionCommand(acSession, this);
        }
    }
}
