
namespace Anycmd.Util
{
    public class Coder
    {
        public Coder(string codespace, string code)
        {
            this.Codespace = codespace;
            this.Code = code;
        }

        public string Codespace { get; private set; }

        public string Code { get; private set; }
    }
}
