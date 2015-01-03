
namespace Anycmd.Engine
{
    using Edi;
    using Info;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是托管属性读取器。
    /// </summary>
    public interface IManagedPropertyValues
    {
        /// <summary>
        /// 读取给定本体的托管属性集。
        /// </summary>
        /// <param name="ontology"></param>
        /// <returns></returns>
        IEnumerable<InfoItem> GetValues(OntologyDescriptor ontology);
    }
}
