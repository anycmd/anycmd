
namespace Anycmd.Commands
{
    using Bus;

    /// <summary>
    /// 表示该接口的实现类是命令处理器。
    /// </summary>
    /// <typeparam name="TCommand">将被处理的命令.NET类型。</typeparam>
    [RegisterDispatch]
    public interface ICommandHandler<in TCommand> : IHandler<TCommand>
        where TCommand : ICommand
    {

    }
}
