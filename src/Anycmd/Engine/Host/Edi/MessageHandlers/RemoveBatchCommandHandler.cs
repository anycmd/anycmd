
namespace Anycmd.Engine.Host.Edi.MessageHandlers
{
    using Commands;
    using Engine.Edi.Messages;
    using Entities;
    using Repositories;

    public class RemoveBatchCommandHandler : CommandHandler<RemoveBatchCommand>
    {
        private readonly IAcDomain _acDomain;

        public RemoveBatchCommandHandler(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public override void Handle(RemoveBatchCommand command)
        {
            var batchRepository = _acDomain.RetrieveRequiredService<IRepository<Batch>>();
            var entity = batchRepository.GetByKey(command.EntityId);
            if (entity == null)
            {
                return;
            }
            batchRepository.Remove(entity);
            batchRepository.Context.Commit();

            _acDomain.PublishEvent(new BatchRemovedEvent(command.AcSession, entity));
            _acDomain.CommitEventBus();
        }
    }
}
