
namespace Anycmd.Engine
{
    using System.Security.Principal;

    public sealed class UnauthenticatedIdentity: IIdentity
    {
        public UnauthenticatedIdentity()
        {
            this.IsAuthenticated = false;
            this.Name = string.Empty;
            this.AuthenticationType = string.Empty;
        }

        public string AuthenticationType { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public string Name { get; private set; }
    }
}
