
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Distribute;
    using Engine.Edi;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 命令消息转移策略。实现该接口的对象称作“命令转移器”，暂实现基于原生Http和WebService两种转移器。
    /// 其它转移器以后根据需要再做实现。该接口抽象了所有转移器，上层只应面向该接口编程不要直接面向具体的转移器编程。
    /// <remarks>
    /// “命令转移”就是将命令转化成数据传输对象，以消息的形式通过通信基础设施成功转移到远端节点的过程。
    /// 命令消息转移策略的实现应用只负责转移命令，不要加入任何其它业务逻辑。
    /// </remarks>
    /// </summary>
    public interface IMessageTransfer : IWfResource, IDisposable
    {
        /// <summary>
        /// 插件标识
        /// </summary>
        new Guid Id { get; }

        /// <summary>
        /// 插件标题
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 插件作者。如xuexs。这个编码是你的开发人员帐号，刘阳的是liuy，窦伟康的是douwk。
        /// <remarks>
        /// 记录下作者，防止以后找不到干系人。
        /// </remarks>
        /// </summary>
        string Author { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        string GetAddress(NodeDescriptor actor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        void IsAlive(BeatContext context);

        Task IsAliveAsync(BeatContext context);

        /// <summary>
        /// 把给定的命令消息发送到给定的节点
        /// <remarks>
        /// 传进的待发送命令消息集合中每一条元素的ClientID应相同且与给定的发送事件参数的ToNode的Id相同
        /// </remarks>
        /// </summary>
        /// <param name="context">发送事件参数</param>
        /// <returns></returns>
        void Transmit(DistributeContext context);

        Task TransmitAsync(DistributeContext context);
    }
}
