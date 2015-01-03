
namespace Anycmd.Engine
{
    using Edi;
    using Info;

    /// <summary>
    /// 表示该接口的实现类是权限托管对象。
    /// </summary>
    public interface IManagedObject
    {
        /// <summary>
        /// 本体
        /// </summary>
        OntologyDescriptor Ontology { get; }

        /// <summary>
        /// 输入
        /// </summary>
        InfoItem[] InputValues { get; }

        /// <summary>
        /// 实体。实体标识在InfoItem[]中，是其一个标识为Id的InfoItem。
        /// // TODO:考虑封装InfoItem[]
        /// </summary>
        InfoItem[] Entity { get; }
    }
}
