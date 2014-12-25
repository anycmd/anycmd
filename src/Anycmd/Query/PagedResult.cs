
namespace Anycmd.Query
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 表示整个对象集中的给定的一页子集。
    /// </summary>
    /// <typeparam name="T">对象集的元素类型。</typeparam>
    public class PagedResult<T> : ICollection<T>
    {
        #region Ctor
        /// <summary>
        /// 初始化一个 <c>PagedResult</c> 类型的对象。
        /// </summary>
        public PagedResult()
        {
            this._data = new List<T>();
        }
        /// <summary>
        /// 初始化一个 <c>PagedResult</c> 类型的对象。
        /// </summary>
        /// <param name="totalRecords">整个对象集中的总记录数。</param>
        /// <param name="totalPages">总页数。</param>
        /// <param name="pageSize">页尺寸。</param>
        /// <param name="pageNumber">当前页号。</param>
        /// <param name="data">当前页中的对象集。</param>
        public PagedResult(int? totalRecords, int? totalPages, int? pageSize, int? pageNumber, IList<T> data)
        {
            this._totalRecords = totalRecords;
            this._totalPages = totalPages;
            this._pageSize = pageSize;
            this._pageNumber = pageNumber;
            this._data = data;
        }
        #endregion

        #region Public Properties
        private int? _totalRecords;
        /// <summary>
        /// 读取或设置总记录数。
        /// </summary>
        public int? TotalRecords
        {
            get { return _totalRecords; }
            set { _totalRecords = value; }
        }

        private int? _totalPages;
        /// <summary>
        /// 读取或设置总页数。
        /// </summary>
        public int? TotalPages
        {
            get { return _totalPages; }
            set { _totalPages = value; }
        }

        private int? _pageSize;
        /// <summary>
        /// 读取或设置页尺寸。
        /// </summary>
        public int? PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        private int? _pageNumber;
        /// <summary>
        /// 读取或设置页号。
        /// </summary>
        public int? PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }

        private readonly IList<T> _data;
        /// <summary>
        /// 读取当前页中的对象集。
        /// </summary>
        public IEnumerable<T> Data
        {
            get { return _data; }
        }
        #endregion

        #region ICollection<T> Members
        /// <summary>
        /// Adds an item to the System.Collections.Generic.ICollection{T}.
        /// </summary>
        /// <param name="item">The object to add to the System.Collections.Generic.ICollection{T}.</param>
        public void Add(T item)
        {
            _data.Add(item);
        }
        /// <summary>
        /// Removes all items from the System.Collections.Generic.ICollection{T}.
        /// </summary>
        public void Clear()
        {
            _data.Clear();
        }
        /// <summary>
        /// Determines whether the System.Collections.Generic.ICollection{T} contains
        /// a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.ICollection{T}.</param>
        /// <returns>true if item is found in the System.Collections.Generic.ICollection{T}; otherwise,
        /// false.</returns>
        public bool Contains(T item)
        {
            return _data.Contains(item);
        }
        /// <summary>
        /// Copies the elements of the System.Collections.Generic.ICollection{T} to an
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements
        /// copied from System.Collections.Generic.ICollection{T}. The System.Array must
        /// have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Gets the number of elements contained in the System.Collections.Generic.ICollection{T}.
        /// </summary>
        public int Count
        {
            get { return _data.Count; }
        }
        /// <summary>
        /// Gets a value indicating whether the System.Collections.Generic.ICollection{T}
        /// is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// Removes the first occurrence of a specific object from the System.Collections.Generic.ICollection{T}.
        /// </summary>
        /// <param name="item">The object to remove from the System.Collections.Generic.ICollection{T}.</param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            return _data.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A System.Collections.Generic.IEnumerator{T} that can be used to iterate through
        /// the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An System.Collections.IEnumerator object that can be used to iterate through
        /// the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        #endregion
    }
}
