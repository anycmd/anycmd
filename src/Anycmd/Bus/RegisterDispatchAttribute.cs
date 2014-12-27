
namespace Anycmd.Bus
{
    using System;

    /// <summary>
    /// 表示一个特性，表示标记了该特性的接口可以被注册进消息分发器。
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class RegisterDispatchAttribute : Attribute
    {
    }
}
