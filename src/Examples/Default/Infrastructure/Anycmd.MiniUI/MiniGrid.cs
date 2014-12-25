
namespace Anycmd.MiniUI
{
    using System;
    using ViewModel;

    /// <summary>
    /// 封装miniui的datagrid的数据源的类
    /// </summary>
    public class MiniGrid : IViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Int64 total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object data { get; set; }
    }
}
