
namespace Anycmd.Storage
{
    using Model;
    using Specifications;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是仓库。
    /// </summary>
    public interface IStorage : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// 从仓库中获取第一个对象。
        /// </summary>
        /// <typeparam name="T">要获取的对象的.NET类型。</typeparam>
        /// <returns>获取到的对象。</returns>
        T SelectFirstOnly<T>() where T : class, new();

        /// <summary>
        /// 从仓库中获取满足给定规格的第一个对象。
        /// </summary>
        /// <typeparam name="T">要获取的对象的.NET类型。</typeparam>
        /// <param name="specification">对象规格。</param>
        /// <returns>获取到的对象。</returns>
        T SelectFirstOnly<T>(ISpecification<T> specification) where T : class, new();

        /// <summary>
        /// 获取仓库中给定类型对象的总记录数。
        /// </summary>
        /// <typeparam name="T">要获取的对象的.NET类型。</typeparam>
        /// <returns>给定类型的对象的总记录数。</returns>
        int GetRecordCount<T>() where T : class, new();

        /// <summary>
        /// 获取仓库中给定类型的满足给定规格的对象的总记录数。
        /// </summary>
        /// <typeparam name="T">要获取的对象的.NET类型。</typeparam>
        /// <param name="specification">对象规格。</param>
        /// <returns>给定类型的对象的总记录数。</returns>
        int GetRecordCount<T>(ISpecification<T> specification) where T : class, new();

        /// <summary>
        /// 从仓库中获取给定类型的全部对象。
        /// </summary>
        /// <typeparam name="T">要获取的对象的.NET类型。</typeparam>
        /// <returns>对象列表。</returns>
        IEnumerable<T> Select<T>() where T : class, new();

        /// <summary>
        /// 从仓库中获取给定类型的满足给定规格的全部对象。
        /// </summary>
        /// <typeparam name="T">要获取的对象的.NET类型。</typeparam>
        /// <param name="specification">对象规格。</param>
        /// <returns>对象列表。</returns>
        IEnumerable<T> Select<T>(ISpecification<T> specification) where T : class, new();

        /// <summary>
        /// 从仓库中排序和获取给定类型的满足给定规格的对象。
        /// </summary>
        /// <typeparam name="T">要获取的对象的.NET类型。</typeparam>
        /// <param name="specification">对象规格。</param>
        /// <param name="orders"><c>PropertyBag</c> 对象。</param>
        /// <param name="sortOrder">排序方向。</param>
        /// <returns>一个排序的对象列表。</returns>
        IEnumerable<T> Select<T>(ISpecification<T> specification, PropertyBag orders, SortOrder sortOrder) where T : class, new();

        /// <summary>
        /// 往仓库中插入给定类型的对象。
        /// </summary>
        /// <typeparam name="T">被插入的对象的.NET类型。</typeparam>
        /// <param name="allFields"><c>PropertyBag</c>对象。</param>
        void Insert<T>(PropertyBag allFields) where T : class, new();

        /// <summary>
        /// 从仓库中删除给定类型的全部对象。
        /// </summary>
        /// <typeparam name="T">被删除的对象的.NET类型。</typeparam>
        void Delete<T>() where T : class, new();

        /// <summary>
        /// 从仓库中删除满足给定规格的对象。
        /// </summary>
        /// <typeparam name="T">被删除对象的类型。</typeparam>
        /// <param name="specification">对象规格。</param>
        void Delete<T>(ISpecification<T> specification) where T : class, new();

        /// <summary>
        /// 更新仓库中的给定类型的全部对象。
        /// </summary>
        /// <typeparam name="T">被更新的对象的类型。</typeparam>
        /// <param name="newValues"><c>PropertyBag</c> 对象。</param>
        void Update<T>(PropertyBag newValues) where T : class, new();

        /// <summary>
        /// 更新仓库中的给定类型的满足给定规格的全部对象。
        /// </summary>
        /// <typeparam name="T">被更新的对象的类型。</typeparam>
        /// <param name="newValues"><c>PropertyBag</c> 对象。</param>
        /// <param name="specification">对象规格。</param>
        void Update<T>(PropertyBag newValues, ISpecification<T> specification) where T : class, new();
    }
}
