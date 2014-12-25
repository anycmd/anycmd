
namespace Anycmd.Commands
{
    /// <summary>
    /// 表示命令处理器的基类。
    /// </summary>
    /// <typeparam name="TCommand">被处理的消息的.NET类型。</typeparam>
    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        #region Ctor
        /// <summary>
        /// 初始化一个 <c>CommandHandler&lt;TCommand&gt;</c> 类型的对象。
        /// </summary>
        protected CommandHandler()
        {

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 处理给定的命令对象。
        /// </summary>
        /// <param name="command">将被处理的命令对象。</param>
        public abstract void Handle(TCommand command);
        #endregion
    }
}
