

namespace Anycmd.Edi.ViewModels.MessageViewModels
{

    /// <summary>
    /// 命令详细信息展示模型抽象类
    /// </summary>
    public class MessageInfo : MessageTr
    {
        protected internal MessageInfo(IAcDomain acDomain)
            : base(acDomain)
        {
        }
    }
}
