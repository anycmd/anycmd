
namespace Anycmd.Engine.Host.Ac.Infra
{
    using System;

    /// <summary>
    /// 鉴别结果。审核鉴别器和权限鉴别器的返回值类型。
    /// </summary>
    public class DiscriminateResult
    {
        public static readonly DiscriminateResult Yes = new DiscriminateResult(true, string.Empty);
        public static readonly DiscriminateResult No = new DiscriminateResult(false, string.Empty);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yes"></param>
        /// <param name="description"></param>
        public DiscriminateResult(bool yes, string description)
        {
            this.IsYes = yes;
            this.Description = description;
        }

        public DiscriminateResult(Exception ex)
        {
            this.IsYes = false;
            if (ex != null)
            {
                this.Description = ex.Message;
            }
            this.Exception = ex;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsYes { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNo
        {
            get { return !IsYes; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 表示鉴别器执行鉴别逻辑的时候发生了异常。
        /// <remarks>
        /// .NET运行时不是能够捕获到所有的异常的，多线程的时候需要传递异常。
        /// </remarks>
        /// </summary>
        public Exception Exception { get; set; }
    }
}
