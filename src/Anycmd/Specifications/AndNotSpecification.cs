
namespace Anycmd.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 表示给定的对象应满足这个组合的规格的左规格但不应满足这个组合规格的右规格的规格。
    /// </summary>
    /// <typeparam name="T">规格被应用到该类型的对象。</typeparam>
    public class AndNotSpecification<T> : CompositeSpecification<T>
    {
        #region Ctor
        /// <summary>
        /// 初始化一个 <c>AndNotSpecification&lt;T&gt;</c> 类型的对象。
        /// </summary>
        /// <param name="left">左规格。</param>
        /// <param name="right">右规格。</param>
        public AndNotSpecification(ISpecification<T> left, ISpecification<T> right) : base(left, right) { }
        #endregion

        #region Public Methods
        /// <summary>
        /// 获取表达了当前规格的LINQ表达式。
        /// </summary>
        /// <returns>LINQ表达式。</returns>
        public override Expression<Func<T, bool>> GetExpression()
        {
            var bodyNot = Expression.Not(Right.GetExpression().Body);
            var bodyNotExpression = Expression.Lambda<Func<T, bool>>(bodyNot, Right.GetExpression().Parameters);

            return Left.GetExpression().And(bodyNotExpression);
        }
        #endregion
    }
}
