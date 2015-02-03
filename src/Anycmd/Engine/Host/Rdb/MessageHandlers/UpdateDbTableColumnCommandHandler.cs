
namespace Anycmd.Engine.Host.Rdb.MessageHandlers
{
    using Commands;
    using Engine.Rdb;
    using Engine.Rdb.Messages;
    using Exceptions;

    public class UpdateDbTableColumnCommandHandler : CommandHandler<UpdateDbTableColumnCommand>
    {
        private readonly IAcDomain _acDomain;

        public UpdateDbTableColumnCommandHandler(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public override void Handle(UpdateDbTableColumnCommand command)
        {
            RdbDescriptor db;
            if (!_acDomain.Rdbs.TryDb(command.Input.DatabaseId, out db))
            {
                throw new ValidationException("意外的数据库Id");
            }
            _acDomain.GetRequiredService<IRdbMetaDataService>().CrudDescription(db, RDbMetaDataType.TableColumn, command.Input.Id, command.Input.Description);
        }
    }
}
