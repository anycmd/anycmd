
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using Engine.Edi;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public partial class OntologyTr
    {
        public OntologyTr() { }

        public static OntologyTr Create(OntologyDescriptor ontology)
        {
            return new OntologyTr
            {
                CanAction = ontology.Ontology.CanAction,
                CanCommand = ontology.Ontology.CanCommand,
                CanEvent = ontology.Ontology.CanEvent,
                Code = ontology.Ontology.Code,
                CreateOn = ontology.Ontology.CreateOn,
                DispatcherLoadCount = ontology.Ontology.DispatcherLoadCount,
                DispatcherSleepTimeSpan = ontology.Ontology.DispatcherSleepTimeSpan,
                EntityDatabaseId = ontology.Ontology.EntityDatabaseId,
                EntitySchemaName = ontology.Ontology.EntitySchemaName,
                EntityTableName = ontology.Ontology.EntityTableName,
                ExecutorLoadCount = ontology.Ontology.ExecutorLoadCount,
                ExecutorSleepTimeSpan = ontology.Ontology.ExecutorSleepTimeSpan,
                Icon = ontology.Ontology.Icon,
                Id = ontology.Ontology.Id,
                IsEnabled = ontology.Ontology.IsEnabled,
                IsLogicalDeletionEntity = ontology.Ontology.IsLogicalDeletionEntity,
                IsCataloguedEntity = ontology.Ontology.IsCataloguedEntity,
                IsSystem = ontology.Ontology.IsSystem,
                MessageDatabaseId = ontology.Ontology.MessageDatabaseId,
                MessageSchemaName = ontology.Ontology.MessageSchemaName,
                Name = ontology.Ontology.Name,
                ServiceIsAlive = ontology.Ontology.ServiceIsAlive,
                SortCode = ontology.Ontology.SortCode
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ServiceIsAlive { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid EntityDatabaseId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSystem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsCataloguedEntity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsLogicalDeletionEntity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid MessageDatabaseId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EntitySchemaName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MessageSchemaName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EntityTableName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CanAction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CanCommand { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CanEvent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int EditHeight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int EditWidth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ExecutorLoadCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DispatcherLoadCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ExecutorSleepTimeSpan { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DispatcherSleepTimeSpan { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }
    }
}
