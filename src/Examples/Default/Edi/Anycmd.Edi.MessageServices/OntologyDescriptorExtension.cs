
namespace Anycmd.Edi.MessageServices
{
    using Engine.Edi;
    using ServiceModel.Types;
    using System.Linq;

    /// <summary>
    /// 提供<see cref="OntologyDescriptor"/>转换为<see cref="OntologyData"/>的方法。
    /// </summary>
    public static class OntologyDescriptorExtension
    {
        /// <summary>
        /// 转换为数据传输对象。
        /// </summary>
        /// <param name="ontology"></param>
        /// <returns></returns>
        public static OntologyData ToOntologyData(this OntologyDescriptor ontology)
        {
            if (ontology == null)
            {
                return null;
            }
            var ontologyData = new OntologyData()
            {
                Code = ontology.Ontology.Code,
                Name = ontology.Ontology.Name,
                IsSystem = ontology.Ontology.IsSystem
            };
            if (ontology.Actions != null && ontology.Actions.Count > 0)
            {
                foreach (var item in ontology.Actions.Values.OrderBy(a => a.Verb))
                {
                    ontologyData.Actions.Add(new ActionData
                    {
                        Verb = item.Verb,
                        Name = item.Name
                    });
                }
            }
            if (ontology.Elements != null && ontology.Elements.Count > 0)
            {
                foreach (var item in ontology.Elements.Values.OrderBy(a => a.Element.SortCode))
                {
                    if (!item.IsRuntimeElement)
                    {
                        InfoDicState infoDic = null;
                        if (item.Element.InfoDicId.HasValue)
                        {
                            ontology.Host.NodeHost.InfoDics.TryGetInfoDic(item.Element.InfoDicId.Value, out infoDic);
                        }
                        var infoDicCode = infoDic == null ? "" : infoDic.Code;
                        var infoDicName = infoDic == null ? "" : infoDic.Name;
                        var serializableElement = new ElementData()
                        {
                            Name = item.Element.Name,
                            Key = item.Element.Code,
                            Nullable = item.Element.Nullable,
                            MaxLength = item.Element.MaxLength,
                            OType = item.Element.OType,
                            ValueDic = infoDicCode,
                            IsEnabled = item.Element.IsEnabled
                        };
                        ontologyData.Elements.Add(serializableElement);
                    }
                }
            }

            return ontologyData;
        }
    }
}
