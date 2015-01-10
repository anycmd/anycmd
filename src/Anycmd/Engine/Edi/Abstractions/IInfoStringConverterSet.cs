
namespace Anycmd.Engine.Edi.Abstractions
{
    using Info;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 定义信息字符串转化器访问接口。
    /// </summary>
    public interface IInfoStringConverterSet : IEnumerable<IInfoStringConverter>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 获取与指定的信息格式标识字符串相关联的信息转化器对象。
        /// </summary>
        /// <param name="infoFormat">
        /// 信息格式标识字符串。
        /// </param>
        /// <param name="converter">
        /// 信息转化器对象。当此方法返回时，如果找到指定键，则返回与该键相关联的值；
        /// 否则，将返回 <paramref name="converter"/> 参数的类型的默认值。该参数未经初始化即被传递。
        /// </param>
        /// <returns>
        /// 如果实现<seealso cref="IInfoStringConverterSet"/>的对象包含具有指定键的元素，则为
        /// true；否则，为 false。
        /// </returns>
        bool TryGetInfoStringConverter(string infoFormat, out IInfoStringConverter converter);
    }
}
