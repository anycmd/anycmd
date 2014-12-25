
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Engine.Ac;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是资源类型集。
    /// </summary>
    public interface IResourceTypeSet : IEnumerable<ResourceTypeState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceTypeId"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        bool TryGetResource(Guid resourceTypeId, out ResourceTypeState resource);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSystem"></param>
        /// <param name="resourceCode"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        bool TryGetResource(AppSystemState appSystem, string resourceCode, out ResourceTypeState resource);
    }
}
