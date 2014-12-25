
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using InOuts;
    using Model;

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
