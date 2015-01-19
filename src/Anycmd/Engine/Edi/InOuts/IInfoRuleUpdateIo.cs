
namespace Anycmd.Engine.Edi.InOuts
{
    public interface IInfoRuleUpdateIo : IEntityUpdateInput
    {
        string OntologyCode { get; }

        string Verb { get; }

        int IsEnabled { get; }
    }
}
