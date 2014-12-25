
namespace Anycmd.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 表示规约的基类。
    /// </summary>
    /// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
    public abstract class Specification<T> : ISpecification<T>
    {
        #region Public Methods
        /// <summary>
        /// 将给定的LINQ表达式评估为相应的规约。
        /// </summary>
        /// <param name="expression">将被评估的LINQ表达式。</param>
        /// <returns>和给定的LINQ表达式表达了同样的语义的规约。</returns>
        public static Specification<T> Eval(Expression<Func<T, bool>> expression)
        {
            return new ExpressionSpecification<T>(expression);
        }
        #endregion

        #region ISpecification<T> Members
        /// <summary>
        /// 返回一个 <see cref="System.Boolean"/> 值，表示给定的对象是否满足当前规格。
        /// </summary>
        /// <param name="obj">规格应用到的对象。</param>
        /// <returns>true表示满足规格。</returns>
        public virtual bool IsSatisfiedBy(T obj)
        {
            return this.GetExpression().Compile()(obj);
        }

        /// <summary>
        ///将给定的规格与当前规格合并，合并后的规格表达的是被应用对象需同时满足这两个规格。
        /// </summary>
        /// <param name="other">与当前规格对象合并的规格对象。</param>
        /// <returns>合并后的规格对象。</returns>
        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        /// <summary>
        /// 将给定的规格与当前规格合并，合并后的规格表达的是被应用对象应至少满足这两个规格其中之一。
        /// </summary>
        /// <param name="other">与当前规格对象合并的规格对象。</param>
        /// <returns>合并后的规格对象。</returns>
        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        /// <summary>
        /// 将给定的规格与当前规格合并，合并后的规格表达的是被应用对象应满足当前规格但不应满足给定的规格。
        /// </summary>
        /// <param name="other">与当前规格对象合并的规格对象。</param>
        /// <returns>合并后的规格对象。</returns>
        public ISpecification<T> AndNot(ISpecification<T> other)
        {
            return new AndNotSpecification<T>(this, other);
        }

        /// <summary>
        /// 取反当前规格。
        /// </summary>
        /// <returns>当前规格取反后的规格。</returns>
        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }

        /// <summary>
        /// 读取表达了当前规格的LINQ表达式。
        /// </summary>
        /// <returns>LINQ表达式。</returns>
        public abstract Expression<Func<T, bool>> GetExpression();
        #endregion
    }
}
