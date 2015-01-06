
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using InOuts;

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