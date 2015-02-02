
namespace Anycmd.Engine.Host.Ac
{
    using Engine.Ac.Messages.Identity;
    using Engine.Ac.Messages.Infra;
    using System;

    public static class AcDomainExtension
    {
        public static void RemoveAppSystem(this IAcDomain host, IAcSession acSession, Guid appSystemId)
        {
            host.Handle(new RemoveAppSystemCommand(acSession, appSystemId));
        }

        public static void RemoveAccount(this IAcDomain host, IAcSession acSession, Guid accountId)
        {
            host.Handle(new RemoveAccountCommand(acSession, accountId));
        }

        public static void RemoveButton(this IAcDomain host, IAcSession acSession, Guid buttonId)
        {
            host.Handle(new RemoveButtonCommand(acSession, buttonId));
        }

        public static void RemoveDic(this IAcDomain host, IAcSession acSession, Guid dicId)
        {
            host.Handle(new RemoveDicCommand(acSession, dicId));
        }

        public static void RemoveDicItem(this IAcDomain host, IAcSession acSession, Guid dicItemId)
        {
            host.Handle(new RemoveDicItemCommand(acSession, dicItemId));
        }

        public static void RemoveEntityType(this IAcDomain host, IAcSession acSession, Guid entityTypeId)
        {
            host.Handle(new RemoveEntityTypeCommand(acSession, entityTypeId));
        }
    }
}
