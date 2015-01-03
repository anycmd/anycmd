
namespace Anycmd.Engine
{
    using System;

    public class EntityCreateInput : ManagedPropertyValues, IEntityCreateInput
    {
        private Guid? _id = null;

        public Guid? Id
        {
            get { return _id ?? (_id = Guid.NewGuid()); }
            set { _id = value; }
        }
    }
}
