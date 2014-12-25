
namespace Anycmd.Util
{
    using System;

    /// <summary>
    /// 表示一个特性，在该特性中指明标定的类或方法的作者。如果您熟悉了或改了原作者的代码，留下你的名字。系统异常日志记录的时候使用这个名字关联上作者。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor, AllowMultiple = false)]
    public class ByAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="developerCode">开发人员编码</param>
        public ByAttribute(string developerCode)
        {
            this.DeveloperCode = developerCode;
        }

        /// <summary>
        /// 开发人员编码
        /// </summary>
        public string DeveloperCode { get; set; }
    }
}
