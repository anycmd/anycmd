
namespace Anycmd.Ac.ViewModels.PrivilegeViewModels
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class RoleAssignFunctionTr
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAssigned { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid FunctionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid AppSystemId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AppSystemCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ResourceTypeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ResourceName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FunctionCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AcContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AcContentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
    }
}
