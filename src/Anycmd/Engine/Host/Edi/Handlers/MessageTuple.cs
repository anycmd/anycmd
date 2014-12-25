
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Info;

    /// <summary>
    /// 
    /// </summary>
    public class MessageTuple
    {
        public MessageTuple(MessageContext context, InfoItem[] tuple)
        {
            this.Context = context;
            this.Tuple = tuple;
        }

        /// <summary>
        /// 
        /// </summary>
        public MessageContext Context { get; private set; }

        /// <summary>
        /// 如果建造命令所需的所有元组字典都在这个信息元组里的话则建造器就不用再次查询数据库了。
        /// </summary>
        public InfoItem[] Tuple { get; private set; }
    }
}
