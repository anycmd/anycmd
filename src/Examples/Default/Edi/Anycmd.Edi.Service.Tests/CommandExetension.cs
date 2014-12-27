
namespace Anycmd.Edi.Service.Tests
{
    using DataContracts;
    using Engine.Hecp;
    using ServiceModel.Operations;
    using System;
    using Util;

    public static class CommandExetension
    {
        private const string JspxPublicKey = "41e711c6-f215-4606-a0bf-9af11cce1d54";
        private const string UiaPublicKey = "69E58EC0-5EB2-4633-9117-B433FC205B8F";
        private const string YxtPublicKey = "87E9DAAB-2EA4-4A99-92BA-6C9DDB0F868C";

        private const string JspxSecretKey = "DF25BCB5-35E3-41E4-980F-64D916D806FF";
        private const string UiaSecretKey = "DF25BCB5-35E3-41E4-980F-64D916D806FF";
        private const string YxtSecretKey = "df25bcb5-35e3-41e4-980f-64d916d806ff";

        /// <summary>
        /// 教师培训节点
        /// </summary>
        /// <returns></returns>
        public static Message JspxToken(this Message cmd)
        {
            cmd.Credential = CreateToken(JspxPublicKey, JspxSecretKey);
            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static Message JspxSignature(this Message cmd)
        {
            Signature(cmd, JspxPublicKey);
            return cmd;
        }

        /// <summary>
        /// Uia代理节点
        /// </summary>
        /// <returns></returns>
        public static Message UiaToken(this Message cmd)
        {
            cmd.Credential = CreateToken(UiaPublicKey, UiaSecretKey);
            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static Message UiaSignature(this Message cmd)
        {
            Signature(cmd, UiaPublicKey);
            return cmd;
        }

        /// <summary>
        /// 一线通节点
        /// </summary>
        /// <returns></returns>
        public static Message YxtToken(this Message cmd)
        {
            cmd.Credential = CreateToken(YxtPublicKey, YxtSecretKey);
            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static Message YxtSignature(this Message cmd)
        {
            Signature(cmd, YxtPublicKey);
            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Message TimeOutToken(this Message cmd)
        {
            cmd.Credential = CreateToken(YxtPublicKey, SystemTime.UtcNow().AddMinutes(-6).Ticks, YxtSecretKey);
            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static Message TimeOutSignature(this Message cmd)
        {
            Signature(cmd, YxtPublicKey, SystemTime.UtcNow().AddMinutes(-6).Ticks);
            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static CredentialData CreateToken(string publicKey, string secretKey)
        {
            return CreateToken(publicKey, SystemTime.UtcNow().Ticks, secretKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static void Signature(Message data, string publicKey)
        {
            Signature(data, publicKey, SystemTime.UtcNow().Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="ticks"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static CredentialData CreateToken(string publicKey, Int64 ticks, string secretKey)
        {
            var credential = new CredentialData
            {
                ClientType = ClientType.Node.ToName(),
                CredentialType = CredentialType.Token.ToName(),
                ClientId = publicKey,
                Ticks = ticks,
                UserName = "UnitTest"
            };
            credential.Password = TokenObject.Token(publicKey, ticks, secretKey);

            return credential;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="publicKey"></param>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static void Signature(Message command, string publicKey, Int64 ticks)
        {
            var credential = new CredentialData
            {
                ClientType = ClientType.Node.ToName(),
                CredentialType = CredentialType.Signature.ToName(),
                SignatureMethod = SignatureMethod.HMAC_SHA1.ToName(),
                ClientId = publicKey,
                Ticks = ticks,
                UserName = "UnitTest"
            };
            command.Credential = credential;
        }
    }
}
