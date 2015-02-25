
namespace Anycmd.Engine.Ac.Abstractions
{
    using System;
    using System.Collections.Generic;
    using Util;

    /// <summary>
    /// 表示该接口的实现类是系统实体类型集。
    /// </summary>
    public interface IEntityTypeSet : IEnumerable<EntityTypeState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityTypeId"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        bool TryGetEntityType(Guid entityTypeId, out EntityTypeState entityType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        bool TryGetEntityType(Coder code, out EntityTypeState entityType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        bool TryGetProperty(Guid propertyId, out PropertyState property);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="propertyCode"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        bool TryGetProperty(EntityTypeState entityType, string propertyCode, out PropertyState property);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        IReadOnlyDictionary<string, PropertyState> GetProperties(EntityTypeState entityType);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<PropertyState> GetProperties();
    }
}
