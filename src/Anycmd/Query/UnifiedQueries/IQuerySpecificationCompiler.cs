namespace Anycmd.Query.UnifiedQueries
{
    /// <summary>
    /// 表示该接口的实现类是查询规约编译器。
    /// </summary>
    public interface IQuerySpecificationCompiler
    {
        /// <summary>
        /// 将给定的查询规约编译成另外一种形式。
        /// </summary>
        /// <param name="querySpecification">将被编译的查询规约。</param>
        /// <returns>查询规约的另外一种形式。</returns>
        object Compile(QuerySpecification querySpecification);
    }
}
