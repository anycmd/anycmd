
namespace Anycmd.Specifications
{
    /// <summary>
    /// 表示该接口的实现类是将规约解析为领域特定条件对象的解析器, 比如NHibernate中的 <c>ICriteria</c>类型对象.
    /// </summary>
    /// <typeparam name="TCriteria">领域特定条件对象。</typeparam>
    public interface ISpecificationParser<out TCriteria>
    {
        /// <summary>
        /// 将给定的规约解析为领域特定条件对象。
        /// </summary>
        /// <typeparam name="T">规约被应用到的对象的类型。</typeparam>
        /// <param name="specification">给定的规约对象。</param>
        /// <returns>领域特定条件对象。</returns>
        TCriteria Parse<T>(ISpecification<T> specification);
    }
}
