
namespace Anycmd.Engine.InOuts
{
    public interface IAnycmdInput : IInputModel
    {
        string HecpOntology { get; }

        string HecpVerb { get; }

        IAnycmdCommand ToCommand(IAcSession acSession);
    }
}
