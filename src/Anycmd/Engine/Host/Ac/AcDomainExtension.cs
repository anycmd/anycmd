
namespace Anycmd.Engine.Host.Ac
{
    using Engine.Ac.Accounts;
    using Engine.Ac.AppSystems;
    using Engine.Ac.EntityTypes;
    using Engine.Ac.UiViews;
    using System;

    public static class AcDomainExtension
    {
        public static void RemoveAppSystem(this IAcDomain acDomain, IAcSession acSession, Guid appSystemId)
        {
            acDomain.Handle(new RemoveAppSystemCommand(acSession, appSystemId));
        }

        public static void RemoveAccount(this IAcDomain acDomain, IAcSession acSession, Guid accountId)
        {
            acDomain.Handle(new RemoveAccountCommand(acSession, accountId));
        }

        public static void RemoveButton(this IAcDomain acDomain, IAcSession acSession, Guid buttonId)
        {
            acDomain.Handle(new RemoveButtonCommand(acSession, buttonId));
        }

        public static void RemoveEntityType(this IAcDomain acDomain, IAcSession acSession, Guid entityTypeId)
        {
            acDomain.Handle(new RemoveEntityTypeCommand(acSession, entityTypeId));
        }
    }
}
