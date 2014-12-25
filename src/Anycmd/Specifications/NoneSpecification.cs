
namespace Anycmd.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 表示任何情况下给定的对象都不满足该规约的规约。
    /// </summary>
    /// <typeparam name="T">规约被应用到的对象的类型。</typeparam>
    public sealed class NoneSpecification<T> : Specification<T>
    {
        #region Public Methods
        /// <summary>
        /// 获取表达了当前规约的表达式。
        /// </summary>
        /// <returns>LINQ表达式。</returns>
        public override Expression<Func<T, bool>> GetExpression()
        {
            return o => false;
        }
        #endregion
    }
}
