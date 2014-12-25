
namespace Anycmd.Engine.Host
{
    /// <summary>
    /// 表示该接口的实现类是密码加密服务。
    /// </summary>
    public interface IPasswordEncryptionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawPwd"></param>
        /// <returns></returns>
        string Encrypt(string rawPwd);
    }
}
