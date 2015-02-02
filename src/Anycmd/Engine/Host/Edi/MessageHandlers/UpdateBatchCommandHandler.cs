
namespace Anycmd.Engine.Host.Edi.MessageHandlers
{
    using Commands;
    using Engine.Edi.Messages;
    using Entities;
    using Exceptions;
    using Repositories;

    public class UpdateBatchCommandHandler : CommandHandler<UpdateBatchCommand>
    {
        private readonly IAcDomain _host;

        public UpdateBatchCommandHandler(IAcDomain host)
        {
            this._host = host;
        }

        public override void Handle(UpdateBatchCommand command)
        {
            var batchRepository = _host.RetrieveRequiredService<IRepository<Batch>>();
            var entity = batchRepository.GetByKey(command.Input.Id);
            if (entity == null)
            {
                throw new NotExistException();
            }

            entity.Update(command.Input);

            batchRepository.Update(entity);
            batchRepository.Context.Commit();

            _host.EventBus.Publish(new BatchUpdatedEvent(command.AcSession, entity));
            _host.EventBus.Commit();
        }
    }
}
