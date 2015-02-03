
namespace Anycmd.Engine.Host.Edi.MessageHandlers
{
    using Commands;
    using Engine.Edi.Messages;
    using Entities;
    using Exceptions;
    using Repositories;

    public class UpdateBatchCommandHandler : CommandHandler<UpdateBatchCommand>
    {
        private readonly IAcDomain _acDomain;

        public UpdateBatchCommandHandler(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public override void Handle(UpdateBatchCommand command)
        {
            var batchRepository = _acDomain.RetrieveRequiredService<IRepository<Batch>>();
            var entity = batchRepository.GetByKey(command.Input.Id);
            if (entity == null)
            {
                throw new NotExistException();
            }

            entity.Update(command.Input);

            batchRepository.Update(entity);
            batchRepository.Context.Commit();

            _acDomain.EventBus.Publish(new BatchUpdatedEvent(command.AcSession, entity));
            _acDomain.EventBus.Commit();
        }
    }
}
