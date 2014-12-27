
namespace Anycmd.Bus
{

    /// <summary>
    /// 表示该接口的实现类是消息处理器类型。
    /// </summary>
    /// <!--
    /// 添加注释的时候思考到一个问题：消息的类型即消息的分类，分类就是按照消息的性质、特点、用途作为区分的标准将符合同一标准的消息聚类。
    /// 那么问题是：这个分类是否考虑从技术平台运行时中抽象出来？这句注释中为了准确的说明必须修饰为“.NET类型”，因为这个分类是借助运行时环境分类的而不是独立的分类。
    /// -->
    /// <typeparam name="TMessage">将被处理的消息的.NET类型。</typeparam>
    public interface IHandler<in TMessage> where TMessage : IMessage
    {
        /// <summary>
        /// 处理给定类型的消息。
        /// </summary>
        /// <param name="message">将被处理的消息。</param>
        void Handle(TMessage message);
    }
}
