
namespace Anycmd.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 表示给定的对象至少满足左右两个规约中的一个规约的组合规约。
    /// </summary>
    /// <typeparam name="T">规约被应用到的对象的类型。</typeparam>
    public class OrSpecification<T> : CompositeSpecification<T>
    {
        #region Ctor
        /// <summary>
        /// 初始化一个 <c>OrSpecification&lt;T&gt;</c> 类型的规约。
        /// </summary>
        /// <param name="left">左规约。</param>
        /// <param name="right">右规约。</param>
        public OrSpecification(ISpecification<T> left, ISpecification<T> right) : base(left, right) { }
        #endregion

        #region Public Methods
        /// <summary>
        /// 获取表达了当前规约的LINQ表达式。
        /// </summary>
        /// <returns>LINQ表达式。</returns>
        public override Expression<Func<T, bool>> GetExpression()
        {
            //var body = Expression.OrElse(Left.GetExpression().Body, Right.GetExpression().Body);
            //return Expression.Lambda<Func<T, bool>>(body, Left.GetExpression().Parameters);
            return Left.GetExpression().Or(Right.GetExpression());
        }
        #endregion
    }
}
