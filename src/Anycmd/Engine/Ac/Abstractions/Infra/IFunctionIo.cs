
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是计算单元的输入或输出参数类型。
    /// </summary>
    public interface IFunctionIo
    {
        /// <summary>
        /// 按钮标识
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid FunctionId { get; }
        /// <summary>
        /// 
        /// </summary>
        string Direction { get; }
        /// <summary>
        /// 编码
        /// </summary>
        string Code { get; }
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
    }
}
