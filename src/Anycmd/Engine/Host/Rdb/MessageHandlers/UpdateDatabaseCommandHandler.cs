
namespace Anycmd.Engine.Host.Rdb.MessageHandlers
{
    using Commands;
    using Engine.Rdb;
    using Engine.Rdb.Messages;

    public class UpdateDatabaseCommandHandler : CommandHandler<UpdateDatabaseCommand>
    {
        private readonly IAcDomain _host;

        public UpdateDatabaseCommandHandler(IAcDomain host)
        {
            this._host = host;
        }

        public override void Handle(UpdateDatabaseCommand command)
        {
            _host.GetRequiredService<IRdbMetaDataService>().UpdateDatabase(command.Input.Id, command.Input.DataSource, command.Input.Description);
        }
    }
}
