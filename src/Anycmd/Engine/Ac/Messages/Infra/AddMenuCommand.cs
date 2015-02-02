﻿
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddMenuCommand : AddEntityCommand<IMenuCreateIo>, IAnycmdCommand
    {
        public AddMenuCommand(IAcSession userSession, IMenuCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
