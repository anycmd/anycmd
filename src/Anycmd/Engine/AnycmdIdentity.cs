
namespace Anycmd.Engine
{
    using System.Security.Principal;

    /// <summary>
    /// 定义用于标识主体的类型。
    /// </summary>
    public sealed class AnycmdIdentity : IIdentity
    {
        /// <summary>
        /// 构造并返回一个AnycmdIdentity类型的对象。
        /// </summary>
        /// <param name="authenticationType"></param>
        /// <param name="isAuthenticated"></param>
        /// <param name="name">loginName</param>
        public AnycmdIdentity(string name, string authenticationType = "Form", bool isAuthenticated = true)
        {
            this.AuthenticationType = authenticationType;
            this.IsAuthenticated = isAuthenticated;
            this.Name = name;
        }

        /// <summary>
        /// 获取所使用的身份验证的类型。
        /// <returns>用于标识用户的身份验证的类型。</returns>
        /// </summary>
        public string AuthenticationType { get; private set; }

        /// <summary>
        /// 获取一个值，该值指示是否验证了用户。
        /// <returns>如果用户已经过验证，则为 true；否则为 false。</returns>
        /// </summary>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// 获取当前用户的名称。
        /// <returns>用户名，代码当前即以该用户的名义运行。</returns>
        /// </summary>
        public string Name { get; private set; }
    }
}
