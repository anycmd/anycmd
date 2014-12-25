
namespace Anycmd.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 表示该接口的实现类是规格。关于规格模式的更多信息参见 http://martinfowler.com/apsupp/spec.pdf.
    /// </summary>
    /// <typeparam name="T">规格被应用到的对象的类型。</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// 返回一个 <see cref="System.Boolean"/> 值，表示给定的对象是否满足当前规格。
        /// </summary>
        /// <param name="obj">规格应用到的对象。</param>
        /// <returns>true表示满足规格。</returns>
        bool IsSatisfiedBy(T obj);

        /// <summary>
        ///将给定的规格与当前规格合并，合并后的规格表达的是被应用对象需同时满足这两个规格。
        /// </summary>
        /// <param name="other">与当前规格对象合并的规格对象。</param>
        /// <returns>合并后的规格对象。</returns>
        ISpecification<T> And(ISpecification<T> other);

        /// <summary>
        /// 将给定的规格与当前规格合并，合并后的规格表达的是被应用对象应至少满足这两个规格其中之一。
        /// </summary>
        /// <param name="other">与当前规格对象合并的规格对象。</param>
        /// <returns>合并后的规格对象。</returns>
        ISpecification<T> Or(ISpecification<T> other);

        /// <summary>
        /// 将给定的规格与当前规格合并，合并后的规格表达的是被应用对象应满足当前规格但不应满足给定的规格。
        /// </summary>
        /// <param name="other">与当前规格对象合并的规格对象。</param>
        /// <returns>合并后的规格对象。</returns>
        ISpecification<T> AndNot(ISpecification<T> other);

        /// <summary>
        /// 取反当前规格。
        /// </summary>
        /// <returns>当前规格取反后的规格。</returns>
        ISpecification<T> Not();

        /// <summary>
        /// 读取表达了当前规格的LINQ表达式。
        /// </summary>
        /// <returns>LINQ表达式。</returns>
        Expression<Func<T, bool>> GetExpression();
    }
}
