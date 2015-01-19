
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using System;

    public class SaveHelpCommand : Command, IAnycmdCommand
    {
        public SaveHelpCommand(IUserSession userSession, Guid functionId, string content, int? isEnabled)
        {
            this.UserSession = userSession;
            this.FunctionId = functionId;
            this.Content = content;
            this.IsEnabled = isEnabled;
        }

        public IUserSession UserSession { get; private set; }

        public Guid FunctionId { get; private set; }
        public string Content { get; private set; }
        public int? IsEnabled { get; private set; }
    }
}
