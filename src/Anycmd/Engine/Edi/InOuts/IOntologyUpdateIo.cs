
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;
    using System;

    public interface IOntologyUpdateIo : IEntityUpdateInput
    {
        string Code { get; }
        Guid MessageDatabaseId { get; }
        string MessageSchemaName { get; }
        string Description { get; }
        int EditHeight { get; }
        int EditWidth { get; }
        Guid EntityDatabaseId { get; }
        Guid EntityProviderId { get; }
        string EntitySchemaName { get; }
        string EntityTableName { get; }
        string Icon { get; }
        int IsEnabled { get; }
        Guid MessageProviderId { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
