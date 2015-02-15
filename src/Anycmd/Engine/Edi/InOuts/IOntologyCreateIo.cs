
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;
    using System;

    public interface IOntologyCreateIo : IEntityCreateInput
    {
        string Code { get; }
        string Description { get; }
        int EditHeight { get; }
        int EditWidth { get; }
        Guid EntityDatabaseId { get; }
        Guid EntityProviderId { get; }
        string EntitySchemaName { get; }
        string EntityTableName { get; }
        Guid MessageDatabaseId { get; }
        Guid MessageProviderId { get; }
        string MessageSchemaName { get; }
        string Icon { get; }
        int IsEnabled { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
