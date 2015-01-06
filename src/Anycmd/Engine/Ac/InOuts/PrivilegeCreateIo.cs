
namespace Anycmd.Engine.Ac.InOuts
{
    using System;

    /// <summary>
    /// 创建权限二元组时的输入或输出参数类型。
    /// </summary>
    public class PrivilegeCreateIo : EntityCreateInput, IPrivilegeCreateIo
    {
        public string SubjectType { get; set; }

        public Guid SubjectInstanceId { get; set; }

        public string ObjectType { get; set; }

        public Guid ObjectInstanceId { get; set; }

        public string AcContent { get; set; }

        public string AcContentType { get; set; }
    }
}
