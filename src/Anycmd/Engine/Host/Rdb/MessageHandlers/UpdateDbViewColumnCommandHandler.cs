
namespace Anycmd.Engine.Host.Rdb.MessageHandlers
{
    using Commands;
    using Engine.Rdb;
    using Engine.Rdb.Messages;
    using Exceptions;

    public class UpdateDbViewColumnCommandHandler : CommandHandler<UpdateDbViewColumnCommand>
    {
        private readonly IAcDomain _host;

        public UpdateDbViewColumnCommandHandler(IAcDomain host)
        {
            this._host = host;
        }

        public override void Handle(UpdateDbViewColumnCommand command)
        {
            RdbDescriptor db;
            if (!_host.Rdbs.TryDb(command.Input.DatabaseId, out db))
            {
                throw new ValidationException("意外的数据库Id");
            }
            _host.GetRequiredService<IRdbMetaDataService>().CrudDescription(db, RDbMetaDataType.ViewColumn, command.Input.Id, command.Input.Description);
        }
    }
}
