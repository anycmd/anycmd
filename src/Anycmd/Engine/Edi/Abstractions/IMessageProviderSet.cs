
namespace Anycmd.Engine.Edi.Abstractions
{
    using Host.Edi.Handlers;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 定义命令提供程序集合
    /// </summary>
    public interface IMessageProviderSet : IEnumerable<IMessageProvider>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 获取与指定的命令提供程序标识相关联的命令提供程序对象。
        /// </summary>
        /// <param name="providerId">
        /// 命令提供程序标识。
        /// </param>
        /// <param name="provider">
        /// 命令提供程序对象。当此方法返回时，如果找到指定键，则返回与该键相关联的值；
        /// 否则，将返回 <paramref name="provider"/> 参数的类型的默认值。该参数未经初始化即被传递。
        /// </param>
        /// <returns>
        /// 如果实现<seealso cref="IMessageProviderSet"/>的对象包含具有指定键的元素，则为
        /// true；否则，为 false。
        /// </returns>
        bool TryGetMessageProvider(Guid providerId, out IMessageProvider provider);
    }
}
