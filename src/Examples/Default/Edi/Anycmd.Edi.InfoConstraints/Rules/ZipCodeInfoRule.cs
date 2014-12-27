
namespace Anycmd.Edi.InfoConstraints.Rules
{
    using Engine.Host.Edi;
    using Engine.Info;
    using System;
    using System.ComponentModel.Composition;
    using System.Text.RegularExpressions;

    [Export(typeof(IInfoRule))]
    public class ZipCodeInfoCheck : InfoRuleBase, IInfoRule
    {
        private const string ActorId = "2B237AC1-3779-4637-95E7-6AA419EFC2C8";
        private static readonly Guid id = new Guid(ActorId);
        private const string title = "邮政编码验证器";
        private const string description = "使用正则表达式验证邮政编码格式的合法性";
        private const string author = "xuexs";
        private static readonly Regex EmailExpression = new Regex(@"^[1-9][0-9]{5}$", RegexOptions.Singleline | RegexOptions.Compiled);

        public ZipCodeInfoCheck()
            : base(id, title, author, description)
        {

        }

        public ProcessResult Valid(string value)
        {
            try
            {
                bool isValid = true;
                var stateCode = Status.Ok;
                string msg = "邮政编码验证通过";
                isValid = !string.IsNullOrEmpty(value) && EmailExpression.IsMatch(value);
                if (!isValid)
                {
                    msg = "非法的邮政编码";
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
