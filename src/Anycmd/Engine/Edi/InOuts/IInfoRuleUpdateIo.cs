
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;

    public interface IInfoRuleUpdateIo : IEntityUpdateInput
    {
        int IsEnabled { get; }
    }
}
