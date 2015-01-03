
namespace Anycmd.Engine
{
    using Edi;
    using Info;
    using System;

    /// <summary>
    /// 表示权限托管对象。<see cref="IManagedObject"/>
    /// </summary>
    public class ManagedObject : IManagedObject
    {
        public ManagedObject(OntologyDescriptor ontology, InfoItem[] entity, InfoItem[] inputValues)
        {
            if (ontology == null)
            {
                throw new ArgumentNullException("ontology");
            }
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this.Ontology = ontology;
            this.Entity = entity;
            this.InputValues = inputValues;
        }

        public OntologyDescriptor Ontology { get; private set; }

        public InfoItem[] Entity { get; private set; }

        public InfoItem[] InputValues { get; private set; }
    }
}
