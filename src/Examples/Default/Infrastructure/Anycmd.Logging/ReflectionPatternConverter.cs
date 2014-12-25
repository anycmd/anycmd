
namespace Anycmd.Logging
{
    using log4net.Core;
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// <see cref="log4net.Layout.Pattern.PatternLayoutConverter"/>
    /// </summary>
    public class ReflectionPatternConverter : log4net.Layout.Pattern.PatternLayoutConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="loggingEvent"></param>
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (Option != null)
            {
                // 写入指定键的值
                WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
            }
            else
            {
                // 写入所有关键值对
                WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
            }
        }

        /// <summary>
        /// 通过反射获取传入的日志对象的某个属性的值
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <param name = "loggingEvent"></param>
        private object LookupProperty(string property, LoggingEvent loggingEvent)
        {
            object propertyValue = string.Empty;
            var messageObjectType = loggingEvent.MessageObject.GetType();
            PropertyInfo propertyInfo = messageObjectType.GetProperty(property);
            if (propertyInfo == null)
            {
                return propertyValue;
            }
            propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject, null);
            if (property.ToLower() == "id")
            {
                if (Guid.Empty.Equals(propertyValue))
                {
                    propertyValue = Guid.NewGuid();
                }
            }

            return propertyValue;
        }
    }
}
