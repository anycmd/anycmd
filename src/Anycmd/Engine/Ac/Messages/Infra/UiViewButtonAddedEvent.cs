
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonAddedEvent : EntityAddedEvent<IUiViewButtonCreateIo>
    {
        public UiViewButtonAddedEvent(UiViewButtonBase source, IUiViewButtonCreateIo input)
            : base(source, input)
        {
        }
    }
}
