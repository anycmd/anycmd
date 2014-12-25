
namespace Anycmd.Engine.Host.Hecp
{
    using Exceptions;
    using Util;

    /// <summary>
    /// 心跳请求模型
    /// </summary>
    public sealed class BeatRequest
    {
        // ReSharper disable once InconsistentNaming
        private static readonly BeatRequest _v1Request = new BeatRequest(ApiVersion.V1);

        public static BeatRequest V1Request
        {
            get { return _v1Request; }
        }

        private BeatRequest() { }

        public BeatRequest(ApiVersion version)
        {
            if (version == ApiVersion.Undefined || version == default(ApiVersion))
            {
                throw new CoreException("非法的协议版本号" + version.ToName());
            }
            this.Version = version.ToName();
        }

        /// <summary>
        /// 协议版本号
        /// </summary>
        public string Version { get; private set; }
    }
}
