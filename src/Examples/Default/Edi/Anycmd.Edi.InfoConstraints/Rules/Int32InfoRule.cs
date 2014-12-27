
namespace Anycmd.Edi.InfoConstraints.Rules
{
    using Engine.Host.Edi;
    using Engine.Info;
    using System;
    using System.ComponentModel.Composition;

    [Export(typeof(IInfoRule))]
    public class Int32InfoCheck : InfoRuleBase, IInfoRule
    {
        private const string ActorId = "9E7522F9-11B8-49D1-9803-D755A2A27530";
        private static readonly Guid id = new Guid(ActorId);
        private const string title = "Int32验证器";
        private const string description = "Int32验证器";
        private const string author = "xuexs";

        public Int32InfoCheck()
            : base(id, title, author, description)
        {

        }

        public ProcessResult Valid(string value)
        {
            try
            {
                bool isValid = true;
                var stateCode = Status.Ok;
                string msg = "Int32验证通过";
                Guid guid;
                if (!string.IsNullOrEmpty(value) && !Guid.TryParse(value, out guid))
                {
                    msg = "非法的Int32" + value;
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
