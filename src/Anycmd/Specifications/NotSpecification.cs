
namespace Anycmd.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 表示与给定的规约相反的规约。
    /// </summary>
    /// <typeparam name="T">规约被应用到的对象的类型。</typeparam>
    public class NotSpecification<T> : Specification<T>
    {
        #region Private Fields
        private readonly ISpecification<T> _spec;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个 <c>NotSpecification&lt;T&gt;</c> 类型的对象。
        /// </summary>
        /// <param name="specification">被反义的规约对象。</param>
        public NotSpecification(ISpecification<T> specification)
        {
            this._spec = specification;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 获取表达了当前规约的LINQ表达式。
        /// </summary>
        /// <returns>LINQ表达式。</returns>
        public override Expression<Func<T, bool>> GetExpression()
        {
            var body = Expression.Not(this._spec.GetExpression().Body);
            return Expression.Lambda<Func<T, bool>>(body, this._spec.GetExpression().Parameters);
        }
        #endregion
    }
}
