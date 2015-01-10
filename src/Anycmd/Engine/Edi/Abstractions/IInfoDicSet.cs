
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 定义信息字典的访问接口。
    /// </summary>
    public interface IInfoDicSet : IEnumerable<InfoDicState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 获取与指定的信息字典标识相关联的信息字典对象。
        /// </summary>
        /// <param name="dicId">
        /// 信息字典标识。
        /// </param>
        /// <param name="infoDic">
        /// 信息字典对象。当此方法返回时，如果找到指定键，则返回与该键相关联的值；
        /// 否则，将返回 <paramref name="infoDic"/> 参数的类型的默认值。该参数未经初始化即被传递。
        /// </param>
        /// <returns>
        /// 如果实现<seealso cref="IInfoDicSet"/>的对象包含具有指定键的元素，则为
        /// true；否则，为 false。
        /// </returns>
        bool TryGetInfoDic(Guid dicId, out InfoDicState infoDic);

        /// <summary>
        /// 获取与指定的信息字典编码相关联的字典对象。
        /// </summary>
        /// <param name="dicCode">
        /// 信息字典编码。
        /// </param>
        /// <param name="infoDic">
        /// 信息字典对象。当此方法返回时，如果找到指定键，则返回与该键相关联的值；
        /// 否则，将返回 <paramref name="infoDic"/> 参数的类型的默认值。该参数未经初始化即被传递。
        /// </param>
        /// <returns>
        /// 如果实现<seealso cref="IInfoDicSet"/>的对象包含具有指定键的元素，则为
        /// true；否则，为 false。
        /// </returns>
        bool TryGetInfoDic(string dicCode, out InfoDicState infoDic);

        /// <summary>
        /// 获取给定字典标识的字典项列表
        /// </summary>
        /// <param name="infoDic">字典标识</param>
        /// <returns></returns>
        IReadOnlyCollection<InfoDicItemState> GetInfoDicItems(InfoDicState infoDic);

        /// <summary>
        /// 获取与指定的信息字典标识和信息字典项编码相关联的字典项对象。
        /// </summary>
        /// <param name="infoDic">
        /// 信息字典标识标识。
        /// </param>
        /// <param name="itemCode">信息字典项编码</param>
        /// <param name="infoDicItem">
        /// 信息字典项对象。当此方法返回时，如果找到指定键，则返回与该键相关联的值；
        /// 否则，将返回 <paramref name="infoDicItem"/> 参数的类型的默认值。该参数未经初始化即被传递。
        /// </param>
        /// <returns>
        /// 如果实现<seealso cref="IInfoDicSet"/>的对象包含具有指定键的元素，则为
        /// true；否则，为 false。
        /// </returns>
        bool TryGetInfoDicItem(InfoDicState infoDic, string itemCode, out InfoDicItemState infoDicItem);

        /// <summary>
        /// 获取与指定的信息字典项标识相关联的字典项对象。
        /// </summary>
        /// <param name="dicItemId">字典项标识</param>
        /// <param name="infoDicItem">字典项对象</param>
        /// <returns></returns>
        bool TryGetInfoDicItem(Guid dicItemId, out InfoDicItemState infoDicItem);
    }
}
