
namespace Anycmd.Engine.Host.Edi.MessageHandlers
{
    using Commands;
    using Engine.Edi.Messages;
    using Entities;
    using Exceptions;
    using Repositories;

    public class UpdateInfoRuleCommandHandler : CommandHandler<UpdateInfoRuleCommand>
    {
        private readonly IAcDomain _acDomain;

        public UpdateInfoRuleCommandHandler(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public override void Handle(UpdateInfoRuleCommand command)
        {
            var repository = _acDomain.GetRequiredService<IRepository<InfoRule>>();
            var entity = repository.GetByKey(command.Input.Id);
            if (entity == null)
            {
                throw new NotExistException();
            }
            entity.IsEnabled = command.Input.IsEnabled;
            repository.Update(entity);

            repository.Update(entity);
            repository.Context.Commit();

            _acDomain.EventBus.Publish(new InfoRuleUpdatedEvent(command.AcSession, entity, command.Input));
            _acDomain.EventBus.Commit();
        }
    }
}
