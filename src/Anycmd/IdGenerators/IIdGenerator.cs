
namespace Anycmd.IdGenerators
{
    /// <summary>
    /// 表示实现该接口的类是标识生成器。
    /// </summary>
    public interface IIdGenerator
    {
        /// <summary>
        /// 生成标识。
        /// </summary>
        /// <returns>生成的标识。</returns>
        object Generate();
    }
}
