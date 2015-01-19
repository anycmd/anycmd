
namespace Anycmd.Engine.Edi.InOuts
{
    public interface IInfoRuleUpdateIo : IEntityUpdateInput
    {
        string HecpOntology { get; }

        string HecpVerb { get; }

        int IsEnabled { get; }
    }
}
