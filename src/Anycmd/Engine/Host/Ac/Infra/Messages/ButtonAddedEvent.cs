
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using InOuts;
    using Model;

    /// <summary>
    /// 
    /// </summary>
    public class ButtonAddedEvent : EntityAddedEvent<IButtonCreateIo>
    {
        public ButtonAddedEvent(ButtonBase source, IButtonCreateIo input)
            : base(source, input)
        {
        }
    }
}