
namespace Anycmd.Specifications
{
    /// <summary>
    /// 表示该接口的实现类是复合规约。
    /// </summary>
    /// <typeparam name="T">规约被应用到的对象的类型。</typeparam>
    public interface ICompositeSpecification<T> : ISpecification<T>
    {
        /// <summary>
        /// 读取左规约。
        /// </summary>
        ISpecification<T> Left { get; }

        /// <summary>
        /// 读取右规约。
        /// </summary>
        ISpecification<T> Right { get; }
    }
}
