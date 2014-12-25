
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using Model;

    /// <summary>
    /// 操作帮助基类。<see cref="IOperationHelp"/>
    /// </summary>
    public abstract class OperationHelpBase : EntityBase, IOperationHelp
    {
        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }
    }
}
