
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using System;

    public class SaveHelpCommand : Command, IAnycmdCommand
    {
        public SaveHelpCommand(Guid functionId, string content, int? isEnabled)
        {
            this.FunctionId = functionId;
            this.Content = content;
            this.IsEnabled = isEnabled;
        }

        public Guid FunctionId { get; private set; }
        public string Content { get; private set; }
        public int? IsEnabled { get; private set; }
    }
}
