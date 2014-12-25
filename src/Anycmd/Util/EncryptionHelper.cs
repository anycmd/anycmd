
namespace Anycmd.Util
{
    using Exceptions;
    using System;
    using System.Diagnostics;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// 加密助手
    /// </summary>
    public static class EncryptionHelper
    {
        /// <summary>
        /// 返回MD5加密后的Base64字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string Hash(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new CoreException("加密字符串不能为空");
            }
            var crypto = new MD5CryptoServiceProvider();
            var bytes = Encoding.UTF8.GetBytes(value);
            bytes = crypto.ComputeHash(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
