
namespace Anycmd.Engine.Ac.Accounts
{
    using Model;
    using System;

    public interface IAcSessionEntity : IEntityBase
    {
        string AuthenticationType { get; }
        bool IsAuthenticated { get; set; }
        string LoginName { get; }
        Guid AccountId { get; }
        int IsEnabled { get; }
        string Description { get; }
    }
}
