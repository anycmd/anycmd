
namespace Anycmd.Storage.Builders
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 表示该接口的实现类是建造sql风格的where子句的建造器。
    /// </summary>
    /// <typeparam name="T">对象类型，它通常被映射为关系数据库的表。</typeparam>
    public interface IWhereClauseBuilder<T>
        where T : class, new()
    {
        /// <summary>
        /// 基于给定的表达式对象构建where子句。
        /// </summary>
        /// <param name="expression">表达式对象。</param>
        /// <returns>包含建造结果的<c>Anycmd.Storage.Builders.WhereClauseBuildResult</c> where子句对象实例。</returns>
        WhereClauseBuildResult BuildWhereClause(Expression<Func<T, bool>> expression);
    }
}
