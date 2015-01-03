
namespace Anycmd.Engine
{
    using Edi;
    using Exceptions;
    using Info;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public abstract class ManagedPropertyValues : IManagedPropertyValues
    {
        public IEnumerable<InfoItem> GetValues(OntologyDescriptor ontology)
        {
            var properties = this.GetType().GetProperties(BindingFlags.Public & BindingFlags.SetProperty);
            return properties.Select(property => GetProperty(ontology, property.Name, property.GetValue(this).ToString()));
        }

        private static InfoItem GetProperty(OntologyDescriptor ontology, string propertyCode, string value)
        {
            if (propertyCode.Equals(ontology.Ontology.Code + "Id", System.StringComparison.OrdinalIgnoreCase))
            {
                propertyCode = "Id";
            }
            ElementDescriptor element;
            if (!ontology.Elements.TryGetValue(propertyCode, out element))
            {
                throw new AnycmdException("意外的" + ontology.Ontology.Name + "实体属性编码" + propertyCode);
            }
            return InfoItem.Create(element, value);
        }
    }
}
