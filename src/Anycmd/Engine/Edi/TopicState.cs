using System;

namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Exceptions;
    using Model;

    public sealed class TopicState : StateObject<TopicState>, ITopic, IStateObject
    {
        private Guid _ontologyId;
        private readonly IAcDomain _acDomain;

        private TopicState(IAcDomain acDomain, Guid id)
            : base(id)
        {
            this._acDomain = acDomain;
        }

        public static TopicState Create(IAcDomain acDomain, ITopic topic)
        {
            if (topic == null)
            {
                throw new ArgumentNullException("topic");
            }
            return new TopicState(acDomain, topic.Id)
            {
                Code = topic.Code,
                CreateOn = topic.CreateOn,
                Description = topic.Description,
                IsAllowed = topic.IsAllowed,
                Name = topic.Name,
                OntologyId = topic.OntologyId
            };
        }

        public Guid OntologyId
        {
            get { return _ontologyId; }
            private set
            {
                OntologyDescriptor ontology;
                if (!_acDomain.NodeHost.Ontologies.TryGetOntology(value, out ontology))
                {
                    throw new ValidationException("意外的本体标识" + value);
                }
                _ontologyId = value;
            }
        }

        public string Code { get; private set; }

        public string Name { get; private set; }

        public bool IsAllowed { get; private set; }

        public string Description { get; private set; }

        public DateTime? CreateOn { get; private set; }

        protected override bool DoEquals(TopicState other)
        {
            return
                Id == other.Id &&
                OntologyId == other.OntologyId &&
                Code == other.Code &&
                Name == other.Name &&
                IsAllowed == other.IsAllowed &&
                Description == other.Description;
        }
    }
}
