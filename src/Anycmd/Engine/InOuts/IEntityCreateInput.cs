
namespace Anycmd.Engine.InOuts
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是创建实体时的数据输入模型。
    /// </summary>
    public interface IEntityCreateInput : IAnycmdInput
    {
        /// <summary>
        /// <remarks>
        /// 在ASP.NET MVC中使用它默认的输入模型验证器时需要Id字段为可空的。
        /// </remarks>
        /// </summary>
        Guid? Id { get; }
    }
}
