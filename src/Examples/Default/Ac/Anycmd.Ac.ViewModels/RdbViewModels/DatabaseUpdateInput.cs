
namespace Anycmd.Ac.ViewModels.RdbViewModels
{
    using Engine;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 关系数据库输入模型
    /// </summary>
    public sealed class DatabaseUpdateInput : IInputModel
    {
        public DatabaseUpdateInput()
        {
            OntologyCode = "Database";
            Verb = "Update";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

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
    }
}
