
namespace Anycmd.Engine.Ac.InOuts
{
    using System;

    /// <summary>
    /// 更新权限二元组时的输入或输出参数类型。
    /// </summary>
    public class PrivilegeUpdateIo : IPrivilegeUpdateIo
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AcContent { get; set; }
    }
}
