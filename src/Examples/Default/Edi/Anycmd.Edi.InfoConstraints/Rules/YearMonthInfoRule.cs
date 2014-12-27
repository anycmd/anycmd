
namespace Anycmd.Edi.InfoConstraints.Rules
{
    using Engine.Host.Edi;
    using Engine.Info;
    using System;
    using System.ComponentModel.Composition;

    [Export(typeof(IInfoRule))]
    public class YearMonthInfoCheck : InfoRuleBase, IInfoRule
    {
        private const string ActorId = "3C300FC9-7E81-4425-8EE2-C4355DD08502";
        private static readonly Guid id = new Guid(ActorId);
        private const string title = "年月（YYYYMM）验证器";
        private const string description = "年月（YYYYMM）验证器";
        private const string author = "xuexs";

        public YearMonthInfoCheck()
            : base(id, title, author, description)
        {

        }

        public ProcessResult Valid(string value)
        {
            return ProcessResult.Ok;
        }

        protected override void Dispose(bool disposing)
        {

        }
    }
}
