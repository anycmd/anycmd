namespace Anycmd.Commands
{
    using Bus;
    using Model;

    /// <summary>
    /// 标记接口。表示该接口的实现类是命令。
    /// </summary>
    public interface ICommand : IMessage, IEntity
    {

    }
}
