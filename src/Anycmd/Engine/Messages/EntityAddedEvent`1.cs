
namespace Anycmd.Engine.Messages
{
    using Events;
    using InOuts;
    using Model;
    using System;

    /// <summary>
    /// 表示实体添加事件的基类。
    /// </summary>
    /// <typeparam name="TEntityCreateInput">事件中的输出对象的类型参数。</typeparam>
    public abstract class EntityAddedEvent<TEntityCreateInput> : DomainEvent where TEntityCreateInput : class, IEntityCreateInput
    {
        protected EntityAddedEvent(IAcSession acSession, IEntity source, TEntityCreateInput output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            this.Output = output;
        }

        /// <summary>
        /// 输出参数。
        /// </summary>
        public TEntityCreateInput Output { get; private set; }
    }
}
