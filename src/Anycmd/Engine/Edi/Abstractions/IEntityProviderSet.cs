
namespace Anycmd.Engine.Edi.Abstractions
{
    using Host.Edi.Handlers;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 定义实体提供程序集合
    /// </summary>
    public interface IEntityProviderSet : IEnumerable<IEntityProvider>
    {
        /// <summary>
        /// 获取与指定的数据提供程序标识相关联的数据提供程序对象。
        /// </summary>
        /// <param name="providerId">
        /// 数据提供程序标识。
        /// </param>
        /// <param name="provider">
        /// 数据提供程序对象。当此方法返回时，如果找到指定键，则返回与该键相关联的值；
        /// 否则，将返回 <paramref name="provider"/> 参数的类型的默认值。该参数未经初始化即被传递。
        /// </param>
        /// <returns>
        /// 如果实现<seealso cref="IEntityProviderSet"/>的对象包含具有指定键的元素，则为
        /// true；否则，为 false。
        /// </returns>
        bool TryGetEntityProvider(Guid providerId, out IEntityProvider provider);
    }
}
