
namespace Anycmd.Engine.Host.Edi.MessageHandlers
{
    using Commands;
    using Entities;
    using Exceptions;
    using Messages;
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
            var batchRepository = _host.GetRequiredService<IRepository<Batch>>();
            var entity = batchRepository.GetByKey(command.Output.Id);
            if (entity == null)
            {
                throw new NotExistException();
            }

            entity.Update(command.Output);

            batchRepository.Update(entity);
            batchRepository.Context.Commit();

            _host.EventBus.Publish(new BatchUpdatedEvent(entity));
            _host.EventBus.Commit();
        }
    }
}
