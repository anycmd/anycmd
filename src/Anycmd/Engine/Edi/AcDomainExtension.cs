
namespace Anycmd.Engine.Edi
{
    using InOuts;
    using Messages;
    using System;

    public static class AcDomainExtension
    {
        #region Edi
        public static void AddArchive(this IAcDomain host, IAcSession acSession, IArchiveCreateIo input)
        {
            host.Handle(new AddArchiveCommand(acSession, input));
        }
        public static void UpdateArchive(this IAcDomain host, IAcSession acSession, IArchiveUpdateIo input)
        {
            host.Handle(new UpdateArchiveCommand(acSession, input));
        }
        public static void RemoveArchive(this IAcDomain host, IAcSession acSession, Guid archiveId)
        {
            host.Handle(new RemoveArchiveCommand(acSession, archiveId));
        }

        public static void AddBatch(this IAcDomain host, IAcSession acSession, IBatchCreateIo input)
        {
            host.Handle(new AddBatchCommand(acSession, input));
        }
        public static void UpdateBatch(this IAcDomain host, IAcSession acSession, IBatchUpdateIo input)
        {
            host.Handle(new UpdateBatchCommand(acSession, input));
        }
        public static void RemoveBatch(this IAcDomain host, IAcSession acSession, Guid batchId)
        {
            host.Handle(new RemoveBatchCommand(acSession, batchId));
        }

        public static void AddElement(this IAcDomain host, IAcSession acSession, IElementCreateIo input)
        {
            host.Handle(new AddElementCommand(acSession, input));
        }
        public static void UpdateElement(this IAcDomain host, IAcSession acSession, IElementUpdateIo input)
        {
            host.Handle(new UpdateElementCommand(acSession, input));
        }
        public static void RemoveElement(this IAcDomain host, IAcSession acSession, Guid elementId)
        {
            host.Handle(new RemoveElementCommand(acSession, elementId));
        }

        public static void AddInfoDic(this IAcDomain host, IAcSession acSession, IInfoDicCreateIo input)
        {
            host.Handle(new AddInfoDicCommand(acSession, input));
        }
        public static void UpdateInfoDic(this IAcDomain host, IAcSession acSession, IInfoDicUpdateIo input)
        {
            host.Handle(new UpdateInfoDicCommand(acSession, input));
        }
        public static void RemoveInfoDic(this IAcDomain host, IAcSession acSession, Guid infoDicId)
        {
            host.Handle(new RemoveInfoDicCommand(acSession, infoDicId));
        }

        public static void AddInfoDicItem(this IAcDomain host, IAcSession acSession, IInfoDicItemCreateIo input)
        {
            host.Handle(new AddInfoDicItemCommand(acSession, input));
        }
        public static void UpdateInfoDicItem(this IAcDomain host, IAcSession acSession, IInfoDicItemUpdateIo input)
        {
            host.Handle(new UpdateInfoDicItemCommand(acSession, input));
        }
        public static void RemoveInfoDicItem(this IAcDomain host, IAcSession acSession, Guid infoDicItemId)
        {
            host.Handle(new RemoveInfoDicItemCommand(acSession, infoDicItemId));
        }

        public static void AddNode(this IAcDomain host, IAcSession acSession, INodeCreateIo input)
        {
            host.Handle(new AddNodeCommand(acSession, input));
        }
        public static void UpdateNode(this IAcDomain host, IAcSession acSession, INodeUpdateIo input)
        {
            host.Handle(new UpdateNodeCommand(acSession, input));
        }
        public static void RemoveNode(this IAcDomain host, IAcSession acSession, Guid nodeId)
        {
            host.Handle(new RemoveNodeCommand(acSession, nodeId));
        }

        public static void RemoveNodeOntologyCare(this IAcDomain host, IAcSession acSession, Guid nodeOntologyCareId)
        {
            host.Handle(new RemoveNodeOntologyCareCommand(acSession, nodeOntologyCareId));
        }

        public static void AddNodeOntologyCare(this IAcDomain host, IAcSession acSession, INodeOntologyCareCreateIo input)
        {
            host.Handle(new AddNodeOntologyCareCommand(acSession, input));
        }

        public static void RemoveNodeElementCare(this IAcDomain host, IAcSession acSession, Guid nodeElementCareId)
        {
            host.Handle(new RemoveNodeElementCareCommand(acSession, nodeElementCareId));
        }

        public static void AddNodeElementCare(this IAcDomain host, IAcSession acSession, INodeElementCareCreateIo input)
        {
            host.Handle(new AddNodeElementCareCommand(acSession, input));
        }

        public static void AddOntology(this IAcDomain host, IAcSession acSession, IOntologyCreateIo input)
        {
            host.Handle(new AddOntologyCommand(acSession, input));
        }
        public static void UpdateOntology(this IAcDomain host, IAcSession acSession, IOntologyUpdateIo input)
        {
            host.Handle(new UpdateOntologyCommand(acSession, input));
        }
        public static void RemoveOntology(this IAcDomain host, IAcSession acSession, Guid ontologyId)
        {
            host.Handle(new RemoveOntologyCommand(acSession, ontologyId));
        }

        public static void AddOntologyCatalog(this IAcDomain host, IAcSession acSession, IOntologyCatalogCreateIo input)
        {
            host.Handle(new AddOntologyCatalogCommand(acSession, input));
        }

        public static void RemoveOntologyCatalog(this IAcDomain host, IAcSession acSession, Guid ontologyId, Guid catalogId)
        {
            host.Handle(new RemoveOntologyCatalogCommand(acSession, ontologyId, catalogId));
        }

        public static void AddInfoGroup(this IAcDomain host, IAcSession acSession, IInfoGroupCreateIo input)
        {
            host.Handle(new AddInfoGroupCommand(acSession, input));
        }

        public static void UpdateInfoGroup(this IAcDomain host, IAcSession acSession, IInfoGroupUpdateIo input)
        {
            host.Handle(new UpdateInfoGroupCommand(acSession, input));
        }

        public static void RemoveInfoGroup(this IAcDomain host, IAcSession acSession, Guid infoGroupId)
        {
            host.Handle(new RemoveInfoGroupCommand(acSession, infoGroupId));
        }

        public static void AddAction(this IAcDomain host, IAcSession acSession, IActionCreateIo input)
        {
            host.Handle(new AddActionCommand(acSession, input));
        }

        public static void UpdateAction(this IAcDomain host, IAcSession acSession, IActionUpdateIo input)
        {
            host.Handle(new UpdateActionCommand(acSession, input));
        }

        public static void RemoveAction(this IAcDomain host, IAcSession acSession, Guid actionId)
        {
            host.Handle(new RemoveActionCommand(acSession, actionId));
        }

        public static void AddTopic(this IAcDomain host, IAcSession acSession, ITopicCreateIo input)
        {
            host.Handle(new AddTopicCommand(acSession, input));
        }

        public static void UpdateTopic(this IAcDomain host, IAcSession acSession, ITopicUpdateIo input)
        {
            host.Handle(new UpdateTopicCommand(acSession, input));
        }

        public static void RemoveTopic(this IAcDomain host, IAcSession acSession, Guid topicId)
        {
            host.Handle(new RemoveTopicCommand(acSession, topicId));
        }

        public static void AddProcess(this IAcDomain host, IAcSession acSession, IProcessCreateIo input)
        {
            host.Handle(new AddProcessCommand(acSession, input));
        }

        public static void UpdateProcess(this IAcDomain host, IAcSession acSession, IProcessUpdateIo input)
        {
            host.Handle(new UpdateProcessCommand(acSession, input));
        }
        #endregion
    }
}
