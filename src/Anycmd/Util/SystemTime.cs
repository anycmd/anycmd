
namespace Anycmd.Util
{
    using System;

    /// <summary>
    /// 封装系统时间。
    /// 涉及到分布式时间戳就不那么不起眼了，时间戳必须被拿到台面上说。关键的只有一句话：在Anycmd系统中所有本地的时间戳全部使用本地时间，
    /// 而当时间戳需要传送到其它系统时全部转化为Utc时间戳，同样Anycmd接收来自其它系统是时间戳的时候全部视为Utc时间戳，接收时转化为本地时间戳。
    /// </summary>
    public static class SystemTime
    {
        /// <summary>
        /// DateTime(1900, 1, 1)
        /// </summary>
        public static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        /// <summary>
        /// new DateTime(9999, 12, 31, 23, 59, 59, 999)
        /// </summary>
        public static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);
        /// <summary>
        /// new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        /// </summary>
        public static readonly DateTime MinUtcDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc)
        /// </summary>
        public static readonly DateTime MaxUtcDate = new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc);
        /// <summary>
        /// 获取一个 System.DateTime 对象，该对象设置为此计算机上的当前日期和时间，表示为协调世界时 (UTC)。
        /// </summary>
        public static Func<DateTime> UtcNow = () => DateTime.UtcNow;
        /// <summary>
        /// 
        /// </summary>
        public static Func<DateTime> Now = () => DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static DateTime ParseUtcTicksToLocalTime(Int64 ticks)
        {
            if (ticks <= SystemTime.MinUtcDate.Ticks)
            {
                return SystemTime.MinDate;
            }
            else if (ticks >= SystemTime.MaxUtcDate.Ticks)
            {
                return SystemTime.MaxDate;
            }
            else
            {
                return new DateTime(ticks, DateTimeKind.Utc).ToLocalTime();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static DateTime? ParseUtcTicksToLocalTime(Int64? ticks)
        {
            if (!ticks.HasValue)
            {
                return null;
            }
            if (ticks <= SystemTime.MinUtcDate.Ticks)
            {
                return SystemTime.MinDate;
            }
            else if (ticks >= SystemTime.MaxUtcDate.Ticks)
            {
                return SystemTime.MaxDate;
            }
            else
            {
                return new DateTime(ticks.Value, DateTimeKind.Utc).ToLocalTime();
            }
        }
    }
}
