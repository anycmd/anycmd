
namespace Anycmd.Engine.Edi
{
    using InOuts;
    using Messages;
    using System;

    public static class AcDomainExtension
    {
        #region Edi
        public static void AddArchive(this IAcDomain acDomain, IAcSession acSession, IArchiveCreateIo input)
        {
            acDomain.Handle(new AddArchiveCommand(acSession, input));
        }
        public static void UpdateArchive(this IAcDomain acDomain, IAcSession acSession, IArchiveUpdateIo input)
        {
            acDomain.Handle(new UpdateArchiveCommand(acSession, input));
        }
        public static void RemoveArchive(this IAcDomain acDomain, IAcSession acSession, Guid archiveId)
        {
            acDomain.Handle(new RemoveArchiveCommand(acSession, archiveId));
        }

        public static void AddBatch(this IAcDomain acDomain, IAcSession acSession, IBatchCreateIo input)
        {
            acDomain.Handle(new AddBatchCommand(acSession, input));
        }
        public static void UpdateBatch(this IAcDomain acDomain, IAcSession acSession, IBatchUpdateIo input)
        {
            acDomain.Handle(new UpdateBatchCommand(acSession, input));
        }
        public static void RemoveBatch(this IAcDomain acDomain, IAcSession acSession, Guid batchId)
        {
            acDomain.Handle(new RemoveBatchCommand(acSession, batchId));
        }

        public static void AddElement(this IAcDomain acDomain, IAcSession acSession, IElementCreateIo input)
        {
            acDomain.Handle(new AddElementCommand(acSession, input));
        }
        public static void UpdateElement(this IAcDomain acDomain, IAcSession acSession, IElementUpdateIo input)
        {
            acDomain.Handle(new UpdateElementCommand(acSession, input));
        }
        public static void RemoveElement(this IAcDomain acDomain, IAcSession acSession, Guid elementId)
        {
            acDomain.Handle(new RemoveElementCommand(acSession, elementId));
        }

        public static void AddInfoDic(this IAcDomain acDomain, IAcSession acSession, IInfoDicCreateIo input)
        {
            acDomain.Handle(new AddInfoDicCommand(acSession, input));
        }
        public static void UpdateInfoDic(this IAcDomain acDomain, IAcSession acSession, IInfoDicUpdateIo input)
        {
            acDomain.Handle(new UpdateInfoDicCommand(acSession, input));
        }
        public static void RemoveInfoDic(this IAcDomain acDomain, IAcSession acSession, Guid infoDicId)
        {
            acDomain.Handle(new RemoveInfoDicCommand(acSession, infoDicId));
        }

        public static void AddInfoDicItem(this IAcDomain acDomain, IAcSession acSession, IInfoDicItemCreateIo input)
        {
            acDomain.Handle(new AddInfoDicItemCommand(acSession, input));
        }
        public static void UpdateInfoDicItem(this IAcDomain acDomain, IAcSession acSession, IInfoDicItemUpdateIo input)
        {
            acDomain.Handle(new UpdateInfoDicItemCommand(acSession, input));
        }
        public static void RemoveInfoDicItem(this IAcDomain acDomain, IAcSession acSession, Guid infoDicItemId)
        {
            acDomain.Handle(new RemoveInfoDicItemCommand(acSession, infoDicItemId));
        }

        public static void AddNode(this IAcDomain acDomain, IAcSession acSession, INodeCreateIo input)
        {
            acDomain.Handle(new AddNodeCommand(acSession, input));
        }
        public static void UpdateNode(this IAcDomain acDomain, IAcSession acSession, INodeUpdateIo input)
        {
            acDomain.Handle(new UpdateNodeCommand(acSession, input));
        }
        public static void RemoveNode(this IAcDomain acDomain, IAcSession acSession, Guid nodeId)
        {
            acDomain.Handle(new RemoveNodeCommand(acSession, nodeId));
        }

        public static void RemoveNodeOntologyCare(this IAcDomain acDomain, IAcSession acSession, Guid nodeOntologyCareId)
        {
            acDomain.Handle(new RemoveNodeOntologyCareCommand(acSession, nodeOntologyCareId));
        }

        public static void AddNodeOntologyCare(this IAcDomain acDomain, IAcSession acSession, INodeOntologyCareCreateIo input)
        {
            acDomain.Handle(new AddNodeOntologyCareCommand(acSession, input));
        }

        public static void RemoveNodeElementCare(this IAcDomain acDomain, IAcSession acSession, Guid nodeElementCareId)
        {
            acDomain.Handle(new RemoveNodeElementCareCommand(acSession, nodeElementCareId));
        }

        public static void AddNodeElementCare(this IAcDomain acDomain, IAcSession acSession, INodeElementCareCreateIo input)
        {
            acDomain.Handle(new AddNodeElementCareCommand(acSession, input));
        }

        public static void AddOntology(this IAcDomain acDomain, IAcSession acSession, IOntologyCreateIo input)
        {
            acDomain.Handle(new AddOntologyCommand(acSession, input));
        }
        public static void UpdateOntology(this IAcDomain acDomain, IAcSession acSession, IOntologyUpdateIo input)
        {
            acDomain.Handle(new UpdateOntologyCommand(acSession, input));
        }
        public static void RemoveOntology(this IAcDomain acDomain, IAcSession acSession, Guid ontologyId)
        {
            acDomain.Handle(new RemoveOntologyCommand(acSession, ontologyId));
        }

        public static void AddOntologyCatalog(this IAcDomain acDomain, IAcSession acSession, IOntologyCatalogCreateIo input)
        {
            acDomain.Handle(new AddOntologyCatalogCommand(acSession, input));
        }

        public static void RemoveOntologyCatalog(this IAcDomain acDomain, IAcSession acSession, Guid ontologyId, Guid catalogId)
        {
            acDomain.Handle(new RemoveOntologyCatalogCommand(acSession, ontologyId, catalogId));
        }

        public static void AddInfoGroup(this IAcDomain acDomain, IAcSession acSession, IInfoGroupCreateIo input)
        {
            acDomain.Handle(new AddInfoGroupCommand(acSession, input));
        }

        public static void UpdateInfoGroup(this IAcDomain acDomain, IAcSession acSession, IInfoGroupUpdateIo input)
        {
            acDomain.Handle(new UpdateInfoGroupCommand(acSession, input));
        }

        public static void RemoveInfoGroup(this IAcDomain acDomain, IAcSession acSession, Guid infoGroupId)
        {
            acDomain.Handle(new RemoveInfoGroupCommand(acSession, infoGroupId));
        }

        public static void AddAction(this IAcDomain acDomain, IAcSession acSession, IActionCreateIo input)
        {
            acDomain.Handle(new AddActionCommand(acSession, input));
        }

        public static void UpdateAction(this IAcDomain acDomain, IAcSession acSession, IActionUpdateIo input)
        {
            acDomain.Handle(new UpdateActionCommand(acSession, input));
        }

        public static void RemoveAction(this IAcDomain acDomain, IAcSession acSession, Guid actionId)
        {
            acDomain.Handle(new RemoveActionCommand(acSession, actionId));
        }

        public static void AddTopic(this IAcDomain acDomain, IAcSession acSession, ITopicCreateIo input)
        {
            acDomain.Handle(new AddTopicCommand(acSession, input));
        }

        public static void UpdateTopic(this IAcDomain acDomain, IAcSession acSession, ITopicUpdateIo input)
        {
            acDomain.Handle(new UpdateTopicCommand(acSession, input));
        }

        public static void RemoveTopic(this IAcDomain acDomain, IAcSession acSession, Guid topicId)
        {
            acDomain.Handle(new RemoveTopicCommand(acSession, topicId));
        }

        public static void AddProcess(this IAcDomain acDomain, IAcSession acSession, IProcessCreateIo input)
        {
            acDomain.Handle(new AddProcessCommand(acSession, input));
        }

        public static void UpdateProcess(this IAcDomain acDomain, IAcSession acSession, IProcessUpdateIo input)
        {
            acDomain.Handle(new UpdateProcessCommand(acSession, input));
        }
        #endregion
    }
}
