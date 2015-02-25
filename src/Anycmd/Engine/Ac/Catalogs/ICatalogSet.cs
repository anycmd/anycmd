
namespace Anycmd.Engine.Ac.Catalogs
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是目录集。
    /// </summary>
    public interface ICatalogSet : IEnumerable<CatalogState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="catalog"></param>
        /// <returns></returns>
        bool TryGetCatalog(Guid catalogId, out CatalogState catalog);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogCode"></param>
        /// <param name="catalog"></param>
        /// <returns></returns>
        bool TryGetCatalog(string catalogCode, out CatalogState catalog);
    }
}
