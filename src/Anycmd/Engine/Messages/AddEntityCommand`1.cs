
namespace Anycmd.Engine.Messages
{
    using Commands;
    using InOuts;
    using System;

    /// <summary>
    /// 表示表达的意愿是“添加实体”的命令的抽象基类。
    /// <remarks>
    /// Command = ResourceType + Action + Input;Command用来完整的描述一件希望发生的事情，Event用来完整的描述一件已经发生的事情。
    /// </remarks>
    /// </summary>
    /// <typeparam name="TEntityCreateInput">命令中的输入对象的类型参数。</typeparam>
    public abstract class AddEntityCommand<TEntityCreateInput> : Command where TEntityCreateInput : class, IEntityCreateInput
    {
        /// <summary>
        /// 初始化一个 <c>AddEntityCommand</c> 类型的对象。
        /// </summary>
        /// <param name="acSession"></param>
        /// <param name="input">命令中的输入参数。</param>
        protected AddEntityCommand(IAcSession acSession, TEntityCreateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.AcSession = acSession;
            this.Input = input;
        }

        public IAcSession AcSession { get; private set; }

        /// <summary>
        /// 输入参数。
        /// </summary>
        public TEntityCreateInput Input { get; private set; }
    }
}
