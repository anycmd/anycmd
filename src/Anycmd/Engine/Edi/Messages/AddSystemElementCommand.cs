
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using Commands;

    // TODO:在界面上添加创建运行时本体元素的按钮
    public class AddSystemElementCommand : Command, IAnycmdCommand
    {
        public AddSystemElementCommand(IAcSession acSession)
        {
            this.AcSession = acSession;
        }

        public IAcSession AcSession { get; private set; }
    }
}
