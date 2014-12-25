
namespace Anycmd.Engine.Host.Impl
{
    using Util;

    public class PasswordEncryptionService : IPasswordEncryptionService
    {
        private readonly IAcDomain _host;

        public PasswordEncryptionService(IAcDomain host)
        {
            this._host = host;
        }

        public string Encrypt(string rawPwd)
        {
            return EncryptionHelper.Hash(rawPwd);
        }
    }
}
