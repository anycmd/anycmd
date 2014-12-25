
namespace Anycmd.Edi.Application
{
    using Engine.Host;
    using Engine.Host.Edi;
    using ServiceStack.Text;
    using System.Collections.Generic;

    /// <summary>
    /// 命令StackTrace格式化器。
    /// </summary>
    public sealed class JsonStackTraceFormater : IStackTraceFormater
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="acts"></param>
        /// <returns></returns>
        public string Format(HashSet<WfAct> acts)
        {
            return JsonSerializer.SerializeToString(acts);
        }
    }
}
