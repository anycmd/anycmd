
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO:放到数据库存储和管理
    /// </summary>
    public static class EventSubjectCode
    {
        private static readonly HashSet<string> codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
            StateCodeChanged,StateCodeChanged_Audit,StateCodeChanged_Execute
        };

        public const string StateCodeChanged = "StateCodeChanged";
        public const string StateCodeChanged_Audit = "StateCodeChanged.Audit";
        public const string StateCodeChanged_Execute = "StateCodeChanged.Execute";

        public static bool Contains(string subjectCode)
        {
            return codes.Contains(subjectCode);
        }
    }
}
