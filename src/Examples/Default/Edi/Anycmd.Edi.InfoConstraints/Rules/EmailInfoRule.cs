
namespace Anycmd.Edi.InfoConstraints.Rules
{
    using Engine.Host.Edi;
    using Engine.Info;
    using System;
    using System.ComponentModel.Composition;
    using System.Text.RegularExpressions;

    [Export(typeof(IInfoRule))]
    public sealed class EmailInfoCheck : InfoRuleBase, IInfoRule
    {
        private const string ActorId = "44780236-BFDD-47DE-A8CD-1BF51ADDB576";
        private static readonly Guid id = new Guid(ActorId);
        private const string title = "电子邮箱（Email）验证器";
        private const string description = "使用正则表达式验证邮箱格式的合法性";
        private const string author = "xuexs";

        private static readonly Regex EmailExpression = new Regex(
            @"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$"
            , RegexOptions.Singleline | RegexOptions.Compiled);

        public EmailInfoCheck()
            : base(id, title, author, description)
        {

        }

        public ProcessResult Valid(string value)
        {
            try
            {
                bool isValid = true;
                var stateCode = Status.Ok;
                string msg = "电子邮箱验证通过";
                isValid = !string.IsNullOrEmpty(value) && EmailExpression.IsMatch(value);
                if (!isValid)
                {
                    msg = "非法的电子邮箱地址";
                    stateCode = Status.InvalidInfoValue;
                }

                return new ProcessResult(isValid, stateCode, msg);
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
