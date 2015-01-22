
namespace Anycmd.DataContracts
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public class Signature
    {
        private Signature() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orignalString">原始字符串</param>
        /// <param name="signValue">签名结果</param>
        /// <returns></returns>
        public static Signature Create(string orignalString, string signValue)
        {
            return new Signature
            {
                OrignalString = orignalString,
                Result = signValue
            };
        }

        /// <summary>
        /// 原始字符串
        /// </summary>
        public string OrignalString { get; private set; }

        /// <summary>
        /// 签名结果
        /// </summary>
        public string Result { get; private set; }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public bool IsValid(string secretKey)
        {
            if (string.IsNullOrEmpty(Result))
            {
                return false;
            }
            var sigString = Sign(this.OrignalString, secretKey);

            return sigString == this.Result;
        }

        /// <summary>
        /// 计算并返回签名字符串
        /// </summary>
        /// <param name="orignalString"></param>
        /// <param name="secretKey">私钥</param>
        /// <returns></returns>
        public static string Sign(string orignalString, string secretKey)
        {
            if (orignalString == null)
            {
                throw new ArgumentNullException("orignalString");
            }
            var hmacsha1 = new HMACSHA1 {Key = Encoding.ASCII.GetBytes(secretKey)};
            byte[] hashBytes = hmacsha1.ComputeHash(Encoding.ASCII.GetBytes(orignalString));

            return Convert.ToBase64String(hashBytes); // 编码为Base64
        }
    }
}
