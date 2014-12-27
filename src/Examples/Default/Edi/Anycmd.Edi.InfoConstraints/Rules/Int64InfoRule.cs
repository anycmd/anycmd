
namespace Anycmd.Edi.InfoConstraints.Rules
{
    using Engine.Host.Edi;
    using Engine.Info;
    using System;
    using System.ComponentModel.Composition;

    [Export(typeof(IInfoRule))]
    public class Int64InfoCheck : InfoRuleBase, IInfoRule
    {
        private const string ActorId = "8040267B-F931-4009-A959-62BF344EF1EB";
        private static readonly Guid id = new Guid(ActorId);
        private const string title = "Int64验证器";
        private const string description = "Int64验证器";
        private const string author = "xuexs";

        public Int64InfoCheck()
            : base(id, title, author, description)
        {

        }

        public ProcessResult Valid(string value)
        {
            try
            {
                bool isValid = true;
                var stateCode = Status.Ok;
                string msg = "Int64验证通过";
                Guid guid;
                if (!string.IsNullOrEmpty(value) && !Guid.TryParse(value, out guid))
                {
                    msg = "非法的Int64" + value;
                    stateCode = Status.InvalidInfoValue;
                    isValid = false;
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
