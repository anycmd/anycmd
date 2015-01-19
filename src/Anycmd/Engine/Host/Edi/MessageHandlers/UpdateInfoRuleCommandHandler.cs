
namespace Anycmd.Engine.Host.Edi.MessageHandlers
{
    using Commands;
    using Engine.Edi.Messages;
    using Entities;
    using Exceptions;
    using Repositories;

    public class UpdateInfoRuleCommandHandler: CommandHandler<UpdateInfoRuleCommand>
    {
        private readonly IAcDomain _host;

        public UpdateInfoRuleCommandHandler(IAcDomain host)
        {
            this._host = host;
        }

        public override void Handle(UpdateInfoRuleCommand command)
        {
            var repository = _host.GetRequiredService<IRepository<InfoRule>>();
            var entity = repository.GetByKey(command.Output.Id);
            if (entity == null)
            {
                throw new NotExistException();
            }
            entity.IsEnabled = command.Output.IsEnabled;
            repository.Update(entity);

            repository.Update(entity);
            repository.Context.Commit();

            _host.EventBus.Publish(new InfoRuleUpdatedEvent(command.UserSession, entity, command.Output));
            _host.EventBus.Commit();
        }
    }
}
