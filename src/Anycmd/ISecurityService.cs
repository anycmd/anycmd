
namespace Anycmd
{
    using Engine;
    using Engine.Ac;

    /// <summary>
    /// 安全服务接口
    /// </summary>
    public interface ISecurityService
    {
        // TODO:考虑返回三值：允许、不允许、我异常了。
        /// <summary>
        /// 判断给定的主体是否有对给定的资源对象实施给定的操作的权限。
        /// <remarks>
        /// 该接口是anycmd权限引擎的唯一用户接口。通常情况下应用系统唯一面向该接口编程就可以了。
        /// </remarks>
        /// </summary>
        /// <param name="subject">给定的主体</param>
        /// <param name="function">给定的功能。function = action = resourceType + verb。
        /// 功能是绑定了资源类型的，给定了功能即给定了被操作资源的资源类型。</param>
        /// <param name="obj">可为null，表示不验证实体级权限</param>
        /// <returns></returns>
        bool Permit(IAcSession subject, FunctionState function, IManagedObject obj);
    }
}
