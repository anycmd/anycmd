
namespace Anycmd.Edi.InfoConstraints.Rules
{
    using Engine.Host.Edi;
    using Engine.Info;
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    /// 真实姓名验证器
    /// </summary>
    [Export(typeof(IInfoRule))]
    public class RealNameInfoCheck : InfoRuleBase, IInfoRule
    {
        private const string ActorId = "3B7892D3-9480-4E80-8A08-4D9AEF0EA9F4";
        private static readonly Guid id = new Guid(ActorId);
        private const string title = "真实姓名验证器";
        private const string description = "检测中国人的真实姓名是否合法，要求中间和两头不要有空格，否则将当前命令的状态码置为InvalidInfoValue";
        private const string author = "xuexs";

        public RealNameInfoCheck()
            : base(id, title, author, description)
        {

        }

        public ProcessResult Valid(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return new ProcessResult(true, Status.Ok, "真实姓名验证通过");
                }
                if (value.Trim().Length != value.Length)
                {
                    return new ProcessResult(false, Status.InvalidInfoValue, "真实姓名两头不能有空格");
                }
                if (value.Contains(" "))
                {
                    return new ProcessResult(false, Status.InvalidInfoValue, "真实姓名中间不能有空格");
                }
                return new ProcessResult(true, Status.Ok, "真实姓名验证通过");
            }
            catch (Exception ex)
            {
                return new ProcessResult(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {

        }
    }
}
