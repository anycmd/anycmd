
namespace Anycmd.Engine.Host
{
    using System;

    /// <summary>
    /// 停止前事件参数
    /// </summary>
    public sealed class StoppingEventArgs : EventArgs
    {
        /// <summary>
        /// 取消停止
        /// <remarks>如果把该属性置为True则停止操作被取消</remarks>
        /// </summary>
        public bool Canceled { get; set; }
    }
}
