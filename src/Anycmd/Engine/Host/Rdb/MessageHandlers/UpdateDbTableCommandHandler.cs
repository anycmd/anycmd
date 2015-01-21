
namespace Anycmd.Engine.Host.Rdb.MessageHandlers
{
    using Commands;
    using Engine.Rdb;
    using Engine.Rdb.Messages;
    using Exceptions;

    public class UpdateDbTableCommandHandler : CommandHandler<UpdateDbTableCommand>
    {
        private readonly IAcDomain _host;

        public UpdateDbTableCommandHandler(IAcDomain host)
        {
            this._host = host;
        }

        public override void Handle(UpdateDbTableCommand command)
        {
            RdbDescriptor db;
            if (!_host.Rdbs.TryDb(command.Input.DatabaseId, out db))
            {
                throw new ValidationException("意外的数据库Id");
            }
            _host.GetRequiredService<IRdbMetaDataService>().CrudDescription(db, RDbMetaDataType.Table, command.Input.Id, command.Input.Description);
        }
    }
}
