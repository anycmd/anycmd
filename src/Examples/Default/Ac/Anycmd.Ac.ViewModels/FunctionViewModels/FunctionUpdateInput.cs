
namespace Anycmd.Ac.ViewModels.FunctionViewModels
{
    using Engine.Ac.Functions;
    using Engine.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class FunctionUpdateInput : IFunctionUpdateIo
    {
        public FunctionUpdateInput()
        {
            HecpOntology = "Function";
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

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateFunctionCommand(acSession, this);
        }
    }
}
