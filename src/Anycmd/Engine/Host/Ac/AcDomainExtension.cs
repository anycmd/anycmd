
namespace Anycmd.Engine.Host.Ac
{
    using Identity.Messages;
    using Infra.Messages;
    using InOuts;
    using Messages;
    using System;

    public static class AcDomainExtension
    {
        public static void AddAppSystem(this IAcDomain host, IAppSystemCreateIo input)
        {
            host.Handle(new AddAppSystemCommand(input));
        }

        public static void UpdateAppSystem(this IAcDomain host, IAppSystemUpdateIo input)
        {
            host.Handle(new UpdateAppSystemCommand(input));
        }

        public static void RemoveAppSystem(this IAcDomain host, Guid appSystemId)
        {
            host.Handle(new RemoveAppSystemCommand(appSystemId));
        }

        public static void AssignPassword(this IAcDomain host, IPasswordAssignIo input)
        {
            host.Handle(new AssignPasswordCommand(input));
        }

        public static void ChangePassword(this IAcDomain host, IPasswordChangeIo input)
        {
            host.Handle(new ChangePasswordCommand(input));
        }

        public static void AddAccount(this IAcDomain host, IAccountCreateIo input)
        {
            host.Handle(new AddAccountCommand(input));
        }

        public static void UpdateAccount(this IAcDomain host, IAccountUpdateIo input)
        {
            host.Handle(new UpdateAccountCommand(input));
        }

        public static void RemoveAccount(this IAcDomain host, Guid accountId)
        {
            host.Handle(new RemoveAccountCommand(accountId));
        }

        public static void RemovePrivilegeBigram(this IAcDomain host, Guid privilegeBigramId)
        {
            host.Handle(new RemovePrivilegeBigramCommand(privilegeBigramId));
        }

        public static void UpdatePrivilegeBigram(this IAcDomain host, IPrivilegeBigramUpdateIo input)
        {
            host.Handle(new UpdatePrivilegeBigramCommand(input));
        }

        public static void AddPrivilegeBigram(this IAcDomain host, IPrivilegeBigramCreateIo input)
        {
            host.Handle(new AddPrivilegeBigramCommand(input));
        }

        public static void AddButton(this IAcDomain host, IButtonCreateIo input)
        {
            host.Handle(new AddButtonCommand(input));
        }

        public static void UpdateButton(this IAcDomain host, IButtonUpdateIo input)
        {
            host.Handle(new UpdateButtonCommand(input));
        }

        public static void RemoveButton(this IAcDomain host, Guid buttonId)
        {
            host.Handle(new RemoveButtonCommand(buttonId));
        }

        public static void AddDic(this IAcDomain host, IDicCreateIo input)
        {
            host.Handle(new AddDicCommand(input));
        }

        public static void UpdateDic(this IAcDomain host, IDicUpdateIo input)
        {
            host.Handle(new UpdateDicCommand(input));
        }

        public static void RemoveDic(this IAcDomain host, Guid dicId)
        {
            host.Handle(new RemoveDicCommand(dicId));
        }

        public static void AddDicItem(this IAcDomain host, IDicItemCreateIo input)
        {
            host.Handle(new AddDicItemCommand(input));
        }

        public static void UpdateDicItem(this IAcDomain host, IDicItemUpdateIo input)
        {
            host.Handle(new UpdateDicItemCommand(input));
        }

        public static void RemoveDicItem(this IAcDomain host, Guid dicItemId)
        {
            host.Handle(new RemoveDicItemCommand(dicItemId));
        }

        public static void AddEntityType(this IAcDomain host, IEntityTypeCreateIo input)
        {
            host.Handle(new AddEntityTypeCommand(input));
        }

        public static void UpdateEntityType(this IAcDomain host, IEntityTypeUpdateIo input)
        {
            host.Handle(new UpdateEntityTypeCommand(input));
        }

        public static void RemoveEntityType(this IAcDomain host, Guid entityTypeId)
        {
            host.Handle(new RemoveEntityTypeCommand(entityTypeId));
        }

        public static void AddFunction(this IAcDomain host, IFunctionCreateIo input)
        {
            host.Handle(new AddFunctionCommand(input));
        }
    }
}
