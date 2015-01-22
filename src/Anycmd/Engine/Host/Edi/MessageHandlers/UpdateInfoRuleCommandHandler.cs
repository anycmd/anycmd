
namespace Anycmd.Engine.Host.Edi.MessageHandlers
{
    using Commands;
    using Engine.Edi.Messages;
    using Entities;
    using Exceptions;
    using Repositories;

    public class UpdateInfoRuleCommandHandler : CommandHandler<UpdateInfoRuleCommand>
    {
        private readonly IAcDomain _host;

        public UpdateInfoRuleCommandHandler(IAcDomain host)
        {
            this._host = host;
        }

        public override void Handle(UpdateInfoRuleCommand command)
        {
            var repository = _host.GetRequiredService<IRepository<InfoRule>>();
            var entity = repository.GetByKey(command.Input.Id);
            if (entity == null)
            {
                throw new NotExistException();
            }
            entity.IsEnabled = command.Input.IsEnabled;
            repository.Update(entity);

            repository.Update(entity);
            repository.Context.Commit();

            _host.EventBus.Publish(new InfoRuleUpdatedEvent(command.UserSession, entity, command.Input));
            _host.EventBus.Commit();
        }
    }
}
