
namespace Anycmd.Engine.Host
{
    using System.Collections.Generic;

    /// <summary>
    /// 命令生命周期中涉及到很多资源，这个栈记录下了何时什么资源参与进了命令生命周期。
    /// 通过打印这些信息可以观察命令的处理流程。
    /// </summary>
    public interface IStackTrace : IEnumerable<WfAct>
    {
        /// <summary>
        /// 
        /// </summary>
        void Trace(WfAct act);
    }
}
