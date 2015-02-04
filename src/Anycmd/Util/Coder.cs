
namespace Anycmd.Util
{
    using System;

    public class Coder
    {
        public Coder(string codespace, string code)
        {
            if (string.IsNullOrEmpty(codespace))
            {
                throw new ArgumentNullException("codespace");
            }
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException("code");
            }
            this.Codespace = codespace;
            this.Code = code;
        }

        public string Codespace { get; private set; }

        public string Code { get; private set; }
    }
}
