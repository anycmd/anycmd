
namespace Anycmd.Engine.Host.Rdb.MessageHandlers
{
    using Commands;
    using Engine.Rdb;
    using Engine.Rdb.Messages;

    public class UpdateDatabaseCommandHandler : CommandHandler<UpdateDatabaseCommand>
    {
        private readonly IAcDomain _acDomain;

        public UpdateDatabaseCommandHandler(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public override void Handle(UpdateDatabaseCommand command)
        {
            _acDomain.GetRequiredService<IRdbMetaDataService>().UpdateDatabase(command.Input.Id, command.Input.DataSource, command.Input.Description);
        }
    }
}
