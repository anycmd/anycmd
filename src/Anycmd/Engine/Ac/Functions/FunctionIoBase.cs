
namespace Anycmd.Engine.Ac.Functions
{
    using Exceptions;
    using Model;
    using System;

    public abstract class FunctionIoBase : EntityBase, IFunctionIo
    {
        private string _code;
        private string _name;

        public Guid FunctionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code
        {
            get { return _code; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ValidationException("编码是必须的");
                }
                value = value.Trim();
                _code = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ValidationException("名称是必须的");
                }
                _name = value;
            }
        }

        public string Direction { get; set; }

        public string Description { get; set; }
    }
}
