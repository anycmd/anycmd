
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Model;
    using System;

    public sealed class OntologyState : StateObject<OntologyState>, IOntology, IStateObject
    {
        private OntologyState(Guid id) : base(id) { }

        public static OntologyState Create(IOntology ontology)
        {
            if (ontology == null)
            {
                throw new ArgumentNullException("ontology");
            }
            return new OntologyState(ontology.Id)
            {
                CanAction = ontology.CanAction,
                CanCommand = ontology.CanCommand,
                CanEvent = ontology.CanEvent,
                Code = ontology.Code,
                CreateOn = ontology.CreateOn,
                DispatcherLoadCount = ontology.DispatcherLoadCount,
                DispatcherSleepTimeSpan = ontology.DispatcherSleepTimeSpan,
                EntityDatabaseId = ontology.EntityDatabaseId,
                EntityProviderId = ontology.EntityProviderId,
                EntitySchemaName = ontology.EntitySchemaName,
                EntityTableName = ontology.EntityTableName,
                ExecutorLoadCount = ontology.ExecutorLoadCount,
                ExecutorSleepTimeSpan = ontology.ExecutorSleepTimeSpan,
                Icon = ontology.Icon,
                IsEnabled = ontology.IsEnabled,
                IsLogicalDeletionEntity = ontology.IsLogicalDeletionEntity,
                IsCataloguedEntity = ontology.IsCataloguedEntity,
                IsSystem = ontology.IsSystem,
                MessageDatabaseId = ontology.MessageDatabaseId,
                MessageProviderId = ontology.MessageProviderId,
                MessageSchemaName = ontology.MessageSchemaName,
                Name = ontology.Name,
                ReceivedMessageBufferSize = ontology.ReceivedMessageBufferSize,
                ServiceIsAlive = ontology.ServiceIsAlive,
                SortCode = ontology.SortCode,
                Triggers = ontology.Triggers
            };
        }

        public string Code { get; private set; }

        public string Name { get; private set; }

        public string Triggers { get; private set; }

        public string Icon { get; private set; }

        public bool ServiceIsAlive { get; private set; }

        public Guid MessageProviderId { get; private set; }

        public Guid EntityProviderId { get; private set; }

        public Guid EntityDatabaseId { get; private set; }

        public bool IsSystem { get; private set; }

        public bool IsCataloguedEntity { get; private set; }

        public bool IsLogicalDeletionEntity { get; private set; }

        public Guid MessageDatabaseId { get; private set; }

        public int ReceivedMessageBufferSize { get; private set; }

        public string EntitySchemaName { get; private set; }

        public string MessageSchemaName { get; private set; }

        public string EntityTableName { get; private set; }

        public int ExecutorLoadCount { get; private set; }

        public int ExecutorSleepTimeSpan { get; private set; }

        public int DispatcherLoadCount { get; private set; }

        public int DispatcherSleepTimeSpan { get; private set; }

        public int IsEnabled { get; private set; }

        public bool CanAction { get; private set; }

        public bool CanCommand { get; private set; }

        public bool CanEvent { get; private set; }

        public int SortCode { get; private set; }

        public DateTime? CreateOn { get; private set; }

        protected override bool DoEquals(OntologyState other)
        {
            return Id == other.Id &&
                Code == other.Code &&
                Name == other.Name &&
                Triggers == other.Triggers &&
                Icon == other.Icon &&
                ServiceIsAlive == other.ServiceIsAlive &&
                MessageProviderId == other.MessageProviderId &&
                EntityProviderId == other.EntityProviderId &&
                EntityDatabaseId == other.EntityDatabaseId &&
                IsSystem == other.IsSystem &&
                IsCataloguedEntity == other.IsCataloguedEntity &&
                IsLogicalDeletionEntity == other.IsLogicalDeletionEntity &&
                MessageDatabaseId == other.MessageDatabaseId &&
                ReceivedMessageBufferSize == other.ReceivedMessageBufferSize &&
                EntitySchemaName == other.EntitySchemaName &&
                MessageSchemaName == other.MessageSchemaName &&
                EntityTableName == other.EntityTableName &&
                ExecutorLoadCount == other.ExecutorLoadCount &&
                ExecutorSleepTimeSpan == other.ExecutorSleepTimeSpan &&
                DispatcherLoadCount == other.DispatcherLoadCount &&
                DispatcherSleepTimeSpan == other.DispatcherSleepTimeSpan &&
                IsEnabled == other.IsEnabled &&
                CanAction == other.CanAction &&
                CanCommand == other.CanCommand &&
                CanEvent == other.CanEvent &&
                SortCode == other.SortCode;
        }
    }
}
