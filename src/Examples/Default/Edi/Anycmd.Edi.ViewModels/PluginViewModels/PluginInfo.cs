
namespace Anycmd.Edi.ViewModels.PluginViewModels
{
    using System;
    using System.Collections.Generic;
    using ViewModel;

    public class PluginInfo : Dictionary<string, object>
    {
        public PluginInfo() { }

        public PluginInfo(IAcDomain acDomain, Dictionary<string, object> dic)
        {
            if (dic == null)
            {
                throw new ArgumentNullException("dic");
            }
            foreach (var item in dic)
            {
                this.Add(item.Key, item.Value);
            }
            if (!this.ContainsKey("IsEnabledName"))
            {
                this.Add("IsEnabledName", acDomain.Translate("Edi", "InfoDic", "IsEnabledName", (int)this["IsEnabled"]));
            }
        }
    }
}
