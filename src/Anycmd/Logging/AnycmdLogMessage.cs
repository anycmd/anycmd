
namespace Anycmd.Logging
{
    using System;

    /// <summary>
    /// 异常日志信息对象
    /// </summary>
    public class AnycmdLogMessage
    {
        private readonly string _message;

        public AnycmdLogMessage(string message)
        {
            this.Id = Guid.NewGuid();
            this._message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var log = obj as AnycmdLogMessage;
            if (log == null)
            {
                return false;
            }
            return log.Id == this.Id;
        }

        public override string ToString()
        {
            return _message;
        }
    }
}
