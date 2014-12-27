
namespace Anycmd.Edi.InfoConstraints.Rules
{
    using Engine.Host.Edi;
    using Engine.Info;
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    /// 通信地址验证器
    /// </summary>
    [Export(typeof(IInfoRule))]
    public sealed class AddressInfoCheck : InfoRuleBase, IInfoRule
    {
        private const string ActorId = "6D4AC146-E1D8-4C2A-B13C-82A89E07A7E2";
        private static readonly Guid id = new Guid(ActorId);
        private static readonly string title = "地址验证器";
        private static readonly string description = "检测地址是否合法，否则将当前命令的状态码置为InvalidInfoValue";
        private static readonly string author = "xuexs";

        public AddressInfoCheck()
            : base(id, title, author, description)
        {

        }

        public ProcessResult Valid(string value)
        {
            try
            {
                // TODO:书写地址规则
                return new ProcessResult(true, Status.Ok, "地址验证通过");
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
