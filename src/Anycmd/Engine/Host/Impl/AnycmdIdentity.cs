
namespace Anycmd.Engine.Host.Impl
{
    using System.Security.Principal;

    public sealed class AnycmdIdentity : IIdentity
    {
        /// <summary>
        /// 
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

        public string AuthenticationType { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public string Name { get; private set; }
    }
}
