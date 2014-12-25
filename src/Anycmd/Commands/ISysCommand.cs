
namespace Anycmd.Commands
{
    /// <summary>
    /// 标记接口。表示该接口的实现类是系统命令。
    /// <remarks>
    /// anycmd框架的用户自定义命令类型时应实现ICommand接口而非ISystemCommand接口。
    /// 系统命令用于将anycmd的用户所自定义的命令类型和anycmd本身所定义的命令类型基于约定隔离开。
    /// </remarks>
    /// </summary>
    public interface ISysCommand : ICommand
    {
    }
}
