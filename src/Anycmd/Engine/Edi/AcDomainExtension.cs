
namespace Anycmd.Engine.Edi
{
    using InOuts;
    using Messages;
    using System;

    public static class AcDomainExtension
    {
        #region Edi
        public static void AddArchive(this IAcDomain host, IAcSession userSession, IArchiveCreateIo input)
        {
            host.Handle(new AddArchiveCommand(userSession, input));
        }
        public static void UpdateArchive(this IAcDomain host, IAcSession userSession, IArchiveUpdateIo input)
        {
            host.Handle(new UpdateArchiveCommand(userSession, input));
        }
        public static void RemoveArchive(this IAcDomain host, IAcSession userSession, Guid archiveId)
        {
            host.Handle(new RemoveArchiveCommand(userSession, archiveId));
        }

        public static void AddBatch(this IAcDomain host, IAcSession userSession, IBatchCreateIo input)
        {
            host.Handle(new AddBatchCommand(userSession, input));
        }
        public static void UpdateBatch(this IAcDomain host, IAcSession userSession, IBatchUpdateIo input)
        {
            host.Handle(new UpdateBatchCommand(userSession, input));
        }
        public static void RemoveBatch(this IAcDomain host, IAcSession userSession, Guid batchId)
        {
            host.Handle(new RemoveBatchCommand(userSession, batchId));
        }

        public static void AddElement(this IAcDomain host, IAcSession userSession, IElementCreateIo input)
        {
            host.Handle(new AddElementCommand(userSession, input));
        }
        public static void UpdateElement(this IAcDomain host, IAcSession userSession, IElementUpdateIo input)
        {
            host.Handle(new UpdateElementCommand(userSession, input));
        }
        public static void RemoveElement(this IAcDomain host, IAcSession userSession, Guid elementId)
        {
            host.Handle(new RemoveElementCommand(userSession, elementId));
        }

        public static void AddInfoDic(this IAcDomain host, IAcSession userSession, IInfoDicCreateIo input)
        {
            host.Handle(new AddInfoDicCommand(userSession, input));
        }
        public static void UpdateInfoDic(this IAcDomain host, IAcSession userSession, IInfoDicUpdateIo input)
        {
            host.Handle(new UpdateInfoDicCommand(userSession, input));
        }
        public static void RemoveInfoDic(this IAcDomain host, IAcSession userSession, Guid infoDicId)
        {
            host.Handle(new RemoveInfoDicCommand(userSession, infoDicId));
        }

        public static void AddInfoDicItem(this IAcDomain host, IAcSession userSession, IInfoDicItemCreateIo input)
        {
            host.Handle(new AddInfoDicItemCommand(userSession, input));
        }
        public static void UpdateInfoDicItem(this IAcDomain host, IAcSession userSession, IInfoDicItemUpdateIo input)
        {
            host.Handle(new UpdateInfoDicItemCommand(userSession, input));
        }
        public static void RemoveInfoDicItem(this IAcDomain host, IAcSession userSession, Guid infoDicItemId)
        {
            host.Handle(new RemoveInfoDicItemCommand(userSession, infoDicItemId));
        }

        public static void AddNode(this IAcDomain host, IAcSession userSession, INodeCreateIo input)
        {
            host.Handle(new AddNodeCommand(userSession, input));
        }
        public static void UpdateNode(this IAcDomain host, IAcSession userSession, INodeUpdateIo input)
        {
            host.Handle(new UpdateNodeCommand(userSession, input));
        }
        public static void RemoveNode(this IAcDomain host, IAcSession userSession, Guid nodeId)
        {
            host.Handle(new RemoveNodeCommand(userSession, nodeId));
        }

        public static void RemoveNodeOntologyCare(this IAcDomain host, IAcSession userSession, Guid nodeOntologyCareId)
        {
            host.Handle(new RemoveNodeOntologyCareCommand(userSession, nodeOntologyCareId));
        }

        public static void AddNodeOntologyCare(this IAcDomain host, IAcSession userSession, INodeOntologyCareCreateIo input)
        {
            host.Handle(new AddNodeOntologyCareCommand(userSession, input));
        }

        public static void RemoveNodeElementCare(this IAcDomain host, IAcSession userSession, Guid nodeElementCareId)
        {
            host.Handle(new RemoveNodeElementCareCommand(userSession, nodeElementCareId));
        }

        public static void AddNodeElementCare(this IAcDomain host, IAcSession userSession, INodeElementCareCreateIo input)
        {
            host.Handle(new AddNodeElementCareCommand(userSession, input));
        }

        public static void AddOntology(this IAcDomain host, IAcSession userSession, IOntologyCreateIo input)
        {
            host.Handle(new AddOntologyCommand(userSession, input));
        }
        public static void UpdateOntology(this IAcDomain host, IAcSession userSession, IOntologyUpdateIo input)
        {
            host.Handle(new UpdateOntologyCommand(userSession, input));
        }
        public static void RemoveOntology(this IAcDomain host, IAcSession userSession, Guid ontologyId)
        {
            host.Handle(new RemoveOntologyCommand(userSession, ontologyId));
        }

        public static void AddOntologyCatalog(this IAcDomain host, IAcSession userSession, IOntologyCatalogCreateIo input)
        {
            host.Handle(new AddOntologyCatalogCommand(userSession, input));
        }

        public static void RemoveOntologyCatalog(this IAcDomain host, IAcSession userSession, Guid ontologyId, Guid catalogId)
        {
            host.Handle(new RemoveOntologyCatalogCommand(userSession, ontologyId, catalogId));
        }

        public static void AddInfoGroup(this IAcDomain host, IAcSession userSession, IInfoGroupCreateIo input)
        {
            host.Handle(new AddInfoGroupCommand(userSession, input));
        }

        public static void UpdateInfoGroup(this IAcDomain host, IAcSession userSession, IInfoGroupUpdateIo input)
        {
            host.Handle(new UpdateInfoGroupCommand(userSession, input));
        }

        public static void RemoveInfoGroup(this IAcDomain host, IAcSession userSession, Guid infoGroupId)
        {
            host.Handle(new RemoveInfoGroupCommand(userSession, infoGroupId));
        }

        public static void AddAction(this IAcDomain host, IAcSession userSession, IActionCreateIo input)
        {
            host.Handle(new AddActionCommand(userSession, input));
        }

        public static void UpdateAction(this IAcDomain host, IAcSession userSession, IActionUpdateIo input)
        {
            host.Handle(new UpdateActionCommand(userSession, input));
        }

        public static void RemoveAction(this IAcDomain host, IAcSession userSession, Guid actionId)
        {
            host.Handle(new RemoveActionCommand(userSession, actionId));
        }

        public static void AddTopic(this IAcDomain host, IAcSession userSession, ITopicCreateIo input)
        {
            host.Handle(new AddTopicCommand(userSession, input));
        }

        public static void UpdateTopic(this IAcDomain host, IAcSession userSession, ITopicUpdateIo input)
        {
            host.Handle(new UpdateTopicCommand(userSession, input));
        }

        public static void RemoveTopic(this IAcDomain host, IAcSession userSession, Guid topicId)
        {
            host.Handle(new RemoveTopicCommand(userSession, topicId));
        }

        public static void AddProcess(this IAcDomain host, IAcSession userSession, IProcessCreateIo input)
        {
            host.Handle(new AddProcessCommand(userSession, input));
        }

        public static void UpdateProcess(this IAcDomain host, IAcSession userSession, IProcessUpdateIo input)
        {
            host.Handle(new UpdateProcessCommand(userSession, input));
        }
        #endregion
    }
}
