
namespace Anycmd.Engine.Host.Edi
{
    using System;

    /// <summary>
    /// 命令验证器验证结果
    /// </summary>
    public sealed class ProcessResult
    {
        public static readonly ProcessResult Ok = new ProcessResult(
            true, Status.Ok, string.Empty);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="stateCode"></param>
        /// <param name="description"></param>
        public ProcessResult(bool isSuccess, Status stateCode, string description)
        {
            this.IsSuccess = isSuccess;
            this.StateCode = stateCode;
            this.Description = description;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="stateCode"></param>
        /// <param name="description"></param>
        public ProcessResult(bool isSuccess, int stateCode, string description)
        {
            Status s;
            if (stateCode.TryParse(out s))
            {
                this.StateCode = s;
            }
            else
            {
                this.StateCode = Status.InvalidStatus;
            }
            this.IsSuccess = isSuccess;
            this.Description = description;
        }

        public ProcessResult(Exception ex)
            : this(ex, Status.InternalServerError)
        {
        }

        public ProcessResult(Exception ex, Status stateCode)
            : this(ex, Status.InternalServerError, null)
        {
        }

        public ProcessResult(Exception ex, Status stateCode, string description)
        {
            this.IsSuccess = false;
            this.StateCode = stateCode;
            if (string.IsNullOrEmpty(description))
            {
                if (ex != null)
                {
                    this.Description = ex.Message;
                }
            }
            this.Exception = ex;
        }

        /// <summary>
        /// 验证通过。True表示通过False不通过
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public Status StateCode { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReasonPhrase
        {
            get
            {
                return this.StateCode.ToName();
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get; set; }
    }
}
