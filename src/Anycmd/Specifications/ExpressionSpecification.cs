
namespace Anycmd.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 表示由相应的LINQ表达式表达的规格。
    /// </summary>
    /// <typeparam name="T">规格应用到的对象的类型。</typeparam>
    internal sealed class ExpressionSpecification<T> : Specification<T>
    {
        #region Private Fields
        private readonly Expression<Func<T, bool>> _expression;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个 <c>ExpressionSpecification&lt;T&gt;</c> 类型的对象。
        /// </summary>
        /// <param name="expression">表达了当前规格的LINQ表达式。</param>
        public ExpressionSpecification(Expression<Func<T, bool>> expression)
        {
            this._expression = expression;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 读取表达了当前规格的LINQ表达式。
        /// </summary>
        /// <returns>LINQ表达式。</returns>
        public override Expression<Func<T, bool>> GetExpression()
        {
            return this._expression;
        }
        #endregion
    }
}
