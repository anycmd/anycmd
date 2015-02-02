
namespace Anycmd.Engine
{
    using System;

    public abstract class EntityCreateInput : ManagedPropertyValues, IEntityCreateInput
    {
        private Guid? _id = null;

        public Guid? Id
        {
            get { return _id ?? (_id = Guid.NewGuid()); }
            set { _id = value; }
        }
        public string HecpOntology { get; protected set; }

        public string HecpVerb { get; protected set; }

        public abstract IAnycmdCommand ToCommand(IAcSession acSession);
    }
}
