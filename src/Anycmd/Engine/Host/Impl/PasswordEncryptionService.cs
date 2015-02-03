
namespace Anycmd.Engine.Host.Impl
{
    using Util;

    public class PasswordEncryptionService : IPasswordEncryptionService
    {
        private readonly IAcDomain _acDomain;

        public PasswordEncryptionService(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public string Encrypt(string rawPwd)
        {
            return EncryptionHelper.Hash(rawPwd);
        }
    }
}
