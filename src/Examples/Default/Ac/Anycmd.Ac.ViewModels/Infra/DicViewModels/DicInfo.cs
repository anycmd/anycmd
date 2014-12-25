
namespace Anycmd.Ac.ViewModels.Infra.DicViewModels
{
    using Model;
    using System.Collections.Generic;
    using ViewModel;

    public class DicInfo : Dictionary<string, object>
    {
        private DicInfo() { }

        public static DicInfo Create(DicReader dic)
        {
            if (dic == null)
            {
                return null;
            }
            var data = new DicInfo();
            foreach (var item in dic)
            {
                data.Add(item.Key, item.Value);
            }
            if (!data.ContainsKey("IsEnabledName"))
            {
                data.Add("IsEnabledName", dic.Host.Translate("Ac", "Dic", "IsEnabledName", data["IsEnabled"].ToString()));
            }

            return data;
        }
    }
}
