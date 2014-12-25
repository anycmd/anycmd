
namespace Anycmd.Model
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是更新实体时的数据输入模型。
    /// </summary>
    public interface IEntityUpdateInput : IInputModel
    {
        Guid Id { get; }
    }
}
