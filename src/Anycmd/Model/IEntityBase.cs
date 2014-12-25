
namespace Anycmd.Model
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是实体基类。
    /// </summary>
    public interface IEntityBase : IEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime? CreateOn { get; set; }

        /// <summary>
        /// 创建人标识
        /// </summary>
        Guid? CreateUserId { get; set; }

        /// <summary>
        /// 创建人[姓名|登录名]
        /// </summary>
        string CreateBy { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// 最后修改人标识
        /// </summary>
        Guid? ModifiedUserId { get; set; }

        /// <summary>
        /// 最后修改人[姓名|登录名]
        /// </summary>
        string ModifiedBy { get; set; }
    }
}
