
namespace Anycmd.Ac.ViewModels
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Util;

    public static class ViewModelHelper
    {
        public static List<DicReader> FillVisitingLog(List<DicReader> records)
        {
            if (records == null)
            {
                return null;
            }
            foreach (var dic in records)
            {
                if (dic == null)
                {
                    continue;
                }
                if (!dic.ContainsKey("FromNowString"))
                {
                    dic.Add("FromNowString", GetFromToString((DateTime)dic["VisitOn"], SystemTime.Now()));
                }
                DateTime? visitedOn;
                if (dic["VisitedOn"] == DBNull.Value)
                {
                    visitedOn = null;
                }
                else
                {
                    visitedOn = (DateTime)dic["VisitedOn"];
                }
                var timeSpanString = GetTimeSpanString((DateTime)dic["VisitOn"], visitedOn);
                if (!dic.ContainsKey("TimeSpanString"))
                {
                    dic.Add("TimeSpanString", timeSpanString);
                }
            }
            return records;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        private static string GetFromToString(DateTime from, DateTime? to)
        {
            if (!to.HasValue)
            {
                return "未知";
            }
            TimeSpan span = to.Value - from;
            if (span.TotalDays > 60)
            {
                return from.ToShortDateString();
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return "1秒前";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        private static string GetTimeSpanString(DateTime from, DateTime? to)
        {
            if (!to.HasValue)
            {
                return "未知";
            }
            TimeSpan span = to.Value - from;
            if (span.TotalDays > 60)
            {
                return from.ToShortDateString();
            }
            else if (span.TotalDays > 30)
            {
                return "1个月";
            }
            else if (span.TotalDays > 14)
            {
                return "2周";
            }
            else if (span.TotalDays > 7)
            {
                return "1周";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return "1秒";
            }
        }
    }
}
