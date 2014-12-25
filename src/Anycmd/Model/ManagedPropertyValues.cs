
using System.Linq;

namespace Anycmd.Model
{
    using Engine.Edi;
    using Engine.Host.Info;
    using Exceptions;
    using System.Collections.Generic;
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
                throw new CoreException("意外的" + ontology.Ontology.Name + "实体属性编码" + propertyCode);
            }
            return InfoItem.Create(element, value);
        }
    }
}
