
namespace Anycmd.Ac.ViewModels.RdbViewModels
{
    using Engine.Messages;
    using Engine.Rdb.InOuts;
    using Engine.Rdb.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 关系数据库输入模型
    /// </summary>
    public sealed class DatabaseUpdateInput : IDatabaseUpdateInput
    {
        public DatabaseUpdateInput()
        {
            HecpOntology = "Database";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateDatabaseCommand(acSession, this);
        }
    }
}
