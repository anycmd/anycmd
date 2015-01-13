
namespace Anycmd.IdGenerators
{
    /// <summary>
    /// 表示该接口的实现类是序列标识生成器。
    /// </summary>
    public interface ISequenceIdGenerator
    {
        /// <summary>
        /// 读取下一个序列标识。
        /// </summary>
        object Next { get; }
    }
}
