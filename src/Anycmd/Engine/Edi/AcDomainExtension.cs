
namespace Anycmd.Engine.Edi
{
    using InOuts;
    using Messages;
    using System;

    public static class AcDomainExtension
    {
        #region Edi
        public static void AddArchive(this IAcDomain host, IArchiveCreateIo input)
        {
            host.Handle(new AddArchiveCommand(input));
        }
        public static void UpdateArchive(this IAcDomain host, IArchiveUpdateIo input)
        {
            host.Handle(new UpdateArchiveCommand(input));
        }
        public static void RemoveArchive(this IAcDomain host, Guid archiveId)
        {
            host.Handle(new RemoveArchiveCommand(archiveId));
        }

        public static void AddBatch(this IAcDomain host, IBatchCreateIo input)
        {
            host.Handle(new AddBatchCommand(input));
        }
        public static void UpdateBatch(this IAcDomain host, IBatchUpdateIo input)
        {
            host.Handle(new UpdateBatchCommand(input));
        }
        public static void RemoveBatch(this IAcDomain host, Guid batchId)
        {
            host.Handle(new RemoveBatchCommand(batchId));
        }

        public static void AddElement(this IAcDomain host, IElementCreateIo input)
        {
            host.Handle(new AddElementCommand(input));
        }
        public static void UpdateElement(this IAcDomain host, IElementUpdateIo input)
        {
            host.Handle(new UpdateElementCommand(input));
        }
        public static void RemoveElement(this IAcDomain host, Guid elementId)
        {
            host.Handle(new RemoveElementCommand(elementId));
        }

        public static void AddInfoDic(this IAcDomain host, IInfoDicCreateIo input)
        {
            host.Handle(new AddInfoDicCommand(input));
        }
        public static void UpdateInfoDic(this IAcDomain host, IInfoDicUpdateIo input)
        {
            host.Handle(new UpdateInfoDicCommand(input));
        }
        public static void RemoveInfoDic(this IAcDomain host, Guid infoDicId)
        {
            host.Handle(new RemoveInfoDicCommand(infoDicId));
        }

        public static void AddInfoDicItem(this IAcDomain host, IInfoDicItemCreateIo input)
        {
            host.Handle(new AddInfoDicItemCommand(input));
        }
        public static void UpdateInfoDicItem(this IAcDomain host, IInfoDicItemUpdateIo input)
        {
            host.Handle(new UpdateInfoDicItemCommand(input));
        }
        public static void RemoveInfoDicItem(this IAcDomain host, Guid infoDicItemId)
        {
            host.Handle(new RemoveInfoDicItemCommand(infoDicItemId));
        }

        public static void AddNode(this IAcDomain host, INodeCreateIo input)
        {
            host.Handle(new AddNodeCommand(input));
        }
        public static void UpdateNode(this IAcDomain host, INodeUpdateIo input)
        {
            host.Handle(new UpdateNodeCommand(input));
        }
        public static void RemoveNode(this IAcDomain host, Guid nodeId)
        {
            host.Handle(new RemoveNodeCommand(nodeId));
        }

        public static void RemoveNodeOntologyCare(this IAcDomain host, Guid nodeOntologyCareId)
        {
            host.Handle(new RemoveNodeOntologyCareCommand(nodeOntologyCareId));
        }

        public static void AddNodeOntologyCare(this IAcDomain host, INodeOntologyCareCreateIo input)
        {
            host.Handle(new AddNodeOntologyCareCommand(input));
        }

        public static void RemoveNodeElementCare(this IAcDomain host, Guid nodeElementCareId)
        {
            host.Handle(new RemoveNodeElementCareCommand(nodeElementCareId));
        }

        public static void AddNodeElementCare(this IAcDomain host, INodeElementCareCreateIo input)
        {
            host.Handle(new AddNodeElementCareCommand(input));
        }

        public static void AddOntology(this IAcDomain host, IOntologyCreateIo input)
        {
            host.Handle(new AddOntologyCommand(input));
        }
        public static void UpdateOntology(this IAcDomain host, IOntologyUpdateIo input)
        {
            host.Handle(new UpdateOntologyCommand(input));
        }
        public static void RemoveOntology(this IAcDomain host, Guid ontologyId)
        {
            host.Handle(new RemoveOntologyCommand(ontologyId));
        }

        public static void AddOntologyOrganization(this IAcDomain host, IOntologyOrganizationCreateIo input)
        {
            host.Handle(new AddOntologyOrganizationCommand(input));
        }

        public static void RemoveOntologyOrganization(this IAcDomain host, Guid ontologyId, Guid organizationId)
        {
            host.Handle(new RemoveOntologyOrganizationCommand(ontologyId, organizationId));
        }

        public static void AddInfoGroup(this IAcDomain host, IInfoGroupCreateIo input)
        {
            host.Handle(new AddInfoGroupCommand(input));
        }

        public static void UpdateInfoGroup(this IAcDomain host, IInfoGroupUpdateIo input)
        {
            host.Handle(new UpdateInfoGroupCommand(input));
        }

        public static void RemoveInfoGroup(this IAcDomain host, Guid infoGroupId)
        {
            host.Handle(new RemoveInfoGroupCommand(infoGroupId));
        }

        public static void AddAction(this IAcDomain host, IActionCreateIo input)
        {
            host.Handle(new AddActionCommand(input));
        }

        public static void UpdateAction(this IAcDomain host, IActionUpdateIo input)
        {
            host.Handle(new UpdateActionCommand(input));
        }

        public static void RemoveAction(this IAcDomain host, Guid actionId)
        {
            host.Handle(new RemoveActionCommand(actionId));
        }

        public static void AddTopic(this IAcDomain host, ITopicCreateIo input)
        {
            host.Handle(new AddTopicCommand(input));
        }

        public static void UpdateTopic(this IAcDomain host, ITopicUpdateIo input)
        {
            host.Handle(new UpdateTopicCommand(input));
        }

        public static void RemoveTopic(this IAcDomain host, Guid topicId)
        {
            host.Handle(new RemoveTopicCommand(topicId));
        }

        public static void AddProcess(this IAcDomain host, IProcessCreateIo input)
        {
            host.Handle(new AddProcessCommand(input));
        }

        public static void UpdateProcess(this IAcDomain host, IProcessUpdateIo input)
        {
            host.Handle(new UpdateProcessCommand(input));
        }
        #endregion
    }
}
