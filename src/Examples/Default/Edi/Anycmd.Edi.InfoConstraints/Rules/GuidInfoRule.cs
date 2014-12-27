
namespace Anycmd.Edi.InfoConstraints.Rules
{
    using Engine.Host.Edi;
    using Engine.Info;
    using System;
    using System.ComponentModel.Composition;

    [Export(typeof(IInfoRule))]
    public class GuidInfoCheck : InfoRuleBase, IInfoRule
    {
        private const string ActorId = "A93559F9-5268-4225-92CA-2D394FE9A8B3";
        private static readonly Guid id = new Guid(ActorId);
        private const string title = "Guid验证器";
        private const string description = "Guid验证器";
        private const string author = "xuexs";

        public GuidInfoCheck()
            : base(id, title, author, description)
        {

        }

        public ProcessResult Valid(string value)
        {
            try
            {
                bool isValid = true;
                var stateCode = Status.Ok;
                string msg = "Guid验证通过";
                Guid guid;
                if (!string.IsNullOrEmpty(value) && !Guid.TryParse(value, out guid))
                {
                    msg = "非法的Guid" + value;
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
