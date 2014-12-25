
namespace Anycmd.MiniUI
{
    using System.Collections.Generic;

    /// <summary>
    /// miniui dataGrid控件的数据源模型<seealso cref="IViewModel"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class MiniGrid<T> : MiniGrid
    {
        /// <summary>
        /// 
        /// </summary>
        public new IEnumerable<T> data { get; set; }
    }
}
