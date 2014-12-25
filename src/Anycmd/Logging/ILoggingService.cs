
namespace Anycmd.Logging
{
    using Query;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 日志记录器接口
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="anyLog"></param>
        void Log(IAnyLog anyLog);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="anyLogs"></param>
        void Log(IAnyLog[] anyLogs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IAnyLog Get(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        IList<IAnyLog> GetPlistAnyLogs(List<FilterData> filters, PagingInput paging);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="leftCreateOn"></param>
        /// <param name="rightCreateOn"></param>
        /// <param name="filters"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        IList<OperationLog> GetPlistOperationLogs(Guid? targetId, DateTime? leftCreateOn, DateTime? rightCreateOn, List<FilterData> filters, PagingInput paging);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        IList<ExceptionLog> GetPlistExceptionLogs(List<FilterData> filters, PagingInput paging);

        /// <summary>
        /// 
        /// </summary>
        void ClearAnyLog();

        /// <summary>
        /// 
        /// </summary>
        void ClearExceptionLog();

        void Debug(object message);
        void DebugFormatted(string format, params object[] args);
        void Info(object message);
        void InfoFormatted(string format, params object[] args);
        void Warn(object message);
        void Warn(object message, Exception exception);
        void WarnFormatted(string format, params object[] args);
        void Error(object message);
        void Error(object message, Exception exception);
        void ErrorFormatted(string format, params object[] args);
        void Fatal(object message);
        void Fatal(object message, Exception exception);
        void FatalFormatted(string format, params object[] args);
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
    }
}
