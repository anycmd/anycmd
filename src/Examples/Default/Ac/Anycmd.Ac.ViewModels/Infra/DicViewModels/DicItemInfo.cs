
namespace Anycmd.Ac.ViewModels.Infra.DicViewModels
{
    using Engine;
    using System.Collections.Generic;
    using ViewModel;

    public class DicItemInfo : Dictionary<string, object>
    {
        private DicItemInfo() { }

        public static DicItemInfo Create(DicReader dic)
        {
            if (dic == null)
            {
                return null;
            }
            var data = new DicItemInfo();
            foreach (var item in dic)
            {
                data.Add(item.Key, item.Value);
            }
            if (!data.ContainsKey("IsEnabledName"))
            {
                data.Add("IsEnabledName", dic.AcDomain.Translate("Ac", "DicItem", "IsEnabledName", data["IsEnabled"].ToString()));
            }

            return data;
        }
    }
}
