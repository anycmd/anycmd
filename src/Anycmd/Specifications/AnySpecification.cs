
namespace Anycmd.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 表示在任何情况下给定的对象都会满足的规格。
    /// </summary>
    /// <typeparam name="T">规格被应用到的对象的类型。</typeparam>
    public sealed class AnySpecification<T> : Specification<T>
    {
        #region Public Methods
        /// <summary>
        /// 获取表达了当前规格的LINQ表达式。
        /// </summary>
        /// <returns>LINQ表达式。</returns>
        public override Expression<Func<T, bool>> GetExpression()
        {
            return o => true;
        }
        #endregion
    }
}
