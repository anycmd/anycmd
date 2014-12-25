namespace Anycmd.Query.UnifiedQueries
{
    using System;

    /// <summary>
    /// 表示查询规约编译器的抽象基类。
    /// </summary>
    /// <typeparam name="T">查询规约变异后的类型</typeparam>
    public abstract class QuerySpecificationCompiler<T> : IQuerySpecificationCompiler<T>
    {
        /// <summary>
        /// 将给定的查询规约编译成另外一种形式。
        /// </summary>
        /// <param name="querySpecification">将被编译的查询规约。</param>
        /// <returns>查询规约的另外一种形式。</returns>
        public T Compile(QuerySpecification querySpecification)
        {
            try
            {
                QuerySpecificationValidator.Validate(querySpecification, true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Can't compile the given query specificaiton as the validation was failed. See InnerException for details.",
                    ex);
            }

            return this.PerformCompile(querySpecification);
        }

        /// <summary>
        /// 将给定的查询规约编译成另外一种形式。
        /// </summary>
        /// <param name="querySpecification">将被编译的查询规约。</param>
        /// <returns>查询规约的另外一种形式。</returns>
        object IQuerySpecificationCompiler.Compile(QuerySpecification querySpecification)
        {
            return this.Compile(querySpecification);
        }

        /// <summary>
        /// 将给定的查询规约编译成另外一种形式。
        /// </summary>
        /// <param name="querySpecification">将被编译的查询规约。</param>
        /// <returns>查询规约的另外一种形式。</returns>
        protected abstract T PerformCompile(QuerySpecification querySpecification);
    }
}
