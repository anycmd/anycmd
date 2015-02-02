
namespace Anycmd.Engine
{
    public interface IAnycmdInput : IInputModel
    {
        string HecpOntology { get; }

        string HecpVerb { get; }

        IAnycmdCommand ToCommand(IAcSession userSession);
    }
}
