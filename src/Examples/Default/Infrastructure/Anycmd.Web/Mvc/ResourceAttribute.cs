
namespace Anycmd.Web.Mvc
{
    using System;

    /// <summary>
    /// 表示一个标记，该标记用于设定资源码
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ResourceAttribute : Attribute
    {
        /// <param name="resourceCode">资源码</param>
        public ResourceAttribute(string resourceCode)
        {
            this.ResourceCode = resourceCode;
        }

        /// <summary>
        /// 资源码
        /// </summary>
        public string ResourceCode
        {
            get;
            set;
        }
    }
}
