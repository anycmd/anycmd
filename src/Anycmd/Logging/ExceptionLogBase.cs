
namespace Anycmd.Logging
{
    using Model;
    using System;

    /// <summary>
    /// 系统异常日志
    /// </summary>
    public abstract class ExceptionLogBase : EntityObject
    {
        /// <summary>
        /// 等级
        /// <remarks>
        /// 对于log4net其枚举值就是：OFF > FATAL > ERROR > WARN > INFO > DEBUG > ALL
        /// </remarks>
        /// </summary>
        public virtual string Level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string Machine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Process { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BaseDirectory { get; set; }
        /// <summary>
        /// 线程
        /// </summary>
        public virtual string Thread { get; set; }
        /// <summary>
        /// 记录器
        /// </summary>
        public virtual string Logger { get; set; }
        /// <summary>
        /// 概述
        /// </summary>
        public virtual string Message { get; set; }
        /// <summary>
        /// 异常详细
        /// </summary>
        public virtual string Exception { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public virtual DateTime LogOn { get; set; }
    }
}
