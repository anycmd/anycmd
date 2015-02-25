
namespace Anycmd.Engine.Ac.Functions
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是操作帮助类型。
    /// </summary>
    public interface IOperationHelp
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        string Content { get; set; }
    }
}
