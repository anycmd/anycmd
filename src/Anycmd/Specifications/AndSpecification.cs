
namespace Anycmd.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 表示它的左右规格都应被满足的组合规格。
    /// </summary>
    /// <typeparam name="T">规格被应用到的对象的类型。</typeparam>
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        #region Ctor
        /// <summary>
        /// 初始化一个 <c>AndSpecification&lt;T&gt;</c> 类型的对象。
        /// </summary>
        /// <param name="left">左规格。</param>
        /// <param name="right">右规格。</param>
        public AndSpecification(ISpecification<T> left, ISpecification<T> right) : base(left, right) { }
        #endregion

        #region Public Methods
        /// <summary>
        /// 获取表达了当前规格的LINQ表达式。
        /// </summary>
        /// <returns>LINQ表达式。</returns>
        public override Expression<Func<T, bool>> GetExpression()
        {
            return Left.GetExpression().And(Right.GetExpression());
        }
        #endregion
    }
}
