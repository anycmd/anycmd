
namespace Anycmd.Engine.Host.Edi
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public interface IStackTraceFormater
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="acts"></param>
        /// <returns></returns>
        string Format(HashSet<WfAct> acts);
    }
}
