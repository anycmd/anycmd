
namespace Anycmd.Engine.Host.Edi.Handlers
{

    /// <summary>
    /// 命令输入验证器。只验证输入。
    /// </summary>
    public interface IInputValidator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        ProcessResult Validate(MessageContext context);
    }
}
