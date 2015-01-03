
namespace Anycmd
{
    using Engine.Ac;
    using System;
    using System.Security.Principal;

    /// <summary>
    /// 表示用户会话。起标识用户的作用，在Ac命名空间下会往这个接口上扩展一些Ac方面的方法。
    /// <para>
    /// 持久的UserSession与内存中的UserSession：持久UserSession是对内存中的UserSession的持久跟踪，是对实现会话级的动态责任分离特性的必要准备。
    /// 持久的UserSession是这样一个概念，一个账户在第一次登录的时候会建立一个内存中的UserSession，这个UserSession会被持久化起来。用户退出系统时
    /// 会更新持久的UserSession的IsAuthenticated为false但不会删除这条UserSession记录。用户下次登录的成功时IsAuthenticated会再次更新为true，
    /// 持久的UserSession只在用户登录和退出系统时访问，持久的UserSession的存在使得安全管理员有机会面向用户的UserSession建立用户会话级的动态责任分离策略。
    /// </para>
    /// <para>
    /// 一个账户可以对应多个UserSession，安全管理员可以控制哪个UserSession在什么情况下激活而哪些UserSession不能激活。安全管理员可以为某个账户建立新的
    /// UserSession但不马上切换为它，安全管理员针对这个UserSession进行会话级的动态责任分离授权并测试符合预期后再禁用用户原来的UserSession切换为新的UserSession，
    /// 系统可以让UserSession被禁用的那个账户下线然后他再次登录就切换到新的UserSession了，系统也应该能做到在用户不知觉的情况下平滑的切换掉他的UserSession。
    /// </para>
    /// </summary>
    public interface IUserSession : IPrincipal
    {
        /// <summary>
        /// 用户会话标识。一个用户（Account）可以对应有多个会话，约定会话标识与账户标识相等的那个会话为这个账户的主会话。
        /// <remarks>
        /// 主会话在用户第一次登录使用系统的时候创建并持久跟踪，非主会话由安全管理员按需创建。用户登录成功时该用户的会话列表会加载进应用系统内存，然后由会话激活策略基于会话的属性取其中之一激活。
        /// </remarks>
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 容器引用。
        /// </summary>
        /// <returns></returns>
        IAcDomain AcDomain { get; }
        /// <summary>
        /// 当前会话所属的 账户 = 用户。
        /// </summary>
        /// <returns></returns>
        AccountState Account { get; }

        /// <summary>
        /// 当前账户的权限对象。
        /// </summary>
        AccountPrivilege AccountPrivilege { get; }
    }
}
