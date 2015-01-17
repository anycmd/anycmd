
namespace Anycmd.Engine.Host.Ac
{
    using Engine.Ac.Messages.Identity;
    using Engine.Ac.Messages.Infra;
    using System;

    public static class AcDomainExtension
    {
        public static void RemoveAppSystem(this IAcDomain host, Guid appSystemId)
        {
            host.Handle(new RemoveAppSystemCommand(appSystemId));
        }

        public static void RemoveAccount(this IAcDomain host, Guid accountId)
        {
            host.Handle(new RemoveAccountCommand(accountId));
        }

        public static void RemoveButton(this IAcDomain host, Guid buttonId)
        {
            host.Handle(new RemoveButtonCommand(buttonId));
        }

        public static void RemoveDic(this IAcDomain host, Guid dicId)
        {
            host.Handle(new RemoveDicCommand(dicId));
        }

        public static void RemoveDicItem(this IAcDomain host, Guid dicItemId)
        {
            host.Handle(new RemoveDicItemCommand(dicItemId));
        }

        public static void RemoveEntityType(this IAcDomain host, Guid entityTypeId)
        {
            host.Handle(new RemoveEntityTypeCommand(entityTypeId));
        }
    }
}
