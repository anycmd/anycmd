
namespace Anycmd.Ac.ViewModels.EntityTypeViewModels
{
    using System;
    using System.Collections.Generic;

    public class EntityTypeInfo : Dictionary<string, object>
    {
        public EntityTypeInfo(Dictionary<string, object> dic)
        {
            if (dic == null)
            {
                throw new ArgumentNullException("dic");
            }
            foreach (var item in dic)
            {
                this.Add(item.Key, item.Value);
            }
        }
    }
}
