
namespace Anycmd.Engine.InOuts
{
    using Messages;

    public interface IAnycmdInput : IInputModel
    {
        string HecpOntology { get; }

        string HecpVerb { get; }

        IAnycmdCommand ToCommand(IAcSession acSession);
    }
}
