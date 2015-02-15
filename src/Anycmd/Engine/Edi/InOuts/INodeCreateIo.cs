
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;
    using System;

    public interface INodeCreateIo : IEntityCreateInput
    {
        string Abstract { get; }
        string AnycmdApiAddress { get; }
        string AnycmdWsAddress { get; }
        int? BeatPeriod { get; }
        string Code { get; }
        string Description { get; }
        string Email { get; }
        string Icon { get; }
        int IsEnabled { get; }
        string Mobile { get; }
        string Name { get; }
        string Catalog { get; }
        string PublicKey { get; }
        string Qq { get; }
        string SecretKey { get; }
        int SortCode { get; }
        string Steward { get; }
        string Telephone { get; }
        Guid TransferId { get; }
    }
}
