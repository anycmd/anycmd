
namespace Anycmd.Edi.ViewModels.MessageViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ViewModel;

    /// <summary>
    /// 分页获取命令。
    /// </summary>
    public class GetPlistMessages : GetPlistResult
    {
        /// <summary>
        /// 命令类型
        /// </summary>
        [Required]
        public string CommandType { get; set; }
        /// <summary>
        /// 组织结构码
        /// </summary>
        public string OrganizationCode { get; set; }
        /// <summary>
        /// 动作码
        /// </summary>
        public string ActionCode { get; set; }
        /// <summary>
        /// 节点标识
        /// </summary>
        public Guid? NodeId { get; set; }
        /// <summary>
        /// 本体码
        /// </summary>
        [Required]
        public string OntologyCode { get; set; }
        /// <summary>
        /// 实体标识
        /// </summary>
        public string EntityId { get; set; }
    }
}
