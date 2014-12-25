
namespace Anycmd.Util
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// 令牌对象。
    /// <remarks>此对象不专门为序列化设计若要序列化应使用Anycmd.DataContract程序集中的TokenData</remarks>
    /// </summary>
    public sealed class TokenObject
    {
        private TokenObject() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenString">令牌字符串</param>
        /// <param name="appId">应用系统ID</param>
        /// <param name="ticks">时间戳</param>
        public static TokenObject Create(string tokenString, string appId, Int64 ticks)
        {
            return new TokenObject
            {
                TokenString = tokenString,
                AppId = appId,
                Ticks = ticks
            };
        }

        /// <summary>
        /// 令牌字符串
        /// </summary>
        public string TokenString { get; private set; }

        /// <summary>
        /// 公钥。
        /// </summary>
        public string AppId { get; private set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public Int64 Ticks { get; private set; }

        /// <summary>
        /// 验证令牌
        /// </summary>
        /// <param name="secretKey">私钥</param>
        /// <returns></returns>
        public bool IsValid(string secretKey)
        {
            if (string.IsNullOrEmpty(AppId))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(TokenString))
            {
                return false;
            }
            var myToken = Token(this.AppId, this.Ticks, secretKey);

            return myToken == TokenString;
        }

        /// <summary>
        /// 计算并返回令牌字符串
        /// </summary>
        /// <param name="appId">公钥</param>
        /// <param name="ticks"></param>
        /// <param name="secretKey">私钥</param>
        /// <returns></returns>
        public static string Token(string appId, Int64 ticks, string secretKey)
        {
            var s = (string.Format("{0}{1}{2}", appId, ticks, secretKey)).ToLower();// 转化为小写
            var crypto = new MD5CryptoServiceProvider();
            byte[] bytes = crypto.ComputeHash(Encoding.UTF8.GetBytes(s));
            var sb = new StringBuilder();
            foreach (byte num in bytes)
            {
                sb.AppendFormat("{0:x2}", num);
            }

            return sb.ToString();
        }
    }
}
