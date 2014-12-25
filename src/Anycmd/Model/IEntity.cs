
namespace Anycmd.Model
{
    using System;

    /// <summary>
    /// 标记接口。表示该类型是实体，实体一般在数据库中对应有持久表或视图。
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 获取当前业务实体类的全局唯一标识。
        /// </summary>
        Guid Id { get; }
    }
}
