
namespace Anycmd.Engine.Host.Edi.MessageHandlers
{
    using Commands;
    using Engine.Edi.Messages;
    using Entities;
    using Repositories;

    public class RemoveBatchCommandHandler : CommandHandler<RemoveBatchCommand>
    {
        private readonly IAcDomain _host;

        public RemoveBatchCommandHandler(IAcDomain host)
        {
            this._host = host;
        }

        public override void Handle(RemoveBatchCommand command)
        {
            var batchRepository = _host.RetrieveRequiredService<IRepository<Batch>>();
            var entity = batchRepository.GetByKey(command.EntityId);
            if (entity == null)
            {
                return;
            }
            batchRepository.Remove(entity);
            batchRepository.Context.Commit();

            _host.PublishEvent(new BatchRemovedEvent(command.AcSession, entity));
            _host.CommitEventBus();
        }
    }
}
