
namespace Anycmd.Specifications
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// 表示扩展 Expression[Func[T, bool]] 类型。
    /// 这是解决当使用Anycmd规约时在Entity Framework中遇到的表达式参数问题的解决方案的一部分。 
    /// 更多信息请参见 http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx.
    /// </summary>
    public static class ExpressionFuncExtender
    {
        #region Private Methods
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 以AND语义合并表达式。
        /// </summary>
        /// <typeparam name="T">规约所应用到的对象的类型。</typeparam>
        /// <param name="first">第一个表达式。</param>
        /// <param name="second">第二个表达式。</param>
        /// <returns>合并后的表达式。</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>
        /// 以OR语义合并表达式。
        /// </summary>
        /// <typeparam name="T">规约所应用到的对象的类型。</typeparam>
        /// <param name="first">第一个表达式。</param>
        /// <param name="second">第二个表达式。</param>
        /// <returns>合并后的表达式。</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }
        #endregion
    }
}
