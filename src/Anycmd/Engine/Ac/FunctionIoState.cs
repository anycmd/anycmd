
namespace Anycmd.Engine.Ac
{
    using Abstractions.Infra;
    using Exceptions;
    using Model;
    using System;
    using Util;

    /// <summary>
    /// 表示过程的输入输出参数业务实体。
    /// </summary>
    public sealed class FunctionIoState : StateObject<FunctionIoState>
    {
        private Guid _functionId;
        private IoDirection _direction;
        private string _code;
        private string _name;

        private FunctionIoState(Guid id) : base(id) { }

        public static FunctionIoState Create(FunctionIoBase entity)
        {
            IoDirection direction;
            if (!entity.Direction.TryParse(out direction))
            {
                throw new AnycmdException("意外的输入输出方向" + entity.Direction);
            }
            return new FunctionIoState(entity.Id)
            {
                _direction = direction,
                _code = entity.Code,
                _name = entity.Name,
                _functionId = entity.FunctionId
            };
        }

        public Guid FunctionId
        {
            get { return _functionId; }
        }

        public IoDirection Direction
        {
            get { return _direction; }
        }

        public string Code
        {
            get
            {
                return _code;
            }
        }

        public string Name
        {
            get { return _name; }
        }

        protected override bool DoEquals(FunctionIoState other)
        {
            return FunctionId == other.FunctionId &&
                Direction == other.Direction &&
                Code == other.Code &&
                Name == other.Name;
        }
    }
}
