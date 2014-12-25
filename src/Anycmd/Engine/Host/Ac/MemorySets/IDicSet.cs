
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Engine.Ac;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是系统字典集。
    /// </summary>
    public interface IDicSet : IEnumerable<DicState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicId"></param>
        /// <returns></returns>
        bool ContainsDic(Guid dicId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicCode"></param>
        /// <returns></returns>
        bool ContainsDic(string dicCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicId"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        bool TryGetDic(Guid dicId, out DicState dic);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicCode"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        bool TryGetDic(string dicCode, out DicState dic);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        IReadOnlyDictionary<string, DicItemState> GetDicItems(DicState dic);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicItemId"></param>
        /// <returns></returns>
        bool ContainsDicItem(Guid dicItemId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="dicItemCode"></param>
        /// <returns></returns>
        bool ContainsDicItem(DicState dic, string dicItemCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicItemId"></param>
        /// <param name="dicItem"></param>
        /// <returns></returns>
        bool TryGetDicItem(Guid dicItemId, out DicItemState dicItem);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicState"></param>
        /// <param name="dicItemCode"></param>
        /// <param name="dicItem"></param>
        /// <returns></returns>
        bool TryGetDicItem(DicState dicState, string dicItemCode, out DicItemState dicItem);
    }
}
