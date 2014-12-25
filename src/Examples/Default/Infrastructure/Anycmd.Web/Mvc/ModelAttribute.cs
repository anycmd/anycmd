using System;

namespace Anycmd.Web.Mvc
{
    /// <summary>
    /// 表示一个标记，该标记用于设定模型码
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ModelAttribute : Attribute
    {
        /// <param name="entityTypeCode">模型码</param>
        public ModelAttribute(string entityTypeCode)
        {
            this.EntityTypeCode = entityTypeCode;
        }

        /// <summary>
        /// 模型码
        /// </summary>
        public string EntityTypeCode
        {
            get;
            set;
        }
    }
}
