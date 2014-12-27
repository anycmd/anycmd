
namespace Anycmd.Edi.InfoConstraints.Rules
{
    using Engine.Host.Edi;
    using Engine.Info;
    using System;
    using System.ComponentModel.Composition;
    using System.Text.RegularExpressions;

    [Export(typeof(IInfoRule))]
    public sealed class TelephoneInfoCheck : InfoRuleBase, IInfoRule
    {
        private const string ActorId = "88F8EDE3-BA84-45D1-9D5B-1ACD682381F5";
        private static readonly Guid id = new Guid(ActorId);
        private const string title = "固定电话号码验证器";
        private const string description = "使用正则表达式验证固定电话号码格式的合法性";
        private const string author = "xuexs";
        private static readonly Regex EmailExpression = new Regex(@"(\d{4}-|\d{3}-)?(\d{8}|\d{7})", RegexOptions.Singleline | RegexOptions.Compiled);

        public TelephoneInfoCheck()
            : base(id, title, author, description)
        {

        }

        public ProcessResult Valid(string value)
        {
            try
            {
                bool isValid = true;
                Status stateCode = Status.Ok;
                string msg = "固定电话验证通过";
                isValid = !string.IsNullOrEmpty(value) && EmailExpression.IsMatch(value);
                if (!isValid)
                {
                    msg = "非法的固定电话号码";
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
